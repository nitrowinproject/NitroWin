namespace NitroWin.Cli.Helpers;

internal static class Paths {
    internal static string NitroWinPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
        "NitroWin");

    internal static string TweakPath = Path.Combine(
        NitroWinPath,
        "Tweaks");
}
