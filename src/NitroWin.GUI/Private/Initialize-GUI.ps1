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
    $Global:finishView = Initialize-FinishView
    $Global:appSelectionViewForm = Initialize-AppSelectionView
    $Global:licenseViewForm = Initialize-LicenseView
    $Global:welcomeViewForm = Initialize-WelcomeView

    $Global:mainWindow = Initialize-MainWindow

    $iconpath = ".\assets\logo\icon.ico"
    $Global:mainWindow.Icon = [System.Windows.Media.Imaging.BitmapFrame]::Create([Uri]$iconpath)
    
    # Export views
    Export-ModuleMember -Variable $Global:finishView
    Export-ModuleMember -Variable $Global:appSelectionViewForm
    Export-ModuleMember -Variable $Global:licenseViewForm
    Export-ModuleMember -Variable $Global:welcomeViewForm

    Export-ModuleMember -Variable $Global:mainWindow
}