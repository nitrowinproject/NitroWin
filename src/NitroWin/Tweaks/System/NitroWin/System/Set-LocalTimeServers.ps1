# The base idea of this tweak comes from Atlas, but I modified it to give me more local time servers.

$region = ((Get-WinSystemLocale).Name).Split("-")[1].ToLower()

$servers = "0.$region.pool.ntp.org 1.$region.pool.ntp.org 2.$region.pool.ntp.org 3.$region.pool.ntp.org"

Start-Process -wait -FilePath "w32tm.exe" -ArgumentList "/config /syncfromflags:manual /manualpeerlist:`"$servers`""
Start-Process -wait -FilePath "w32tm.exe" -ArgumentList "/config /update"
Start-Process -wait -FilePath "w32tm" -ArgumentList "/resync"