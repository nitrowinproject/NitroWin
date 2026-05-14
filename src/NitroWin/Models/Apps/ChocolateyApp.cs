using NitroWin.Services;

namespace NitroWin.Models.Apps;

internal sealed class ChocolateyApp(LogService logService, ChocolateyService chocolateyService) : AppBase(logService) {
    internal required string Id { get; set; }

    protected override async Task InstallCoreAsync() =>
        await chocolateyService.InstallAppAsync(Id, Arguments?.ToArray());
}
