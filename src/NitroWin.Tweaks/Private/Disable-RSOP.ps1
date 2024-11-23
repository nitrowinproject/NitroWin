<#
.SYNOPSIS
    Disables RSOP (resultant set of policy logging)

.EXAMPLE
    Disable-OOBEAfterUpdates
#>

function Disable-RSOP {
    $regpath = "HKLM:\SOFTWARE\Policies\Microsoft\Windows\System"

    Test-RegistryPath -path $regpath

    Set-ItemProperty -Path $regpath -Name RSoPLogging -Value 0 -Type DWord
}