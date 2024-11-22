<#
.SYNOPSIS
    Disables the IPv6 protocol on all network interfaces

.EXAMPLE
    Disable-IPv6
#>

function Disable-IPv6 {
    Disable-NetAdapterBinding -Name "*" -ComponentID ms_tcpip6
}