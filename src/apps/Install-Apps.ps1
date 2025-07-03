function Install-Apps {
    <#
    .SYNOPSIS
        Installs applications based on the configuration defined in "NitroWin.Apps.txt".
        The configuration file is searched on all local drives. If not found locally,
        it will be downloaded from the NitroWin GitHub repository.
    #>

    foreach ($drive in (Get-PsDrive -PsProvider FileSystem)) {
        $configPath = Join-Path -Path "$($drive.Name):" -ChildPath "NitroWin.Apps.txt"
        if (Test-Path -Path $configPath -PathType Leaf) {
            Write-Host "Found config under $configPath! Continuing with this configuration..."
            $config = Get-Content -Path $configPath
            break
        }
    }

    if (-Not $config) {
        Write-Host "No configuration found. Downloading from GitHub..."
        try {
            $config = $httpClient.GetStringAsync("https://raw.githubusercontent.com/nitrowinproject/NitroWin/main/assets/Configuration/NitroWin.Apps.txt").Result -split "`r?`n"
            Write-Host "The configuration was downloaded successfully!"
        }
        catch {
            Show-InstallError -name "NitroWin.Apps.txt"
        }
    }

    foreach ($app in $config) {
        if ($app.Split(";")[0] -eq "web") {
            Install-App -url $app.Split(";")[1]
        }
        elseif ($app.Split(";")[0] -eq "winget") {
            Install-AppFromWinGet -id $app.Split(";")[1]
        }
    }
}