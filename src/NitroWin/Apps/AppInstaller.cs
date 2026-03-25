using NitroWin.Helpers;
using Serilog;

namespace NitroWin.Apps
{
    internal static class AppInstaller
    {
        internal static async Task InstallAppsAsync()
        {
            if (Globals.AppInstallerConfig != null)
            {
                Log.Information(ResourceHelper.GetString("AppInstaller_InstallingApps"));

                foreach (var app in Globals.AppInstallerConfig.Apps)
                {
                    await app.InstallAsync();
                }
            }
        }
    }
}
