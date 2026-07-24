using NitroWin.Models.Tweaks.Actions;

namespace NitroWin.Models;

public sealed record Tweak(
    string Title,
    List<ActionBase> Actions,
    string? Description = null);
