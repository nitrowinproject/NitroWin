<#
.SYNOPSIS
    Initializes the MainView to run correctly

.DESCRIPTION
    Runs automatically when the NitroWin.GUI package is imported.

.EXAMPLE
    Initialize-MainWindow
#>

function Initialize-MainView {
    $mainViewForm = Initialize-Form -xamlfile ".\src\NitroWin.GUI\GUI\MainView.xaml"

    # Define elements
    # Top part
    $TopLogoImage = $mainViewForm.FindName("MainViewTopLogoImage")
    $GPUOptionComboBox = $mainViewForm.FindName("MainViewGPUOptionComboBox")

    # Middle part
    $BraveCheckBox = $mainViewForm.FindName("MainViewBraveCheckBox")
    $FirefoxCheckBox = $mainViewForm.FindName("MainViewFirefoxCheckBox")
    $7ZipCheckBox = $mainViewForm.FindName("MainView7ZipCheckBox")
    $WinRARCheckBox = $mainViewForm.FindName("MainViewWinRARCheckBox")
    $KLCPCheckBox = $mainViewForm.FindName("MainViewKLCPCheckBox")
    $VLCCheckBox = $mainViewForm.FindName("MainViewVLCCheckBox")

    $WinUtilCheckBox = $mainViewForm.FindName("MainViewWinUtilCheckBox")
    $OOSUCheckBox = $mainViewForm.FindName("MainViewOOSUCheckBox")
    $KeePassXCCheckBox = $mainViewForm.FindName("MainViewKeePassXCCheckBox")
    $UniGetUICheckBox = $mainViewForm.FindName("MainViewUniGetUICheckBox")
    $NotepadPPCheckBox = $mainViewForm.FindName("MainViewNotepadPPCheckBox")
    $PowerShell7CheckBox = $mainViewForm.FindName("MainViewPowerShell7CheckBox")

    # Bottom part
    $ContinueCurrentSystemButton = $mainViewForm.FindName("MainViewContinueCurrentSystemButton")
    $ContinueInstallMediaButton = $mainViewForm.FindName("MainViewContinueInstallMediaButton")

    # Load image in top part
    $imagePath = Join-Path -Path (Get-Location) -ChildPath "assets/logo/NitroWin.png"
    $imageSource = New-Object System.Windows.Media.Imaging.BitmapImage
    $imageSource.BeginInit()
    $imageSource.UriSource = [Uri]::new("file:///$imagePath")
    $imageSource.EndInit()

    $TopLogoImage.Source = $imageSource

    # Set Other as default option in ComboBox
    $GPUOptionComboBox.SelectedIndex = 0
    

    return $mainViewForm
}