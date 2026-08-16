using NitroWin.Core.Helpers;

namespace NitroWin.Core.Models.Tweaks.Actions;

public sealed class PowerShellAction : ActionBase {
    public required string Command { get; init; }

    protected override async Task<int> ApplyAsyncCore(CancellationToken cancellationToken) =>
        await ProcessHelper.StartProcessAsync("powershell.exe", $"-ExecutionPolicy Bypass -NoProfile -Command \"{Command}\"", true, RunAs, cancellationToken);
}
