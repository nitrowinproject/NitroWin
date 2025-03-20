[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
$downloadFolder = (New-Object -ComObject Shell.Application).NameSpace('shell:Downloads').Self.Path
$outFile = Join-Path $downloadFolder "Discord.exe"
$url = "https://discord.com/api/downloads/distributions/app/installers/latest?channel=stable&platform=win&arch=x64"
Invoke-WebRequest $url -OutFile $outFile
Start-Process $outFile -Wait