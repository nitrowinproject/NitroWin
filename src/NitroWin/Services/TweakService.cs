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

        await Parallel.ForEachAsync(tweaks, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount / 2 },
            async (tweak, _) => {
                await ApplyTweakAsync(tweak);
            });
    }

    private async Task DownloadTweaksAsync() {
        var tweaksArchive = await downloaderService.DownloadFileAsync(_config!.Options.TweakUrl, "Downloads") ?? throw new NullReferenceException();
        await extractionService.ExtractZipFile(tweaksArchive, TweakPath);
    }

    private async Task<List<Tweak>> ParseTweaksAsync() {
        var tweaks = new List<Tweak>();

        foreach (var file in Directory.EnumerateFiles(TweakPath, "*.yml", SearchOption.AllDirectories)) {
            try {
                var content = await File.ReadAllTextAsync(file);
                tweaks.Add(deserializer.Deserialize<Tweak>(content));
            } catch (Exception ex) {
                logService.TweakReadError(file, ex);
            }
        }

        return tweaks;
    }

    private async Task ApplyTweakAsync(Tweak tweak) {
        logService.ApplyingTweak(tweak);

        foreach (var action in tweak.Actions) {
            await ApplyActionAsync(tweak, action);
        }
    }

    private async Task ApplyActionAsync(Tweak tweak, ActionBase action) {
        try {
            var returnCode = await action.ApplyAsync();

            if (returnCode != 0) {
                throw new InvalidOperationException(returnCode.ToString());
            }

            logService.AppliedTweak(tweak);
        } catch (Exception ex) {
            if (!action.IgnoreErrors) {
                logService.TweakApplyError(tweak, ex);
            }
        }
    }

    public async Task StartAsync(CancellationToken cancellationToken) =>
        _config = await configService.GetAsync();

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
