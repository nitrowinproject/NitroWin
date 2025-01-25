<#
.SYNOPSIS
    Installs an app via winget

.PARAMETER id
    The package ID of the desired app

.EXAMPLE
    Install-AppFromWinget -id 
#>

function Install-AppFromWinget {
    param (
        [string]$id
    )
    
    $command = "winget.exe install $id -e -s winget --scope machine"
    Start-Process powershell -ArgumentList "-NoProfile -Command $command" -Verb RunAs
}