using Microsoft.Extensions.Localization;
using NitroWin.Core.Helpers;
using NitroWin.Core.Services;
using NitroWin.Services;
using Spectre.Console;
using Spectre.Console.Cli;

namespace NitroWin.Models;

internal sealed class UpdateCommand(TweakService tweakService, IAnsiConsole console, IStringLocalizer<UpdateCommand> localizer, HelperService helperService) : AsyncCommand {
    protected override async Task<int> ExecuteAsync(CommandContext context, CancellationToken cancellationToken) {
        await helperService.WaitForNetwork(cancellationToken);

        try {
            if (Directory.Exists(Paths.TweakPath))
                Directory.Delete(Paths.TweakPath, true);

            await console.Status()
                .StartAsync(localizer["StatusUpdating"], async _ => {
                    await tweakService.DownloadTweaksAsync(cancellationToken);
                });
        } catch (Exception ex) {
            console.MarkupLine(localizer["UpdateError", ex.Message]);
            return 1;
        }

        console.MarkupLine(localizer["UpdateSuccess"]);

        return helperService.CleanupDownloads();
    }
}
