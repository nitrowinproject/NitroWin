<#
.SYNOPSIS
    Runs NitroWin

.DESCRIPTION
    Optimizes your Windows installation and help you set it up

.EXAMPLE
    iex (irm https://raw.githubusercontent.com/Nitro4542/NitroWin/main/NitroWin.ps1)

.NOTES
    https://github.com/Nitro4542/NitroWin/
#>

Import-Module ".\src\NitroWin.Helper\NitroWin.Helper.psm1"
Import-Module ".\src\NitroWin.Installer\NitroWin.Installer.psm1"
Import-Module ".\src\NitroWin.Tweaks\NitroWin.Tweaks.psm1"
Import-Module ".\src\NitroWin.GUI\NitroWin.GUI.psm1"

Start-GUI