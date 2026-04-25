using NitroWin.Helpers;
using NitroWin.Helpers.Downloader;
using Serilog;
using TweakLib.Actions;
using TweakLib.Models;
using TweakLib.Parser;

namespace NitroWin.Tweaks
{
    internal static class TweakLoader
    {
        private const string tweakPath = "Tweaks";

        private static async Task DownloadTweaksAsync()
        {
            string tweaksArchive = await FileDownloader.DownloadFileAsync(Globals.Config.Options.TweakUrl, Globals.DownloadFolder) ?? throw new NullReferenceException();
            await ExtractionHelper.ExtractZipFile(tweaksArchive, tweakPath);
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
                    LogHelper.TweakReadError(file, ex);
                }
            }

            return tweaks;
        }

        private static async Task ApplyActionAsync(Tweak tweak, ActionBase action)
        {
            try
            {
                if (await action.ApplyAsync() != 0)
                {
                    throw new InvalidOperationException();
                }

                LogHelper.AppliedTweak(tweak);
            }
            catch (Exception ex)
            {
                if (!action.IgnoreErrors)
                {
                    LogHelper.TweakApplyError(tweak, ex);
                }
            }
        }

        private static async Task ApplyTweakAsync(Tweak tweak)
        {
            LogHelper.ApplyingTweak(tweak);

            foreach (var action in tweak.Actions)
            {
                await ApplyActionAsync(tweak, action);
            }
        }

        internal static async Task ApplyTweaksAsync()
        {
            Log.Information(ResourceHelper.GetString("TweakLoader_DownloadingTweaks"));
            await DownloadTweaksAsync();

            Log.Information(ResourceHelper.GetString("TweakLoader_ApplyingTweaks"));
            var tweaks = await ParseTweaksAsync();

            await Parallel.ForEachAsync(tweaks, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount / 2 <= 1 ? 1 : Environment.ProcessorCount / 2 },
                async (tweak, _) =>
                {
                    await ApplyTweakAsync(tweak);
                });
        }
    }
}
