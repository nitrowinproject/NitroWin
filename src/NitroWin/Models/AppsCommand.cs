using Microsoft.Extensions.Localization;
using NitroWin.Core.Services;
using NitroWin.Services;
using Spectre.Console;
using Spectre.Console.Cli;

namespace NitroWin.Models;

internal sealed class AppsCommand(IAnsiConsole console, IStringLocalizer<AppsCommand> localizer, HelperService helperService, NitroWinService nitroWinService) : AsyncCommand {
    protected override async Task<int> ExecuteAsync(CommandContext context, CancellationToken cancellationToken) {
        await helperService.WaitForNetwork(cancellationToken);

        try {
            await console.Status()
                .StartAsync(localizer["StatusInstallingApps"], async _ => {
                    await nitroWinService.InstallAppsAsync(cancellationToken);
                });
        } catch (Exception ex) {
            console.MarkupLine(localizer["InstallError", ex.Message]);
            return 1;
        }

        console.MarkupLine(localizer["InstallSuccess"]);

        return helperService.CleanupDownloads();
    }
}
