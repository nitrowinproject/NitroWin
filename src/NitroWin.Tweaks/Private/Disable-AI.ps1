<#
.SYNOPSIS
    Disables Copilot and Recall

.EXAMPLE
    Disable-AI
#>

function Disable-AI {
    # Disable Recall
    $recallregpath = "HKLM:\SOFTWARE\Policies\Microsoft\Windows\WindowsAI"
    $recallname = "DisableAIDataAnalysis"
    $recallvalue = 1
    $recalltype = "DWord"

    Test-RegistryPath -path $recallregpath

    Set-ItemProperty -Path $recallregpath -Name $recallname -Value $recallvalue -Type $recalltype

    # Disable and remove Copilot
    Get-AppxPackage -AllUsers Microsoft.Copilot* | Remove-AppxPackage -AllUsers

    $copilotpaths = @{
        "HKCU:\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced" = @{
            "ShowCopilotButton" = 0
        }
        "HKCU:\Software\Policies\Microsoft\Windows\WindowsCopilot"          = @{
            "TurnOffWindowsCopilot" = 1
        }
    }

    foreach ($path in $copilotpaths.Keys) {
        Test-RegistryPath -Path $path
        foreach ($key in $copilotpaths[$path].Keys) {
            Set-ItemProperty -Path $path -Name $key -Value $copilotpaths[$path][$key] -Type DWord
        }
    }
}