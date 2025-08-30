function Get-DesktopFolder {
    <#
    .SYNOPSIS
        This returns the current user's desktop folder.
    #>

    $value = (New-Object -ComObject Shell.Application).NameSpace('shell:Desktop').Self.Path
    return $value
}