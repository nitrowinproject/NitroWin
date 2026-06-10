using NitroWin.Services;

namespace NitroWin.Models.Apps;

public sealed class WingetApp(LogService logService, WingetService wingetService) : AppBase(logService) {
    public required string Id { get; set; }

    protected override async Task InstallCoreAsync(CancellationToken cancellationToken = default) =>
        await wingetService.InstallAppAsync(Id, Arguments?.ToArray(), cancellationToken);
}
