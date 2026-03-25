using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace NitroWin.Parser
{
    public static class ConfigParser
    {
        public static IDeserializer Deserializer { get; } = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
    }
}
