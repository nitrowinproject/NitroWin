using Serilog;
using System.Diagnostics;

namespace NitroWin.Helpers
{
    public static class WingetInstaller
    {
        private static async Task<bool> IsWingetInstalledAsync()
        {
            try
            {
                ProcessStartInfo startInfo = new()
                {
                    FileName = "winget.exe",
                    Arguments = "--version",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };

                using var process = Process.Start(startInfo) ?? throw new NullReferenceException();
                await process.WaitForExitAsync();

                return process.ExitCode == 0;
            }
            catch
            {
                return false;
            }
        }

        private static async Task InstallWingetDependenciesAsync()
        {
            string depsPath = Path.Join(Globals.DownloadFolder, "DesktopAppInstaller_Dependencies");

            try
            {
                string depsArchive = await Downloader.FileDownloader.DownloadFileAsync("https://github.com/microsoft/winget-cli/releases/latest/download/DesktopAppInstaller_Dependencies.zip", depsPath);

                await System.IO.Compression.ZipFile.ExtractToDirectoryAsync(depsArchive, depsPath, overwriteFiles: true);
            }
            catch (Exception ex)
            {
                Log.Error(Globals.StringsResourceManager.GetString("WingetInstaller_DownloadError") + Globals.StringsResourceManager.GetString("WingetInstaller_WingetDependencies") + ": " + ex.Message);
            }

            string depsArchitecture = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE") switch
            {
                "ARM64" => "arm64",
                _ => "x64"
            };

            foreach (string file in Directory.GetFiles(Path.Join(depsPath, depsArchitecture)))
            {
                await InstallAppxPackageAsync(file);
            }
        }

        private static async Task InstallAppxPackageAsync(string path)
        {
            try
            {
                var startInfo = new ProcessStartInfo()
                {
                    FileName = "powershell.exe",
                    Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"Add-AppxPackage -Path '{path}'\"",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };

                using var process = Process.Start(startInfo) ?? throw new NullReferenceException();
                await process.WaitForExitAsync();
            }
            catch (Exception ex)
            {
                Log.Error(Globals.StringsResourceManager.GetString("AppInstaller_InstallError") + Path.GetFileName(path) + ": " + ex.Message);
            }
        }

        public static async Task InstallWingetAsync()
        {
            if (await IsWingetInstalledAsync()) { return; }

            Log.Information(Globals.StringsResourceManager.GetString("AppInstaller_InstallingApp") + Globals.StringsResourceManager.GetString("WingetInstaller_WingetDependencies") + "...");
            await InstallWingetDependenciesAsync();

            Log.Information(Globals.StringsResourceManager.GetString("AppInstaller_InstallingApp") + "WinGet...");
            try
            {
                string winget = await Downloader.FileDownloader.DownloadFileAsync("https://github.com/microsoft/winget-cli/releases/latest/download/Microsoft.DesktopAppInstaller_8wekyb3d8bbwe.msixbundle", Globals.DownloadFolder);

                await InstallAppxPackageAsync(winget);
            }
            catch (Exception ex)
            {
                Log.Error(Globals.StringsResourceManager.GetString("AppInstaller_InstallError") + "WinGet: " + ex.Message);
                return;
            }
        }
    }
}
