if (Test-Path "nitrowin.ps1" -PathType Leaf) {
    Remove-Item -Path "nitrowin.ps1"
}

$files = Get-ChildItem -Recurse -Filter *.ps1 | Where-Object { $_.Name -notin @('compile.ps1', 'logic.ps1') }
$content = foreach ($file in $files) {
    Get-Content -Path $file.FullName
}
$content | Out-File -FilePath nitrowin.ps1

$logic = Get-Content -Path logic.ps1
Add-Content -Path nitrowin.ps1 -Value $logic