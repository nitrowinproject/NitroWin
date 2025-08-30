function Get-PublicDesktopFolder {
    <#
    .SYNOPSIS
        This returns the public desktop folder.
    #>

    $value = [Environment]::GetFolderPath("CommonDesktopDirectory")
    return $value
}