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
            .WriteTo.File(Path.Join("Logs", "NitroWin.txt"), rollingInterval: RollingInterval.Minute));

        services.AddSingleton<CommandLineService>();

        services.AddNitroWin();
    })
    .Build();

try {
    await AppHost.StartAsync();

    var applicationLifetime = AppHost.Services.GetRequiredService<IHostApplicationLifetime>();

    if (applicationLifetime.ApplicationStopping.IsCancellationRequested)
        return;

    var commandLineService = AppHost.Services.GetRequiredService<CommandLineService>();
    var tweakService = AppHost.Services.GetRequiredService<TweakService>();
    var nitroWinService = AppHost.Services.GetRequiredService<NitroWinService>();

    nitroWinService.WriteBranding(args);

    await nitroWinService.WaitForNetworkAsync(true, applicationLifetime.ApplicationStopping);

    var options = commandLineService.ParseArguments(args);

    if (!options.NoApps)
        await nitroWinService.InstallAppsAsync(applicationLifetime.ApplicationStopping);

    if (!options.NoTweaks)
        await tweakService.ApplyTweaksAsync(applicationLifetime.ApplicationStopping);
} catch (Exception ex) {
    Console.WriteLine($"FATAL ERROR: {ex.Message}");
    Environment.Exit(1);
} finally {
    await AppHost.StopAsync();
    AppHost.Dispose();
}
