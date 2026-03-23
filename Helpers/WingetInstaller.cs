using NitroWin.Apps;
using Serilog;

namespace NitroWin.Helpers
{
    public static class WingetInstaller
    {
        private static string GetArchitectureFolder()
        {
            return Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE") switch
            {
                "ARM64" => "arm64",
                _ => "x64"
            };
        }

        private static async Task InstallWingetDependenciesAsync()
        {
            string depsPath = Path.Join(Globals.DownloadFolder, "DesktopAppInstaller_Dependencies");
            string depsArchitecture = GetArchitectureFolder();

            string depsArchive = await Downloader.FileDownloader.DownloadFileAsync("https://github.com/microsoft/winget-cli/releases/latest/download/DesktopAppInstaller_Dependencies.zip", depsPath) ?? throw new NullReferenceException();
            await ExtractionHelper.ExtractZipFile(depsArchive, depsPath);

            foreach (var app in Directory.GetFiles(Path.Join(depsPath, depsArchitecture))
                .Select(file => new AppxApp { Path = file }))
            {
                await app.InstallAsync();
            }
        }

        public static async Task InstallWingetAsync()
        {
            if (await ProcessHelper.IsAppAvailable("winget.exe", "--version")) { return; }

            Log.Information(Globals.StringsResourceManager.GetString("WinGetInstaller_InstallingDependencies")!);
            await InstallWingetDependenciesAsync();

            var app = new AppxWebApp()
            {
                Url = "https://github.com/microsoft/winget-cli/releases/latest/download/Microsoft.DesktopAppInstaller_8wekyb3d8bbwe.msixbundle"
            };

            await app.InstallAsync();
        }
    }
}
