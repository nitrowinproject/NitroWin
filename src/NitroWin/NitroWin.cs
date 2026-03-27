using NitroWin.Apps;
using NitroWin.Helpers;
using NitroWin.Helpers.PackageManagers;
using NitroWin.Tweaks;
using Serilog;
using Serilog.Events;

namespace NitroWin
{
    internal class NitroWin
    {
        internal static async Task Main()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(outputTemplate: "[{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(Path.Join("Logs", "NitroWin.txt"), rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Debug)
                .CreateLogger();

            ConsoleHelper.WriteBranding();

            await InstallHelper.InstallAsync();

            await AppInstaller.InstallAppsAsync();

            await TweakLoader.ApplyTweaksAsync();

            await Log.CloseAndFlushAsync();
        }
    }
}
