[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
$downloadFolder = (New-Object -ComObject Shell.Application).NameSpace('shell:Downloads').Self.Path
$outFile = Join-Path $downloadFolder "Brave.exe"
$url = "https://laptop-updates.brave.com/latest/winx64"
Invoke-WebRequest $url -OutFile $outFile
Start-Process $outFile -Wait