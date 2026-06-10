using Microsoft.Extensions.Hosting;
using NitroWin.Models;
using NitroWin.Models.Tweaks.Actions;
using YamlDotNet.Serialization;

namespace NitroWin.Services;

internal sealed class TweakService(LogService logService, ConfigService configService, ExtractionService extractionService, DownloaderService downloaderService, IDeserializer deserializer) : IHostedService {
    private Config? _config;

    private const string TweakPath = "Tweaks";

    internal async Task ApplyTweaksAsync() {
        logService.DownloadingTweaks();
        await DownloadTweaksAsync();

        logService.ApplyingTweaks();
        var tweaks = await ParseTweaksAsync();

        await Parallel.ForEachAsync(
            tweaks,
            new ParallelOptions { MaxDegreeOfParallelism = Math.Max(1, Environment.ProcessorCount / 2) },
            async (tweak, _) => {
                try {
                    await ApplyTweakAsync(tweak);
                } catch (Exception ex) {
                    logService.TweakApplyError(tweak, ex);
                }
            });
    }

    private async Task DownloadTweaksAsync() {
        if (_config is null)
            throw new InvalidOperationException("Config has not been initialized.");

        var tweaksArchive = await downloaderService.DownloadFileAsync(_config.Options.TweakUrl, "Downloads")
            ?? throw new InvalidOperationException("Failed to download tweaks.");

        await extractionService.ExtractZipFile(tweaksArchive, TweakPath);
    }

    private async Task<List<Tweak>> ParseTweaksAsync() {
        var tweaks = new List<Tweak>();

        if (!Directory.Exists(TweakPath))
            throw new InvalidOperationException($"Tweak directory '{TweakPath}' not found after extraction.");

        foreach (var file in Directory.EnumerateFiles(TweakPath, "*.yml", SearchOption.AllDirectories)) {
            try {
                var content = await File.ReadAllTextAsync(file);
                var tweak = deserializer.Deserialize<Tweak>(content);

                if (tweak is not null)
                    tweaks.Add(tweak);
            } catch (Exception ex) {
                logService.TweakReadError(file, ex);
            }
        }

        return tweaks;
    }

    private async Task ApplyTweakAsync(Tweak tweak) {
        logService.ApplyingTweak(tweak);

        foreach (var action in tweak.Actions)
            await ApplyActionAsync(tweak, action);

        logService.AppliedTweak(tweak);
    }

    private static async Task ApplyActionAsync(Tweak tweak, ActionBase action) {
        int returnCode;

        try {
            returnCode = await action.ApplyAsync();
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
