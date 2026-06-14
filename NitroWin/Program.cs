using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Resources;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NitroWin.Factories;
using NitroWin.Models.Apps;
using NitroWin.Models.Tweaks.Actions;
using NitroWin.Services;
using Serilog;
using Serilog.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

var AppHost = Host.CreateDefaultBuilder()
    .ConfigureServices((hostContext, services) => {
        services.AddLogging(loggingBuilder =>
            loggingBuilder.AddSerilog(dispose: true));

        services.AddSerilog((ctx, lc) => lc
#if DEBUG
            .MinimumLevel.Debug()
#endif
            .WriteTo.Console(outputTemplate: "[{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .WriteTo.File(Path.Join("Logs", "NitroWin.txt"), rollingInterval: RollingInterval.Minute, restrictedToMinimumLevel: LogEventLevel.Debug));

        services.AddSingleton<LogService>();

        services.AddSingleton(_ => new ResourceManager(
            "NitroWin.Resources.Strings", Assembly.GetExecutingAssembly()));

        services.AddSingleton(_ => new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .WithObjectFactory(new ServiceProviderObjectFactory(services.BuildServiceProvider()))
            .WithTagMapping("!choco:", typeof(ChocolateyApp))
            .WithTagMapping("!web:", typeof(WebApp))
            .WithTagMapping("!webAppx:", typeof(AppxWebApp))
            .WithTagMapping("!winget:", typeof(WingetApp))
            .WithTagMapping("!cmd:", typeof(CmdAction))
            .WithTagMapping("!powerShell:", typeof(PowerShellAction))
            .WithTagMapping("!registryValue:", typeof(RegistryValueAction))
            .WithTagMapping("!run:", typeof(RunAction))
            .WithTagMapping("!scheduledTask:", typeof(ScheduledTaskAction))
            .WithTagMapping("!service:", typeof(ServiceAction))
            .Build());

        services.AddSingleton<ConfigService>();

        services.AddSingleton<CommandLineService>();

        services.AddHttpClient("Default", client => {
            client.DefaultRequestHeaders.UserAgent.Add(
                new ProductInfoHeaderValue(
                    "NitroWin",
                    Assembly.GetExecutingAssembly().GetName().Version?.ToString()
                )
            );
        });
        services.AddSingleton<DownloaderService>();

        services.AddSingleton<ExtractionService>();

        services.AddSingleton<TweakService>();
        services.AddHostedService(sp => sp.GetRequiredService<TweakService>());

        services.AddSingleton<ChocolateyService>();
        services.AddHostedService(sp => sp.GetRequiredService<ChocolateyService>());

        services.AddSingleton<WingetService>();
        services.AddHostedService(sp => sp.GetRequiredService<WingetService>());
    })
    .Build();

try {
    await AppHost.StartAsync();

    var configService = AppHost.Services.GetRequiredService<ConfigService>();
    var logService = AppHost.Services.GetRequiredService<LogService>();
    var commandLineService = AppHost.Services.GetRequiredService<CommandLineService>();
    var tweakService = AppHost.Services.GetRequiredService<TweakService>();
    var wingetService = AppHost.Services.GetRequiredService<WingetService>();
    var chocolateyService = AppHost.Services.GetRequiredService<ChocolateyService>();
    var applicationLifetime = AppHost.Services.GetRequiredService<IHostApplicationLifetime>();

    commandLineService.WriteBranding();
    var options = commandLineService.ParseArguments(args);

    if (applicationLifetime.ApplicationStopping.IsCancellationRequested) {
        await AppHost.StopAsync();
        AppHost.Dispose();
        return;
    }

    logService.CommandLineArguments(args);

    while (!NetworkInterface.GetIsNetworkAvailable()) {
        logService.NoNetworkError();
        await Task.Delay(5000);
    }

    var appInstallerConfig = await configService.GetAppInstallerAsync(applicationLifetime.ApplicationStopping);

    if (!options.NoApps) {
        if (chocolateyService.IsInstallationNeeded() && !await chocolateyService.IsInstalledAsync(applicationLifetime.ApplicationStopping))
            await chocolateyService.App.InstallAsync(applicationLifetime.ApplicationStopping);

        if (wingetService.IsInstallationNeeded() && !await wingetService.IsInstalledAsync(applicationLifetime.ApplicationStopping))
            await wingetService.App.InstallAsync(applicationLifetime.ApplicationStopping);

        if (appInstallerConfig.Apps is not null) {
            logService.InstallingApps();

            foreach (var app in appInstallerConfig.Apps)
                await app.InstallAsync(applicationLifetime.ApplicationStopping);
        }
    }

    if (!options.NoTweaks)
        await tweakService.ApplyTweaksAsync(applicationLifetime.ApplicationStopping);
} catch (Exception ex) {
    try {
        var logService = AppHost.Services.GetRequiredService<LogService>();
        logService.CriticalError(ex);
    } catch {
        Console.WriteLine($"FATAL ERROR: {ex.Message}");
    }

    Environment.Exit(1);
} finally {
    await AppHost.StopAsync();
    AppHost.Dispose();
}
