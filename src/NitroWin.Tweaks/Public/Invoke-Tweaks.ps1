<#
.SYNOPSIS
    Runs all tweaks from NitroWin

.EXAMPLE
    Invoke-Tweaks
#>

function Invoke-Tweaks {
    $tweakFiles = Get-ChildItem -Path ".\src\NitroWin.Tweaks\Tweaks" -Filter "*.reg" -Recurse

    foreach ($file in $tweakFiles) {
        try {
            Start-Process -FilePath "reg" -ArgumentList "import `"$($file.FullName)`"" -Wait -NoNewWindow
        }
        catch {
            Show-Prompt -message "$($_.Exception.Message)" -title "Error while importing file: $($file.FullName)" -buttons OK -icon Error
        }
    }

    Disable-AI
    Disable-Telemetry
    Set-LocalTimeServers
    Set-NTFSOptions
}