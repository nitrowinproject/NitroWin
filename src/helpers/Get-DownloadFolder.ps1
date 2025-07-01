function Get-DownloadFolder {
    <#
    .SYNOPSIS
        This returns the current user's download folder.
    #>
    
    $value = (New-Object -ComObject Shell.Application).NameSpace('shell:Downloads').Self.Path
    return $value
}