<#
.SYNOPSIS
    Disables SMB bandwith throttling

.EXAMPLE
    Disable-SMBThrottling
#>

function Disable-SMBThrottling {
    $regpath = "HKLM:\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters"

    Set-ItemProperty -Path $regpath -Name DisableBandwidthThrottling -Value 1
}