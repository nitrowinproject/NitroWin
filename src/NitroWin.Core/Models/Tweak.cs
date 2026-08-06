using NitroWin.Core.Models.Tweaks.Actions;

namespace NitroWin.Core.Models;

public sealed class Tweak {
    public required string Title { get; init; }
    public string? Description { get; init; } = null;
    public required List<ActionBase> Actions { get; init; }
}
