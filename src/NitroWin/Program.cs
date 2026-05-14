using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Resources;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NitroWin.Models.Apps;
using NitroWin.Services;
using Serilog;
using Serilog.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

var AppHost = Host.CreateDefaultBuilder()
    .ConfigureServices((hostContext, services) => {
        services.AddSerilog((ctx, lc) => lc
#if DEBUG
            .MinimumLevel.Debug()
#endif
            .WriteTo.Console(outputTemplate: "[{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .WriteTo.File(Path.Join("Logs", "NitroWin.txt"), rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Debug));

        services.AddSingleton<LogService>();

        services.AddSingleton(_ => new ResourceManager(
            "NitroWin.Resources.Strings", Assembly.GetExecutingAssembly()));

        services.AddSingleton(_ => new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .WithTagMapping("!choco:", typeof(ChocolateyApp))
            .WithTagMapping("!web:", typeof(WebApp))
            .WithTagMapping("!winget:", typeof(WingetApp))
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

        services.AddSingleton<ChocolateyService>();
        services.AddSingleton<WingetService>();
    })
    .Build();

await AppHost.StartAsync();

try {
    var configService = AppHost.Services.GetRequiredService<ConfigService>();
    var logService = AppHost.Services.GetRequiredService<LogService>();
    var commandLineService = AppHost.Services.GetRequiredService<CommandLineService>();
    var tweakService = AppHost.Services.GetRequiredService<TweakService>();

    var wingetService = AppHost.Services.GetRequiredService<WingetService>();
    var chocolateyService = AppHost.Services.GetRequiredService<ChocolateyService>();

    var appInstallerConfig = await configService.GetAppInstallerAsync();

    commandLineService.WriteBranding();

    var options = commandLineService.ParseArguments(args);

    if (args.Length > 0)
        logService.CommandLineArguments(args);

    while (!NetworkInterface.GetIsNetworkAvailable()) {
        logService.NoNetworkError();
        Thread.Sleep(5000);
    }

    if (!options.NoApps) {
        if (chocolateyService.IsInstallationNeeded() && !await chocolateyService.IsInstalledAsync())
            await chocolateyService.App.InstallAsync();

        if (wingetService.IsInstallationNeeded() && !await wingetService.IsInstalledAsync())
            await wingetService.App.InstallAsync();

        if (appInstallerConfig != null) {
            logService.InstallingApps();

            foreach (var app in appInstallerConfig.Apps) {
                await app.InstallAsync();
            }
        }
    }

    if (!options.NoTweaks)
        await tweakService.ApplyTweaksAsync();
} finally {
    await AppHost!.StopAsync();
    AppHost.Dispose();
}
