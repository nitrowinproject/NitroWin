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

            await WingetInstaller.InstallWinget();

            await AppInstaller.InstallApps();

            await TweakLoader.ApplyTweaks();
        }
    }
}
