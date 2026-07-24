namespace NitroWin.Models;

public sealed record Config : ConfigBase {
    public Options Options { get; init; } = new();
}
