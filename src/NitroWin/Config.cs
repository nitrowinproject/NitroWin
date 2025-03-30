using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace NitroWin {
    public class Config {
        private const string ConfigFileName = "config.yml";
        private const string ConfigUrl = "https://raw.githubusercontent.com/Nitro4542/NitroWin/v2/assets/Configuration/config.yml";
        public static async Task Initialize() {
            if (!File.Exists(ConfigFileName)) {
                await Helper.DownloadFile(ConfigUrl, Helper.WorkingDirectory, ConfigFileName);

                Console.WriteLine("Please configure your config file and re-run this executable again.\nPress any key to quit.");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
        public static async Task<ConfigFile> ParseConfig() {
            string yml = await File.ReadAllTextAsync(ConfigFileName);
            
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(UnderscoredNamingConvention.Instance)
                .Build();
            
            var config = deserializer.Deserialize<ConfigFile>(yml);
            return config;
        }
    }
    public class ConfigFile {
        public Apps? Apps { get; set; }
        public List<string>? Gpu { get; set; }
    }
    public class Apps {
        public List<string>? Browser { get; set; }
        public List<string>? Archiving { get; set; }
        public List<string>? Multimedia { get; set; }
        public List<string>? Communication { get; set; }
        public List<string>? Gaming { get; set; }
        public List<string>? Tools { get; set; }
    }
}