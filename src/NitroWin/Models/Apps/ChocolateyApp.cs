using NitroWin.Services;

namespace NitroWin.Models.Apps;

public sealed class ChocolateyApp(LogService logService, ChocolateyService chocolateyService) : AppBase(logService) {
    public required string Id { get; set; }

    protected override async Task InstallCoreAsync() =>
        await chocolateyService.InstallAppAsync(Id, Arguments?.ToArray());
}
