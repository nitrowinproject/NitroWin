<#
.SYNOPSIS
    Disables the OOBE after updates

.EXAMPLE
    Disable-OOBEAfterUpdates
#>

function Disable-OOBEAfterUpdates {
    $regpath = "HKLM:\SOFTWARE\Policies\Microsoft\Windows\OOBE"

    Test-RegistryPath -path $regpath

    Set-ItemProperty -Path $regpath -Name DisablePrivacyExperience -Value 1
}