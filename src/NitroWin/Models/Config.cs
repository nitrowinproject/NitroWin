namespace NitroWin.Models;

public sealed class Config : ConfigBase {
    public Options Options { get; init; } = new();
}
