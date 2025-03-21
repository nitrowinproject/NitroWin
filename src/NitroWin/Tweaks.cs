namespace NitroWin {
    public class Tweaks {
        public static async Task Download() {
            String workingDirectory = AppDomain.CurrentDomain.BaseDirectory;

            var files = new (string Name, string Url)[] {
                ("NitroWin.Tweaks.User.reg", "https://raw.githubusercontent.com/Nitro4542/NitroWin.Tweaks/main/NitroWin.Tweaks.User.reg"),
                ("NitroWin.Tweaks.User.ps1", "https://raw.githubusercontent.com/Nitro4542/NitroWin.Tweaks/main/NitroWin.Tweaks.User.ps1"),
                ("NitroWin.Tweaks.System.reg", "https://raw.githubusercontent.com/Nitro4542/NitroWin.Tweaks/main/NitroWin.Tweaks.System.reg"),
                ("NitroWin.Tweaks.System.ps1", "https://raw.githubusercontent.com/Nitro4542/NitroWin.Tweaks/main/NitroWin.Tweaks.System.ps1")
            };
            
            foreach (var file in files) {
                string filePath = Path.Combine(workingDirectory, file.Name);

                if (File.Exists(filePath)) {
                    File.Delete(filePath);
                }

                await Helper.DownloadFile(file.Url, workingDirectory, file.Name);
            }
        }
    }
}