using System.ServiceProcess;
using NitroWin.Helpers;
using NitroWin.Models.Tweaks.Actions.Operations;

namespace NitroWin.Models.Tweaks.Actions;

public sealed class ServiceAction : ActionBase {
    public required string Name { get; set; }
    public required ServiceOperation Operation { get; set; }

    protected override async Task<int> ApplyAsyncCore(CancellationToken cancellationToken = default) {
        switch (Operation) {
            case ServiceOperation.Delete:
                return await ProcessHelper.StartProcessAsync("sc.exe", $"delete {Name}", true, RunAs, cancellationToken);

            case ServiceOperation.Disable:
                return await ProcessHelper.StartProcessAsync("sc.exe", $"config {Name} start= disabled", true, RunAs, cancellationToken);

            case ServiceOperation.MakeManual:
                return await ProcessHelper.StartProcessAsync("sc.exe", $"config {Name} start= demand", true, RunAs, cancellationToken);

            case ServiceOperation.MakeDelayed:
                return await ProcessHelper.StartProcessAsync("sc.exe", $"config {Name} start= delayed-auto", true, RunAs, cancellationToken);
        }

        using ServiceController sc = new(Name);
        switch (Operation) {
            case ServiceOperation.Start:
                if (sc.Status == ServiceControllerStatus.Stopped)
                    sc.Start();
                break;

            case ServiceOperation.Stop:
                if (sc.Status == ServiceControllerStatus.Running)
                    sc.Stop();
                break;

            case ServiceOperation.Pause:
                if (sc.CanPauseAndContinue && sc.Status == ServiceControllerStatus.Running)
                    sc.Pause();
                break;

            case ServiceOperation.Continue:
                if (sc.CanPauseAndContinue && sc.Status == ServiceControllerStatus.Paused)
                    sc.Continue();
                break;
        }

        await ServiceHelper.WaitForStatusAsync(sc, GetTargetStatus(), TimeSpan.FromSeconds(10), cancellationToken);
        return 0;
    }

    private ServiceControllerStatus GetTargetStatus() =>
        Operation switch {
            ServiceOperation.Start => ServiceControllerStatus.Running,
            ServiceOperation.Stop => ServiceControllerStatus.Stopped,
            ServiceOperation.Pause => ServiceControllerStatus.Paused,
            ServiceOperation.Continue => ServiceControllerStatus.Running,
            _ => throw new NotImplementedException()
        };

    public ServiceAction() {
        if (RunAs == Privilege.TrustedInstaller) throw new NotImplementedException();
    }
}
