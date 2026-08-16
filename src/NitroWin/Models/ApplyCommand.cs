using Microsoft.Extensions.Localization;
using NitroWin.Core.Services;
using Spectre.Console;
using Spectre.Console.Cli;

namespace NitroWin.Models;

internal sealed class ApplyCommand(TweakService tweakService, IAnsiConsole console, IStringLocalizer<ApplyCommand> localizer) : AsyncCommand {
    protected override async Task<int> ExecuteAsync(CommandContext context, CancellationToken cancellationToken) {
        try {
            await console.Status()
                .StartAsync(localizer["StatusApplying"], async _ => {
                    await tweakService.ApplyTweaksAsync(cancellationToken);
                });
        } catch (Exception ex) {
            console.MarkupLine(localizer["ApplyError", ex.Message]);
            return 1;
        }

        console.MarkupLine(localizer["ApplySuccess"]);

        return 0;
    }

}
