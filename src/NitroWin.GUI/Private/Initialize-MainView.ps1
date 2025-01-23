<#
.SYNOPSIS
    Initializes the MainView to run correctly

.DESCRIPTION
    Runs automatically when the NitroWin.GUI package is imported.

.EXAMPLE
    Initialize-MainWindow
#>

function Initialize-MainView {
    $mainViewForm = Initialize-Form -xamlfile ".\src\NitroWin.GUI\GUI\MainView.xaml"

    return $mainViewForm
}