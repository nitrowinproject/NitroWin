using NitroWin.Helpers;
using NitroWin.Helpers.Downloader;
using System.Diagnostics;

namespace NitroWin.Tweaks
{
    public static class TweakLoader
    {
        private static List<Tweak> _tweaks = [
            new Tweak(){
                Url = "https://raw.githubusercontent.com/nitrowinproject/Tweaks/v3/Generated/NitroWin.Tweaks.User.reg",
                Scope = TweakScope.User,
                Type = TweakType.Registry
            },
            new Tweak(){
                Url = "https://raw.githubusercontent.com/nitrowinproject/Tweaks/v3/Generated/NitroWin.Tweaks.User.ps1",
                Scope = TweakScope.User,
                Type = TweakType.Script
            },
            new Tweak(){
                Url = "https://raw.githubusercontent.com/nitrowinproject/Tweaks/v3/Generated/NitroWin.Tweaks.System.reg",
                Scope = TweakScope.System,
                Type = TweakType.Registry
            },
            new Tweak(){
                Url = "https://raw.githubusercontent.com/nitrowinproject/Tweaks/v3/Generated/NitroWin.Tweaks.System.ps1",
                Scope = TweakScope.System,
                Type = TweakType.Script
            }
        ];

        private static async Task ApplyScript(Tweak tweak)
        {
            var tweakPath = Path.Join("Tweaks", tweak.FileName);

            var startInfo = tweak.Scope switch
            {
                _ => new ProcessStartInfo()
                {
                    FileName = "powershell.exe",
                    Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{tweakPath}\""
                }
            };

            var process = Process.Start(startInfo);

            if (process == null) { return; }

            await process.WaitForExitAsync();
        }

        private static async Task ApplyRegistry(Tweak tweak)
        {
            var tweakPath = Path.Join("Tweaks", tweak.FileName);

            var startInfo = tweak.Scope switch
            {
                _ => new ProcessStartInfo()
                {
                    FileName = "reg.exe",
                    Arguments = $"import {tweakPath}"
                }
            };

            var process = Process.Start(startInfo);

            if (process == null) { return; }

            await process.WaitForExitAsync();
        }

        private static async Task ApplyTweak(Tweak tweak)
        {
            switch (tweak.Type)
            {
                case TweakType.Script:
                    await ApplyScript(tweak);
                    break;
                case TweakType.Registry:
                    await ApplyRegistry(tweak);
                    break;
            }
        }

        private static async Task DownloadTweaks()
        {
            Console.WriteLine(Globals.StringsResourceManager.GetString("TweakLoader_DownloadingTweaks"));
            try
            {
                await Task.WhenAll(_tweaks.Select(tweak => FileDownloader.DownloadFileAsync(tweak.Url, "Tweaks")));
            }
            catch (Exception ex)
            {
                ConsoleHelper.WriteError(Globals.StringsResourceManager.GetString("TweakLoader_DownloadError") + ex.Message);
            }
        }

        public static async Task ApplyTweaks()
        {
            await DownloadTweaks();

            Console.WriteLine(Globals.StringsResourceManager.GetString("TweakLoader_ApplyingTweaks"));
            foreach (var tweak in _tweaks)
            {
                try
                {
                    await ApplyTweak(tweak);
                }
                catch (Exception ex)
                {
                    ConsoleHelper.WriteError(Globals.StringsResourceManager.GetString("TweakLoader_ApplyError") + ex.Message);
                }
            }
        }
    }
}
