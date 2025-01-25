<#
.SYNOPSIS
    Installs O&O ShutUp 10++ to the user's desktop folder

.EXAMPLE
    Install-OOSU
#>

function Install-OOSU {
    Get-FileFromURL -url "https://dl5.oo-software.com/files/ooshutup10/OOSU10.exe" -outpath (Get-DesktopFolder)
}