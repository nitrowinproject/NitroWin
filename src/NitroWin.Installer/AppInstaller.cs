using System.Diagnostics;

namespace NitroWin.Installer {
    public class AppInstaller {
        public static async Task InstallRuntimes() {
            var files = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE") switch {
                "AMD64" => new (string, string)[] {
                    ("https://aka.ms/vs/17/release/vc_redist.x86.exe", "vc_redist.x86.exe"),
                    ("https://aka.ms/vs/17/release/vc_redist.x64.exe", "vc_redist.x64.exe"),
                },
                _ => [
                    ("https://aka.ms/vs/17/release/vc_redist.arm64.exe", "vc_redist.arm64.exe"),
                ]
            };

            foreach (var (url, filename) in files) {
                try {
                    await InstallApp(url, filename);
                }
                catch (Exception ex) {
                    Console.WriteLine($"Failed to install {filename}: {ex.Message}");
                }
            }
        }
        public static async Task InstallApp(string url, string name) {
            string downloadFolder = Helper.GetDownloadsFolderPath();
            string filePath = Path.Combine(downloadFolder, name);

            await Helper.DownloadFile(url, downloadFolder, name);
            Process.Start(new ProcessStartInfo {
                FileName = filePath,
                UseShellExecute = true,
                Verb = "runas"
            })?.WaitForExit();
        }
    }
}