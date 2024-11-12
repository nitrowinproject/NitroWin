<#
.SYNOPSIS
    Disables mouse acceleration

.EXAMPLE
    Disable-MouseAcceleration
#>

function Disable-MouseAcceleration {
    $RegPath = "HKCU:\Control Panel\Mouse"
    
    Set-ItemProperty -Path $RegPath -Name MouseSpeed -Value 0
    Set-ItemProperty -Path $RegPath -Name MouseThreshold1 -Value 0
    Set-ItemProperty -Path $RegPath -Name MouseThreshold2 -Value 0
}