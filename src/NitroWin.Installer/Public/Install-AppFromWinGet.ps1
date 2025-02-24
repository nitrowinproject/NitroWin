<#
.SYNOPSIS
    Installs an app via WinGet

.PARAMETER id
    The package ID of the desired app

.EXAMPLE
    Install-AppFromWinGet -id "Example.Example"
#>

function Install-AppFromWinGet {
    param (
        [Parameter(Mandatory=$true)]
        [string]$id
    )

    Write-Host "Installing $id..."
    Start-Process -FilePath "winget.exe" -Wait -NoNewWindow -ArgumentList "install --id $($id) --exact --scope machine --accept-package-agreements --accept-source-agreements --interactive"
}