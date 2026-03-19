using NitroWin.Helpers.Downloader;

namespace NitroWin.Helpers
{
    public static class RunAsTiDownloader
    {
        public static async Task Download()
        {
            Console.WriteLine(Globals.StringsResourceManager.GetString("RunAsTiDownloader_Downloading"));
            try
            {
                await FileDownloader.DownloadFileAsync("https://github.com/fafalone/RunAsTrustedInstaller/releases/latest/download/RunAsTI64.exe", "Downloads");
            }
            catch (Exception ex)
            {
                ConsoleHelper.WriteError(Globals.StringsResourceManager.GetString("RunAsTiDownloader_DownloadError") + ex.Message);
            }
        }
    }
}
