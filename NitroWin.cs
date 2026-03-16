using NitroWin.Apps;
using NitroWin.Helpers;

namespace NitroWin
{
    public class NitroWin
    {
        public static async Task Main()
        {
            ConsoleHelper.WriteBranding();

            await WingetInstaller.InstallWinget();

            await AppInstaller.InstallApps();
        }
    }
}
