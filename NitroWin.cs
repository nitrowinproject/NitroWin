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
                .WriteTo.Console(outputTemplate: "[{Level:u4}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(Path.Join("Logs", "NitroWin.txt"), rollingInterval: RollingInterval.Day, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u4}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            ConsoleHelper.WriteBranding();

            await TweakLoader.ApplyTweaksAsync();

            await WingetInstaller.InstallWingetAsync();

            await AppInstaller.InstallAppsAsync();

            await Log.CloseAndFlushAsync();
        }
    }
}
