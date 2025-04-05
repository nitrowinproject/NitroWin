namespace NitroWin.Installer {
    public class Installer {
        public static async Task Main() {
            Console.Title = "NitroWin Installer";

            bool result = Helper.Prompt("This will heavily modify your system. Continue?");

            if (!result) {
                Environment.Exit(0);
            }

            Console.WriteLine("Applying tweaks...");
            Tweaks.Apply();

            Helper.NetworkConnectionPrompt();
            
            Console.WriteLine("Installing runtimes...");
            await AppInstaller.InstallRuntimes();
        }
    }
}