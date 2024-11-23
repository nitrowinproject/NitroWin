<#
.SYNOPSIS
    Sets security settings for network shares

.EXAMPLE
    Set-NetworkShareSecuritySettings
#>

function Set-NetworkShareSecuritySettings {
    $regpath = "HKLM:\SYSTEM\CurrentControlSet\Services\LanManServer\Parameters"
    $regpath2 = "HKLM:\SYSTEM\CurrentControlSet\Control\Lsa"

    Set-ItemProperty -Path $regpath -Name RestrictNullSessAccess -Value 1
    Set-ItemProperty -Path $regpath2 -Name RestrictAnonymous -Value 1
}