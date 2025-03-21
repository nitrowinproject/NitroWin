namespace NitroWin {
    public class NitroWin {
        public static async Task Main() {
            Console.Title = "NitroWin";

            bool tweakResult = Helper.Prompt("Download tweaks?");

            if (tweakResult) {
                Tweaks.Download();
            }

            bool appInstallScriptResult = Helper.Prompt("Create app install script?");

            if (appInstallScriptResult) {
                await Config.Initialize();
                await ScriptGenerator.WriteToFile("NitroWin.AppInstallScript.ps1");
            }
        }
    }
}