<#
.SYNOPSIS
    Sets your time servers to a pool of local time servers from ntppool.org

.EXAMPLE
    Set-LocalTimeServers
#>

function Set-LocalTimeServers {
    $lang = Get-LangWithoutRegion

    $servers = "0.$lang.pool.ntp.org 1.$lang.pool.ntp.org 2.$lang.pool.ntp.org 3.$lang.pool.ntp.org"

    Start-Process -wait -FilePath "w32tm.exe" -ArgumentList "/config /syncfromflags:manual /manualpeerlist:`"$servers`""
    Start-Process -wait -FilePath "w32tm.exe" -ArgumentList "/config /update"
    Start-Process -wait -FilePath "w32tm" -ArgumentList "/resync"
}