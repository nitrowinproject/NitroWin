<#
.SYNOPSIS
    Runs WinUtil by ChrisTitusTech

.EXAMPLE
    Invoke-WinUtil
#>

function Invoke-WinUtil {
    Invoke-Expression (New-Object Net.WebClient).DownloadString('https://github.com/ChrisTitusTech/winutil/releases/latest/download/winutil.ps1')
}