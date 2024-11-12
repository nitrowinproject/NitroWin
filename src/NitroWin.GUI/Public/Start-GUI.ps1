<#
.SYNOPSIS
    Opens the main window

.DESCRIPTION
    Content defaults to the WelcomeView

.EXAMPLE
    Start-GUI
#>

function Start-GUI {
    $mainWindow.ShowDialog() | Out-Null
}