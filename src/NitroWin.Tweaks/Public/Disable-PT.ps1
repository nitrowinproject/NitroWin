<#
.SYNOPSIS
    Disables performance tracking

.EXAMPLE
    Disable-PT
#>

function Disable-PT {
    $regpath = "HKLM:\SOFTWARE\Policies\Microsoft\Windows\WDI\{9c5a40da-b965-4fc3-8781-88dd50a6299d}"

    Test-RegistryPath -path $regpath

    Set-ItemProperty -Path $regpath -Name ScenarioExecutionEnabled -Value 0 -Type DWord
}