namespace NitroWin {
    public class NitroWin {
        public static async Task Main() {
            Console.Title = "NitroWin";

            Console.WriteLine("Downloading answer file...");
            await Downloader.DownloadAnswerFile();

            Console.WriteLine("Downloading tweaks...");
            await Downloader.DownloadTweaks();

            Console.WriteLine("Deploying to installation media...");
            InstallMedia.Deploy();

            Console.WriteLine("Finished. Press any key to exit.");
            Console.ReadKey();
        }
    }
}