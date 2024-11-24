<#
.SYNOPSIS
    Disables Copilot and Recall

.EXAMPLE
    Disable-AI
#>

function Disable-AI {
    Stop-Process -Name "explorer.exe" -Force | Out-Null

    $paths = @{
        "HKCU:\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced" = @{
            "ShowCopilotButton" = 0
        }
        "HKCU:\Software\Policies\Microsoft\Windows\WindowsCopilot"          = @{
            "TurnOffWindowsCopilot" = 1
        }
        "HKLM:\SOFTWARE\Policies\Microsoft\Windows\WindowsAI"               = @{
            "DisableAIDataAnalysis" = 1
        }
    }

    foreach ($path in $paths.Keys) {
        Test-RegistryPath -Path $path
        foreach ($key in $paths[$path].Keys) {
            Set-ItemProperty -Path $path -Name $key -Value $paths[$path][$key] -Type DWord
        }
    }

    try {
        Get-AppxPackage -AllUsers Microsoft.Copilot* | Remove-AppxPackage -AllUsers
    }
    catch {
        Show-Prompt -message "Failed to remove Copilot." -title "Failed to remove Copilot" -buttons OK -icon Error
    }

    Start-Process -FilePath "explorer.exe" | Out-Null
}