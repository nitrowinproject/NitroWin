namespace NitroWin {
    public class NitroWin {
        public static async Task Main() {
            Console.Title = "NitroWin";
            await Config.Initialize();

            Console.WriteLine("Creating answer file...");
            await AnswerFile.WriteToFile();
        }
    }
}