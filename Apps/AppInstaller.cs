using NitroWin.Helpers;
using Serilog;

namespace NitroWin.Apps
{
    public static class AppInstaller
    {
        public static async Task InstallAppsAsync()
        {
            if (Globals.AppConfig != null)
            {
                Log.Information(Globals.StringsResourceManager.GetString("AppInstaller_InstallingApps")!);

                foreach (var app in Globals.AppConfig.Apps)
                {
                    await app.InstallAsync();
                }
            }
        }
    }
}
