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
            string tweaksArchive = await FileDownloader.DownloadFileAsync("https://github.com/nitrowinproject/Tweaks/archive/refs/heads/v3.zip", Globals.DownloadFolder);

            await System.IO.Compression.ZipFile.ExtractToDirectoryAsync(tweaksArchive, tweaksPath);
        }

        private static async Task<List<Tweak>> ParseTweaksAsync()
        {
            var tweaks = new List<Tweak>();

            foreach (string file in Directory.EnumerateFiles(Path.Join("Tweaks", "Tweaks-3", "Tweaks"), "*.yml", SearchOption.AllDirectories))
            {
                var content = await File.ReadAllTextAsync(file);
                tweaks.Add(TweakParser.Deserializer.Deserialize<Tweak>(content));
            }

            return tweaks;
        }

        public static async Task ApplyTweaksAsync()
        {
            Log.Information(Globals.StringsResourceManager.GetString("TweakLoader_DownloadingTweaks")!);
            try
            {
                await DownloadTweaksAsync();
            }
            catch (Exception ex)
            {
                Log.Error(Globals.StringsResourceManager.GetString("TweakLoader_DownloadError") + ex.Message);
            }

            var tweaks = await ParseTweaksAsync();

            var actionsWithNames = tweaks
                .SelectMany(t => t.Actions.Select(a => new { TweakTitle = t.Title, Action = a }));

            await Parallel.ForEachAsync(actionsWithNames, new ParallelOptions
            {
                MaxDegreeOfParallelism = 16
            },
            async (item, _) =>
            {
                try
                {
                    await item.Action.ApplyAsync();
                }
                catch (Exception ex)
                {
                    Log.Error(Globals.StringsResourceManager.GetString("TweakLoader_ApplyError") + item.TweakTitle + ": " + ex.Message);
                }
            });
        }
    }
}
