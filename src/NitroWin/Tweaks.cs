namespace NitroWin {
    public class Tweaks {
        public static async Task DownloadTweaks() {
            var files = new (string Name, string Url)[] {
                ("NitroWin.Tweaks.User.reg", "https://raw.githubusercontent.com/Nitro4542/NitroWin.Tweaks/main/NitroWin.Tweaks.User.reg"),
                ("NitroWin.Tweaks.User.ps1", "https://raw.githubusercontent.com/Nitro4542/NitroWin.Tweaks/main/NitroWin.Tweaks.User.ps1"),
                ("NitroWin.Tweaks.System.reg", "https://raw.githubusercontent.com/Nitro4542/NitroWin.Tweaks/main/NitroWin.Tweaks.System.reg"),
                ("NitroWin.Tweaks.System.ps1", "https://raw.githubusercontent.com/Nitro4542/NitroWin.Tweaks/main/NitroWin.Tweaks.System.ps1")
            };

            foreach (var (name, url) in files) {
                string filePath = Path.Combine(Helper.NitroWinDirectory, name);

                if (File.Exists(filePath)) {
                    File.Delete(filePath);
                }

                await Helper.DownloadFile(url, Helper.NitroWinDirectory, name);
            }
        }
        public static async Task DownloadAnswerFile() {
            const string answerFileName = "autounattend.xml";
            const string answerFileUrl = "https://raw.githubusercontent.com/Nitro4542/NitroWin/main/assets/AnswerFiles/autounattend.xml";
            string answerFilePath = Path.Combine(Helper.NitroWinDirectory, answerFileName);

            if (File.Exists(answerFilePath)) {
                File.Delete(answerFilePath);
            }

            await Helper.DownloadFile(answerFileUrl, Helper.NitroWinDirectory, answerFileName);
        }
    }
}