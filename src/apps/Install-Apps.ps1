function Install-Apps {
    <#
    .SYNOPSIS
        Installs all apps that were previously selected in the config file.
    #>

    foreach ($drive in (Get-PsDrive -PsProvider FileSystem)) {
        $configPath = Join-Path -Path "$($drive.Name):" -ChildPath "NitroWin.Apps.txt"
        if (Test-Path -Path $configPath -PathType Leaf) { 
            $config = Get-Content -Path $configPath
            break
        }
    }

    if (-Not $config) {
        $config = (Invoke-WebRequest -Uri "https://raw.githubusercontent.com/nitrowinproject/NitroWin/main/assets/Configuration/NitroWin.Apps.txt").Content
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