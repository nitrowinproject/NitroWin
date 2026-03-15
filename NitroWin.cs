using NitroWin.Helpers;

namespace NitroWin
{
    public class NitroWin
    {
        public static async Task Main()
        {
            Console.Title = Globals.StringsResourceManager.GetString("NitroWin_ConsoleTitle")!;

            Console.WriteLine(Globals.StringsResourceManager.GetString("NitroWin_InstallingApps"));
            var appInstallerConfig = Globals.AppInstallerConfig;

            foreach (var app in appInstallerConfig.Web)
            {
                try
                {
                    await AppInstaller.AppInstaller.InstallWebAppAsync(app);
                }
                catch
                {
                    ConsoleHelper.WriteError(Globals.StringsResourceManager.GetString("AppInstaller_InstallError") + app.Name + ".");
                }
            }

            foreach (var app in appInstallerConfig.Winget)
            {
                try
                {
                    await AppInstaller.AppInstaller.InstallWingetAppAsync(app);
                }
                catch
                {
                    ConsoleHelper.WriteError(Globals.StringsResourceManager.GetString("AppInstaller_InstallError") + app.Id + Globals.StringsResourceManager.GetString("Strings.AppInstaller_ViaWinget"));
                }
            }
        }
    }
}
