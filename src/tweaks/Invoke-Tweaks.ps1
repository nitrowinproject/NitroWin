function Invoke-Tweaks {
    <#
    .SYNOPSIS
        Downloads and invokes all tweaks from NitroWin. Also checks config for extra tweaks.
    #>

    $jsonFileName = "NitroWin.json"

    foreach ($drive in (Get-PsDrive -PsProvider FileSystem)) {
        $configPath = Join-Path -Path "$($drive.Name):" -ChildPath $jsonFileName
        if (Test-Path -Path $configPath -PathType Leaf) {
            Write-Host "Found config under $configPath! Continuing with this configuration..." -ForegroundColor Green
            $config = Get-Content -Path $configPath -Raw | ConvertFrom-Json
            break
        }
    }

    if (-Not $config) {
        Write-Host "No configuration found. Downloading from GitHub..."
        try {
            $config = $httpClient.GetStringAsync("https://raw.githubusercontent.com/nitrowinproject/NitroWin/main/assets/Configuration/NitroWin.json").Result | ConvertFrom-Json
            Write-Host "The configuration was downloaded successfully!" -ForegroundColor Green
        }
        catch {
            Show-InstallError -name $jsonFileName
        }
    }

    $urls = @(
        "https://raw.githubusercontent.com/nitrowinproject/Tweaks/main/NitroWin.Tweaks.User.reg",
        "https://raw.githubusercontent.com/nitrowinproject/Tweaks/main/NitroWin.Tweaks.User.ps1",
        "https://raw.githubusercontent.com/nitrowinproject/Tweaks/main/NitroWin.Tweaks.System.reg",
        "https://raw.githubusercontent.com/nitrowinproject/Tweaks/main/NitroWin.Tweaks.System.ps1"
    )

    foreach ($url in $urls) {
        $file = Get-FileFromURL -url $url
        switch ($file) {
            { $_.EndsWith("User.reg") } {
                Write-Host "Importing user registry tweaks from $file..."
                Start-Process -FilePath "reg" -ArgumentList "import `"$file`"" -NoNewWindow -Wait
                Write-Host "User registry tweaks imported successfully!" -ForegroundColor Green
            }
            { $_.EndsWith("User.ps1") } {
                Write-Host "Executing user PowerShell script from $file..."
                Start-Process -FilePath "powershell.exe" -ArgumentList "-ExecutionPolicy Bypass -File `"$file`"" -NoNewWindow -Wait
                Write-Host "User PowerShell script executed successfully!" -ForegroundColor Green
            }
            { $_.EndsWith("System.reg") } {
                Write-Host "Importing system registry tweaks from $file..."
                Start-Process -FilePath (Join-Path -Path (Get-DownloadFolder) -ChildPath "RunAsTI$runAsTIBitness.exe") -ArgumentList "$env:windir\System32\reg.exe import ""$file""" -NoNewWindow -Wait
                Write-Host "System registry tweaks imported successfully!" -ForegroundColor Green
            }
            { $_.EndsWith("System.ps1") } {
                Write-Host "Executing system PowerShell script from $file..."
                Start-Process -FilePath (Join-Path -Path (Get-DownloadFolder) -ChildPath "RunAsTI$runAsTIBitness.exe") -ArgumentList "$env:windir\System32\WindowsPowerShell\v1.0\powershell.exe -ExecutionPolicy Bypass -NoProfile -File ""$file""" -NoNewWindow -Wait
                Write-Host "System PowerShell script executed successfully!" -ForegroundColor Green
            }
        }
    }

    if ($config.drivers) {
        Enable-AutomaticDriverInstallation
    }

    if ($config.store) {
        Install-MicrosoftStore
    }
}