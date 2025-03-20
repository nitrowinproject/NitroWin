[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
$downloadFolder = (New-Object -ComObject Shell.Application).NameSpace('shell:Downloads').Self.Path
$outFile = Join-Path $downloadFolder "Firefox.exe"
$url = "https://download.mozilla.org/?product=firefox-latest-ssl&os=win64&lang=$((Get-WinSystemLocale).Name)"
Invoke-WebRequest $url -OutFile $outFile
Start-Process $outFile -Wait