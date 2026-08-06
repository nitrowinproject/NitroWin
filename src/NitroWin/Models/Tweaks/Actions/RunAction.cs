using NitroWin.Helpers;

namespace NitroWin.Models.Tweaks.Actions;

public sealed class RunAction : ActionBase {
    public required string Exe { get; init; }
    public string? Args { get; init; }

    protected override async Task<int> ApplyAsyncCore(CancellationToken cancellationToken) =>
        await ProcessHelper.StartProcessAsync(Exe, Args, true, RunAs, cancellationToken);
}
