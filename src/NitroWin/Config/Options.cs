using YamlDotNet.Serialization;

namespace NitroWin.Config
{
    public class Options
    {
        public enum InstallOptions
        {
            IfNeeded,
            Always,
            Never
        }

        [YamlMember(typeof(InstallOptions), Alias = "installWinget")]
        public InstallOptions InstallWinget { get; set; } = InstallOptions.IfNeeded;

        [YamlMember(typeof(InstallOptions), Alias = "installChocolatey")]
        public InstallOptions InstallChocolatey { get; set; } = InstallOptions.IfNeeded;

        public string TweakUrl { get; set; } = "https://github.com/nitrowinproject/Tweaks/archive/refs/heads/main.zip";
    }
}
