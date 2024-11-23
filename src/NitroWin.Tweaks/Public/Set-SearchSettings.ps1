<#
.SYNOPSIS
    Configures search on the taskbar

.EXAMPLE
    Set-SearchSettings
#>

function Set-SearchSettings {
    $paths = @{
        "HKCU:\SOFTWARE\Microsoft\Windows\CurrentVersion\Search" = @{
            "BingSearchEnabled"         = 0
            "SearchboxTaskbarMode"      = 1
        }
        "HKCU:\SOFTWARE\Microsoft\Windows\CurrentVersion\SearchSettings" = @{
            "IsAADCloudSearchEnabled"   = 0
            "IsDeviceSearchHistoryEnabled" = 0
            "IsMSACloudSearchEnabled"   = 0
            "SafeSearchMode"            = 0
        }
        "HKLM:\SOFTWARE\Policies\Microsoft\Windows\Windows Search" = @{
            "ConnectedSearchUseWeb"     = 0
            "DisableWebSearch"          = 1
            "AllowSearchToUseLocation"  = 0
            "EnableDynamicContentInWSB" = 0
        }
        "HKCU:\SOFTWARE\Policies\Microsoft\Windows\Explorer" = @{
            "DisableSearchBoxSuggestions" = 1
        }
    }

    foreach ($path in $paths.Keys) {
        Test-RegistryPath -Path $path
        foreach ($key in $paths[$path].Keys) {
            Set-ItemProperty -Path $path -Name $key -Value $paths[$path][$key] -PropertyType DWord -Force
        }
    }
}