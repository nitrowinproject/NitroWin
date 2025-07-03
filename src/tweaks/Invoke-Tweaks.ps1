function Invoke-Tweaks {
    <#
    .SYNOPSIS
        Downloads and invokes all tweaks from NitroWin.
    #>
    
    $urls = @(
        "https://raw.githubusercontent.com/nitrowinproject/Tweaks/main/NitroWin.Tweaks.User.reg",
        "https://raw.githubusercontent.com/nitrowinproject/Tweaks/main/NitroWin.Tweaks.User.ps1",
        "https://raw.githubusercontent.com/nitrowinproject/Tweaks/main/NitroWin.Tweaks.System.reg",
        "https://raw.githubusercontent.com/nitrowinproject/Tweaks/main/NitroWin.Tweaks.System.ps1"
    )

    foreach ($url in $urls) {
        try {
            $file = Get-FileFromURL -url $url
            if ($file.EndsWith("System.reg")) {
                Start-Process -FilePath (Join-Path -Path (Get-DownloadFolder) -ChildPath "PsExec$psExecBitness.exe") -ArgumentList "-accepteula -s -i reg.exe import $file" -NoNewWindow
            }
            elseif ($file.EndsWith("System.ps1")) {
                Start-Process -FilePath (Join-Path -Path (Get-DownloadFolder) -ChildPath "PsExec$psExecBitness.exe") -ArgumentList "-accepteula -s -i powershell.exe -ExecutionPolicy Bypass -File $file" -NoNewWindow
            }
            elseif ($file.EndsWith(".reg")) {
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