using NitroWin.Helpers;
using Serilog;

namespace NitroWin.Apps
{
    internal static class AppInstaller
    {
        internal static async Task InstallAppsAsync()
        {
            if (Globals.AppConfig != null)
            {
                Log.Information(ResourceHelper.GetString("AppInstaller_InstallingApps"));

                foreach (var app in Globals.AppConfig.Apps)
                {
                    await app.InstallAsync();
                }
            }
        }
    }
}
