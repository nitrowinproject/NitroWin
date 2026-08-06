using NitroWin.Core.Models.Apps;

namespace NitroWin.Core.Services;

public abstract class PackageManagerServiceBase {
    protected abstract AppBase App { get; }

    public async Task InstallAsync(CancellationToken cancellationToken) =>
        await App.InstallAsync(cancellationToken);

    public abstract bool IsInstallationNeeded();
    public abstract Task<bool> IsInstalledAsync(CancellationToken cancellationToken = default);

    public abstract Task InstallAppAsync(string id, string[]? args, CancellationToken cancellationToken = default);
    public abstract Task InstallAppBundleAsync(string fileName, string[]? args, CancellationToken cancellationToken = default);
}
