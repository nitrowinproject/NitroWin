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
                ConsoleHelper.WriteError("Error while installing " + app.Id + " via WinGet.");
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

            string downloadFolder = "Downloads";

            var download = await FileDownloader.DownloadFileAsync(app.Url, downloadFolder);

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
                ConsoleHelper.WriteError("Error while installing " + app.Name + ".");
                return;
            }

            await process.WaitForExitAsync();
        }
    }
}
