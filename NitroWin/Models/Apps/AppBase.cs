using System.Runtime.InteropServices;
using NitroWin.Services;

namespace NitroWin.Models.Apps;

public abstract class AppBase(LogService logService) {
    public List<string>? Arguments { get; init; }
    public Architectures Architectures { get; init; } = new();

    internal async Task InstallAsync(CancellationToken cancellationToken = default) {
        if (!IsSupportedArchitecture()) {
            logService.NotInstallingApp(this);
            return;
        }

        logService.InstallingApp(this);

        try {
            await InstallCoreAsync(cancellationToken);
        } catch (Exception ex) {
            logService.AppInstallError(this, ex);
        }
    }

    protected abstract Task InstallCoreAsync(CancellationToken cancellationToken);

    protected bool IsSupportedArchitecture() {
        return (RuntimeInformation.ProcessArchitecture == Architecture.X64 && Architectures.X64)
            || (RuntimeInformation.ProcessArchitecture == Architecture.Arm64 && Architectures.Arm64);
    }
}
