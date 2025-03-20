using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace NitroWin {
    public class Config {
        public static async Task Initialize() {
            if (!File.Exists("config.yml")) {
                String configUrl = "https://raw.githubusercontent.com/Nitro4542/NitroWin/v2/assets/Configuration/config.yml";
                String workingDirectory = AppDomain.CurrentDomain.BaseDirectory;
                await Helper.DownloadFile(configUrl, workingDirectory, "config.yml");

                Console.WriteLine("Please configure your config file and re-run this executable again.");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
        public static ConfigFile ParseConfig() {
            String yml = File.ReadAllText("config.yml");
            
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