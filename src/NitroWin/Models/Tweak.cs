using NitroWin.Models.Tweaks.Actions;

namespace NitroWin.Models;

public sealed class Tweak {
    public required string Title { get; set; }
    public string? Description { get; set; } = null;
    public required List<ActionBase> Actions { get; set; }
}
