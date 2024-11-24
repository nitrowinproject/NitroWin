<#
.SYNOPSIS
    Disables the automatic installation of driver updates via Windows Update

.EXAMPLE
    Disable-AutomaticDriverInstall
#>

function Disable-AutomaticDriverInstall {
    $paths = @{
        "HKLM:\SOFTWARE\Microsoft\PolicyManager\current\device\Update"                          = @{
            "ExcludeWUDriversInQualityUpdate" = 1
        }
        "HKLM:\SOFTWARE\Microsoft\PolicyManager\default\Update"                                 = @{
            "ExcludeWUDriversInQualityUpdate" = 1
        }
        "HKLM:\SOFTWARE\Microsoft\WindowsUpdate\UX\Settings"                                    = @{
            "ExcludeWUDriversInQualityUpdate" = 1
        }
        "HKLM:\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"                               = @{
            "ExcludeWUDriversInQualityUpdate" = 1
        }
        "HKLM:\SOFTWARE\Microsoft\PolicyManager\default\Update\ExcludeWUDriversInQualityUpdate" = @{
            "value" = 1
        }
        "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Device Metadata"                       = @{
            "PreventDeviceMetadataFromNetwork" = 1
        }
        "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\DriverSearching"                       = @{
            "SearchOrderConfig"       = 0
            "DontSearchWindowsUpdate" = 1
        }
    }
    
    foreach ($path in $paths.Keys) {
        Test-RegistryPath -Path $path
        foreach ($key in $paths[$path].Keys) {
            Set-ItemProperty -Path $path -Name $key -Value $paths[$path][$key] -Type DWord
        }
    }
}