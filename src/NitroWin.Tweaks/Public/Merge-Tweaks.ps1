<#
.SYNOPSIS
    Merges all of NitroWin's registry tweaks into one file

.EXAMPLE
    Merge-Tweaks
#>

function Merge-Tweaks {
    $tweakFiles = Get-ChildItem -Path ".\src\NitroWin.Tweaks\Tweaks" -Filter "*.reg" -Recurse
    $mergedFile = ".\merged.reg"

    if (Test-Path $mergedFile) {
        Remove-Item $mergedFile
    }

    Add-Content -Path $mergedFile -Value "Windows Registry Editor Version 5.00"

    foreach ($file in $tweakFiles) {
        $content = Get-Content $file.FullName
        Add-Content -Path $mergedFile -Value ($content | Select-Object -Skip 1)
    }
}