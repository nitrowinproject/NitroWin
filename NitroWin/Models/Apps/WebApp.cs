using NitroWin.Helpers;
using NitroWin.Services;

namespace NitroWin.Models.Apps;

public class WebApp(LogService logService, DownloaderService downloaderService) : AppBase(logService) {
    public string? Name { get; set; }
    public required string Url { get; set; }

    protected override async Task InstallCoreAsync(CancellationToken cancellationToken) {
        var download = await downloaderService.DownloadFileAsync(Url, "Downloads", cancellationToken: cancellationToken) ?? throw new InvalidOperationException("Failed to download the executable.");

        await ProcessHelper.StartProcessAsync(download, string.Join(" ", Arguments ?? []), cancellationToken: cancellationToken);
    }
}
