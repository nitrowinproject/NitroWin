using System.Diagnostics;
using Microsoft.Win32.TaskScheduler;
using NitroWin.Models.Tweaks;

namespace NitroWin.Helpers;

internal static class ProcessHelper {
    internal static async Task<int> StartProcessAsync(string fileName, string? arguments = null, bool visible = true, Privilege privilege = Privilege.CurrentUserElevated) {
        if (privilege == Privilege.TrustedInstaller)
            return await RunAsTrustedInstallerAsync(fileName, arguments);

        var startInfo = new ProcessStartInfo() {
            FileName = fileName,
            Arguments = arguments,
            UseShellExecute = visible,
            WindowStyle = visible switch {
                false => ProcessWindowStyle.Hidden,
                _ => ProcessWindowStyle.Normal
            },
            RedirectStandardOutput = !visible,
            RedirectStandardError = !visible,
            CreateNoWindow = !visible
        };

        using var process = Process.Start(startInfo) ?? throw new InvalidOperationException();
        await process.WaitForExitAsync();

        return process.ExitCode;
    }

    internal static async Task<bool> IsAppAvailable(string fileName, string? arguments = null) {
        try {
            var exitCode = await StartProcessAsync(fileName, arguments, false);
            return exitCode == 0;
        } catch {
            return false;
        }
    }

    private static async Task<int> RunAsTrustedInstallerAsync(string filePath, string? arguments) {
        var taskName = "NitroWinTweak" + new Random().Next(999);

        using TaskService ts = new();

        var td = ts.NewTask();
        td.Actions.Add(new ExecAction(filePath, arguments));

        ts.RootFolder.RegisterTaskDefinition(taskName, td);

        var task = ts.GetTask(taskName);

        var runningTask = task.RunEx(TaskRunFlags.NoFlags, 0, @"NT SERVICE\TrustedInstaller");

        try {
            while (runningTask != null && runningTask.State == TaskState.Running) {
                await System.Threading.Tasks.Task.Delay(200);
                try {
                    runningTask.Refresh();
                } catch {
                    break;
                }
            }
        } catch { }

        var result = task.LastTaskResult;

        ts.RootFolder.DeleteTask(taskName);

        return result;
    }
}
