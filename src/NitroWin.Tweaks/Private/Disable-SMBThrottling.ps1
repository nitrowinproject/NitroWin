<#
.SYNOPSIS
    Disables SMB bandwith throttling

.EXAMPLE
    Disable-SMBThrottling
#>

function Disable-SMBThrottling {
    $regpath = "HKLM:\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters"

    Test-RegistryPath -path $regpath
    
    Set-ItemProperty -Path $regpath -Name DisableBandwidthThrottling -Value 1 -Type DWord
}