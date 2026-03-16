using NitroWin.Helpers;

namespace NitroWin
{
    public class NitroWin
    {
        public static async Task Main()
        {
            ConsoleHelper.WriteBranding();

            await WingetInstaller.InstallWinget();

            if (Globals.AppInstallerConfig != null)
            {
                Console.WriteLine(Globals.StringsResourceManager.GetString("NitroWin_InstallingApps"));

                foreach (var app in Globals.AppInstallerConfig.Web)
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

                foreach (var app in Globals.AppInstallerConfig.Winget)
                {
                    try
                    {
                        await AppInstaller.AppInstaller.InstallWingetAppAsync(app);
                    }
                    catch
                    {
                        ConsoleHelper.WriteError(Globals.StringsResourceManager.GetString("AppInstaller_InstallError") + app.Id + Globals.StringsResourceManager.GetString("AppInstaller_ViaWinget"));
                    }
                }
            }
        }
    }
}
