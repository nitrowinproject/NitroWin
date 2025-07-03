function Install-AppFromWinGet {
    <#
    .SYNOPSIS
        Installs an app using WinGet.

    .PARAMETER id
        The package ID of the desired app.

    .EXAMPLE
        Install-AppFromWinGet -id "Example.Example"
    #>

    param (
        [Parameter(Mandatory=$true)]
        [string]$id
    )

    try {
        Write-Host "Installing $id via WinGet..."
        Start-Process -FilePath "winget.exe" -Wait -Verb RunAs -ArgumentList "install --id $($id) --exact --scope machine --accept-package-agreements --accept-source-agreements"
        Write-Host "Installed $id!"
    }
    catch {
        Show-InstallError -name $id
    }
}