function Install-App {
    <#
    .SYNOPSIS
        Installs an app from a given URL.

    .PARAMETER name
        The name of the app which should be installed.

    .PARAMETER url
        The URL of the installer for the desired app.

    .PARAMETER arguments
        Optional arguments to pass to the installer.
    #>

    param (
        [Parameter(Mandatory = $true)]
        [string]$name,

        [Parameter(Mandatory = $true)]
        [string]$url,

        [Parameter(Mandatory = $false)]
        [string]$arguments
    )

    $destinationPath = Get-FileFromURL -url $url

    Write-Host "Installing $name..."

    if ($arguments) {
        Start-Process -FilePath $destinationPath -Wait -Verb RunAs -ArgumentList $arguments
    }
    else {
        Start-Process -FilePath $destinationPath -Wait -Verb RunAs
    }
}