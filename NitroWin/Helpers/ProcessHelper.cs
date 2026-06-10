using System.Diagnostics;
using Microsoft.Win32.TaskScheduler;
using NitroWin.Models.Tweaks;

namespace NitroWin.Helpers;

internal static class ProcessHelper {
    internal static async Task<int> StartProcessAsync(string fileName, string? arguments = null, bool visible = true, Privilege privilege = Privilege.CurrentUserElevated, CancellationToken cancellationToken = default) {
        if (privilege == Privilege.TrustedInstaller)
            return await RunAsTrustedInstallerAsync(fileName, arguments, cancellationToken);

        var startInfo = new ProcessStartInfo() {
            FileName = fileName,
            Arguments = arguments,
            UseShellExecute = visible,
            WindowStyle = visible ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden,
            RedirectStandardOutput = !visible,
            RedirectStandardError = !visible,
            CreateNoWindow = !visible
        };

        using var process = Process.Start(startInfo) ?? throw new InvalidOperationException();
        await process.WaitForExitAsync(cancellationToken);

        return process.ExitCode;
    }

    internal static async Task<bool> IsAppAvailable(string fileName, string? arguments = null, CancellationToken cancellationToken = default) {
        try {
            var exitCode = await StartProcessAsync(fileName, arguments, false, cancellationToken: cancellationToken);
            return exitCode == 0;
        } catch {
            return false;
        }
    }

    private static async Task<int> RunAsTrustedInstallerAsync(string filePath, string? arguments, CancellationToken cancellationToken = default) {
        var taskName = "NitroWinTweak_" + Guid.NewGuid().ToString("N");

        using TaskService ts = new();

        var td = ts.NewTask();
        td.Actions.Add(new ExecAction(filePath, arguments));

        ts.RootFolder.RegisterTaskDefinition(taskName, td);

        var task = ts.GetTask(taskName);

        var runningTask = task.RunEx(TaskRunFlags.NoFlags, 0, @"NT SERVICE\TrustedInstaller")
            ?? throw new InvalidOperationException($"Failed to start task '{taskName}' as TrustedInstaller.");

        int result;

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromMinutes(3));

        try {
            while (runningTask.State == TaskState.Running) {
                await System.Threading.Tasks.Task.Delay(200, cts.Token);
                try {
                    runningTask.Refresh();
                } catch {
                    break;
                }
            }

            result = task.LastTaskResult;
        } finally {
            ts.RootFolder.DeleteTask(taskName);
        }

        return result;
    }
}
