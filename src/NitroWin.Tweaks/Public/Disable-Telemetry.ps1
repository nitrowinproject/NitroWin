<#
.SYNOPSIS
    Disables telemetry

.EXAMPLE
    Disable-Telemetry

.NOTES
    This does not disable all telemetry. Only a few options. Use WinUtil + OOShutUp to disable them all.
#>

function Disable-Telemetry {
    # Disable KMS telemetry
    $kmsregpath = "HKLM:\SOFTWARE\Policies\Microsoft\Windows NT\CurrentVersion\Software Protection Platform"
    $kmsname = "NoGenTicket"
    $kmsvalue = "1"
    $kmstype = "DWord"

    Test-RegistryPath -path $kmsregpath

    New-ItemProperty -Path $kmsregpath -Name $kmsname -Value $kmsvalue -PropertyType $kmstype

    # Disable customer improvement program telemetry
    $customerimprovementregpath = "HKLM:\SOFTWARE\Policies\Microsoft\AppV\CEIP"
    $customerimprovementregpath2 = "HKLM:\SOFTWARE\Policies\Microsoft\SQMClient\Windows"
    $customerimprovementname = "CEIPEnable"
    $customerimprovementvalue = "0"
    $customerimprovementtype = "DWord"

    Test-RegistryPath -path $customerimprovementregpath

    Test-RegistryPath -path $customerimprovementregpath2

    New-ItemProperty -Path $customerimprovementregpath -Name $customerimprovementname -Value $customerimprovementvalue -PropertyType $customerimprovementtype
    New-ItemProperty -Path $customerimprovementregpath2 -Name $customerimprovementname -Value $customerimprovementvalue -PropertyType $customerimprovementtype

    # Disable .NET cli telemetry
    [System.Environment]::SetEnvironmentVariable("DOTNET_CLI_TELEMETRY_OPTOUT", "1", [System.EnvironmentVariableTarget]::Machine)

    # Disable typing insights
    $typinginsightsregpath = "HKCU:\SOFTWARE\Microsoft\Input\Settings"
    $typinginsightsname = "InsightsEnabled"
    $typinginsightsvalue = "0"
    $typinginsightstype = "DWord"

    Test-RegistryPath -path $typinginsightsregpath

    New-ItemProperty -Path $typinginsightsregpath -Name $typinginsightsname -Value $typinginsightsvalue -PropertyType $typinginsightstype

    # Set maximum telemetry to 0
    $maxtelemetryregpath = "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\DataCollection"
    $maxtelemetryregpath2 = "HKLM:\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Policies\DataCollection"
    $maxtelemetryname = "MaxTelemetryAllowed"
    $maxtelemetryvalue = "0"

    Set-ItemProperty -Path $maxtelemetryregpath -Name $maxtelemetryname -Value $maxtelemetryvalue
    Set-ItemProperty -Path $maxtelemetryregpath2 -Name $maxtelemetryname -Value $maxtelemetryvalue

    # Disable generic telemetry
    $genertictelemetryregpath = "HKLM:\Software\Policies\Microsoft\Windows\DataCollection"
    $genertictelemetryname = "AllowDeviceNameInTelemetry"
    $genertictelemetryvalue = "0"
    $genertictelemetrytype = "DWord"

    Test-RegistryPath -path $genertictelemetryregpath

    New-ItemProperty -Path $genertictelemetryregpath -Name $genertictelemetryname -Value $genertictelemetryvalue -PropertyType $genertictelemetrytype

    # Disable diagnostic tracking
    $diagtrackregpath ="HKLM:\SYSTEM\CurrentControlSet\Control\Diagnostics\Performance"
    $diagtrackname = "DisableDiagnosticTracing"
    $diagtrackvalue = "1"
    
    Set-ItemProperty -Path $diagtrackregpath -Name $diagtrackname -Value $diagtrackvalue

    # Disable NVIDIA telemetry
    $nvidiaregpath = "HKCU:\Software\NVIDIA Corporation\NVControlPanel2\Client"
    $nvidianame = "OptInOrOutPreference"
    $nvidiavalue = "0"
    $nvidiatype = "DWord"

    Test-RegistryPath -path $nvidiaregpath

    Set-ItemProperty -Path $nvidiaregpath -Name $nvidianame -Value $nvidiavalue -Type $nvidiatype

    # Disable dha
    $dharegpath = "HKLM:\SOFTWARE\Policies\Microsoft\DeviceHealthAttestationService"
    $dhaname = "EnableDeviceHealthAttestationService"
    $dhavalue = "0"
    $dhatype = "DWord"

    Test-RegistryPath -path $dharegpath

    New-ItemProperty -Path $dharegpath -Name $dhaname -Value $dhavalue -PropertyType $dhatype
}   