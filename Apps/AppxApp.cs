using NitroWin.Helpers;

namespace NitroWin.Apps
{
    public class AppxApp : AppBase
    {
        public string? Name { get; set; }
        public required string Path { get; set; }

        protected async override Task InstallCoreAsync()
        {
            await ProcessHelper.StartProcessAsync(
                "powershell.exe", $"-NoProfile -ExecutionPolicy Bypass -Command \"Add-AppxPackage -Path '{Path}'\" " + string.Join(" ", Arguments ?? []),
                false
            );
        }
    }
}
