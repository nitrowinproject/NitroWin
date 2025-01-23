<#
.SYNOPSIS
    Initializes the GUI to run correctly

.DESCRIPTION
    Runs automatically when the NitroWin.GUI package is imported.

.EXAMPLE
    Initialize-GUI
#>

function Initialize-GUI {
    Add-Type -AssemblyName 'PresentationFramework'

    # Read Views
    $Global:mainViewForm = Initialize-MainView

    $Global:mainWindow = Initialize-MainWindow

    $iconpath = ".\assets\logo\icon.ico"
    $Global:mainWindow.Icon = [System.Windows.Media.Imaging.BitmapFrame]::Create([Uri]$iconpath)
    
    # Export views
    Export-ModuleMember -Variable $Global:mainViewForm
    
    Export-ModuleMember -Variable $Global:mainWindow
}