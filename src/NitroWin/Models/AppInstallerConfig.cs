using NitroWin.Models.Apps;

namespace NitroWin.Models;

internal sealed class AppInstallerConfig : IConfig {
    public string? Name { get; set; }
    public string? Author { get; set; }
    internal List<AppBase> Apps { get; set; } = [];
}
