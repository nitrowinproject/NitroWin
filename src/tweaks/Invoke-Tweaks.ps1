function Invoke-Tweaks {
    $urls = @(
        "https://raw.githubusercontent.com/nitrowinproject/Tweaks/main/NitroWin.Tweaks.User.reg",
        "https://raw.githubusercontent.com/nitrowinproject/Tweaks/main/NitroWin.Tweaks.User.ps1",
        "https://raw.githubusercontent.com/nitrowinproject/Tweaks/main/NitroWin.Tweaks.System.reg",
        "https://raw.githubusercontent.com/nitrowinproject/Tweaks/main/NitroWin.Tweaks.System.ps1"
    )

    foreach ($url in $urls) {
        try {
            $file = Get-FileFromURL -url $url
            if ($file.EndsWith(".reg")) {
                Start-Process -FilePath "reg" -ArgumentList "import `"$file`"" -NoNewWindow
            }
            elseif ($file.EndsWith(".ps1")) {
                Invoke-Expression $file
            }
        }
        catch {
            Show-InstallError -name $file
        }
    }
}