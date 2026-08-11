using NitroWin.Cli.Helpers;
using NitroWin.Core.Services;
using Spectre.Console;
using Spectre.Console.Cli;

namespace NitroWin.Cli.Models;

internal sealed class UpdateCommand(TweakService tweakService, IAnsiConsole console) : AsyncCommand {
    protected override async Task<int> ExecuteAsync(CommandContext context, CancellationToken cancellationToken) {
        try {
            if (Directory.Exists(Paths.TweakPath))
                Directory.Delete(Paths.TweakPath, true);

            await console.Status()
                .StartAsync("Updating tweaks...", async ctx => {
                    await tweakService.DownloadTweaksAsync(Paths.TweakPath, Paths.DownloadPath, cancellationToken);
                });
        } catch (Exception ex) {
            console.MarkupLineInterpolated($"[bold red]✗ Error while updating tweaks:[/] {ex.Message}");
            return 1;
        }

        console.MarkupLine("[green]✓ Tweaks were updated successfully![/]");

        try {
            console.Status()
                .Start("Cleaning up...", ctx => {
                    if (Directory.Exists(Paths.DownloadPath))
                        Directory.Delete(Paths.DownloadPath, true);
                });
        } catch (Exception ex) {
            console.MarkupLineInterpolated($"[bold red]✗ Error while cleaning up after downloading tweaks:[/] {ex.Message}");
            return 1;
        }

        console.MarkupLine("[green]✓ Cleaned up successfully![/]");
        return 0;
    }
}
