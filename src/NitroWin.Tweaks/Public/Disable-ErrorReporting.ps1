<#
.SYNOPSIS
    Disables Windows error reporting

.EXAMPLE
    Disable-ErrorReporting
#>

function Disable-ErrorReporting {
    $paths = @{
        "HKCU:\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting" = @{
            "Disabled" = 1
        }
        "HKLM:\SOFTWARE\Policies\Microsoft\PCHealth\ErrorReporting" = @{
            "DoReport" = 0
            "ShowUI" = 0
        }
        "HKLM:\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting" = @{
            "Disabled" = 1
            "DontShowUI" = 1
            "LoggingDisabled" = 1
            "DontSendAdditionalData" = 1
        }
        "HKLM:\Software\Microsoft\Windows\CurrentVersion\Component Based Servicing" = @{
            "DisableWerReporting" = 1
        }
        "HKLM:\SOFTWARE\Policies\Microsoft\Windows\DeviceInstall\Settings" = @{
            "DisableSendGenericDriverNotFoundToWER" = 1
            "DisableSendRequestAdditionalSoftwareToWER" = 1
        }
    }

    foreach ($path in $paths.Keys) {
        Test-RegistryPath -Path $path
        foreach ($key in $paths[$path].Keys) {
            Set-ItemProperty -Path $path -Name $key -Value $paths[$path][$key] -PropertyType DWord -Force
        }
    }
}