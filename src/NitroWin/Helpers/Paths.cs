namespace NitroWin.Helpers;

internal static class Paths {
    internal static string NitroWinPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
        "NitroWin");

    internal static string DownloadPath = Path.Combine(
        NitroWinPath,
        "Downloads");

    internal static string TweakPath = Path.Combine(
        NitroWinPath,
        "Tweaks");
}
