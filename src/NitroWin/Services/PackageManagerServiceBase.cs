using NitroWin.Models.Apps;

namespace NitroWin.Services;

public abstract class PackageManagerServiceBase {
    internal abstract AppBase App { get; }

    internal abstract bool IsInstallationNeeded();
    internal abstract Task<bool> IsInstalledAsync();

    internal abstract Task InstallAppAsync(string id, string[]? args);
}
