<#
.SYNOPSIS
    Disables telemetry

.EXAMPLE
    Disable-Telemetry

.NOTES
    This does not disable a lot of telemetry. Only a few things. Usage of the REG file is recommended.
#>

function Disable-Telemetry {
    # Disable .NET cli telemetry
    [System.Environment]::SetEnvironmentVariable("DOTNET_CLI_TELEMETRY_OPTOUT", "1", [System.EnvironmentVariableTarget]::Machine)

    # Disable PowerShell 7 Telemetry
    [System.Environment]::SetEnvironmentVariable("POWERSHELL_TELEMETRY_OPTOUT", "1", [System.EnvironmentVariableTarget]::Machine)
}