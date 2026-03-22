using NitroWin.Apps;
using NitroWin.Parser;
using Serilog;
using System.Reflection;
using System.Resources;

namespace NitroWin.Helpers
{
    public static class Globals
    {
        public const string DownloadFolder = "Downloads";
        public static readonly ResourceManager StringsResourceManager = new("NitroWin.Resources.Strings", Assembly.GetExecutingAssembly());

        public static AppConfig? AppConfig { get; } = LoadAppConfig();

        private static AppConfig? LoadAppConfig()
        {
            try
            {
                var yaml = File.ReadAllText(Path.Combine("Configuration", "Apps.yml"));

                return AppParser.Deserializer.Deserialize<AppConfig>(yaml);
            }
            catch
            {
                Log.Warning(StringsResourceManager.GetString("Globals_NoAppConfigFound")!);
                return null;
            }
        }
    }
}
