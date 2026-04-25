using NitroWin.Apps;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace NitroWin.Parser
{
    public static class AppParser
    {
        public static IDeserializer Deserializer { get; } = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .WithTagMapping("!choco:", typeof(ChocolateyApp))
            .WithTagMapping("!web:", typeof(WebApp))
            .WithTagMapping("!winget:", typeof(WingetApp))
            .Build();
    }
}
