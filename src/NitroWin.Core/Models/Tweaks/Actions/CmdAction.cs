using NitroWin.Core.Helpers;

namespace NitroWin.Core.Models.Tweaks.Actions;

public sealed class CmdAction : ActionBase {
    public required string Command { get; init; }

    protected override async Task<int> ApplyAsyncCore(CancellationToken cancellationToken) =>
        await ProcessHelper.StartProcessAsync("cmd.exe", "/c \"" + Command + "\"", true, RunAs, cancellationToken);
}
