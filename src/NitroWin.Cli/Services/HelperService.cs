using Microsoft.Extensions.Localization;
using NitroWin.Cli.Helpers;
using NitroWin.Core.Services;
using Spectre.Console;

namespace NitroWin.Cli.Services;

internal sealed class HelperService(IAnsiConsole console, IStringLocalizer<HelperService> localizer, NitroWinService nitroWinService) {
    internal async Task WaitForNetwork(CancellationToken cancellationToken) =>
        await console.Status()
            .StartAsync(localizer["StatusNetworkCheck"], async _ => {
                await nitroWinService.WaitForNetworkAsync(false, cancellationToken);
            });

    internal int CleanupDownloads() {
        try {
            console.Status()
                .Start(localizer["StatusCleanup"], ctx => {
                    if (Directory.Exists(Paths.DownloadPath))
                        Directory.Delete(Paths.DownloadPath, true);
                });
        } catch (Exception ex) {
            console.MarkupLine(localizer["CleanupError", ex.Message]);
            return 1;
        }

        console.MarkupLine(localizer["CleanupSuccess"]);
        return 0;
    }
}
