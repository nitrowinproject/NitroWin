namespace NitroWin.Config;

public sealed class NitroWinConfig {
    public string? Name { get; set; }
    public string? Author { get; set; }
    public Options Options { get; set; } = new();
}
