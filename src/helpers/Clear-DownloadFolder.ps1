function Clear-DownloadFolder {
    Get-ChildItem -Path (Get-DownloadFolder) -Include *.* -File -Recurse | ForEach-Object { $_.Delete() }
}