<#
.SYNOPSIS
    Initializes the FinishView to run correctly

.DESCRIPTION
    Runs automatically when the NitroWin.GUI package is imported.

.EXAMPLE
    Initialize-FinishView
#>

function Initialize-FinishView {
    $finishViewForm = Initialize-Form -xamlfile ".\src\NitroWin.GUI\GUI\FinishView.xaml"

    $FinishContinueButton.Add_Click({
        Restart-Computer
    })

    return $finishViewForm
}