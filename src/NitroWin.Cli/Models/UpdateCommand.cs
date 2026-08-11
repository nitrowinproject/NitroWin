using NitroWin.Cli.Helpers;
using NitroWin.Core.Services;
using Spectre.Console.Cli;

namespace NitroWin.Cli.Models;

internal sealed class UpdateCommand(TweakService tweakService) : AsyncCommand {
    protected override async Task<int> ExecuteAsync(CommandContext context, CancellationToken cancellationToken) {
        try {
            if (Directory.Exists(Paths.TweakPath))
                Directory.Delete(Paths.TweakPath, true);

            await tweakService.DownloadTweaksAsync(Paths.TweakPath, cancellationToken);
        } catch {
            return 1;
        }

        return 0;
    }
}
