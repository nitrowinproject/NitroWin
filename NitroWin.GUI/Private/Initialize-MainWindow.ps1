<#
.SYNOPSIS
    Initializes the MainWindow run correctly

.DESCRIPTION
    Runs automatically when the NitroWin.GUI package is imported.

.EXAMPLE
    Initialize-MainWindow
#>

function Initialize-MainWindow {
    $mainWindow = Initialize-Form -xamlfile ".\NitroWin.GUI\GUI\MainWindow.xaml"

    $Global:MainContent.Content = $Global:welcomeViewForm

    return $mainWindow
}