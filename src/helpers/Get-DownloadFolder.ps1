function Get-DownloadFolder {
    $value = (New-Object -ComObject Shell.Application).NameSpace('shell:Downloads').Self.Path
    return $value
}