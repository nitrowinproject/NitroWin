namespace NitroWin {
    public class AnswerFile {
        private const string AnswerFileName = "autounattend.xml";
        private const string AnswerFileUrl = "https://raw.githubusercontent.com/Nitro4542/NitroWin/main/assets/AnswerFiles/autounattend.xml";
        public static async Task WriteToFile() {
            var answerFilePath = Path.Combine(Helper.NitroWinDirectory, AnswerFileName);

            if (File.Exists(answerFilePath)) {
                File.Delete(answerFilePath);
            }

            await Helper.DownloadFile(AnswerFileUrl, Helper.NitroWinDirectory, AnswerFileName);          
        }
    }
}
