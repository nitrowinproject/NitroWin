using Microsoft.Extensions.Localization;
using NitroWin.Cli.Helpers;
using NitroWin.Cli.Services;
using NitroWin.Core.Services;
using Spectre.Console;
using Spectre.Console.Cli;

namespace NitroWin.Cli.Models;

internal sealed class UpdateCommand(TweakService tweakService, IAnsiConsole console, IStringLocalizer<UpdateCommand> localizer, HelperService helperService) : AsyncCommand {
    protected override async Task<int> ExecuteAsync(CommandContext context, CancellationToken cancellationToken) {
        await helperService.WaitForNetwork(cancellationToken);

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

        return helperService.CleanupDownloads();
    }
}
