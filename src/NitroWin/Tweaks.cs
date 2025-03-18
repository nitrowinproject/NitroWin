namespace NitroWin {
    public class Tweaks {
        public static void Import() {
            if (Directory.Exists("Tweaks")) {
                Merge();
                Invoke();
            }
            else {
                Download();
                Merge();
                Invoke();
            }
        }
        private static void Download() {
            
        }
        private static void Merge() {
            foreach (string file in Directory.EnumerateFiles("Tweaks", "*.reg", SearchOption.AllDirectories)) {
                Console.WriteLine($"Importing {file}...");
            }
        }
        private static void Invoke() {
            foreach (string file in Directory.EnumerateFiles("Tweaks", "*.bat", SearchOption.AllDirectories)) {
                Console.WriteLine($"Invoking {file}...");
            }
            foreach (string file in Directory.EnumerateFiles("Tweaks", "*.ps1", SearchOption.AllDirectories)) {
                Console.WriteLine($"Invoking {file}...");
            }
        }
    }
}