namespace NitroWin {
    public class NitroWin {
        public static async Task Main() {
            Console.Title = "NitroWin";

            bool mergeResult = Helper.Prompt("Merge tweaks?");

            if (mergeResult) {
                Tweaks.Merge();
            }

            bool appInstallScriptResult = Helper.Prompt("Create app install script?");

            if (appInstallScriptResult) {
                await Config.Initialize();
                await ScriptGenerator.WriteToFile("NitroWin.AppInstallScript.ps1");
            }
        }
    }
}