function Install-AppFromWinGet {
    <#
    .SYNOPSIS
        Installs an app using WinGet.

    .PARAMETER id
        The package ID of the desired app.

    .PARAMETER arguments
        The arguments passed to WinGet.
    #>

    param (
        [Parameter(Mandatory = $true)]
        [string]$id,

        [Parameter(Mandatory = $true)]
        [string]$arguments
    )

    Write-Host "Installing $id via WinGet..."
    Start-Process -FilePath "winget.exe" -Wait -Verb RunAs -ArgumentList "install --id $id --exact --accept-package-agreements --accept-source-agreements $arguments"
}