using NitroWin.Apps;
using NitroWin.Parser;
using Serilog;

namespace NitroWin.Helpers
{
    internal static class Globals
    {
        internal const string DownloadFolder = "Downloads";

        internal static AppConfig? AppConfig { get; } = LoadAppInstallerConfig();

        private static AppConfig? LoadAppInstallerConfig()
        {
            try
            {
                var yaml = File.ReadAllText(Path.Combine("Configuration", "Apps.yml"));

                return AppParser.Deserializer.Deserialize<AppConfig>(yaml);
            }
            catch
            {
                Log.Warning(ResourceHelper.GetString("Globals_NoAppConfigFound")!);
                return null;
            }
        }
    }
}
