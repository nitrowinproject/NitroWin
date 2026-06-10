using NitroWin.Helpers;

namespace NitroWin.Models.Tweaks.Actions;

public sealed class CmdAction : ActionBase {
    public required string Command { get; set; }

    protected override async Task<int> ApplyAsyncCore(CancellationToken cancellationToken = default) =>
        await ProcessHelper.StartProcessAsync("cmd.exe", "/c \"" + Command + "\"", true, RunAs, cancellationToken);
}
