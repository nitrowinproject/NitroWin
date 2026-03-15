using NitroWin.Helpers;
using NitroWin.Helpers.Downloader;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace NitroWin.AppInstaller
{
    public static class AppInstaller
    {
        public static async Task InstallWingetAppAsync(WingetApp app)
        {
            var startInfo = new ProcessStartInfo()
            {
                FileName = "winget.exe",
                Arguments = $"install --id {app.Id} --exact --accept-package-agreements --accept-source-agreements {string.Join(" ", app.Arguments ?? [])}",
                Verb = "RunAs"
            };

            var process = Process.Start(startInfo);

            if (process == null)
            {
                ConsoleHelper.WriteError(Globals.StringsResourceManager.GetString("AppInstaller_InstallError") + app.Id + Globals.StringsResourceManager.GetString("AppInstaller_ViaWinget"));
                return;
            }

            await process.WaitForExitAsync();
        }

        public static async Task InstallWebAppAsync(WebApp app)
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

            var process = Process.Start(startInfo);

            if (process == null)
            {
                ConsoleHelper.WriteError(Globals.StringsResourceManager.GetString("AppInstaller_InstallError") + app.Name + ".");
                return;
            }

            await process.WaitForExitAsync();
        }
    }
}
