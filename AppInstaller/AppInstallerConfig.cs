namespace NitroWin.AppInstaller
{
    public class AppInstallerConfig
    {
        public List<WebApp> Web { get; set; } = [];
        public List<WingetApp> Winget { get; set; } = [];
    }
}
