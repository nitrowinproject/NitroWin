$files = Get-ChildItem -Recurse -Filter *.ps1 | Where-Object { $_.Name -notin @('compile.ps1', 'compiled.ps1') }
$content = foreach ($file in $files) {
    Get-Content -Path $file.FullName
}
$content | Out-File -FilePath nitrowin.ps1