<#
.SYNOPSIS
    Disables frequent apps showing up in the start menu

.EXAMPLE
    Disable-FrequentAppsInStartMenu
#>

function Disable-FrequentAppsInStartMenu {
    $regpath = "HKCU:\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer"

    Test-RegistryPath -path $regpath

    Set-ItemProperty -Path $regpath -Name NoInstrumentation -Value 1 -Type DWord
}