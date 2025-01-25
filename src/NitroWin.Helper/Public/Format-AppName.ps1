<#
.SYNOPSIS
    Formats an app name from Winget to normal app name

.PARAMETER id
    The Winget app id

.EXAMPLE
    Format-AppName -id "Git.Git"
#>

function Format-AppName {
    param (
        [Parameter(Mandatory=$true)]
        [string]$id
    )

    $split = $id.Split(".")[-1]

    return $split
}