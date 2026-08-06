using System.Net.NetworkInformation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NitroWin.Core.Extensions;
using NitroWin.Core.Services;
using NitroWin.Services;
using Serilog;
using Serilog.Events;

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

        services.AddNitroWin();
    })
    .Build();

try {
    await AppHost.StartAsync();

    var applicationLifetime = AppHost.Services.GetRequiredService<IHostApplicationLifetime>();

    if (applicationLifetime.ApplicationStopping.IsCancellationRequested)
        return;

    var configService = AppHost.Services.GetRequiredService<ConfigService>();
    var logService = AppHost.Services.GetRequiredService<LogService>();
    var commandLineService = AppHost.Services.GetRequiredService<CommandLineService>();
    var tweakService = AppHost.Services.GetRequiredService<TweakService>();
    var wingetService = AppHost.Services.GetRequiredService<WingetService>();
    var chocolateyService = AppHost.Services.GetRequiredService<ChocolateyService>();

    commandLineService.WriteBranding();
    var options = commandLineService.ParseArguments(args);

    logService.CommandLineArguments(args);

    while (!NetworkInterface.GetIsNetworkAvailable() && !applicationLifetime.ApplicationStopping.IsCancellationRequested) {
        logService.NoNetworkError();
        try {
            await Task.Delay(5000, applicationLifetime.ApplicationStopping);
        } catch (OperationCanceledException) {
            return;
        }
    }

    var appInstallerConfig = await configService.GetAppInstallerAsync(applicationLifetime.ApplicationStopping);

    if (!options.NoApps) {
        if (chocolateyService.IsInstallationNeeded() && !await chocolateyService.IsInstalledAsync(applicationLifetime.ApplicationStopping))
            await chocolateyService.InstallAsync(applicationLifetime.ApplicationStopping);

        if (wingetService.IsInstallationNeeded() && !await wingetService.IsInstalledAsync(applicationLifetime.ApplicationStopping))
            await wingetService.InstallAsync(applicationLifetime.ApplicationStopping);

        if (appInstallerConfig.Apps is not null) {
            logService.InstallingApps();

            foreach (var app in appInstallerConfig.Apps)
                await app.InstallAsync(applicationLifetime.ApplicationStopping);
        }
    }

    if (!options.NoTweaks)
        await tweakService.ApplyTweaksAsync(applicationLifetime.ApplicationStopping);
} catch (Exception ex) {
    Console.WriteLine($"FATAL ERROR: {ex.Message}");
    Environment.Exit(1);
} finally {
    await AppHost.StopAsync();
    AppHost.Dispose();
}
