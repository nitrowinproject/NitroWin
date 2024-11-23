<#
.SYNOPSIS
    Runs all tweaks from NitroWin

.EXAMPLE
    Invoke-Tweaks
#>

function Invoke-Tweaks {
    Disable-AI
    Disable-ErrorReporting
    Disable-Experimentation
    Disable-FrequentAppsInStartMenu
    Disable-OOBEAfterUpdates
    Disable-ProgramCompatibilityAssistent
    Disable-PT
    Disable-RSOP
    Disable-SMBThrottling
    Disable-SoundReductionOnCall
    Disable-Telemetry
    Disable-WallpaperCompression
    Hide-UnusedWinDefenderPages
    Set-LocalTimeServers
    Set-NetworkShareSecuritySettings
    Set-SearchSettings
}