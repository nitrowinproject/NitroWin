using NitroWin.Core.Models.Apps;

namespace NitroWin.Core.Models;

public sealed class AppInstallerConfig : ConfigBase {
    public List<AppBase>? Apps { get; init; } = null;
}
