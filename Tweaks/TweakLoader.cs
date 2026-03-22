using NitroWin.Helpers;
using NitroWin.Helpers.Downloader;
using Serilog;
using TweakLib.Models;
using TweakLib.Parser;

namespace NitroWin.Tweaks
{
    public static class TweakLoader
    {
        private const string tweakPath = "Tweaks";
        private static async Task DownloadTweaksAsync()
        {
            try
            {
                string tweaksArchive = await FileDownloader.DownloadFileAsync("https://github.com/nitrowinproject/Tweaks/archive/refs/heads/v3.zip", Globals.DownloadFolder);

                await System.IO.Compression.ZipFile.ExtractToDirectoryAsync(tweaksArchive, tweakPath, overwriteFiles: true);
            }
            catch (Exception ex)
            {
                Log.Error(Globals.StringsResourceManager.GetString("TweakLoader_DownloadError") + ex.Message);
            }
        }

        private static async Task<List<Tweak>> ParseTweaksAsync()
        {
            var tweaks = new List<Tweak>();

            foreach (string file in Directory.EnumerateFiles(Path.Join(tweakPath, "Tweaks-3", "Tweaks"), "*.yml", SearchOption.AllDirectories))
            {
                try
                {
                    var content = await File.ReadAllTextAsync(file);
                    tweaks.Add(TweakParser.Deserializer.Deserialize<Tweak>(content));
                }
                catch (Exception ex)
                {
                    LogHelper.TweakParseError(file, ex);
                }
            }

            return tweaks;
        }

        private static async Task ApplyActionAsync(Tweak tweak, ActionBase action)
        {
            try
            {
                await action.ApplyAsync();
            }
            catch (Exception ex)
            {
                if (!action.IgnoreErrors)
                {
                    LogHelper.TweakApplyError(tweak, ex);
                }
            }
        }

        public static async Task ApplyTweaksAsync()
        {
            Log.Information(Globals.StringsResourceManager.GetString("TweakLoader_DownloadingTweaks")!);
            await DownloadTweaksAsync();

            Log.Information(Globals.StringsResourceManager.GetString("TweakLoader_ApplyingTweaks")!);
            var tweaks = await ParseTweaksAsync();

            var parallelData = tweaks
                .SelectMany(tweak => tweak.Actions.Select(action => (tweak, action)));

            await Parallel.ForEachAsync(parallelData, new ParallelOptions { MaxDegreeOfParallelism = 16 },
                async (item, _) =>
                {
                    await ApplyActionAsync(item.tweak, item.action);
                });
        }
    }
}
