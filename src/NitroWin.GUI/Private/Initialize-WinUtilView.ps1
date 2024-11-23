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
    
    $WinUtilCheckBox.Add_Checked({
        $WinUtilContinueButton.isEnabled = $true
    })

    $WinUtilCheckBox.Add_Unchecked({
        $WinUtilContinueButton.isEnabled = $false
    })

    $WinUtilSkipButton.Add_Click({
        if ((Show-Prompt -message "Install Ultimate Power Plan without CTT branding?" -buttons YesNo -icon Question) -eq "Yes") {
            Install-UltimatePowerPlan
        }
        $Global:mainWindow.Content = $Global:finishView
    })

    $WinUtilContinueButton.Add_Click({
        if ((Show-Prompt -message "Install Ultimate Power Plan without CTT branding?" -buttons YesNo -icon Question) -eq "Yes") {
            Install-UltimatePowerPlan
        }
        Invoke-WinUtil
        $Global:mainWindow.Content = $Global:finishView
    })

    return $winUtilViewForm
}