using NitroWin.Helpers;
using System.Runtime.InteropServices;

namespace NitroWin.Apps
{
    public abstract class AppBase
    {
        public List<string>? Arguments { get; set; }
        public Architectures Architectures { get; set; } = new();


        public async Task InstallAsync()
        {
            if (!IsSupportedArchitecture())
            {
                LogHelper.NotInstallingApp(this);
                return;
            }

            LogHelper.InstallingApp(this);

            try
            {
                await InstallCoreAsync();
            }
            catch (Exception ex)
            {
                LogHelper.AppInstallError(this, ex);
            }
        }

        protected abstract Task InstallCoreAsync();

        protected bool IsSupportedArchitecture()
        {
            return (RuntimeInformation.ProcessArchitecture == Architecture.X64 && Architectures.X64)
                || (RuntimeInformation.ProcessArchitecture == Architecture.Arm64 && Architectures.Arm64);
        }
    }
}
