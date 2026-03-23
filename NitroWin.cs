using NitroWin.Apps;
using NitroWin.Helpers;
using NitroWin.Tweaks;
using Serilog;
using Serilog.Events;

namespace NitroWin
{
    public class NitroWin
    {
        public static async Task Main()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(outputTemplate: "[{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(Path.Join("Logs", "NitroWin.txt"), rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Debug)
                .CreateLogger();

            ConsoleHelper.WriteBranding();

            await TweakLoader.ApplyTweaksAsync();

            await ChocolateyInstaller.InstallChocolateyAsync();

            await WingetInstaller.InstallWingetAsync();

            await AppInstaller.InstallAppsAsync();

            await Log.CloseAndFlushAsync();
        }
    }
}
