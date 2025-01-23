<#
.SYNOPSIS
    Runs all tweaks from NitroWin

.EXAMPLE
    Invoke-Tweaks
#>

function Invoke-Tweaks {
    Disable-AI
    Start-Process reg -ArgumentList "import .\NitroWin.Tweaks\Tweaks\Disable-AutomaticDriverInstall.reg"
    Start-Process reg -ArgumentList "import .\NitroWin.Tweaks\Tweaks\Disable-ErrorReporting.reg"
    Start-Process reg -ArgumentList "import .\NitroWin.Tweaks\Tweaks\Disable-ErrorReportingExtended.reg"
    Start-Process reg -ArgumentList "import .\NitroWin.Tweaks\Tweaks\Disable-Experimentation.reg"
    Start-Process reg -ArgumentList "import .\NitroWin.Tweaks\Tweaks\Set-ExplorerSettings.reg"
    Start-Process reg -ArgumentList "import .\NitroWin.Tweaks\Tweaks\Disable-OOBEAfterUpdates.reg"
    Start-Process reg -ArgumentList "import .\NitroWin.Tweaks\Tweaks\Disable-ProgramCompatibilityAssistent.reg"
    Start-Process reg -ArgumentList "import .\NitroWin.Tweaks\Tweaks\Disable-PT.reg"
    Start-Process reg -ArgumentList "import .\NitroWin.Tweaks\Tweaks\Disable-RSOP.reg"
    Start-Process reg -ArgumentList "import .\NitroWin.Tweaks\Tweaks\Disable-SoundReductionOnCall.reg"
    Start-Process reg -ArgumentList "import .\NitroWin.Tweaks\Tweaks\Disable-Telemetry.reg"
    Disable-Telemetry
    Start-Process reg -ArgumentList "import .\NitroWin.Tweaks\Tweaks\Disable-WallpaperCompression.reg"
    Start-Process reg -ArgumentList "import .\NitroWin.Tweaks\Tweaks\Hide-UnusedWinDefenderPages.reg"
    Start-Process reg -ArgumentList "import .\NitroWin.Tweaks\Tweaks\Set-KeyboardDelay.reg"
    Set-LocalTimeServers
    Start-Process reg -ArgumentList "import .\NitroWin.Tweaks\Tweaks\Set-NetworkShareSettings.reg"
    Start-Process reg -ArgumentList "import .\NitroWin.Tweaks\Tweaks\Set-SearchSettings.reg"
    Set-NTFSOptions
    Start-Process reg -ArgumentList "import .\NitroWin.Tweaks\Tweaks\Enable-PowerPlanImporting.reg"
}