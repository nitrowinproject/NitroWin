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

    # Load image in top part
    $imagePath = Join-Path -Path (Get-Location) -ChildPath "assets/logo/NitroWin.png"
    $imageSource = New-Object System.Windows.Media.Imaging.BitmapImage
    $imageSource.BeginInit()
    $imageSource.UriSource = [Uri]::new("file:///$imagePath")
    $imageSource.EndInit()

    $MainViewTopLogoImage.Source = $imageSource

    # Set Other as default option in ComboBox
    $MainViewGPUOptionComboBox.SelectedIndex = 0
    
    # Give buttons some functionality
    $MainViewContinueCurrentSystemButton.Add_Click({
        Invoke-Tweaks
        Show-Prompt -message "If you want to use the Ultimate Power Plan, enable it in the Control Panel. If you have multiple instances of the Ultimate Power Plan, delete all except one and use that." -title "Ultimate Power Plan" -buttons OK -icon Information
        if ($MainViewBraveCheckBox.isChecked) {
            Install-Brave
        }
        if ($MainViewFirefoxCheckBox.isChecked) {
            Install-AppFromWinGet -id "Mozilla.Firefox"
        }
        if ($MainView7ZipCheckBox.isChecked) {
            Install-AppFromWinGet -id "7zip.7zip"
        }
        if ($MainViewWinRARCheckBox.isChecked) {
            Install-AppFromWinGet -id "RARLab.WinRAR"
        }
        if ($MainViewKLCPCheckBox.isChecked) {
            Install-AppFromWinGet -id "CodecGuide.K-LiteCodecPack.Mega"
        }
        if ($MainViewVLCCheckBox.isChecked) {
            Install-AppFromWinGet -id "VideoLAN.VLC"
        }

        if ($MainViewWinUtilCheckBox.isChecked) {
            Add-WinUtilShortcut
        }
        if ($MainViewOOSUCheckBox.isChecked) {
            Install-OOSU
        }
        if ($MainViewKeePassXCCheckBox.isChecked) {
            Install-AppFromWinGet -id "KeePassXCTeam.KeePassXC"
        }
        if ($MainViewUniGetUICheckBox.isChecked) {
            Install-AppFromWinGet -id "MartiCliment.UniGetUI"
        }
        if ($MainViewNotepadPPCheckBox.isChecked) {
            Install-AppFromWinGet -id "Notepad++.Notepad++"
        }
        if ($MainViewPowerShell7CheckBox.isChecked) {
            Install-AppFromWinGet -id "Microsoft.PowerShell"
        }
    })

    $MainViewContinueInstallMediaButton.Add_Click({
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