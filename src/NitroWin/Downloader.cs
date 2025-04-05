namespace NitroWin {
    public class Downloader {
        public static async Task DownloadFiles() {
            await DownloadTweaks();
            await DownloadAnswerFile();
            await DownloadConfigFile();
            await DownloadPsExec();
        }
        private static async Task DownloadTweaks() {
            var files = new (string Name, string Url)[] {
                ("NitroWin.Tweaks.User.reg", "https://raw.githubusercontent.com/Nitro4542/NitroWin.Tweaks/main/NitroWin.Tweaks.User.reg"),
                ("NitroWin.Tweaks.User.ps1", "https://raw.githubusercontent.com/Nitro4542/NitroWin.Tweaks/main/NitroWin.Tweaks.User.ps1"),
                ("NitroWin.Tweaks.System.reg", "https://raw.githubusercontent.com/Nitro4542/NitroWin.Tweaks/main/NitroWin.Tweaks.System.reg"),
                ("NitroWin.Tweaks.System.ps1", "https://raw.githubusercontent.com/Nitro4542/NitroWin.Tweaks/main/NitroWin.Tweaks.System.ps1")
            };

            foreach (var (name, url) in files) {
                await Helper.DownloadFile(url, Helper.NitroWinDirectory, name);
            }
        }
        private static async Task DownloadAnswerFile() {
            const string answerFileName = "autounattend.xml";
            const string answerFileUrl = "https://raw.githubusercontent.com/Nitro4542/NitroWin/main/assets/AnswerFiles/autounattend.xml";

            await Helper.DownloadFile(answerFileUrl, Helper.NitroWinDirectory, answerFileName);
        }
        private static async Task DownloadConfigFile() {
            const string configFileName = "Apps.txt";
            const string configFileUrl = "https://raw.githubusercontent.com/nitrowinproject/NitroWin/main/assets/Configuration/Apps.txt";
            
            await Helper.DownloadFile(configFileUrl, Helper.NitroWinDirectory, configFileName);
        }
        private static async Task DownloadPsExec() {
            const string psexecFileName = "PsExec64.exe";
            const string psexecFileUrl = "https://live.sysinternals.com/PsExec64.exe";

            bool result = Helper.Prompt("Do you want to download PsExec?");
            if (!result) {
                Console.WriteLine("PsExec will not be downloaded. NitroWin will not work without it.");
                Environment.Exit(0);
            }

            await Helper.DownloadFile(psexecFileUrl, Helper.NitroWinDirectory, psexecFileName);
        }
    }
}