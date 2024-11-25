<#
.SYNOPSIS
    Runs NitroWin

.DESCRIPTION
    Optimizes your Windows installation and help you set it up

.NOTES
    https://github.com/Nitro4542/NitroWin/
#>

#Requires -RunAsAdministrator

Import-Module ".\src\NitroWin.Helper\NitroWin.Helper.psm1"
Import-Module ".\src\NitroWin.Installer\NitroWin.Installer.psm1"
Import-Module ".\src\NitroWin.Tweaks\NitroWin.Tweaks.psm1"
Import-Module ".\src\NitroWin.GUI\NitroWin.GUI.psm1"

if (-Not ($args[0] -eq "--create-install-media")) {
    Start-GUI
}
else {
    Deploy-InstallMedia -drive $args[1]
}