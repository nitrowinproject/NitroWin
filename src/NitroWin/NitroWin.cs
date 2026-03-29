using NitroWin.Apps;
using NitroWin.Helpers;
using NitroWin.Helpers.CommandLine;
using NitroWin.Helpers.PackageManagers;
using NitroWin.Tweaks;
using Serilog;
using Serilog.Events;

namespace NitroWin
{
    internal class NitroWin
    {
        internal static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
#if DEBUG
                .MinimumLevel.Debug()
#endif
                .WriteTo.Console(outputTemplate: "[{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(Path.Join("Logs", "NitroWin.txt"), rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Debug)
                .CreateLogger();

            CommandLineHelper.WriteBranding();

            var options = CommandLineHelper.ParseArguments(args);

            if (args.Length > 0)
            {
                Log.Debug(string.Format(ResourceHelper.GetString("Log_CommandLineArguments"), string.Join(", ", args)));
            }

            if (!options.NoApps)
            {
                await InstallHelper.InstallAsync();
                await AppInstaller.InstallAppsAsync();
            }

            if (!options.NoTweaks)
            {
                await TweakLoader.ApplyTweaksAsync();
            }

            await Log.CloseAndFlushAsync();
        }
    }
}
