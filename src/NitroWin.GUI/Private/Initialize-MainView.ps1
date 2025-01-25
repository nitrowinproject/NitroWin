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
            Install-AppFromWinget -id "7zip.7zip"
        }
        if ($WinRARCheckBox.IsChecked) {
            Install-AppFromWinget -id "RARLab.WinRAR"
        }
        if ($KLCPCheckBox.IsChecked) {
            Install-AppFromWinget -id "CodecGuide.K-LiteCodecPack.Mega"
        }
        if ($VLCCheckBox.IsChecked) {
            Install-AppFromWinget -id "VideoLAN.VLC"
        }

        if ($WinUtilCheckBox.IsChecked) {
            Add-WinUtilShortcut
        }
        if ($OOSUCheckBox.IsChecked) {
            Install-OOSU
        }
        if ($KeePassXCCheckBox.IsChecked) {
            Install-AppFromWinget -id "KeePassXCTeam.KeePassXC"
        }
        if ($UniGetUICheckBox.IsChecked) {
            Install-AppFromWinget -id "MartiCliment.UniGetUI"
        }
        if ($NotepadPPCheckBox.IsChecked) {
            Install-AppFromWinget -id "Notepad++.Notepad++"
        }
        if ($PowerShell7CheckBox.IsChecked) {
            Install-AppFromWinget -id "Microsoft.PowerShell"
        }
    })

    $ContinueInstallMediaButton.Add_Click({
        [void][System.Reflection.Assembly]::LoadWithPartialName('Microsoft.VisualBasic')
        $driveLetter = [Microsoft.VisualBasic.Interaction]::InputBox(
            "Please enter the drive letter of your installation media like the example below.",
            "Enter drive letter",
            "D:\"
        )
        if (-not [string]::IsNullOrWhiteSpace($driveLetter)) {
            Deploy-InstallMedia -drive $driveLetter
        }
    })

    return $mainViewForm
}