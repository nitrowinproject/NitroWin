using System.Diagnostics;

namespace NitroWin.Helpers
{
    public static class WingetInstaller
    {
        private static async Task<bool> IsWingetInstalled()
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

        private static async Task InstallWingetDependencies()
        {
            string depsPath = Path.Join("Downloads", "DesktopAppInstaller_Dependencies");

            string depsArchive = await Downloader.FileDownloader.DownloadFileAsync("https://github.com/microsoft/winget-cli/releases/latest/download/DesktopAppInstaller_Dependencies.zip", depsPath);

            await System.IO.Compression.ZipFile.ExtractToDirectoryAsync(depsArchive, depsPath);

            string depsArchitecture = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE") switch
            {
                "ARM64" => "arm64",
                _ => "x64"
            };

            foreach (string file in Directory.GetFiles(Path.Join(depsPath, depsArchitecture)))
            {
                await InstallAppxPackage(file);
            }
        }

        private static async Task InstallAppxPackage(string path)
        {
            try
            {
                var startInfo = new ProcessStartInfo()
                {
                    FileName = "powershell.exe",
                    Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"Add-AppxPackage -Path '{path}'\""
                };

                using var process = Process.Start(startInfo);

                if (process == null) { return; }

                await process.WaitForExitAsync();
            }
            catch (Exception ex)
            {
                ConsoleHelper.WriteError(Globals.StringsResourceManager.GetString("AppInstaller_InstallError") + Path.GetFileName(path) + ": " + ex.Message);
            }
        }

        public static async Task InstallWinget()
        {
            if (await IsWingetInstalled()) { return; }

            Console.WriteLine(Globals.StringsResourceManager.GetString("WingetInstaller_InstallingWinGetDependencies"));
            await InstallWingetDependencies();

            Console.WriteLine(Globals.StringsResourceManager.GetString("WingetInstaller_InstallingWinGet"));
            string winget = await Downloader.FileDownloader.DownloadFileAsync("https://github.com/microsoft/winget-cli/releases/latest/download/Microsoft.DesktopAppInstaller_8wekyb3d8bbwe.msixbundle", "Downloads");
            await InstallAppxPackage(winget);
        }
    }
}
