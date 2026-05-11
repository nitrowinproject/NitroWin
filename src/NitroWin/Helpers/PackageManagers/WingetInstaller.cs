using System.Runtime.InteropServices;
using NitroWin.Apps;

namespace NitroWin.Helpers.PackageManagers;

internal static class WingetInstaller {
    private class WingetInstallerApp : AppxWebApp {
        private static async Task InstallDependenciesAsync() {
            var depsPath = Path.Join(Globals.DownloadFolder, "DesktopAppInstaller_Dependencies");
            var depsArchitecture = RuntimeInformation.ProcessArchitecture switch {
                Architecture.Arm64 => "arm64",
                Architecture.X64 => "x64",
                _ => throw new NotImplementedException()
            };

            var depsArchive = await Downloader.FileDownloader.DownloadFileAsync("https://github.com/microsoft/winget-cli/releases/latest/download/DesktopAppInstaller_Dependencies.zip", depsPath) ?? throw new NullReferenceException();
            await ExtractionHelper.ExtractZipFile(depsArchive, depsPath);

            foreach (var app in Directory.GetFiles(Path.Join(depsPath, depsArchitecture))
                .Select(file => new AppxApp { Path = file })) {
                await app.InstallAsync();
            }
        }

        protected override async Task InstallCoreAsync() {
            await InstallDependenciesAsync();

            await base.InstallCoreAsync();
        }
    }

    internal static async Task InstallWingetAsync() {
        if (await ProcessHelper.IsAppAvailable("winget.exe", "--version")) { return; }

        var wingetInstallerApp = new WingetInstallerApp() {
            Name = "WinGet",
            Url = "https://github.com/microsoft/winget-cli/releases/latest/download/Microsoft.DesktopAppInstaller_8wekyb3d8bbwe.msixbundle"
        };

        await wingetInstallerApp.InstallAsync();
    }
}
