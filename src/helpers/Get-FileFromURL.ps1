function Get-FileFromURL {
    <#
    .SYNOPSIS
        Downloads a file from the Internet and returns its path after downloading.

    .PARAMETER url
        The URL of the file to be downloaded.

    .EXAMPLE
        Get-FileFromURL -url "https://example.com/example.txt"
    #>

    param (
        [Parameter(Mandatory = $true)]
        [string]$url
    )

    try {
        $global:fileName = [System.IO.Path]::GetFileName($url)
        $destinationPath = Join-Path -Path (Get-DownloadFolder) -ChildPath $fileName

        Write-Host "Downloading: $fileName..."

        $response = $httpClient.GetAsync($url).Result
        [System.IO.File]::WriteAllBytes($destinationPath, $response.Content.ReadAsByteArrayAsync().Result)

        Write-Host "Downloaded: $fileName!" -ForegroundColor Green

        return $destinationPath
    }
    catch {
        Show-InstallError -name $fileName
    }
}