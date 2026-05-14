using NitroWin.Models;
using TweakLib.Actions;
using TweakLib.Models;
using TweakLib.Parser;

namespace NitroWin.Services;

internal sealed class TweakService {
    private readonly LogService _logService;
    private readonly ExtractionService _extractionService;
    private readonly DownloaderService _downloaderService;

    private Config? _config;

    private const string TweakPath = "Tweaks";

    public TweakService(LogService logService, ConfigService configService, ExtractionService extractionService, DownloaderService downloaderService) {
        _logService = logService;
        _extractionService = extractionService;
        _downloaderService = downloaderService;

        _ = InitializeAsync(configService);
    }

    internal async Task ApplyTweaksAsync() {
        _logService.DownloadingTweaks();
        await DownloadTweaksAsync();

        _logService.ApplyingTweaks();
        var tweaks = await ParseTweaksAsync();

        await Parallel.ForEachAsync(tweaks, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount / 2 },
            async (tweak, _) => {
                await ApplyTweakAsync(tweak);
            });
    }

    private async Task InitializeAsync(ConfigService configService) {
        _config = await configService.GetAsync();
    }

    private async Task DownloadTweaksAsync() {
        var tweaksArchive = await _downloaderService.DownloadFileAsync(_config!.Options.TweakUrl, "Downloads") ?? throw new NullReferenceException();
        await _extractionService.ExtractZipFile(tweaksArchive, TweakPath);
    }

    private async Task<List<Tweak>> ParseTweaksAsync() {
        var tweaks = new List<Tweak>();

        foreach (var file in Directory.EnumerateFiles(TweakPath, "*.yml", SearchOption.AllDirectories)) {
            try {
                var content = await File.ReadAllTextAsync(file);
                tweaks.Add(TweakParser.Deserializer.Deserialize<Tweak>(content));
            } catch (Exception ex) {
                _logService.TweakReadError(file, ex);
            }
        }

        return tweaks;
    }

    private async Task ApplyTweakAsync(Tweak tweak) {
        _logService.ApplyingTweak(tweak);

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

            _logService.AppliedTweak(tweak);
        } catch (Exception ex) {
            if (!action.IgnoreErrors) {
                _logService.TweakApplyError(tweak, ex);
            }
        }
    }
}
