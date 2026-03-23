using NitroWin.Apps;

namespace NitroWin.Helpers
{
    public static class ChocolateyInstaller
    {
        public class ChocolateyInstallerApp : AppBase
        {
            protected override async Task InstallCoreAsync()
            {
                await ProcessHelper.StartProcessAsync(
                    "powershell.exe",
                    $"-NoProfile -ExecutionPolicy Bypass -Command \"[System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072; iex ((New-Object System.Net.WebClient).DownloadString('https://community.chocolatey.org/install.ps1'))\""
                );
            }
        }

        public static async Task InstallChocolateyAsync()
        {
            if (await ProcessHelper.IsAppAvailable("choco.exe", "--version")) { return; }

            var chocolateyInstallerApp = new ChocolateyInstallerApp();
            await chocolateyInstallerApp.InstallAsync();
        }
    }
}
