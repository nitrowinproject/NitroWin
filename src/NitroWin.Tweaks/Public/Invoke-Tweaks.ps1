<#
.SYNOPSIS
    Runs all tweaks from NitroWin

.EXAMPLE
    Invoke-Tweaks
#>

function Invoke-Tweaks {
    $tweakFiles = Get-ChildItem -Path ".\src\NitroWin.Tweaks\Tweaks" -Filter "*.reg" -Recurse
    
    foreach ($file in $tweakFiles) {
        Write-Host "Importing $($file.FullName)..."
        Start-Process -FilePath "reg" -ArgumentList "import `"$($file.FullName)`"" -Wait -NoNewWindow
    }

    Disable-AI
    Disable-Telemetry
    Set-LocalTimeServers
    Set-NTFSOptions
}