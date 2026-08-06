using NitroWin.Services;

namespace NitroWin.Models.Apps;

public sealed class WingetApp(LogService logService, WingetService wingetService) : AppBase(logService) {
    public required string Id { get; init; }

    protected override async Task InstallCoreAsync(CancellationToken cancellationToken) =>
        await wingetService.InstallAppAsync(Id, Arguments?.ToArray(), cancellationToken);
}
