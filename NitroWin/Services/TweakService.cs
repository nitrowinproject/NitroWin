using Microsoft.Extensions.Hosting;
using NitroWin.Models;
using NitroWin.Models.Tweaks.Actions;
using YamlDotNet.Serialization;

namespace NitroWin.Services;

internal sealed class TweakService(LogService logService, ConfigService configService, ExtractionService extractionService, DownloaderService downloaderService, IDeserializer deserializer) : IHostedService {
    private Config? _config;

    private const string TweakPath = "Tweaks";

    internal async Task ApplyTweaksAsync(CancellationToken cancellationToken = default) {
        logService.DownloadingTweaks();
        await DownloadTweaksAsync(cancellationToken);

        logService.ApplyingTweaks();
        var tweaks = await ParseTweaksAsync(cancellationToken);

        foreach (var tweak in tweaks)
            await ApplyTweakAsync(tweak, cancellationToken);
    }

    private async Task DownloadTweaksAsync(CancellationToken cancellationToken) {
        if (_config is null)
            throw new InvalidOperationException("Config has not been initialized.");

        var tweaksArchive = await downloaderService.DownloadFileAsync(_config.Options.TweakUrl, "Downloads", cancellationToken: cancellationToken)
            ?? throw new InvalidOperationException("Failed to download tweaks.");

        await extractionService.ExtractZipFile(tweaksArchive, TweakPath, cancellationToken);
    }

    private async Task<List<Tweak>> ParseTweaksAsync(CancellationToken cancellationToken) {
        var tweaks = new List<Tweak>();

        if (!Directory.Exists(TweakPath))
            throw new InvalidOperationException($"Tweak directory '{TweakPath}' not found after extraction.");

        foreach (var file in Directory.EnumerateFiles(TweakPath, "*.yml", SearchOption.AllDirectories)) {
            try {
                var content = await File.ReadAllTextAsync(file, cancellationToken);
                var tweak = deserializer.Deserialize<Tweak>(content);

                if (tweak is not null)
                    tweaks.Add(tweak);
            } catch (Exception ex) {
                logService.TweakReadError(file, ex);
            }
        }

        return tweaks;
    }

    private async Task ApplyTweakAsync(Tweak tweak, CancellationToken cancellationToken) {
        logService.ApplyingTweak(tweak);

        foreach (var action in tweak.Actions)
            await ApplyActionAsync(tweak, action, cancellationToken);

        logService.AppliedTweak(tweak);
    }

    private static async Task ApplyActionAsync(Tweak tweak, ActionBase action, CancellationToken cancellationToken) {
        int returnCode;

        try {
            returnCode = await action.ApplyAsync(cancellationToken);
        } catch when (action.IgnoreErrors) {
            return;
        }

        if (returnCode != 0)
            throw new InvalidOperationException(
                $"{action.GetType().Name} from tweak '{tweak.Title}' returned exit code '{returnCode}'.");
    }

    public async Task StartAsync(CancellationToken cancellationToken) =>
        _config = await configService.GetAsync(cancellationToken);

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
