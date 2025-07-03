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
        Start-Process -FilePath "winget.exe" -Wait -NoNewWindow -Verb RunAs -ArgumentList "download --id $($id) --exact --skip-license --scope machine --accept-package-agreements --accept-source-agreements --interactive"
        Write-Host "Installed $id!"
    }
    catch {
        Show-InstallError -name $id
    }
}