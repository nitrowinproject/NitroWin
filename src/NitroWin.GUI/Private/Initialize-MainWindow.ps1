<#
.SYNOPSIS
    Initializes the MainWindow to run correctly

.DESCRIPTION
    Runs automatically when the NitroWin.GUI package is imported.

.EXAMPLE
    Initialize-MainWindow
#>

function Initialize-MainWindow {
    $mainWindow = Initialize-Form -xamlfile ".\src\NitroWin.GUI\GUI\MainWindow.xaml"

    $Global:MainContent.Content = $Global:welcomeViewForm

    return $mainWindow
}