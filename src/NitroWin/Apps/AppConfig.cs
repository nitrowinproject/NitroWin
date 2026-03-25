namespace NitroWin.Apps
{
    public class AppConfig
    {
        public string? Name { get; set; }
        public string? Author { get; set; }
        public List<AppBase> Apps { get; set; } = [];
    }
}
