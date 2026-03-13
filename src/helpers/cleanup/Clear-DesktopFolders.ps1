function Clear-DesktopFolders {
    <#
    .SYNOPSIS
        This deletes everything in the current user's and the public desktop folder.
    #>

    $desktopFolder = (New-Object -ComObject Shell.Application).NameSpace('shell:Desktop').Self.Path
    $publicDesktopFolder = [Environment]::GetFolderPath("CommonDesktopDirectory")

    foreach ($path in @($desktopFolder, $publicDesktopFolder)) {
        Get-ChildItem -Path $path -File -Recurse | Remove-Item -ErrorAction SilentlyContinue -Force

        Get-ChildItem -Path $path -Directory -Recurse |
        Where-Object { !(Get-ChildItem -Path $_.FullName -Force) } |
        Remove-Item -Force -Recurse -ErrorAction SilentlyContinue
    }
}