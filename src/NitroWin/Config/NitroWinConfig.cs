namespace NitroWin.Config
{
    public class NitroWinConfig
    {
        public string? Name { get; set; }
        public string? Author { get; set; }
        public Options Options { get; set; } = new();
    }
}
