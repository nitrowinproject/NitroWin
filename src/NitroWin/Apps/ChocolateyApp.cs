using NitroWin.Helpers;

namespace NitroWin.Apps
{
    public class ChocolateyApp : AppBase
    {
        public required string Id { get; set; }

        protected override async Task InstallCoreAsync()
        {
            await ProcessHelper.StartProcessAsync(
                "choco.exe",
                $"install {Id} --yes {Arguments}"
            );
        }
    }
}
