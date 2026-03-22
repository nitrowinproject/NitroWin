using NitroWin.Helpers;

namespace NitroWin.Apps
{
    public class WingetApp : AppBase
    {
        public required string Id { get; set; }

        protected async override Task InstallCoreAsync()
        {
            await ProcessHelper.StartProcessAsync(
                "winget.exe", $"install --id {Id} --exact --accept-package-agreements --accept-source-agreements {string.Join(" ", Arguments ?? [])}"
            );
        }
    }
}
