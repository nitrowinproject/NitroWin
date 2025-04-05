namespace NitroWin {
    public class NitroWin {
        public static async Task Main() {
            Console.Title = "NitroWin";

            Helper.CreateNitroWinDirectory();

            Console.WriteLine("Downloading answer file...");
            await Tweaks.DownloadAnswerFile();

            Console.WriteLine("Downloading tweaks...");
            await Tweaks.DownloadTweaks();
        }
    }
}