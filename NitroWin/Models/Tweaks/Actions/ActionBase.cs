using NitroWin.Helpers;
using YamlDotNet.Serialization;

namespace NitroWin.Models.Tweaks.Actions;

public abstract class ActionBase {
    [YamlMember(typeof(Privilege), Alias = "runas")]
    public Privilege RunAs { get; set; } = Privilege.CurrentUserElevated;
    public bool IgnoreErrors { get; set; } = false;
    public Platforms Platforms { get; set; } = new();
    public int Timeout { get; set; } = 30;

    internal async Task<int> ApplyAsync(CancellationToken cancellationToken = default) {
        if ((!Platforms.Mobile && PlatformHelper.IsMobile()) || (!Platforms.Desktop && PlatformHelper.IsDesktop()))
            return 0;

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromSeconds(Timeout));

        try {
            return await ApplyAsyncCore(cts.Token);
        } catch (OperationCanceledException) when (cts.IsCancellationRequested && !cancellationToken.IsCancellationRequested) {
            throw new TimeoutException($"Action timed out after {Timeout} seconds.");
        }
    }

    protected abstract Task<int> ApplyAsyncCore(CancellationToken cancellationToken);
}
