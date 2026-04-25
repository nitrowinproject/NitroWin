using NitroWin.Apps;

namespace NitroWin.Helpers.PackageManagers
{
    internal static class ChocolateyInstaller
    {
        private class ChocolateyInstallerApp : WebApp
        {
            protected override async Task InstallCoreAsync()
            {
                await ProcessHelper.StartProcessAsync(
                    "powershell.exe",
                    $"-NoProfile -ExecutionPolicy Bypass -Command \"[System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072; iex ((New-Object System.Net.WebClient).DownloadString('{Url}'))\""
                );
            }
        }

        internal static async Task InstallChocolateyAsync()
        {
            if (await ProcessHelper.IsAppAvailable("choco.exe", "--version")) { return; }

            var chocolateyInstallerApp = new ChocolateyInstallerApp()
            {
                Name = "Chocolatey",
                Url = "https://community.chocolatey.org/install.ps1"
            };

            await chocolateyInstallerApp.InstallAsync();
        }
    }
}
