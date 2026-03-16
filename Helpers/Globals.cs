using NitroWin.Apps;
using System.Reflection;
using System.Resources;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace NitroWin.Helpers
{
    public static class Globals
    {
        public const string DownloadFolder = "Downloads";
        public static readonly ResourceManager StringsResourceManager = new("NitroWin.Resources.Strings", Assembly.GetExecutingAssembly());

        public static AppInstallerConfig? AppInstallerConfig { get; } = LoadConfig();

        private static AppInstallerConfig? LoadConfig()
        {
            try
            {
                var yaml = File.ReadAllText(Path.Combine("Configuration", "Apps.yml"));

                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .Build();

                return deserializer.Deserialize<AppInstallerConfig>(yaml);
            }
            catch
            {
                ConsoleHelper.WriteWarning(StringsResourceManager.GetString("Globals_NoAppConfigFound")!);
                return null;
            }
        }
    }
}
