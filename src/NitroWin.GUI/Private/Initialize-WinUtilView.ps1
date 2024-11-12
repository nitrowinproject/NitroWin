<#
.SYNOPSIS
    Initializes the WinUtilView to run correctly

.DESCRIPTION
    Runs automatically when the NitroWin.GUI package is imported.

.EXAMPLE
    Initialize-WinUtilView
#>

function Initialize-WinUtilView {
    $winUtilViewForm = Initialize-Form -xamlfile ".\src\NitroWin.GUI\GUI\WinUtilView.xaml"

    return $winUtilViewForm
}