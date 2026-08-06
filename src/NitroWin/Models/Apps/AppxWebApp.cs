using NitroWin.Helpers;
using NitroWin.Services;

namespace NitroWin.Models.Apps;

public class AppxWebApp(LogService logService, DownloaderService downloaderService) : AppBase(logService) {
    public string? Name { get; init; }
    public required string Url { get; init; }

    protected override async Task InstallCoreAsync(CancellationToken cancellationToken) {
        var path = await downloaderService.DownloadFileAsync(Url, "Downloads", cancellationToken: cancellationToken) ?? throw new InvalidOperationException("Failed to download the Appx package.");

        await ProcessHelper.StartProcessAsync(
            "powershell.exe",
            $"-NoProfile -ExecutionPolicy Bypass -Command \"Add-AppxPackage -Path '{path}'\" " + string.Join(" ", Arguments ?? []),
            false,
            cancellationToken: cancellationToken
        );
    }
}
