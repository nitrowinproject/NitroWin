using NitroWin.Models.Apps;

namespace NitroWin.Models;

public sealed record AppInstallerConfig : ConfigBase {
    public List<AppBase>? Apps { get; init; } = null;
}
