using YamlDotNet.Serialization;

namespace NitroWin.Models;

public sealed class Options {
    public enum InstallOptions {
        IfNeeded,
        Always,
        Never
    }

    [YamlMember(typeof(InstallOptions), Alias = "installWinget")]
    public InstallOptions InstallWinget { get; init; } = InstallOptions.IfNeeded;

    [YamlMember(typeof(InstallOptions), Alias = "installChocolatey")]
    public InstallOptions InstallChocolatey { get; init; } = InstallOptions.IfNeeded;

    public string TweakUrl { get; init; } = "https://github.com/nitrowinproject/Tweaks/archive/refs/heads/main.zip";
}
