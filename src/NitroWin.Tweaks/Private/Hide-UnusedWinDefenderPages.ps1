<#
.SYNOPSIS
    Hides unused Windows defender pages

.EXAMPLE
    Hide-UnusedWinDefenderPages.ps1
#>

function Hide-UnusedWinDefenderPages {
    $paths = @{
        "HKLM:\SOFTWARE\Policies\Microsoft\Windows Defender Security Center\Account protection"            = @{
            "UILockdown" = 1
        }
        "HKLM:\SOFTWARE\Policies\Microsoft\Windows Defender Security Center\Family options"                = @{
            "UILockdown" = 1
        }
        "HKLM:\SOFTWARE\Policies\Microsoft\Windows Defender Security Center\Device performance and health" = @{
            "UILockdown" = 1
        }
    }

    foreach ($path in $paths.Keys) {
        Test-RegistryPath -Path $path
        foreach ($key in $paths[$path].Keys) {
            Set-ItemProperty -Path $path -Name $key -Value $paths[$path][$key] -Type DWord
        }
    }
}