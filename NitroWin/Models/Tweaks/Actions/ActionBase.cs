using NitroWin.Helpers;
using YamlDotNet.Serialization;

namespace NitroWin.Models.Tweaks.Actions;

public abstract class ActionBase {
    [YamlMember(typeof(Privilege), Alias = "runas")]
    public Privilege RunAs { get; set; } = Privilege.CurrentUserElevated;
    public bool IgnoreErrors { get; set; } = false;
    public Platforms Platforms { get; set; } = new();
    public int Timeout { get; set; } = 30;

    internal async Task<int> ApplyAsync() {
        if ((!Platforms.Mobile && PlatformHelper.IsMobile()) || (!Platforms.Desktop && !PlatformHelper.IsMobile()))
            return 0;

        try {
            return await ApplyAsyncCore().WaitAsync(TimeSpan.FromSeconds(Timeout));
        } catch (OperationCanceledException) {
            throw new TimeoutException($"Action timed out after {Timeout} seconds");
        }
    }

    protected abstract Task<int> ApplyAsyncCore();
}
