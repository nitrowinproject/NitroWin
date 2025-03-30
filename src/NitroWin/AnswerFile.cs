namespace NitroWin {
    public class AnswerFile {
        private const string AnswerFileName = "autounattend.xml";
        private const string AnswerFileUrl = "https://raw.githubusercontent.com/Nitro4542/NitroWin/v2/assets/AnswerFiles/autounattend.xml";
        public static async Task WriteToFile() {
            var tweaks = await GetTweaks();

            if (File.Exists(AnswerFileName)) {
                File.Delete(AnswerFileName);
            }

            await Helper.DownloadFile(AnswerFileUrl, Helper.WorkingDirectory, AnswerFileName);

            ConfigFile config = await Config.ParseConfig();
            string scriptContent = await ScriptGenerator.GenerateAppInstallScript(config);
            Helper.InsertIntoFile(AnswerFileName, scriptContent, "#NITROWINPOWERSHELLFIRSTUSER");

            var tweakFiles = new(string content, string insertPoint)[] {
                (tweaks["NitroWin.Tweaks.User.reg"], ";NITROWINREGISTRYFIRSTUSER"),
                (tweaks["NitroWin.Tweaks.User.reg"], ";NITROWINREGISTRYOTHERUSER"),
                (tweaks["NitroWin.Tweaks.User.ps1"], "#NITROWINPOWERSHELLFIRSTUSER"),
                (tweaks["NitroWin.Tweaks.User.ps1"], "#NITROWINPOWERSHELLOTHERUSER"),
                (tweaks["NitroWin.Tweaks.System.reg"], ";NITROWINREGISTRYSYSTEM"),
                (tweaks["NitroWin.Tweaks.System.ps1"], "#NITROWINPOWERSHELLSYSTEM")
            };

            foreach (var (content, marker) in tweakFiles) {
                Helper.InsertIntoFile(AnswerFileName, content, marker);
            }
        }
        public static async Task<Dictionary<string, string>> GetTweaks() {
            var tweakDictionary = await Tweaks.DownloadToDict();

            var files = new (string Name, string Content)[] {
                ("NitroWin.Tweaks.User.reg", FormatRegistryFile(tweakDictionary["NitroWin.Tweaks.User.reg"])),
                ("NitroWin.Tweaks.User.ps1", tweakDictionary["NitroWin.Tweaks.User.ps1"]),
                ("NitroWin.Tweaks.System.reg", FormatRegistryFile(tweakDictionary["NitroWin.Tweaks.System.reg"])),
                ("NitroWin.Tweaks.System.ps1", tweakDictionary["NitroWin.Tweaks.System.ps1"])
            };

            return files.ToDictionary(file => file.Name, file => file.Content);
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