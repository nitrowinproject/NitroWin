using System.Runtime.InteropServices;
using NitroWin.Helpers;
using NitroWin.Models;
using NitroWin.Models.Apps;

namespace NitroWin.Services;

internal sealed class WingetService : PackageManagerServiceBase {
    private sealed class WingetInstallerApp(ExtractionService extractionService, DownloaderService downloaderService, LogService logService) : AppxWebApp(logService, downloaderService) {
        private readonly ExtractionService _extractionService = extractionService;
        private readonly DownloaderService _downloaderService = downloaderService;
        private readonly LogService _logService = logService;

        private async Task InstallDependenciesAsync() {
            var depsPath = Path.Join("Downloads", "DesktopAppInstaller_Dependencies");
            var depsArchitecture = RuntimeInformation.ProcessArchitecture switch {
                Architecture.Arm64 => "arm64",
                Architecture.X64 => "x64",
                _ => throw new NotImplementedException()
            };

            var depsArchive = await _downloaderService.DownloadFileAsync("https://github.com/microsoft/winget-cli/releases/latest/download/DesktopAppInstaller_Dependencies.zip", depsPath) ?? throw new NullReferenceException();
            await _extractionService.ExtractZipFile(depsArchive, depsPath);

            foreach (var app in Directory.GetFiles(Path.Join(depsPath, depsArchitecture))
                .Select(file => new AppxApp(_logService) { Path = file })) {
                await app.InstallAsync();
            }
        }

        protected override async Task InstallCoreAsync() {
            await InstallDependenciesAsync();

            await base.InstallCoreAsync();
        }
    }

    private Config? _config;
    private AppInstallerConfig? _appInstallerConfig;

    private readonly ExtractionService _extractionService;
    private readonly DownloaderService _downloaderService;
    private readonly LogService _logService;

    public WingetService(ConfigService configService, ExtractionService extractionService, DownloaderService downloaderService, LogService logService) {
        _extractionService = extractionService;
        _downloaderService = downloaderService;
        _logService = logService;

        _ = InitializeAsync(configService);
    }

    private async Task InitializeAsync(ConfigService configService) {
        _config = await configService.GetAsync();
        _appInstallerConfig = await configService.GetAppInstallerAsync();
    }

    internal override AppBase App => new WingetInstallerApp(_extractionService, _downloaderService, _logService) {
        Name = "WinGet",
        Url = "https://github.com/microsoft/winget-cli/releases/latest/download/Microsoft.DesktopAppInstaller_8wekyb3d8bbwe.msixbundle"
    };

    internal override bool IsInstallationNeeded() {
        if (_config!.Options.InstallWinget == Options.InstallOptions.Always) {
            return true;
        } else if (_appInstallerConfig != null && _config!.Options.InstallWinget == Options.InstallOptions.IfNeeded) {
            foreach (var app in _appInstallerConfig!.Apps) {
                if (app is WingetApp)
                    return true;
            }
        }

        return false;
    }

    internal override async Task<bool> IsInstalledAsync() =>
        await ProcessHelper.IsAppAvailable("winget.exe", "--version");

    internal override async Task InstallAppAsync(string id, string[]? args) =>
        await ProcessHelper.StartProcessAsync("winget.exe", $"install --id {id} {string.Join(" ", args ?? [])} --accept-package-agreements --accept-source-agreements");
}
