using NitroWin.Core.Models;
using NitroWin.Core.Models.Tweaks.Actions;
using YamlDotNet.Serialization;

namespace NitroWin.Core.Services;

public sealed class TweakService(LogService logService, ConfigService configService, ExtractionService extractionService, DownloaderService downloaderService, IDeserializer deserializer) {
    private Config? _config;

    public async Task ApplyTweaksAsync(string tweakPath, CancellationToken cancellationToken = default) {
        logService.ApplyingTweaks();
        var tweaks = await ParseTweaksAsync(tweakPath, cancellationToken);

        foreach (var tweak in tweaks)
            await ApplyTweakAsync(tweak, cancellationToken);
    }

    public async Task DownloadTweaksAsync(string tweakPath, string downloadPath, CancellationToken cancellationToken = default) {
        _config ??= await configService.GetAsync(cancellationToken)
            ?? throw new InvalidOperationException("Config has not been initialized.");

        var tweaksArchive = await downloaderService.DownloadFileAsync(_config.Options.TweakUrl, downloadPath, cancellationToken: cancellationToken)
            ?? throw new InvalidOperationException("Failed to download tweaks.");

        await extractionService.ExtractZipFile(tweaksArchive, tweakPath, cancellationToken);
    }

    private async Task<List<Tweak>> ParseTweaksAsync(string tweakPath, CancellationToken cancellationToken) {
        var tweaks = new List<Tweak>();

        if (!Directory.Exists(tweakPath))
            throw new InvalidOperationException($"Tweak directory '{tweakPath}' was not found.");

        foreach (var file in Directory.EnumerateFiles(tweakPath, "*.yml", SearchOption.AllDirectories)) {
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
#if DEBUG
        logService.ApplyingTweak(tweak);
#endif

        foreach (var action in tweak.Actions)
            await ApplyActionAsync(tweak, action, cancellationToken);

#if DEBUG
        logService.AppliedTweak(tweak);
#endif
    }

    private async Task ApplyActionAsync(Tweak tweak, ActionBase action, CancellationToken cancellationToken) {
        var returnCode = 0;

        try {
            returnCode = await action.ApplyAsync(cancellationToken);
        } catch (Exception ex) {
            logService.TweakApplyError(tweak, ex);

            if (action.IgnoreErrors)
                return;

            return;
        }

        if (returnCode != 0) {
            var ex = new InvalidOperationException(
                $"{action.GetType().Name} from tweak '{tweak.Title}' returned exit code '{returnCode}'.");

            logService.TweakApplyError(tweak, ex);

            if (action.IgnoreErrors)
                return;

            return;
        }
    }
}
