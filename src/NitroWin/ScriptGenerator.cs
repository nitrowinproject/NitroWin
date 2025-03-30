namespace NitroWin {
    public class ScriptGenerator {
        public async static Task WriteToFile(String filePath) {
            ConfigFile config = await Config.ParseConfig();
            string scriptContent = await ScriptGenerator.GenerateAppInstallScript(config);
            await File.WriteAllTextAsync(filePath, scriptContent);
        }
        public static async Task<string> GenerateAppInstallScript(ConfigFile configFile) {
            var scriptUrls = ScriptUrls();
            var scriptTasks = new List<Task<string>>();
            
            void AddScriptTasks(IEnumerable<string> apps)
            {
                foreach (var app in apps)
                {
                    if (scriptUrls.TryGetValue(app, out var url))
                    {
                        scriptTasks.Add(Helper.DownloadFileToString(url));
                    }
                }
            }
            
            try {
                AddScriptTasks(configFile.Apps.Browser);
            }
            catch {}
            try {
                AddScriptTasks(configFile.Apps.Archiving);
            }
            catch {}
            try {
                AddScriptTasks(configFile.Apps.Multimedia);
            }
            catch {}
            try {
                AddScriptTasks(configFile.Apps.Communication);
            }
            catch {}
            try {
                AddScriptTasks(configFile.Apps.Gaming);
            }
            catch {}
            try {
                AddScriptTasks(configFile.Apps.Tools);
            }
            catch {}
            try {
                AddScriptTasks(configFile.Gpu);
            }
            catch {}

            var scriptContents = await Task.WhenAll(scriptTasks);
            return string.Join(Environment.NewLine, scriptContents);
        }
        private static Dictionary<string, string> ScriptUrls() {
            const string baseUrl = "https://raw.githubusercontent.com/Nitro4542/NitroWin/v2/src/NitroWin.InstallScripts/";

            return new Dictionary<string, string> {
                { "brave", $"{baseUrl}Browsers/Brave.ps1" },
                { "librewolf", $"{baseUrl}Browsers/LibreWolf.ps1" },
                { "firefox", $"{baseUrl}Browsers/Firefox.ps1" },

                { "7zip", $"{baseUrl}Archiving/7-Zip.ps1" },
                { "winrar", $"{baseUrl}Archiving/WinRAR.ps1" },

                { "vlc", $"{baseUrl}Multimedia/VLC.ps1" },
                { "klcp", $"{baseUrl}Multimedia/KLCP.ps1" },

                { "discord", $"{baseUrl}Communication/Discord.ps1" },

                { "steam", $"{baseUrl}Gaming/Steam.ps1" },
                { "epicgames", $"{baseUrl}Gaming/EpicGames.ps1" },

                { "winutil", $"{baseUrl}Tools/WinUtil.ps1" },
                { "oosu", $"{baseUrl}Tools/OOSU.ps1" },
                { "msiafterburner", $"{baseUrl}Tools/MsiAfterburner.ps1" },
                { "unigetui", $"{baseUrl}Tools/UniGetUI.ps1" },
                { "notepadplusplus", $"{baseUrl}Tools/NotepadPlusPlus.ps1" },
                { "startallback", $"{baseUrl}Tools/StartAllBack.ps1" },
                { "keepassxc", $"{baseUrl}Tools/KeePassXC.ps1" },
                { "powershell7", $"{baseUrl}Tools/PowerShell7.ps1" },

                { "amd", $"{baseUrl}GPU/AMD.ps1" },
                { "nvidia", $"{baseUrl}GPU/NVIDIA.ps1" }
            };
        }
    }
}