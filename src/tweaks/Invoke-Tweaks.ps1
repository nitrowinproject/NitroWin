function Invoke-Tweaks {
    <#
    .SYNOPSIS
        Downloads and invokes all tweaks from NitroWin. Also checks config for extra tweaks.
    #>

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
                $runAsTIExe = Join-Path -Path (Get-DownloadFolder) -ChildPath "RunAsTI64.exe"
                $regArgs = "$env:windir\System32\reg.exe import \"$file\""
                Start-Process -FilePath $runAsTIExe -ArgumentList $regArgs -NoNewWindow -Wait
                Write-Host "System registry tweaks imported successfully!" -ForegroundColor Green
            }
            { $_.EndsWith("System.ps1") } {
                Write-Host "Executing system PowerShell script from $file..."
                $runAsTIExe = Join-Path -Path (Get-DownloadFolder) -ChildPath "RunAsTI64.exe"
                $psArgs = "$env:windir\System32\WindowsPowerShell\v1.0\powershell.exe -ExecutionPolicy Bypass -NoProfile -File \"$file\""
                Start-Process -FilePath $runAsTIExe -ArgumentList $psArgs -NoNewWindow -Wait
                Write-Host "System PowerShell script executed successfully!" -ForegroundColor Green
            }
        }
    }

    $config = Get-NitroWinConfig
    if (-Not $config) { return }

    if ($config.config.drivers) {
        Enable-AutomaticDriverInstallation
    }

    if ($config.config.store) {
        Install-MicrosoftStore
    }
}