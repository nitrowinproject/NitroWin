namespace NitroWin {
    public class AnswerFile {
        private const string AnswerFileName = "autounattend.xml";
        private const string AnswerFileUrl = "https://raw.githubusercontent.com/Nitro4542/NitroWin/v2/assets/AnswerFiles/autounattend.xml";
        public static async Task WriteToFile() {
            var tweaks = await GetTweaks();

            if (!File.Exists(AnswerFileName)) {
                await Helper.DownloadFile(AnswerFileUrl, Helper.WorkingDirectory, AnswerFileName);
            }

            ConfigFile config = await Config.ParseConfig();
            string scriptContent = await ScriptGenerator.GenerateAppInstallScript(config);
            Helper.InsertIntoFile(AnswerFileName, scriptContent, "#NITROWINPOWERSHELLFIRSTUSER");

            Helper.InsertIntoFile(AnswerFileName, tweaks["NitroWin.Tweaks.User.reg"], ";NITROWINREGISTRYFIRSTUSER");
            Helper.InsertIntoFile(AnswerFileName, tweaks["NitroWin.Tweaks.User.reg"], ";NITROWINREGISTRYOTHERUSER");

            Helper.InsertIntoFile(AnswerFileName, tweaks["NitroWin.Tweaks.User.ps1"], "#NITROWINPOWERSHELLFIRSTUSER");
            Helper.InsertIntoFile(AnswerFileName, tweaks["NitroWin.Tweaks.User.ps1"], "#NITROWINPOWERSHELLOTHERUSER");

            Helper.InsertIntoFile(AnswerFileName, tweaks["NitroWin.Tweaks.System.reg"], ";NITROWINREGISTRYSYSTEM");
            Helper.InsertIntoFile(AnswerFileName, tweaks["NitroWin.Tweaks.System.ps1"], "#NITROWINPOWERSHELLSYSTEM");
        }
        public static async Task<Dictionary<string, string>> GetTweaks() {
            var tweakDictionary = await Tweaks.DownloadToDict();
            var formattedDictionary = new Dictionary<string, string>();

            var files = new (string Name, string Content)[] {
                ("NitroWin.Tweaks.User.reg", FormatRegistryFile(tweakDictionary["NitroWin.Tweaks.User.reg"])),
                ("NitroWin.Tweaks.User.ps1", tweakDictionary["NitroWin.Tweaks.User.ps1"]),
                ("NitroWin.Tweaks.System.reg", FormatRegistryFile(tweakDictionary["NitroWin.Tweaks.System.reg"])),
                ("NitroWin.Tweaks.System.ps1", tweakDictionary["NitroWin.Tweaks.System.ps1"])
            };

            foreach (var file in files) {
                formattedDictionary.Add(file.Name, file.Content);
            }

            return formattedDictionary;
        }
        private static string FormatRegistryFile(string fileContent) {
            int firstLineBreak = fileContent.IndexOf('\n');
        
            if (firstLineBreak == -1)
            {
                return fileContent;
            }

            return fileContent.Substring(firstLineBreak + 1);
        }
    }
}