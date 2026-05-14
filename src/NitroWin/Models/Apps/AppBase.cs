using System.Runtime.InteropServices;
using NitroWin.Services;

namespace NitroWin.Models.Apps;

public abstract class AppBase(LogService logService) {
    public List<string>? Arguments { get; set; }
    public Architectures Architectures { get; set; } = new();

    internal async Task InstallAsync() {
        if (!IsSupportedArchitecture()) {
            logService.NotInstallingApp(this);
            return;
        }

        logService.InstallingApp(this);

        try {
            await InstallCoreAsync();
        } catch (Exception ex) {
            logService.AppInstallError(this, ex);
        }
    }

    protected abstract Task InstallCoreAsync();

    protected bool IsSupportedArchitecture() {
        return (RuntimeInformation.ProcessArchitecture == Architecture.X64 && Architectures.X64)
            || (RuntimeInformation.ProcessArchitecture == Architecture.Arm64 && Architectures.Arm64);
    }
}
