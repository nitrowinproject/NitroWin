using NitroWin.Apps;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace NitroWin.Parser
{
    public class AppParser
    {
        public static IDeserializer Deserializer { get; } = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .WithTagMapping("!web:", typeof(WebApp))
            .WithTagMapping("!winget:", typeof(WingetApp))
            .Build();
    }
}
