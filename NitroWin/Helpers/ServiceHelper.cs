using System.ServiceProcess;

namespace NitroWin.Helpers;

internal static class ServiceHelper {
    internal static async Task WaitForStatusAsync(ServiceController sc, ServiceControllerStatus desiredStatus, TimeSpan timeout, CancellationToken cancellationToken = default) {
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(timeout);

        while (sc.Status != desiredStatus) {
            try {
                await Task.Delay(500, cts.Token);
            } catch (OperationCanceledException) when (cts.IsCancellationRequested && !cancellationToken.IsCancellationRequested) {
                throw new System.TimeoutException();
            }
            sc.Refresh();
        }
    }
}
