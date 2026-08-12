using Microsoft.Extensions.Localization;
using NitroWin.Cli.Helpers;
using NitroWin.Core.Services;
using Spectre.Console;
using Spectre.Console.Cli;

namespace NitroWin.Cli.Models;

internal sealed class UpdateCommand(TweakService tweakService, IAnsiConsole console, IStringLocalizer<UpdateCommand> localizer, NitroWinService nitroWinService) : AsyncCommand {
    protected override async Task<int> ExecuteAsync(CommandContext context, CancellationToken cancellationToken) {
        await console.Status()
            .StartAsync(localizer["StatusNetworkCheck"], async _ => {
                await nitroWinService.WaitForNetworkAsync(false, cancellationToken);
            });

        try {
            if (Directory.Exists(Paths.TweakPath))
                Directory.Delete(Paths.TweakPath, true);

            await console.Status()
                .StartAsync(localizer["StatusUpdating"], async _ => {
                    await tweakService.DownloadTweaksAsync(Paths.TweakPath, Paths.DownloadPath, cancellationToken);
                });
        } catch (Exception ex) {
            console.MarkupLine(localizer["UpdateError", ex.Message]);
            return 1;
        }

        console.MarkupLine(localizer["UpdateSuccess"]);

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
