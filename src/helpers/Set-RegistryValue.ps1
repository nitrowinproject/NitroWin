function Set-RegistryValue {
    <#
    .SYNOPSIS
        Sets a specific registry value.

    .PARAMETER Path
        Path to the registry value

    .PARAMETER Name
        Name of the registry value

    .PARAMETER Value
        Registry value
    #>

    param (
        [Parameter(Mandatory = $true)]
        [string]$Path,

        [Parameter(Mandatory = $true)]
        [string]$Name,

        [Parameter(Mandatory = $true)]
        [UInt32]$Value
    )

    if (-not (Test-Path $Path)) {
        New-Item -Path $Path -Force | Out-Null
    }
    Set-ItemProperty -Path $Path -Name $Name -Value $Value -Type DWord
}