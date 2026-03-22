namespace NitroWin.Apps
{
    public abstract class AppBase
    {
        public List<string>? Arguments { get; set; }
        public Architectures Architectures { get; set; } = new();
    }
}
