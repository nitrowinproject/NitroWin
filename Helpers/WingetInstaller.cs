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
                    Arguments = "--version"
                };

                using var process = Process.Start(startInfo);

                if (process == null)
                {
                    return false;
                }

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

            string depsArchive = await Downloader.FileDownloader.DownloadFileAsync("https://github.com/microsoft/winget-cli/releases/latest/download/DesktopAppInstaller_Dependencies.zip", depsPath);

            await System.IO.Compression.ZipFile.ExtractToDirectoryAsync(depsArchive, depsPath);

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
                    Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"Add-AppxPackage -Path '{path}'\""
                };

                using var process = Process.Start(startInfo);

                if (process == null)
                {
                    Log.Error(Globals.StringsResourceManager.GetString("AppInstaller_InstallError") + Path.GetFileName(path) + ".");
                    return;
                }

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

            Log.Information(Globals.StringsResourceManager.GetString("WingetInstaller_InstallingWinGetDependencies")!);
            await InstallWingetDependenciesAsync();

            Log.Information(Globals.StringsResourceManager.GetString("WingetInstaller_InstallingWinGet")!);
            string winget = await Downloader.FileDownloader.DownloadFileAsync("https://github.com/microsoft/winget-cli/releases/latest/download/Microsoft.DesktopAppInstaller_8wekyb3d8bbwe.msixbundle", Globals.DownloadFolder);
            await InstallAppxPackageAsync(winget);
        }
    }
}
