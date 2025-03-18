<#
.SYNOPSIS
    Disables Copilot and Recall

.EXAMPLE
    Disable-AI
#>

function Disable-AI {
    try {
        Get-AppxPackage -AllUsers Microsoft.Copilot* | Remove-AppxPackage -AllUsers
    }
    catch {
        Show-Prompt -message "Failed to remove Copilot." -title "Failed to remove Copilot" -buttons OK -icon Error
    }
}