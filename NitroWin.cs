using NitroWin.Apps;
using NitroWin.Helpers;
using NitroWin.Tweaks;

namespace NitroWin
{
    public class NitroWin
    {
        public static async Task Main()
        {
            ConsoleHelper.WriteBranding();

            await RunAsTiDownloader.DownloadAsync();

            await TweakLoader.ApplyTweaksAsync();

            await WingetInstaller.InstallWingetAsync();

            await AppInstaller.InstallAppsAsync();
        }
    }
}
