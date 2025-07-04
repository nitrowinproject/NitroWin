function Install-Apps {
    <#
    .SYNOPSIS
        Installs applications based on the configuration defined in "NitroWin.Apps.json".
        The configuration file is searched on all local drives. If not found locally,
        it will be downloaded from the NitroWin GitHub repository.
    #>

    $jsonFileName = "NitroWin.Apps.json"

    foreach ($drive in (Get-PsDrive -PsProvider FileSystem)) {
        $configPath = Join-Path -Path "$($drive.Name):" -ChildPath $jsonFileName
        if (Test-Path -Path $configPath -PathType Leaf) {
            Write-Host "Found config under $configPath! Continuing with this configuration..."
            $config = Get-Content -Path $configPath -Raw | ConvertFrom-Json
            break
        }
    }

    if (-Not $config) {
        Write-Host "No configuration found. Downloading from GitHub..."
        try {
            $config = $httpClient.GetStringAsync("https://raw.githubusercontent.com/nitrowinproject/NitroWin/main/assets/Configuration/NitroWin.Apps.json").Result | Convert-FromJson
            Write-Host "The configuration was downloaded successfully!"
        }
        catch {
            Show-InstallError -name $jsonFileName
        }
    }

    foreach ($app in $config.apps) {
        if ($app.arch -notcontains $arch) { continue }

        switch ($app.source) {
            "web" {
                $arguments = $app.args -join " "
                Install-App -url $app.url -arguments $arguments
            }
            "winget" {
                $arguments = if ($app.args) { "$($app.args)" } else { "" }
                Install-AppFromWinGet -id $app.id -arguments $arguments
            }
        }
    }
}