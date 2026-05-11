namespace NitroWin.Apps;

public sealed class AppInstallerConfig {
    public string? Name { get; set; }
    public string? Author { get; set; }
    public List<AppBase> Apps { get; set; } = [];
}
