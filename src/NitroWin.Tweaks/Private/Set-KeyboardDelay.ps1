<#
.SYNOPSIS
    Sets the keyboard repeat rate and delay settings

.EXAMPLE
    Set-KeyboardDelay
#>

function Set-KeyboardDelay {
    $regpath = "HKCU:\Control Panel\Keyboard"
    
    $delayname = "KeyboardDelay"
    $delayvalue = "0"
    $delaytype = "DWord"

    $speedname = "KeyboardSpeed"
    $speedvalue = "31"
    $speedtype = "String"

    Test-RegistryPath -path $regpath

    Set-ItemProperty -Path $regpath -Name $delayname -Value $delayvalue -Type $delaytype
    Set-ItemProperty -Path $regpath -Name $speedname -Value $speedvalue -Type $speedtype
}