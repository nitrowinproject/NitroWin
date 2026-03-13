function Get-NitroWinConfig {
    <#
    .SYNOPSIS
        Loads the NitroWin JSON configuration from local disk or downloads it from GitHub.
        Returns the parsed configuration object.
    #>

    $jsonFileName = "NitroWin.json"

    foreach ($drive in (Get-PsDrive -PsProvider FileSystem)) {
        $configPath = Join-Path -Path "$($drive.Name):" -ChildPath $jsonFileName
        if (Test-Path -Path $configPath -PathType Leaf) {
            Write-Host "Found config under $configPath! Continuing with this configuration..." -ForegroundColor Green
            return Get-Content -Path $configPath -Raw | ConvertFrom-Json
        }
    }

    Write-Host "No configuration found. Downloading from GitHub..."
    try {
        $config = $global:httpClient.GetStringAsync("https://raw.githubusercontent.com/nitrowinproject/NitroWin/main/assets/Configuration/NitroWin.json").Result | ConvertFrom-Json
        Write-Host "The configuration was downloaded successfully!" -ForegroundColor Green
        return $config
    }
    catch {
        Show-InstallError -name $jsonFileName
        return $null
    }
}
