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

    $WinUtilContinueButton.Add_Click({
        Invoke-WinUtil
        $Global:mainWindow.Content = $Global:finishView
    })

    return $winUtilViewForm
}