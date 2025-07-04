function Install-App {
    <#
    .SYNOPSIS
        Installs an app from a given URL.

    .PARAMETER url
        The URL of the installer for the desired app.

    .PARAMETER arguments
        Optional arguments to pass to the installer.
    #>

    param (
        [Parameter(Mandatory=$true)]
        [string]$url,

        [Parameter(Mandatory=$false)]
        [string]$arguments
    )

    try {
        $destinationPath = Get-FileFromURL -url $url

        Write-Host "Installing $fileName..."
        Start-Process -FilePath $destinationPath -Wait -Verb RunAs -ArgumentList $arguments
        Write-Host "Installed $fileName!" -ForegroundColor Green
    }
    catch {
        Show-InstallError -name $fileName
    }
}