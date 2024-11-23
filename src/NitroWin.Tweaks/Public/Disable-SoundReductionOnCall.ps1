<#
.SYNOPSIS
    Disables Windows reducing system audio when you're on a call

.EXAMPLE
    Disable-SoundReductionOnCall
#>

function Disable-SoundReductionOnCall {
    $regpath = "HKCU:\SOFTWARE\Microsoft\Multimedia\Audio"

    Test-RegistryPath -path $regpath

    Set-ItemProperty -Path $regpath -Name UserDuckingPreference -Value 3 -Type DWord
}