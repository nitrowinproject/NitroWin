<#
.SYNOPSIS
    Installs the Ultimate Power Plan if not already installed

.DESCRIPTION
    Uses the following GUID: e9a42b02-d5df-448d-aa00-03f14749eb61

.EXAMPLE
    Install-UltimatePowerPlan
#>

function Install-UltimatePowerPlan {
    if (-Not (Find-InstalledPowerPlan -guid "e9a42b02-d5df-448d-aa00-03f14749eb61")) {
        Invoke-Expression "powercfg /duplicatescheme e9a42b02-d5df-448d-aa00-03f14749eb61"
    }
}