<#
.SYNOPSIS
    Initializes the LicenseView to run correctly

.DESCRIPTION
    Runs automatically when the NitroWin.GUI package is imported.

.EXAMPLE
    Initialize-LicenseView
#>

function Initialize-LicenseView {
    $licenseViewForm = Initialize-Form -xamlfile ".\NitroWin.GUI\GUI\LicenseView.xaml"

    $LicenseCheckBox.Add_Checked({
        $LicenseContinueButton.isEnabled = $true
    })

    $LicenseCheckBox.Add_Unchecked({
        $LicenseContinueButton.isEnabled = $false
    })

    $LicenseContinueButton.Add_Click({
        $Global:mainWindow.Content = Initialize-AppSelectionView
    })

    return $licenseViewForm
}