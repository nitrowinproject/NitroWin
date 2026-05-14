namespace NitroWin.Models;

public sealed class Config : IConfig {
    public string? Name { get; set; }
    public string? Author { get; set; }
    public Options Options { get; set; } = new();
}
