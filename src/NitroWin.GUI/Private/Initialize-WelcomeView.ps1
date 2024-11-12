<#
.SYNOPSIS
    Initializes the WelcomeView to run correctly

.DESCRIPTION
    Runs automatically when the NitroWin.GUI package is imported.

.EXAMPLE
    Initialize-WelcomeView
#>

function Initialize-WelcomeView {
    $welcomeViewForm = Initialize-Form -xamlfile ".\NitroWin.GUI\GUI\WelcomeView.xaml"

    $Global:WelcomeContinueButton.Add_Click({
        $Global:mainWindow.Content = $Global:licenseViewForm
    })

    return $welcomeViewForm
}