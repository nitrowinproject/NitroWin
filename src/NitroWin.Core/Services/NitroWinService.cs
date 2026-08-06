using System.Net.NetworkInformation;
using System.Reflection;
using System.Resources;
using Microsoft.Extensions.Logging;
using NitroWin.Core.Models;

namespace NitroWin.Core.Services;

public sealed class NitroWinService(ChocolateyService chocolateyService, WingetService wingetService, LogService logService, ConfigService configService, ResourceManager resourceManager, ILogger<NitroWinService> logger) {
    private readonly string? _version = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
    private readonly string _name = resourceManager.GetString("App_Name")!;

    private AppInstallerConfig? _appInstallerConfig;

    public async Task InstallAppsAsync(CancellationToken cancellationToken = default) {
        if (chocolateyService.IsInstallationNeeded() && !await chocolateyService.IsInstalledAsync(cancellationToken))
            await chocolateyService.InstallAsync(cancellationToken);

        if (wingetService.IsInstallationNeeded() && !await wingetService.IsInstalledAsync(cancellationToken))
            await wingetService.InstallAsync(cancellationToken);

        _appInstallerConfig ??= await configService.GetAppInstallerAsync(cancellationToken);

        if (_appInstallerConfig.Apps is not null) {
            logService.InstallingApps();

            foreach (var app in _appInstallerConfig.Apps)
                await app.InstallAsync(cancellationToken);
        }
    }

    public async Task WaitForNetworkAsync(bool log, CancellationToken cancellationToken = default) {
        while (!NetworkInterface.GetIsNetworkAvailable() && !cancellationToken.IsCancellationRequested) {
            if (log)
                logService.NoNetworkError();

            try {
                await Task.Delay(5000, cancellationToken);
            } catch (OperationCanceledException) {
                return;
            }
        }
    }

    public void WriteBranding(string[]? args) {
        Console.Title = string.Join(" ", _name, _version);

        if (!logger.IsEnabled(LogLevel.Information)) return;

        string[] branding = [
            "d8b   db d888888b d888888b d8888b.  .d88b.  db   d8b   db d888888b d8b   db",
            "888o  88   `88'   `~~88~~' 88  `8D .8P  Y8. 88   I8I   88   `88'   888o  88",
            "88V8o 88    88       88    88oobY' 88    88 88   I8I   88    88    88V8o 88",
            "88 V8o88    88       88    88`8b   88    88 Y8   I8I   88    88    88 V8o88",
            "88  V888   .88.      88    88 `88. `8b  d8' `8b d8'8b d8'   .88.   88  V888",
            "VP   V8P Y888888P    YP    88   YD  `Y88P'   `8b8' `8d8'  Y888888P VP   V8P",
            resourceManager.GetString("App_Description")!
            ];

        foreach (var line in branding)
            logger.LogInformation("{Line}", line);

        logService.HelloFrom(_name, _version ?? resourceManager.GetString("CommandLine_UnknownVersion")!);

        if (args is not null)
            logService.CommandLineArguments(args);
    }
}
