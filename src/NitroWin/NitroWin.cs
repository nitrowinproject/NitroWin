namespace NitroWin {
    public class NitroWin {
        public static void Main() {
            Console.Title = "NitroWin";

            bool installTweaks = Prompt("Install tweaks?");

            if (installTweaks) {
                Tweaks.Apply();
            }
        }
        public static bool Prompt(string message) {
            Console.WriteLine(message);
            Console.WriteLine("[y|N]");

            string? result = Console.ReadLine();

            return result switch {
                "y" => true,
                "n" => false,
                _ => false
            };
        }
    }
}