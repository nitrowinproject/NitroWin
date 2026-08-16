using NitroWin.Core.Helpers;
using NitroWin.Core.Models;
using NitroWin.Core.Models.Apps;

namespace NitroWin.Core.Services;

public sealed class ChocolateyService(ConfigService configService, DownloaderService downloaderService, LogService logService) : PackageManagerServiceBase {
    private sealed class ChocolateyInstallerApp(LogService logService, DownloaderService downloaderService) : WebApp(logService, downloaderService) {
        protected override async Task InstallCoreAsync(CancellationToken cancellationToken) {
            await ProcessHelper.StartProcessAsync(
                "powershell.exe",
                $"-NoProfile -ExecutionPolicy Bypass -Command \"[System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072; iex ((New-Object System.Net.WebClient).DownloadString('{Url}'))\"",
                cancellationToken: cancellationToken
            );
        }
    }

    private Config? _config;
    private AppInstallerConfig? _appInstallerConfig;

    protected override AppBase App => new ChocolateyInstallerApp(logService, downloaderService) {
        Name = "Chocolatey",
        Url = "https://community.chocolatey.org/install.ps1"
    };

    public override async Task<bool> IsInstallationNeededAsync(CancellationToken cancellationToken = default) {
        _config ??= await configService.GetAsync(cancellationToken);
        _appInstallerConfig ??= await configService.GetAppInstallerAsync(cancellationToken);

        if (_config.Options.InstallChocolatey == Options.InstallOptions.Always)
            return true;

        if (_appInstallerConfig.Apps is not null && _config.Options.InstallChocolatey == Options.InstallOptions.IfNeeded) {
            foreach (var app in _appInstallerConfig.Apps) {
                if (app is ChocolateyApp or ChocolateyBundleApp)
                    return true;
            }
        }

        return false;
    }

    public override async Task<bool> IsInstalledAsync(CancellationToken cancellationToken = default) =>
        await ProcessHelper.IsAppAvailable("choco.exe", "--version", cancellationToken);

    public override async Task InstallAppAsync(string id, string[]? args, CancellationToken cancellationToken = default) =>
        await ProcessHelper.StartProcessAsync("choco.exe", $"install {id} --yes {string.Join(" ", args ?? [])}", cancellationToken: cancellationToken);

    public override async Task InstallAppBundleAsync(string fileName, string[]? args, CancellationToken cancellationToken = default) =>
        await ProcessHelper.StartProcessAsync("choco.exe", $"install {Path.Combine(AppContext.BaseDirectory,
            "Configuration", "Bundles", fileName)} --yes {string.Join(" ", args ?? [])}", cancellationToken: cancellationToken);
}
