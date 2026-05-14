using NitroWin.Helpers;
using NitroWin.Services;

namespace NitroWin.Models.Apps;

internal sealed class AppxApp(LogService logService) : AppBase(logService) {
    internal string? Name { get; set; }
    internal required string Path { get; set; }

    protected override async Task InstallCoreAsync() {
        await ProcessHelper.StartProcessAsync(
            "powershell.exe",
            $"-NoProfile -ExecutionPolicy Bypass -Command \"Add-AppxPackage -Path '{Path}'\" " + string.Join(" ", Arguments ?? []),
            false
        );
    }
}
