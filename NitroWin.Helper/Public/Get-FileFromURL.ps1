function Get-FileFromURL {
    param (
        [string]$url,
        [string]$outpath
    )

    # Add filename from URL if outpath is a directory
    if (-not (Test-Path $outpath)) {
        $outpath = Join-Path $outpath (Split-Path $url -Leaf)
    }

    # Download file
    (New-Object System.Net.WebClient).DownloadFile($url, $outpath)

    return $outpath
}