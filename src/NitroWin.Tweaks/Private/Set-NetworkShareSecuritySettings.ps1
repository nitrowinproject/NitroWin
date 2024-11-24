<#
.SYNOPSIS
    Sets security settings for network shares

.EXAMPLE
    Set-NetworkShareSecuritySettings
#>

function Set-NetworkShareSecuritySettings {
    $paths = @{
        "HKLM:\SYSTEM\CurrentControlSet\Services\LanManServer\Parameters" = @{
            "RestrictNullSessAccess" = 1
        }
        "HKLM:\SYSTEM\CurrentControlSet\Control\Lsa" = @{
            "RestrictAnonymous" = 1
        }
    }

    foreach ($path in $paths.Keys) {
        Test-RegistryPath -Path $path
        foreach ($key in $paths[$path].Keys) {
            Set-ItemProperty -Path $path -Name $key -Value $paths[$path][$key] -Type DWord
        }
    }
}