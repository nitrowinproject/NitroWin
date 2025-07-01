function Install-WinGet {
    if (-Not (Get-Command winget -ErrorAction SilentlyContinue)) {
        $arch = switch ($env:PROCESSOR_ARCHITECTURE) {
            "AMD64" { "x64" }
            "x86"   { "x86" }
            "ARM64" { "arm64" }
            "ARM"   { "arm" }
            default { "unknown" }
        }

        $apiUrl = "https://api.github.com/repos/microsoft/winget-cli/releases/latest"
        $release = Invoke-RestMethod -Uri $apiUrl
        $dependenciesAsset = $release.assets | Where-Object { $_.name -eq "DesktopAppInstaller_Dependencies.zip" }
        $dependencies = $dependenciesAsset.browser_download_url

        $dependenciesArchive = Get-FileFromURL -url $dependencies
        Expand-Archive -Path $dependenciesArchive -DestinationPath (Get-DownloadFolder)
        
        $files = Get-ChildItem (Join-Path -Path (Get-DownloadFolder) -ChildPath $arch)
        foreach ($file in $files) {
            Add-AppxPackage -Path $file
        }

        $wingetAsset = $release.assets | Where-Object { $_.name -eq "Microsoft.DesktopAppInstaller_8wekyb3d8bbwe.msixbundle" }
        $winget = $wingetAsset.browser_download_url
        $wingetInstaller = Get-FileFromURL -url $winget

        Add-AppxPackage $wingetInstaller
    }
}