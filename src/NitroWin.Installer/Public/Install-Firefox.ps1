<#
.SYNOPSIS
    Installs Firefox ESR

.EXAMPLE
    Install-Firefox
#>

function Install-Firefox {
    $lang = Get-LangWithoutRegion
    $url = "https://download.mozilla.org/?product=firefox-esr-msi-latest-ssl&os=win64&lang=$lang"
    Install-AppFromURL -url $url -name "Firefox ESR"
}