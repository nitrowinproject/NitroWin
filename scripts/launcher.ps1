$nitroWinExe = "NitroWin\NitroWin.exe"

function Get-NitroWinPath {
    foreach ($drive in (Get-PsDrive -PsProvider FileSystem)) {
        $nitroWinPath = Join-Path -Path $drive.Name -ChildPath $nitroWinExe
        if (Test-Path -Path $nitroWinPath -PathType Leaf) {
            return $nitroWinPath
        }
    }
    return $null
}

$nitroWinPath = Get-NitroWinPath

if (-Not $nitroWinPath) {
    Write-Host "NitroWin was not found. Please re-download it or reinstall Windows."
    Read-Host
}

Start-Process -FilePath $nitroWinPath
