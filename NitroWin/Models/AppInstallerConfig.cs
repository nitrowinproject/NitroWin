using NitroWin.Models.Apps;

namespace NitroWin.Models;

public sealed class AppInstallerConfig : ConfigBase {
    public List<AppBase>? Apps { get; set; } = null;
}
