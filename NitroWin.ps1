<#
.SYNOPSIS
    Runs NitroWin

.DESCRIPTION
    Optimizes and helps you setting it up your Windows installation

.NOTES
    https://github.com/Nitro4542/NitroWin/
#>

#Requires -RunAsAdministrator

$Host.UI.RawUI.WindowTitle = "NitroWin"

Import-Module ".\src\NitroWin.Helper\NitroWin.Helper.psm1" -Force
Import-Module ".\src\NitroWin.Installer\NitroWin.Installer.psm1" -Force
Import-Module ".\src\NitroWin.Tweaks\NitroWin.Tweaks.psm1" -Force
Import-Module ".\src\NitroWin.GUI\NitroWin.GUI.psm1" -Force

if ($args[0] -eq "--create-install-media") {
    Write-Warning "If you want to use my fork of WinUtil to create a modified ISO, use --winutil."
    if ($args.Count -ge 2) {
        Write-Host "Deploying install media to drive $($args[1])"
        Deploy-InstallMedia -drive $args[1]
    }
    else {
        Write-Error "No drive letter specified. Exiting..."
        Exit
    }
}
elseif ($args[0] -eq "--export-registry-tweaks") {
    if ($args.Count -ge 2) {
        Merge-Tweaks -mergedFile $args[1]
    }
    else {
        Merge-Tweaks -mergedFile ".\merged.reg"
    }
}
elseif ($args[0] -eq "--winutil") {
    Invoke-Expression (Invoke-WebRequest "https://raw.githubusercontent.com/Nitro4542/winutil/main/winutil.ps1")
}
else {
    Start-GUI
}