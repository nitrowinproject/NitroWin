<#
.SYNOPSIS
    Initializes the AppSelectionView to run correctly

.DESCRIPTION
    Runs automatically when the NitroWin.GUI package is imported.

.EXAMPLE
    Initialize-AppSelectionView
#>

function Initialize-AppSelectionView {
    $appSelectionViewForm = Initialize-Form -xamlfile ".\src\NitroWin.GUI\GUI\AppSelectionView.xaml"

    $AppLicenseCheckBox.Add_Checked({
        $AppContinueButton.isEnabled = $true
    })

    $AppLicenseCheckBox.Add_Unchecked({
        $AppContinueButton.isEnabled = $false
    })
    
    $AppSkipButton.Add_Click({
        $Global:mainWindow.Content = $Global:dnsViewForm
    })

    $AppContinueButton.Add_Click({
        $Global:mainWindow.Content = $Global:dnsViewForm
    })

    $AppClearButton.Add_Click({
        $AppBrowserFirefoxCheckBox.isChecked = $false
        $AppBrowserBraveCheckBox.isChecked = $false

        $AppArchiving7ZipCheckBox.isChecked = $false
        $AppArchivingWinRARCheckBox.isChecked = $false

        $AppMediaPlayerVLCCheckBox.isChecked = $false
        $AppMediaPlayerKLCPCheckBox.isChecked = $false

        $AppOtherStartAllBackCheckBox.isChecked = $false
        $AppOtherKeePassXCCheckBox.isChecked = $false
        $AppOtherUniGetUICheckBox.isChecked = $false
    })

    return $appSelectionViewForm
}