using NitroWin.Helpers;
using NitroWin.Services;

namespace NitroWin.Models.Apps;

public class WebApp(LogService logService, DownloaderService downloaderService) : AppBase(logService) {
    public string? Name { get; set; }
    public required string Url { get; set; }

    protected override async Task InstallCoreAsync() {
        var download = await downloaderService.DownloadFileAsync(Url, "Downloads") ?? throw new InvalidOperationException();

        await ProcessHelper.StartProcessAsync(download, string.Join(" ", Arguments ?? []));
    }
}
