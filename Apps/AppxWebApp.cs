using NitroWin.Helpers;
using NitroWin.Helpers.Downloader;

namespace NitroWin.Apps
{
    public class AppxWebApp : AppBase
    {
        public string? Name { get; set; }
        public required string Url { get; set; }

        protected async override Task InstallCoreAsync()
        {
            var path = await FileDownloader.DownloadFileAsync(Url, Globals.DownloadFolder);

            await ProcessHelper.StartProcessAsync(
                "powershell.exe", $"-NoProfile -ExecutionPolicy Bypass -Command \"Add-AppxPackage -Path '{path}'\" " + string.Join(" ", Arguments ?? []),
                false
            );
        }
    }
}
