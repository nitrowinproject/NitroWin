namespace NitroWin {
    public class Config {
        public static async Task Initialize() {
            if (!File.Exists("config.yml")) {
                String configUrl = "https://raw.githubusercontent.com/Nitro4542/NitroWin/v2/src/NitroWin/config.yml";
                String workingDirectory = AppDomain.CurrentDomain.BaseDirectory;
                await Helper.DownloadFile(configUrl, workingDirectory, "config.yml");

                Console.WriteLine("Please configure your config file and re-run this executable again.");
                Environment.Exit(0);
            }
        }
    }
}