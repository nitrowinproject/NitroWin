using Microsoft.Extensions.Hosting;
using NitroWin.Helpers;
using NitroWin.Models;
using NitroWin.Models.Apps;

namespace NitroWin.Services;

public sealed class ChocolateyService(ConfigService configService, DownloaderService downloaderService, LogService logService) : PackageManagerServiceBase, IHostedService {
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

    internal override AppBase App => new ChocolateyInstallerApp(logService, downloaderService) {
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

    internal override async Task<bool> IsInstalledAsync(CancellationToken cancellationToken = default) =>
        await ProcessHelper.IsAppAvailable("choco.exe", "--version", cancellationToken);

    internal override async Task InstallAppAsync(string id, string[]? args, CancellationToken cancellationToken = default) =>
        await ProcessHelper.StartProcessAsync("choco.exe", $"install {id} --yes {string.Join(" ", args ?? [])}", cancellationToken: cancellationToken);

    public async Task StartAsync(CancellationToken cancellationToken) {
        _config = await configService.GetAsync(cancellationToken);
        _appInstallerConfig = await configService.GetAppInstallerAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
