using Microsoft.Win32.TaskScheduler;
using NitroWin.Models.Tweaks.Actions.Operations;

namespace NitroWin.Models.Tweaks.Actions;

public sealed class ScheduledTaskAction : ActionBase {
    public required string Path { get; set; }
    public required ScheduledTaskOperation Operation { get; set; }

    protected override Task<int> ApplyAsyncCore(CancellationToken cancellationToken = default) {
        using var ts = new TaskService();

        var task = ts.GetTask(Path) ?? throw new NullReferenceException();

        switch (Operation) {
            case ScheduledTaskOperation.Disable:
                task.Enabled = false;
                break;

            case ScheduledTaskOperation.Delete:
                ts.RootFolder.DeleteTask(Path, false);
                break;
        }

        return System.Threading.Tasks.Task.FromResult(0);
    }

    public ScheduledTaskAction() {
        if (RunAs == Privilege.TrustedInstaller) throw new NotImplementedException();
    }
}
