using NitroWin.Helpers;
using NitroWin.Helpers.Downloader;

namespace NitroWin.Tweaks
{
    public static class TweakLoader
    {
        private static async Task DownloadTweaks()
        {
            string[] urls = [
                "https://raw.githubusercontent.com/nitrowinproject/Tweaks/v3/Generated/NitroWin.Tweaks.User.reg",
                "https://raw.githubusercontent.com/nitrowinproject/Tweaks/v3/Generated/NitroWin.Tweaks.User.ps1",
                "https://raw.githubusercontent.com/nitrowinproject/Tweaks/v3/Generated/NitroWin.Tweaks.System.reg",
                "https://raw.githubusercontent.com/nitrowinproject/Tweaks/v3/Generated/NitroWin.Tweaks.System.ps1"
            ];

            List<Task> downloadTasks = [];

            foreach (string url in urls)
            {
                downloadTasks.Add(FileDownloader.DownloadFileAsync(url, "Tweaks"));
            }

            await Task.WhenAll(downloadTasks);
        }
        public static async Task ApplyTweaks()
        {
            try
            {
                await DownloadTweaks();
            }
            catch (Exception ex)
            {
                ConsoleHelper.WriteError(Globals.StringsResourceManager.GetString("TweakLoader_DownloadError") + ex.Message);
            }
        }
    }
}
