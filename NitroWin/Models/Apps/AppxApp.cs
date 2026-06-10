using NitroWin.Helpers;
using NitroWin.Services;

namespace NitroWin.Models.Apps;

public sealed class AppxApp(LogService logService) : AppBase(logService) {
    public string? Name { get; set; }
    public required string Path { get; set; }

    protected override async Task InstallCoreAsync(CancellationToken cancellationToken = default) {
        await ProcessHelper.StartProcessAsync(
            "powershell.exe",
            $"-NoProfile -ExecutionPolicy Bypass -Command \"Add-AppxPackage -Path '{Path}'\" " + string.Join(" ", Arguments ?? []),
            false,
            cancellationToken: cancellationToken
        );
    }
}
