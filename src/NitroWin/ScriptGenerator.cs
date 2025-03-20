namespace NitroWin {
    public class ScriptGenerator {
        public static async Task<string> GenerateAppInstallScript(ConfigFile configFile) {
            List<string> appInstallScriptContent = new List<string>();
            Dictionary<string,string> scriptUrls = ScriptUrls();

            if (configFile.Apps.Browser.Contains("brave")) {
                appInstallScriptContent.Add(await Helper.DownloadFileToString(scriptUrls["brave"]));
            }
            if (configFile.Apps.Browser.Contains("librewolf")) {
                appInstallScriptContent.Add(await Helper.DownloadFileToString(scriptUrls["librewolf"]));
            }
            if (configFile.Apps.Browser.Contains("firefox")) {
                appInstallScriptContent.Add(await Helper.DownloadFileToString(scriptUrls["firefox"]));
            }

            if (configFile.Apps.Archiving.Contains("7zip")) {
                appInstallScriptContent.Add(await Helper.DownloadFileToString(scriptUrls["7zip"]));
            }
            if (configFile.Apps.Archiving.Contains("winrar")) {
                appInstallScriptContent.Add(await Helper.DownloadFileToString(scriptUrls["winrar"]));
            }

            if (configFile.Apps.Multimedia.Contains("vlc")) {
                appInstallScriptContent.Add(await Helper.DownloadFileToString(scriptUrls["vlc"]));
            }
            if (configFile.Apps.Multimedia.Contains("klcp")) {
                appInstallScriptContent.Add(await Helper.DownloadFileToString(scriptUrls["klcp"]));
            }

            if (configFile.Apps.Communication.Contains("discord")) {
                appInstallScriptContent.Add(await Helper.DownloadFileToString(scriptUrls["discord"]));
            }

            if (configFile.Apps.Gaming.Contains("steam")) {
                appInstallScriptContent.Add(await Helper.DownloadFileToString(scriptUrls["steam"]));
            }
            if (configFile.Apps.Gaming.Contains("epicgames")) {
                appInstallScriptContent.Add(await Helper.DownloadFileToString(scriptUrls["epicgames"]));
            }

            if (configFile.Apps.Tools.Contains("winutil")) {
                appInstallScriptContent.Add(await Helper.DownloadFileToString(scriptUrls["winutil"]));
            }
            if (configFile.Apps.Tools.Contains("oosu")) {
                appInstallScriptContent.Add(await Helper.DownloadFileToString(scriptUrls["oosu"]));
            }
            if (configFile.Apps.Tools.Contains("msiafterburner")) {
                appInstallScriptContent.Add(await Helper.DownloadFileToString(scriptUrls["msiafterburner"]));
            }
            if (configFile.Apps.Tools.Contains("unigetui")) {
                appInstallScriptContent.Add(await Helper.DownloadFileToString(scriptUrls["unigetui"]));
            }
            if (configFile.Apps.Tools.Contains("notepadplusplus")) {
                appInstallScriptContent.Add(await Helper.DownloadFileToString(scriptUrls["notepadplusplus"]));
            }
            if (configFile.Apps.Tools.Contains("startallback")) {
                appInstallScriptContent.Add(await Helper.DownloadFileToString(scriptUrls["startallback"]));
            }
            if (configFile.Apps.Tools.Contains("keepassxc")) {
                appInstallScriptContent.Add(await Helper.DownloadFileToString(scriptUrls["keepassxc"]));
            }
            if (configFile.Apps.Tools.Contains("powershell7")) {
                appInstallScriptContent.Add(await Helper.DownloadFileToString(scriptUrls["powershell7"]));
            }

            if (configFile.Gpu.Contains("amd")) {
                appInstallScriptContent.Add(await Helper.DownloadFileToString(scriptUrls["amd"]));
            }
            if (configFile.Gpu.Contains("nvidia")) {
                appInstallScriptContent.Add(await Helper.DownloadFileToString(scriptUrls["nvidia"]));
            }
            return appInstallScriptContent.ToString();
        }
        private static Dictionary<string, string> ScriptUrls() {
            Dictionary<string, string> urls = new Dictionary<string, string>();

            String baseUrl = "https://raw.githubusercontent.com/Nitro4542/NitroWin/v2/src/NitroWin.InstallScripts/";

            urls.Add("brave", baseUrl + "Browsers/Brave.ps1");
            urls.Add("librewolf", baseUrl + "Browsers/LibreWolf.ps1");
            urls.Add("firefox", baseUrl + "Browsers/Firefox.ps1");

            urls.Add("7zip", baseUrl + "Archiving/7-Zip.ps1");
            urls.Add("winrar", baseUrl + "Archiving/WinRAR.ps1");

            urls.Add("vlc", baseUrl + "Multimedia/VLC.ps1");
            urls.Add("klcp", baseUrl + "Multimedia/KLCP.ps1");

            urls.Add("discord", baseUrl + "Communication/Discord.ps1");

            urls.Add("steam", baseUrl + "Gaming/Steam.ps1");
            urls.Add("epicgames", baseUrl + "Gaming/EpicGames.ps1");

            urls.Add("winutil", baseUrl + "Tools/WinUtil.ps1");
            urls.Add("oosu", baseUrl + "Tools/OOSU.ps1");
            urls.Add("msiafterburner", baseUrl + "Tools/MsiAfterburner.ps1");
            urls.Add("unigetui", baseUrl + "Tools/UniGetUI.ps1");
            urls.Add("notepadplusplus", baseUrl + "Tools/NotepadPlusPlus.ps1");
            urls.Add("startallback", baseUrl + "Tools/StartAllBack.ps1");
            urls.Add("keepassxc", baseUrl + "Tools/KeePassXC.ps1");
            urls.Add("powershell7", baseUrl + "Tools/PowerShell7.ps1");

            urls.Add("amd", baseUrl + "GPU/AMD.ps1");
            urls.Add("nvidia", baseUrl + "GPU/NVIDIA.ps1");

            return urls;
        }
    }
}