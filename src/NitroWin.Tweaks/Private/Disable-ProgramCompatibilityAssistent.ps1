<#
.SYNOPSIS
    Disables the Program Compatibility Assistent

.EXAMPLE
    Disable-ProgramCompatibilityAssistent
#>

function Disable-ProgramCompatibilityAssistent {
    $paths = @{
        "HKLM:\SOFTWARE\Policies\Microsoft\Windows\AppCompat" = @{
            "AITEnable"        = 0
            "AllowTelemetry"   = 0
            "DisableEngine"    = 1
            "DisableInventory" = 1
            "DisablePCA"       = 1
            "DisableUAR"       = 1
        }
    }

    foreach ($path in $paths.Keys) {
        Test-RegistryPath -Path $path
        foreach ($key in $paths[$path].Keys) {
            Set-ItemProperty -Path $path -Name $key -Value $paths[$path][$key] -Type DWord
        }
    }
}