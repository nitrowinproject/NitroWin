<#
.SYNOPSIS
    Copies the autounattend.xml file from NitroWin to a specified installation media

.PARAMETER drive
    The drive letter of your installation media

.EXAMPLE
    Export-AnswerFile -drive d:\
#>

function Export-AnswerFile {
    param (
        [string]$drive
    )
    
    Copy-Item -Path ".\assets\autounattend\autounattend.xml" -Destination $drive
}