function Install-App {
    <#
    .SYNOPSIS
        Installs an app from a given URL.

    .PARAMETER url
        The URL of the installer for the desired app.
    #>

    param (
        [Parameter(Mandatory=$true)]
        [string]$url
    )

    try {
        $destinationPath = Get-FileFromURL -url $url
        
        Write-Host "Installing..."
        Start-Process -FilePath $destinationPath -Wait -Verb RunAs
        Write-Host "Installed!"
    }
    catch {
        Show-InstallError -name $fileName
    }
}