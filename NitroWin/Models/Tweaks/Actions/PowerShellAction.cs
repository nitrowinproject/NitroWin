using NitroWin.Helpers;

namespace NitroWin.Models.Tweaks.Actions;

public sealed class PowerShellAction : ActionBase {
    public required string Command { get; set; }

    protected override async Task<int> ApplyAsyncCore(CancellationToken cancellationToken) =>
        await ProcessHelper.StartProcessAsync("powershell.exe", $"-ExecutionPolicy Bypass -NoProfile -Command \"{Command}\"", true, RunAs, cancellationToken);
}
