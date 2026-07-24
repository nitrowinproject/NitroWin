using NitroWin.Services;

namespace NitroWin.Models.Apps;

public sealed class WingetBundleApp(LogService logService, WingetService wingetService) : AppBase(logService) {
    public required string FileName { get; init; }

    protected override async Task InstallCoreAsync(CancellationToken cancellationToken) =>
        await wingetService.InstallAppBundleAsync(FileName, Arguments?.ToArray(), cancellationToken);
}
