using NitroWin.Helpers;
using NitroWin.Helpers.Downloader;
using Serilog;
using TweakLib.Models;
using TweakLib.Parser;

namespace NitroWin.Tweaks
{
    public static class TweakLoader
    {
        private static async Task DownloadTweaksAsync()
        {
            string tweaksPath = "Tweaks";

            try
            {
                string tweaksArchive = await FileDownloader.DownloadFileAsync("https://github.com/nitrowinproject/Tweaks/archive/refs/heads/v3.zip", Globals.DownloadFolder);

                await System.IO.Compression.ZipFile.ExtractToDirectoryAsync(tweaksArchive, tweaksPath, overwriteFiles: true);
            }
            catch (Exception ex)
            {
                Log.Error(Globals.StringsResourceManager.GetString("TweakLoader_DownloadError") + ex.Message);
            }
        }

        private static async Task<List<Tweak>> ParseTweaksAsync()
        {
            var tweaks = new List<Tweak>();

            foreach (string file in Directory.EnumerateFiles(Path.Join("Tweaks", "Tweaks-3", "Tweaks"), "*.yml", SearchOption.AllDirectories))
            {
                try
                {
                    var content = await File.ReadAllTextAsync(file);
                    tweaks.Add(TweakParser.Deserializer.Deserialize<Tweak>(content));
                }
                catch (Exception ex)
                {
                    Log.Error(Globals.StringsResourceManager.GetString("TweakLoader_ParseError") + Path.GetFileName(file) + ": " + ex.Message);
                }
            }

            return tweaks;
        }

        private static async Task ApplyActionAsync(Tweak tweak, ActionBase action)
        {
            try
            {
                Log.Debug(Globals.StringsResourceManager.GetString("TweakLoader_ApplyingTweak") + "'" + tweak.Title + "'...");
                await action.ApplyAsync();
            }
            catch (Exception ex)
            {
                if (!action.IgnoreErrors)
                {
                    Log.Error(Globals.StringsResourceManager.GetString("TweakLoader_ApplyError") + "'" + tweak.Title + "': " + ex.Message);
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
