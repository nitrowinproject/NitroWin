function Install-WinGet {
    <#
    .SYNOPSIS
        It installs WinGet, if it isn't already installed.
    #>

    if (-Not (Get-Command winget -ErrorAction SilentlyContinue)) {
        $arch = switch ($env:PROCESSOR_ARCHITECTURE) {
            "AMD64" { "x64" }
            "x86"   { "x86" }
            "ARM64" { "arm64" }
            "ARM"   { "arm" }
            default { "unknown" }
        }

        $dependencies = "https://github.com/microsoft/winget-cli/releases/latest/download/DesktopAppInstaller_Dependencies.zip"
        $dependenciesArchive = Get-FileFromURL -url $dependencies

        Expand-Archive -Path $dependenciesArchive -DestinationPath (Get-DownloadFolder)
        $files = Get-ChildItem (Join-Path -Path (Get-DownloadFolder) -ChildPath $arch)
        foreach ($file in $files) {
            Add-AppxPackage -Path $file
        }

        $winget = "https://github.com/microsoft/winget-cli/releases/latest/download/Microsoft.DesktopAppInstaller_8wekyb3d8bbwe.msixbundle"
        $wingetInstaller = Get-FileFromURL -url $winget

        Add-AppxPackage $wingetInstaller
    }
}