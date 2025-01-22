<#
.SYNOPSIS
    Sets NTFS options to be the best for modern systems

.DESCRIPTION
    Disables 8dot3 and lass access information

.EXAMPLE
    Set-NTFSOptions
#>

function Set-NTFSOptions {
    Start-Process -FilePath "fsutil.exe" -ArgumentList "behavior set disablelastaccess 1" -Wait
    Start-Process -FilePath "fsutil.exe" -ArgumentList "behavior set disable8dot3 1" -Wait
}