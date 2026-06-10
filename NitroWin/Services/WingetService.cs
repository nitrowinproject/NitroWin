using System.Runtime.InteropServices;
using Microsoft.Extensions.Hosting;
using NitroWin.Helpers;
using NitroWin.Models;
using NitroWin.Models.Apps;

namespace NitroWin.Services;

public sealed class WingetService(ConfigService configService, ExtractionService extractionService, DownloaderService downloaderService, LogService logService) : PackageManagerServiceBase, IHostedService {
    private sealed class WingetInstallerApp(ExtractionService extractionService, DownloaderService downloaderService, LogService logService) : AppxWebApp(logService, downloaderService) {
        private readonly ExtractionService _extractionService = extractionService;
        private readonly DownloaderService _downloaderService = downloaderService;
        private readonly LogService _logService = logService;

        private async Task InstallDependenciesAsync(CancellationToken cancellationToken = default) {
            var depsPath = Path.Join("Downloads", "DesktopAppInstaller_Dependencies");
            var depsArchitecture = RuntimeInformation.ProcessArchitecture switch {
                Architecture.Arm64 => "arm64",
                Architecture.X64 => "x64",
                _ => throw new NotImplementedException()
            };

            var depsArchive = await _downloaderService.DownloadFileAsync("https://github.com/microsoft/winget-cli/releases/latest/download/DesktopAppInstaller_Dependencies.zip", depsPath, cancellationToken: cancellationToken) ?? throw new InvalidOperationException();
            await _extractionService.ExtractZipFile(depsArchive, depsPath, cancellationToken);

            foreach (var app in Directory.GetFiles(Path.Join(depsPath, depsArchitecture))
                .Select(file => new AppxApp(_logService) { Path = file }))
                await app.InstallAsync(cancellationToken);
        }

        protected override async Task InstallCoreAsync(CancellationToken cancellationToken = default) {
            await InstallDependenciesAsync(cancellationToken);

            await base.InstallCoreAsync(cancellationToken);
        }
    }

    private Config? _config;
    private AppInstallerConfig? _appInstallerConfig;

    internal override AppBase App => new WingetInstallerApp(extractionService, downloaderService, logService) {
        Name = "WinGet",
        Url = "https://github.com/microsoft/winget-cli/releases/latest/download/Microsoft.DesktopAppInstaller_8wekyb3d8bbwe.msixbundle"
    };

    internal override bool IsInstallationNeeded() {
        if (_config!.Options.InstallWinget == Options.InstallOptions.Always)
            return true;

        if (_appInstallerConfig!.Apps is not null && _config!.Options.InstallWinget == Options.InstallOptions.IfNeeded) {
            foreach (var app in _appInstallerConfig.Apps) {
                if (app is WingetApp)
                    return true;
            }
        }

        return false;
    }

    internal override async Task<bool> IsInstalledAsync(CancellationToken cancellationToken = default) =>
        await ProcessHelper.IsAppAvailable("winget.exe", "--version", cancellationToken);

    internal override async Task InstallAppAsync(string id, string[]? args, CancellationToken cancellationToken = default) =>
        await ProcessHelper.StartProcessAsync("winget.exe", $"install --id {id} {string.Join(" ", args ?? [])} --accept-package-agreements --accept-source-agreements", cancellationToken: cancellationToken);

    public async Task StartAsync(CancellationToken cancellationToken) {
        _config = await configService.GetAsync(cancellationToken);
        _appInstallerConfig = await configService.GetAppInstallerAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
