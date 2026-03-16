using NitroWin.Helpers;
using NitroWin.Helpers.Downloader;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace NitroWin.Apps
{
    public static class AppInstaller
    {
        private static async Task InstallWingetAppAsync(WingetApp app)
        {
            var startInfo = new ProcessStartInfo()
            {
                FileName = "winget.exe",
                Arguments = $"install --id {app.Id} --exact --accept-package-agreements --accept-source-agreements {string.Join(" ", app.Arguments ?? [])}",
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            using var process = Process.Start(startInfo);

            if (process == null)
            {
                ConsoleHelper.WriteError(Globals.StringsResourceManager.GetString("AppInstaller_InstallError") + app.Id + Globals.StringsResourceManager.GetString("AppInstaller_ViaWinget"));
                return;
            }

            await process.WaitForExitAsync();
        }

        private static async Task InstallWebAppAsync(WebApp app)
        {
            if (!(RuntimeInformation.ProcessArchitecture == Architecture.X64 && app.Architectures.X64 || RuntimeInformation.ProcessArchitecture == Architecture.Arm64 && app.Architectures.Arm64))
            {
                return;
            }

            var download = await FileDownloader.DownloadFileAsync(app.Url, Globals.DownloadFolder);

            var startInfo = new ProcessStartInfo()
            {
                FileName = download,
                Arguments = string.Join(" ", app.Arguments ?? []),
                UseShellExecute = true,
                Verb = "RunAs"
            };

            using var process = Process.Start(startInfo);

            if (process == null)
            {
                ConsoleHelper.WriteError(Globals.StringsResourceManager.GetString("AppInstaller_InstallError") + app.Name + ".");
                return;
            }

            await process.WaitForExitAsync();
        }

        public static async Task InstallApps()
        {
            if (Globals.AppInstallerConfig != null)
            {
                Console.WriteLine(Globals.StringsResourceManager.GetString("AppInstaller_InstallingApps"));

                foreach (var app in Globals.AppInstallerConfig.Web)
                {
                    try
                    {
                        await InstallWebAppAsync(app);
                    }
                    catch
                    {
                        ConsoleHelper.WriteError(Globals.StringsResourceManager.GetString("AppInstaller_InstallError") + app.Name + ".");
                    }
                }

                foreach (var app in Globals.AppInstallerConfig.Winget)
                {
                    try
                    {
                        await InstallWingetAppAsync(app);
                    }
                    catch
                    {
                        ConsoleHelper.WriteError(Globals.StringsResourceManager.GetString("AppInstaller_InstallError") + app.Id + Globals.StringsResourceManager.GetString("AppInstaller_ViaWinget"));
                    }
                }
            }
        }
    }
}
