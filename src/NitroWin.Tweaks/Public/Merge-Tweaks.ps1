<#
.SYNOPSIS
    Merges all of NitroWin's registry tweaks into one file

.PARAMETER drive
    The drive letter of the installation media

.EXAMPLE
    Merge-Tweaks -mergedFile ".\merged.reg"
#>

function Merge-Tweaks {
    param (
        [string]$mergedFile
    )

    $tweakFiles = Get-ChildItem -Path ".\src\NitroWin.Tweaks\Tweaks" -Filter "*.reg" -Recurse

    if (Test-Path $mergedFile) {
        Remove-Item $mergedFile
    }

    Add-Content -Path $mergedFile -Value "Windows Registry Editor Version 5.00"

    foreach ($file in $tweakFiles) {
        $content = Get-Content $file.FullName
        Add-Content -Path $mergedFile -Value ($content | Select-Object -Skip 1)
    }
}