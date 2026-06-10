using NitroWin.Models.Apps;

namespace NitroWin.Services;

public abstract class PackageManagerServiceBase {
    internal abstract AppBase App { get; }

    internal abstract bool IsInstallationNeeded();
    internal abstract Task<bool> IsInstalledAsync(CancellationToken cancellationToken = default);

    internal abstract Task InstallAppAsync(string id, string[]? args, CancellationToken cancellationToken = default);
}
