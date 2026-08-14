using System.Net.NetworkInformation;
using Microsoft.Extensions.Localization;
using NitroWin.Core.Models;

namespace NitroWin.Core.Services;

public sealed class NitroWinService(ChocolateyService chocolateyService, WingetService wingetService, LogService logService, ConfigService configService, IStringLocalizer<NitroWinService> localizer) {
    private AppInstallerConfig? _appInstallerConfig;

    public async Task InstallAppsAsync(CancellationToken cancellationToken = default) {
        if (chocolateyService.IsInstallationNeeded() && !await chocolateyService.IsInstalledAsync(cancellationToken))
            await chocolateyService.InstallAsync(cancellationToken);

        if (wingetService.IsInstallationNeeded() && !await wingetService.IsInstalledAsync(cancellationToken))
            await wingetService.InstallAsync(cancellationToken);

        _appInstallerConfig ??= await configService.GetAppInstallerAsync(cancellationToken)
            ?? throw new InvalidOperationException(localizer["AppInstallerConfigNotInitializedError"]);

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
}
