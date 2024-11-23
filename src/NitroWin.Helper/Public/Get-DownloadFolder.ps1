<#
.SYNOPSIS
    Returns the path to the current user's download folder

.EXAMPLE
    Get-DownloadFolder
#>

function Get-DownloadFolder {
    $regPath = "HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders"
    $valueName = "{374DE290-123F-4565-9164-39C4925E467B}"

    $value = (Get-ItemProperty -Path $regPath -Name $valueName).$valueName

    return $value
}