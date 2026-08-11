using NitroWin.Cli.Helpers;
using NitroWin.Core.Services;
using Spectre.Console.Cli;

namespace NitroWin.Cli.Models;

internal sealed class ApplyCommand(TweakService tweakService) : AsyncCommand {
    protected override async Task<int> ExecuteAsync(CommandContext context, CancellationToken cancellationToken) {
        try {
            await tweakService.ApplyTweaksAsync(Paths.TweakPath, cancellationToken);
        } catch {
            return 1;
        }

        return 0;
    }

}
