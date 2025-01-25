<#
.SYNOPSIS
    Returns the path to the current user's download folder

.EXAMPLE
    Get-DownloadFolder
#>

function Get-DownloadFolder {
    $value = (New-Object -ComObject Shell.Application).NameSpace('shell:Downloads').Self.Path

    return $value
}