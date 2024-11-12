<#
.SYNOPSIS
    Initializes the helper to run correctly

.DESCRIPTION
    Runs automatically when the NitroWin.Helper module is imported.

.EXAMPLE
    Initialize-Helper
#>

function Initialize-Helper {
    [System.Reflection.Assembly]::LoadWithPartialName("System.Windows.Forms")
    [System.Windows.Forms.Application]::EnableVisualStyles();

    # Enable TLS 1.2
    [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12

    Set-ExecutionPolicy Unrestricted -Scope Process -Force
}