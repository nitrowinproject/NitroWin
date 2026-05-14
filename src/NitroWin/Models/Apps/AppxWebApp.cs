using NitroWin.Helpers;
using NitroWin.Services;

namespace NitroWin.Models.Apps;

internal class AppxWebApp(LogService logService, DownloaderService downloaderService) : AppBase(logService) {
    internal string? Name { get; set; }
    internal required string Url { get; set; }

    protected override async Task InstallCoreAsync() {
        var path = await downloaderService.DownloadFileAsync(Url, "Downloads");

        await ProcessHelper.StartProcessAsync(
            "powershell.exe",
            $"-NoProfile -ExecutionPolicy Bypass -Command \"Add-AppxPackage -Path '{path}'\" " + string.Join(" ", Arguments ?? []),
            false
        );
    }
}
