using System.ServiceProcess;

namespace NitroWin.Helpers;

internal static class ServiceHelper {
    internal static async Task WaitForStatusAsync(ServiceController sc, ServiceControllerStatus desiredStatus, TimeSpan timeout) {
        var startTime = DateTime.UtcNow;

        while (sc.Status != desiredStatus) {
            if (DateTime.UtcNow - startTime > timeout)
                throw new System.TimeoutException($"Service did not reach status {desiredStatus}.");

            await Task.Delay(500);
            sc.Refresh();
        }
    }
}
