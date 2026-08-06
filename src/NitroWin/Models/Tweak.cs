using NitroWin.Models.Tweaks.Actions;

namespace NitroWin.Models;

public sealed class Tweak {
    public required string Title { get; init; }
    public string? Description { get; init; } = null;
    public required List<ActionBase> Actions { get; init; }
}
