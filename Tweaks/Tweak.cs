namespace NitroWin.Tweaks
{
    public class Tweak
    {
        public string FileName => Url.Split("/").Last();
        public required string Url { get; set; }
        public required TweakScope Scope { get; set; }
        public required TweakType Type { get; set; }
    }
}
