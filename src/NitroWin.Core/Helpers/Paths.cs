namespace NitroWin.Core.Helpers;

public static class Paths {
    public static readonly string NitroWinPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
        "NitroWin");

    public static readonly string ConfigPath = Path.Combine(
        NitroWinPath,
        "Configuration");

    public static readonly string DownloadPath = Path.Combine(
        NitroWinPath,
        "Downloads");

    public static readonly string TweakPath = Path.Combine(
        NitroWinPath,
        "Tweaks");
}
