using NitroWin.Apps;
using NitroWin.Helpers;
using NitroWin.Tweaks;
using Serilog;

namespace NitroWin
{
    public class NitroWin
    {
        public static async Task Main()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(Path.Join("Logs", "NitroWin.txt"), rollingInterval: RollingInterval.Day)
                .CreateLogger();

            ConsoleHelper.WriteBranding();

            await TweakLoader.ApplyTweaksAsync();

            await WingetInstaller.InstallWingetAsync();

            await AppInstaller.InstallAppsAsync();

            await Log.CloseAndFlushAsync();
        }
    }
}
