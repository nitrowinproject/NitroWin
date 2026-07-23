using NitroWin.Helpers;

namespace NitroWin.Models.Tweaks.Actions;

public sealed class RunAction : ActionBase {
    public required string Exe { get; set; }
    public string? Args { get; set; }

    protected override async Task<int> ApplyAsyncCore(CancellationToken cancellationToken) =>
        await ProcessHelper.StartProcessAsync(Exe, Args, true, RunAs, cancellationToken);
}
