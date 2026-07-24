namespace NitroWin.Models;

internal sealed record CommandLineOptions(
    bool NoApps = false,
    bool NoTweaks = false);
