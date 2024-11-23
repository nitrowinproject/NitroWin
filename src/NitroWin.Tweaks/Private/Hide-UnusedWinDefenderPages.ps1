<#
.SYNOPSIS
    Hides unused Windows defender pages

.EXAMPLE
    Hide-UnusedWinDefenderPages.ps1
#>

function Hide-UnusedWinDefenderPages {
    $regpath = "HKLM:\SOFTWARE\Policies\Microsoft\Windows Defender Security Center\Account protection"
    $regpath2 = "HKLM:\SOFTWARE\Policies\Microsoft\Windows Defender Security Center\Family options"
    $regpath3 = "HKLM:\SOFTWARE\Policies\Microsoft\Windows Defender Security Center\Device performance and health"

    $name = "UILockdown"
    $value = 1
    $type = "DWord"

    if (-not (Test-Path -Path $regpath)) {
        New-Item -Path $regpath -Force | Out-Null
    }

    New-ItemProperty -Path $regpath -Name $name -Value $value -PropertyType $type
    New-ItemProperty -Path $regpath2 -Name $name -Value $value -PropertyType $type
    New-ItemProperty -Path $regpath3 -Name $name -Value $value -PropertyType $type
}