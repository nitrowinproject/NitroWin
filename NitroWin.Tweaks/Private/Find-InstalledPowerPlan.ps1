<#
.SYNOPSIS
    Returns whether a specific power plan is installed on the system

.PARAMETER guid
    The GUID of the desired power plan

.EXAMPLE
    Find-InstalledPowerPlan -guid e9a42b02-d5df-448d-aa00-03f14749eb61
#>

function Find-InstalledPowerPlan {
    param (
        [string]$guid
    )

    $installed = Invoke-Expression "powercfg /list"

    if ($installed -match $guid) {
        return $true
    }
    else {
        return $false
    }
}