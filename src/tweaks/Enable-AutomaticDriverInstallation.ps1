function Enable-AutomaticDriverInstallation {
    <#
    .SYNOPSIS
        Enables the automatic installation of drivers via Windows Update.
    #>
    try {
        Write-Host "Enabling automatic driver installation..."

        Set-RegistryValue "HKLM:\SOFTWARE\Microsoft\PolicyManager\current\device\Update" "ExcludeWUDriversInQualityUpdate" 0
        Set-RegistryValue "HKLM:\SOFTWARE\Microsoft\PolicyManager\default\Update" "ExcludeWUDriversInQualityUpdate" 0
        Set-RegistryValue "HKLM:\SOFTWARE\Microsoft\WindowsUpdate\UX\Settings" "ExcludeWUDriversInQualityUpdate" 0
        Set-RegistryValue "HKLM:\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate" "ExcludeWUDriversInQualityUpdate" 0
        Set-RegistryValue "HKLM:\SOFTWARE\Microsoft\PolicyManager\default\Update\ExcludeWUDriversInQualityUpdate" "value" 0
        Set-RegistryValue "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Device Metadata" "PreventDeviceMetadataFromNetwork" 0

        Set-RegistryValue "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\DriverSearching" "SearchOrderConfig" 1
        Set-RegistryValue "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\DriverSearching" "DontSearchWindowsUpdate" 0

        Write-Host "Enabled automatic driver installation successfully!" -ForegroundColor Green
    }
    catch {
        Show-InstallError -name "automatic driver installation"
    }
}