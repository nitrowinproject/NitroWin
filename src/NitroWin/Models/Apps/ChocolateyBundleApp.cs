using NitroWin.Services;

namespace NitroWin.Models.Apps;

public sealed class ChocolateyBundleApp(LogService logService, ChocolateyService chocolateyService) : AppBase(logService) {
    public required string FileName { get; init; }

    protected override async Task InstallCoreAsync(CancellationToken cancellationToken) =>
        await chocolateyService.InstallAppBundleAsync(FileName, Arguments?.ToArray(), cancellationToken);
}
