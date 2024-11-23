<#
.SYNOPSIS
    Disables the Program Compatibility Assistent

.EXAMPLE
    Disable-ProgramCompatibilityAssistent
#>

function Disable-ProgramCompatibilityAssistent {
    $regpath = "HKLM:\SOFTWARE\Policies\Microsoft\Windows\AppCompat"

    Test-RegistryPath -path $regpath

    $names = @("AITEnable", "AllowTelemetry", "DisableEngine", "DisableInventory", "DisablePCA", "DisableUAR")
    $values = @(0, 0, 1, 1, 1, 1)

    for (($i = 0); $i -lt $names.Count; $i++) {
        Set-ItemProperty -Path $regpath -Name $names[$i] -Value $values[$i] -Type DWord
    }
}