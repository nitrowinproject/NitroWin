namespace NitroWin {
    public class NitroWin {
        public static async Task Main() {
            Console.Title = "NitroWin";
            await Config.Initialize();

            bool unattendResult = Helper.Prompt("Create autounattend.xml?");

            if (unattendResult) {
                await AnswerFile.WriteToFile("autounattend.xml");
            }
        }
    }
}