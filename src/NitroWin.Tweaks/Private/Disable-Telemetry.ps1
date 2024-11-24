<#
.SYNOPSIS
    Disables telemetry

.EXAMPLE
    Disable-Telemetry

.NOTES
    This does not disable all telemetry. Only a few options. Use WinUtil + OOShutUp to disable them all.
#>

function Disable-Telemetry {
    $paths = @{
        # Disable KMS telemetry
        "HKLM:\SOFTWARE\Policies\Microsoft\Windows NT\CurrentVersion\Software Protection Platform" = @{
            "NoGenTicket" = 1
        }
        # Disable customer improvement program
        "HKLM:\SOFTWARE\Policies\Microsoft\AppV\CEIP"                                              = @{
            "CEIPEnable" = 0
        }
        "HKLM:\SOFTWARE\Policies\Microsoft\SQMClient\Windows"                                      = @{
            "CEIPEnable" = 0
        }
        # Disable typing insights
        "HKCU:\SOFTWARE\Microsoft\Input\Settings"                                                  = @{
            "InsightsEnabled" = 0
        }
        # Set maximum telemetry to 0
        "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\DataCollection"                  = @{
            "MaxTelemetryAllowed" = 0
        }
        "HKLM:\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Policies\DataCollection"      = @{
            "MaxTelemetryAllowed" = 0
        }
        # Disable generic telemetry
        "HKLM:\Software\Policies\Microsoft\Windows\DataCollection"                                 = @{
            "AllowDeviceNameInTelemetry" = 0
        }
        # Disable diagnostic tracking
        "HKLM:\SYSTEM\CurrentControlSet\Control\Diagnostics\Performance"                           = @{
            "DisableDiagnosticTracing" = 1
        }
        # Disable NVIDIA telemetry
        "HKCU:\Software\NVIDIA Corporation\NVControlPanel2\Client"                                 = @{
            "OptInOrOutPreference" = 0
        }
        # Disable DHA
        "HKLM:\SOFTWARE\Policies\Microsoft\DeviceHealthAttestationService"                         = @{
            "EnableDeviceHealthAttestationService" = 0
        }
    }

    foreach ($path in $paths.Keys) {
        Test-RegistryPath -Path $path
        foreach ($key in $paths[$path].Keys) {
            Set-ItemProperty -Path $path -Name $key -Value $paths[$path][$key] -Type DWord
        }
    }

    # Disable .NET cli telemetry
    [System.Environment]::SetEnvironmentVariable("DOTNET_CLI_TELEMETRY_OPTOUT", "1", [System.EnvironmentVariableTarget]::Machine)
}