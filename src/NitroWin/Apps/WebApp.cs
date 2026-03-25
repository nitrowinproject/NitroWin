using NitroWin.Helpers;
using NitroWin.Helpers.Downloader;

namespace NitroWin.Apps
{
    public class WebApp : AppBase
    {
        public string? Name { get; set; }
        public required string Url { get; set; }

        protected async override Task InstallCoreAsync()
        {
            var download = await FileDownloader.DownloadFileAsync(Url, Globals.DownloadFolder) ?? throw new NullReferenceException();

            await ProcessHelper.StartProcessAsync(download, string.Join(" ", Arguments ?? []));
        }
    }
}
