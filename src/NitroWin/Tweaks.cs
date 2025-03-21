namespace NitroWin {
    public class Tweaks {
        public static async Task Download() {
            string registryUserUrl = "https://raw.githubusercontent.com/Nitro4542/NitroWin.Tweaks/main/NitroWin.Tweaks.User.reg";
            string powershellUserUrl = "https://raw.githubusercontent.com/Nitro4542/NitroWin.Tweaks/main/NitroWin.Tweaks.User.ps1";
            
            string registrySystemUrl = "https://raw.githubusercontent.com/Nitro4542/NitroWin.Tweaks/main/NitroWin.Tweaks.System.reg";
            string powershellSystemUrl = "https://raw.githubusercontent.com/Nitro4542/NitroWin.Tweaks/main/NitroWin.Tweaks.System.ps1";

            String workingDirectory = AppDomain.CurrentDomain.BaseDirectory;

            if (File.Exists("NitroWin.Tweaks.User.reg")) {
                File.Delete("NitroWin.Tweaks.User.reg");
            }

            if (File.Exists("NitroWin.Tweaks.User.ps1")) {
                File.Delete("NitroWin.Tweaks.User.ps1");
            }

            if (File.Exists("NitroWin.Tweaks.System.reg")) {
                File.Delete("NitroWin.Tweaks.System.reg");
            }

            if (File.Exists("NitroWin.Tweaks.System.ps1")) {
                File.Delete("NitroWin.Tweaks.System.ps1");
            }

            await Task.WhenAll(
                Helper.DownloadFile(registryUserUrl, workingDirectory, "NitroWin.Tweaks.User.reg"),
                Helper.DownloadFile(powershellUserUrl, workingDirectory, "NitroWin.Tweaks.User.ps1"),
                Helper.DownloadFile(registrySystemUrl, workingDirectory, "NitroWin.Tweaks.System.reg"),
                Helper.DownloadFile(powershellSystemUrl, workingDirectory, "NitroWin.Tweaks.System.ps1")
            );
        }
    }
}