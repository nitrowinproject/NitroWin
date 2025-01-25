<#
.SYNOPSIS
    Returns the path to the current user's desktop folder

.EXAMPLE
    Get-DesktopFolder
#>

function Get-DesktopFolder {
    $value = (New-Object -ComObject Shell.Application).NameSpace('shell:Desktop').Self.Path

    return $value
}