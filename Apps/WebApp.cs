namespace NitroWin.Apps
{
    public class WebApp
    {
        public List<string>? Arguments { get; set; }
        public string? Name { get; set; }
        public Architectures Architectures { get; set; } = new();
        public required string Url { get; set; }
    }
}
