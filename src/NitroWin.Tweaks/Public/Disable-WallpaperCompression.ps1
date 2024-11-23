<#
.SYNOPSIS
    Disables the wallpaper compression

.EXAMPLE
    Disable-WallpaperCompression
#>

function Disable-WallpaperCompression {
    $regpath = "HKCU:\Control Panel\Desktop"

    Test-RegistryPath -path $regpath

    Set-ItemProperty -Path $regpath -Name JPEGImportQuality -Value 100 -Type DWord
}