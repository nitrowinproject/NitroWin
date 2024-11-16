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
        [string]$id
    )

    $split = $id -split "."

    return $split[1]
}