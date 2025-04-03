namespace NitroWin {
    public class NitroWin {
        public static async Task Main() {
            Console.Title = "NitroWin";

            Helper.CreateNitroWinDirectory();

            Console.WriteLine("Downloading answer file...");
            await AnswerFile.WriteToFile();

            Console.WriteLine("Downloading tweaks...");
            await Tweaks.Download();
        }
    }
}