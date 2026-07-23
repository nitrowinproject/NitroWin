using NitroWin.Helpers;
using NitroWin.Services;

namespace NitroWin.Models.Apps;

public class AppxWebApp(LogService logService, DownloaderService downloaderService) : AppBase(logService) {
    public string? Name { get; set; }
    public required string Url { get; set; }

    protected override async Task InstallCoreAsync(CancellationToken cancellationToken) {
        var path = await downloaderService.DownloadFileAsync(Url, "Downloads", cancellationToken: cancellationToken);

        await ProcessHelper.StartProcessAsync(
            "powershell.exe",
            $"-NoProfile -ExecutionPolicy Bypass -Command \"Add-AppxPackage -Path '{path}'\" " + string.Join(" ", Arguments ?? []),
            false,
            cancellationToken: cancellationToken
        );
    }
}
