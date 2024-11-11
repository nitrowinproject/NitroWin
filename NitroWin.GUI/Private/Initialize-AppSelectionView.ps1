<#
.SYNOPSIS
    Initializes the AppSelectionView to run correctly

.DESCRIPTION
    Runs automatically when the NitroWin.GUI package is imported.

.EXAMPLE
    Initialize-AppSelectionView
#>

function Initialize-AppSelectionView {
    $appSelectionViewForm = Initialize-Form -xamlfile ".\NitroWin.GUI\GUI\AppSelectionView.xaml"

    $AppCheckBox.Add_Checked({
        $AppContinueButton.isEnabled = $true
    })

    $AppCheckBox.Add_Unchecked({
        $AppContinueButton.isEnabled = $false
    })
    
    $AppSkipButton.Add_click({
        $Global:mainWindow.Content = $Global:dnsViewForm
    })

    $AppContinueButton.Add_click({
        $Global:mainWindow.Content = $Global:dnsViewForm
    })

    return $appSelectionViewForm
}