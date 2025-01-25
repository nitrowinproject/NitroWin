<#
.SYNOPSIS
    Downloads a file from the Internet and returns its path after downloading

.PARAMETER url
    The URL of the file to be downloaded

.PARAMETER outpath
    The download location of the file to be downloaded

.EXAMPLE
    Get-FileFromURL -url "https://example.com/example.txt" -outpath "C:\Downloads"
#>

function Get-FileFromURL {
    param (
        [Parameter(Mandatory=$true)]
        [string]$url,
        
        [Parameter(Mandatory=$true)]
        [string]$outpath
    )
    
    $outpath = Join-Path $outpath (Split-Path $url -Leaf)

    # Download file
    (New-Object System.Net.WebClient).DownloadFile($url, $outpath)

    return $outpath
}