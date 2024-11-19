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

    $appsToInstallWinget = @()

    $AppLicenseCheckBox.Add_Checked({
        $AppContinueButton.isEnabled = $true
    })

    $AppLicenseCheckBox.Add_Unchecked({
        $AppContinueButton.isEnabled = $false
    })
    
    $AppSkipButton.Add_Click({
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

    $AppContinueButton.Add_Click({
        if ($AppBrowserFirefoxCheckBox.isChecked) {
            Copy-Item -Path ".\assets\firefox\user.js" -Destination Get-DownloadFolder
            Show-Prompt -message "You can find the hardened user.js in your downloads folder." -title "Firefox hardened user.js" -buttons OK -icon Information
            Install-Firefox
        }
        if ($AppBrowserBraveCheckBox.isChecked) {
            Start-Process "https://brave.com/"
        }
        if ($AppArchiving7ZipCheckBox.isChecked) {
            $appsToInstallWinget += "7zip.7zip"
        }
        if ($AppArchivingWinRARCheckBox.isChecked) {
            $appsToInstallWinget += "RARLab.WinRAR"
        }
        if ($AppMediaPlayerVLCCheckBox.isChecked) {
            $appsToInstallWinget += "VideoLAN.VLC"
        }
        if ($AppMediaPlayerKLCPCheckBox.isChecked) {
            Start-Process "https://codecguide.com/download_k-lite_codec_pack_mega.htm"
        }
        if ($AppOtherStartAllBackCheckBox.isChecked) {
            $appsToInstallWinget += "StartIsBack.StartAllBack"
        }
        if ($AppOtherKeePassXCCheckBox.isChecked) {
            $appsToInstallWinget += "KeePassXCTeam.KeePassXC"
        }
        if ($AppOtherUniGetUICheckBox.isChecked) {
            $appsToInstallWinget += "MartiCliment.UniGetUI"
        }

        $appsToInstallWinget | ForEach-Object {
            Install-AppFromWinget -id $_ -name (Format-AppName -id $_)
        }
        $Global:mainWindow.Content = $Global:dnsViewForm
    })

    return $appSelectionViewForm
}