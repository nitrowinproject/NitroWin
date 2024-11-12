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
    $Global:winUtilView = Initialize-WinUtilView
    $Global:dnsViewForm = Initialize-DNSView
    $Global:appSelectionViewForm = Initialize-AppSelectionView
    $Global:licenseViewForm = Initialize-LicenseView
    $Global:welcomeViewForm = Initialize-WelcomeView

    $Global:mainWindow = Initialize-MainWindow
    
    # Export views
    Export-ModuleMember -Variable $Global:winUtilView
    Export-ModuleMember -Variable $Global:dnsViewForm
    Export-ModuleMember -Variable $Global:appSelectionViewForm
    Export-ModuleMember -Variable $Global:licenseViewForm
    Export-ModuleMember -Variable $Global:welcomeViewForm

    Export-ModuleMember -Variable $Global:mainWindow

    <#
        $mainWindow.ShowDialog() | Out-Null
    #>
}