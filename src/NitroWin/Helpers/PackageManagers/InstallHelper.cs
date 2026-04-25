using NitroWin.Apps;

namespace NitroWin.Helpers.PackageManagers
{
    internal static class InstallHelper
    {
        private static bool IsWingetNeeded()
        {
            if (Globals.Config.Options.InstallWinget == Config.Options.InstallOptions.Always)
            {
                return true;
            }
            else if (Globals.Config.Options.InstallWinget == Config.Options.InstallOptions.IfNeeded && Globals.AppInstallerConfig != null)
            {
                foreach (var app in Globals.AppInstallerConfig.Apps)
                {
                    if (app is WingetApp)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool IsChocolateyNeeded()
        {
            if (Globals.Config.Options.InstallChocolatey == Config.Options.InstallOptions.Always)
            {
                return true;
            }
            else if (Globals.Config.Options.InstallChocolatey == Config.Options.InstallOptions.IfNeeded && Globals.AppInstallerConfig != null)
            {
                foreach (var app in Globals.AppInstallerConfig.Apps)
                {
                    if (app is ChocolateyApp)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        internal static async Task InstallAsync()
        {
            if (IsWingetNeeded())
            {
                await WingetInstaller.InstallWingetAsync();
            }

            if (IsChocolateyNeeded())
            {
                await ChocolateyInstaller.InstallChocolateyAsync();
            }
        }
    }
}
