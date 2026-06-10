namespace NitroWin.Models;

public sealed class Config : ConfigBase {
    public Options Options { get; set; } = new();
}
