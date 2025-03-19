using System.Diagnostics;

namespace NitroWin {
    public class Tweaks {
        public static void Apply() {
            if (Directory.Exists("Tweaks")) {
                Import();
                Invoke();
            }
            else {
                Download();
                Import();
                Invoke();
            }
        }
        private static void Download() {
            Console.WriteLine("Download comming soon. Please download the Tweaks folder manually.");
        }
        private static void Import() {
            foreach (string file in Directory.EnumerateFiles("Tweaks", "*.reg", SearchOption.AllDirectories)) {
                Console.WriteLine($"Importing {file}...");
                Process.Start("reg.exe", $"import \"{file}\"");
            }
        }
        private static void Invoke() {
            foreach (string file in Directory.EnumerateFiles("Tweaks", "*.bat", SearchOption.AllDirectories)) {
                Console.WriteLine($"Invoking {file}...");
                Process.Start("cmd.exe", $"/c \"{file}\"");
            }
            foreach (string file in Directory.EnumerateFiles("Tweaks", "*.ps1", SearchOption.AllDirectories)) {
                Console.WriteLine($"Invoking {file}...");
                Process.Start("powershell.exe", $"-Command \"Set-ExecutionPolicy -Scope Process -ExecutionPolicy Unrestricted -Force; & '{file}'\"");
            }
        }
    }
}