function Clear-DownloadFolder {
    <#
    .SYNOPSIS
        This deletes everything in the current user's download folder.
    #>
    
    Get-ChildItem -Path (Get-DownloadFolder) -Include *.* -File -Recurse | ForEach-Object { $_.Delete() }
}