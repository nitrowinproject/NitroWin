<#
.SYNOPSIS
    Installs Brave

.EXAMPLE
    Install-Brave
#>

function Install-Firefox {
    $url = "https://laptop-updates.brave.com/latest/winx64"
    Install-AppFromURL -url $url -name "Brave"
}