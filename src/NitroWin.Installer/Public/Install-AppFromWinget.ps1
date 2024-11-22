<#
.SYNOPSIS
    Installs an app via winget

.PARAMETER id
    The package ID of the desired app

.PARAMETER name
    The name of the app

.EXAMPLE
    Install-AppFromWinget -id 
#>

function Install-AppFromWinget {
    param (
        [string]$id,
        [string]$name
    )
    
    $command = "winget.exe install --id=$id --exact --source winget --accept-package-agreements --accept-source-agreements --interactive --scope machine"
    Start-Process powershell -ArgumentList "-NoProfile -Command $command" -Verb RunAs
}