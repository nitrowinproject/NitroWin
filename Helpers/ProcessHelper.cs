using System.Diagnostics;

namespace NitroWin.Helpers
{
    public static class ProcessHelper
    {
        public static async Task<int> StartProcessAsync(string fileName, string? arguments = null, bool visible = true)
        {
            var startInfo = new ProcessStartInfo()
            {
                FileName = fileName,
                Arguments = arguments,
                UseShellExecute = visible,
                WindowStyle = visible switch
                {
                    false => ProcessWindowStyle.Hidden,
                    _ => ProcessWindowStyle.Normal
                },
                RedirectStandardOutput = !visible,
                RedirectStandardError = !visible,
                CreateNoWindow = !visible
            };

            using var process = Process.Start(startInfo) ?? throw new NullReferenceException();
            await process.WaitForExitAsync();

            return process.ExitCode;
        }
    }
}
