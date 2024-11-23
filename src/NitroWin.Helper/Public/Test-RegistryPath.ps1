<#
.SYNOPSIS
    Checks if a registry path exists and creates it if it doesn't exist

.PARAMETER path
    The registry path to check

.EXAMPLE
    Test-RegistryPath -path "HKLM:\SOFTWARE\Example"
#>

function Test-RegistryPath {
    param (
        [string]$path
    )
    
    if (-not (Test-Path -Path $Path)) {
        New-Item -Path $Path -Force | Out-Null
    }
}
