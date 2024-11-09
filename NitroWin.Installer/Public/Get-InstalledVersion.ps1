<#
.SYNOPSIS
    Returns the installed version of NitroWin

.DESCRIPTION
    If NitroWin is not installed, it returns null.

.EXAMPLE
    Get-InstalledVersion
#>

function Get-InstalledVersion {
    if (Test-Path env:NitroWinVersion) {
        return $env:NitroWinVersion
    }
    else {
        return $null
    }
}