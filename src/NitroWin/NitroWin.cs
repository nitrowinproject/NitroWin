namespace NitroWin {
    public class NitroWin {
        public static async Task Main() {
            Console.Title = "NitroWin";

            bool unattendResult = Helper.Prompt("Create autounattend.xml?");

            if (unattendResult) {
                await AnswerFile.WriteToFile("autounattend.xml");
            }
        }
    }
}