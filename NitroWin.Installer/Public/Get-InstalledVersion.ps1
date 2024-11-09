function Get-InstalledVersion {
    if (Test-Path env:NitroWinVersion) {
        return $env:NitroWinVersion
    }
    else {
        return $null
    }
}