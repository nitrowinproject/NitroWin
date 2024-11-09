<#
.SYNOPSIS
    Initializes the helper to run correctly

.DESCRIPTION
    Runs automatically when the NitroWin.Helper package is imported.

.EXAMPLE
    Initialize-Helper
#>

function Initialize-Helper {
    [System.Reflection.Assembly]::LoadWithPartialName("System.Windows.Forms")
    [System.Windows.Forms.Application]::EnableVisualStyles();
}