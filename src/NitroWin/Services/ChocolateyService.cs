using NitroWin.Helpers;
using NitroWin.Models;
using NitroWin.Models.Apps;

namespace NitroWin.Services;

internal sealed class ChocolateyService : PackageManagerServiceBase {
    private sealed class ChocolateyInstallerApp(LogService logService, DownloaderService downloaderService) : WebApp(logService, downloaderService) {
        protected override async Task InstallCoreAsync() {
            await ProcessHelper.StartProcessAsync(
                "powershell.exe",
                $"-NoProfile -ExecutionPolicy Bypass -Command \"[System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072; iex ((New-Object System.Net.WebClient).DownloadString('{Url}'))\""
            );
        }
    }

    private Config? _config;
    private AppInstallerConfig? _appInstallerConfig;

    private readonly DownloaderService _downloaderService;
    private readonly LogService _logService;

    public ChocolateyService(ConfigService configService, DownloaderService downloaderService, LogService logService) {
        _downloaderService = downloaderService;
        _logService = logService;

        _ = InitializeAsync(configService);
    }

    private async Task InitializeAsync(ConfigService configService) {
        _config = await configService.GetAsync();
        _appInstallerConfig = await configService.GetAppInstallerAsync();
    }

    internal override AppBase App => new ChocolateyInstallerApp(_logService, _downloaderService) {
        Name = "Chocolatey",
        Url = "https://community.chocolatey.org/install.ps1"
    };

    internal override bool IsInstallationNeeded() {
        if (_config!.Options.InstallChocolatey == Options.InstallOptions.Always)
            return true;

        if (_appInstallerConfig!.Apps is not null && _config!.Options.InstallChocolatey == Options.InstallOptions.IfNeeded) {
            foreach (var app in _appInstallerConfig.Apps) {
                if (app is ChocolateyApp)
                    return true;
            }
        }

        return false;
    }

    internal override async Task<bool> IsInstalledAsync() =>
        await ProcessHelper.IsAppAvailable("choco.exe", "--version");

    internal override async Task InstallAppAsync(string id, string[]? args) =>
        await ProcessHelper.StartProcessAsync("choco.exe", $"install {id} --yes {string.Join(" ", args ?? [])}");
}
