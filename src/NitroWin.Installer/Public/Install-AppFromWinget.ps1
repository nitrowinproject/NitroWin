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
        [string]$id
    )

    Start-Process -FilePath "winget.exe" -Wait -NoNewWindow -Verb RunAs -ArgumentList "download --id $($id) --exact --skip-license --scope machine --architecture x64 --locale $(Get-WinSystemLocale) --accept-package-agreements --accept-source-agreements --interactive"
}