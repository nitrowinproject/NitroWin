[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
$downloadFolder = (New-Object -ComObject Shell.Application).NameSpace('shell:Downloads').Self.Path
$outFile = Join-Path $downloadFolder "Steam.exe"
$url = "https://cdn.akamai.steamstatic.com/client/installer/SteamSetup.exe"
Invoke-WebRequest $url -OutFile $outFile
Start-Process $outFile -Wait
