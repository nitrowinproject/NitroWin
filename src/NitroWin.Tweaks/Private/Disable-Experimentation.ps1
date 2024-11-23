<#
.SYNOPSIS
    Disables Experimentation

.EXAMPLE
    Disable-Experimentation
#>

function Disable-Experimentation {
    $regpath = "HKLM:\SOFTWARE\Microsoft\PolicyManager\default\System\AllowExperimentation"

    Set-ItemProperty -Path $regpath -Name value -Value 0 -Type DWord
}