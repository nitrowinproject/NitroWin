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
    
    # Give buttons some functionality
    $ContinueCurrentSystemButton.Add_Click({
        Invoke-Tweaks
        if ($BraveCheckBox.IsChecked) {
            Install-Brave
        }
        if ($FirefoxCheckBox.IsChecked) {
            Install-Firefox
        }
        if ($7ZipCheckBox.IsChecked) {
            Install-AppFromWinGet -id "7zip.7zip"
        }
        if ($WinRARCheckBox.IsChecked) {
            Install-AppFromWinGet -id "RARLab.WinRAR"
        }
        if ($KLCPCheckBox.IsChecked) {
            Install-AppFromWinGet -id "CodecGuide.K-LiteCodecPack.Mega"
        }
        if ($VLCCheckBox.IsChecked) {
            Install-AppFromWinGet -id "VideoLAN.VLC"
        }

        if ($WinUtilCheckBox.IsChecked) {
            Add-WinUtilShortcut
        }
        if ($OOSUCheckBox.IsChecked) {
            Install-OOSU
        }
        if ($KeePassXCCheckBox.IsChecked) {
            Install-AppFromWinGet -id "KeePassXCTeam.KeePassXC"
        }
        if ($UniGetUICheckBox.IsChecked) {
            Install-AppFromWinGet -id "MartiCliment.UniGetUI"
        }
        if ($NotepadPPCheckBox.IsChecked) {
            Install-AppFromWinGet -id "Notepad++.Notepad++"
        }
        if ($PowerShell7CheckBox.IsChecked) {
            Install-AppFromWinGet -id "Microsoft.PowerShell"
        }
    })

    $ContinueInstallMediaButton.Add_Click({
        [void][System.Reflection.Assembly]::LoadWithPartialName('Microsoft.VisualBasic')
        $driveLetter = [Microsoft.VisualBasic.Interaction]::InputBox(
            "Enter the drive letter of your installation media as shown in the example below.",
            "Enter drive letter",
            "D:\"
        )
        if (-not [string]::IsNullOrWhiteSpace($driveLetter)) {
            Deploy-InstallMedia -drive $driveLetter
        }
    })

    return $mainViewForm
}