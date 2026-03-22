using NitroWin.Helpers;
using NitroWin.Helpers.Downloader;
using Serilog;
using System.Runtime.InteropServices;

namespace NitroWin.Apps
{
    public static class AppInstaller
    {
        private static async Task InstallWingetAppAsync(WingetApp app)
        {
            Log.Information(Globals.StringsResourceManager.GetString("AppInstaller_InstallingApp") + app.Id + Globals.StringsResourceManager.GetString("AppInstaller_ViaWinget") + "...");

            await ProcessHelper.StartProcessAsync("winget.exe", $"install --id {app.Id} --exact --accept-package-agreements --accept-source-agreements {string.Join(" ", app.Arguments ?? [])}");
        }

        private static async Task InstallWebAppAsync(WebApp app)
        {
            if (!(RuntimeInformation.ProcessArchitecture == Architecture.X64 && app.Architectures.X64 || RuntimeInformation.ProcessArchitecture == Architecture.Arm64 && app.Architectures.Arm64))
            {
                Log.Debug(Globals.StringsResourceManager.GetString("AppInstaller_NotInstallingApp") + app.Name + Globals.StringsResourceManager.GetString("AppInstaller_UnsupportedArchitecture"));
                return;
            }

            Log.Information(Globals.StringsResourceManager.GetString("AppInstaller_InstallingApp") + app.Name + "...");

            var download = await FileDownloader.DownloadFileAsync(app.Url, Globals.DownloadFolder);

            await ProcessHelper.StartProcessAsync(download, string.Join(" ", app.Arguments ?? []));
        }

        public static async Task InstallAppsAsync()
        {
            if (Globals.AppInstallerConfig != null)
            {
                Log.Information(Globals.StringsResourceManager.GetString("AppInstaller_InstallingApps")!);

                foreach (var app in Globals.AppInstallerConfig.Web)
                {
                    try
                    {
                        await InstallWebAppAsync(app);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(Globals.StringsResourceManager.GetString("AppInstaller_InstallError") + app.Name + ": " + ex.Message);
                    }
                }

                foreach (var app in Globals.AppInstallerConfig.Winget)
                {
                    try
                    {
                        await InstallWingetAppAsync(app);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(Globals.StringsResourceManager.GetString("AppInstaller_InstallError") + app.Id + Globals.StringsResourceManager.GetString("AppInstaller_ViaWinget") + ": " + ex.Message);
                    }
                }
            }
        }
    }
}
