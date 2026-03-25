using NitroWin.Apps;
using NitroWin.Config;
using NitroWin.Parser;
using Serilog;

namespace NitroWin.Helpers
{
    internal static class Globals
    {
        internal const string DownloadFolder = "Downloads";

        internal static AppInstallerConfig? AppInstallerConfig { get; } = LoadAppInstallerConfig();

        private static AppInstallerConfig? LoadAppInstallerConfig()
        {
            try
            {
                var yaml = File.ReadAllText(Path.Combine("Configuration", "Apps.yml"));

                return AppParser.Deserializer.Deserialize<AppInstallerConfig>(yaml);
            }
            catch
            {
                Log.Warning(ResourceHelper.GetString("Globals_NoAppInstallerConfigFound"));
                return null;
            }
        }

        internal static NitroWinConfig Config = LoadNitroWinConfig() ?? new();

        private static NitroWinConfig? LoadNitroWinConfig()
        {
            try
            {
                var yaml = File.ReadAllText(Path.Combine("Configuration", "Config.yml"));

                return ConfigParser.Deserializer.Deserialize<NitroWinConfig>(yaml);
            }
            catch
            {
                Log.Warning(ResourceHelper.GetString("Globals_NoConfigFound"));
                return null;
            }
        }
    }
}
