using System.Net.NetworkInformation;
using System.Reflection;
using Microsoft.Extensions.Localization;
using NitroWin.Core.Models;

namespace NitroWin.Core.Services;

public sealed class NitroWinService(ChocolateyService chocolateyService, WingetService wingetService, LogService logService, ConfigService configService, IStringLocalizer<NitroWinService> localizer) {
    private readonly string? _version = Assembly.GetExecutingAssembly().GetName().Version?.ToString();

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
        Console.Title = string.Join(" ", localizer["AppName"], _version);

#if DEBUG
        logService.HelloFrom(localizer["AppName"], _version ?? localizer["UnknownVersion"]);

        if (args is not null)
            logService.CommandLineArguments(args);
#endif
    }
}
