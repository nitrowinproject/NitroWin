function Initialize-Environment {
    <#
    .SYNOPSIS
        Initializes the PowerShell environment for NitroWin.
    #>

    Add-Type -AssemblyName "System.Windows.Forms"
    [System.Windows.Forms.Application]::EnableVisualStyles();

    [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12

    Set-ExecutionPolicy Unrestricted -Scope Process -Force

    Add-Type -AssemblyName "System.Net.Http"
    $global:httpClient = [System.Net.Http.HttpClient]::new()

    Get-FileFromURL -url "https://github.com/fafalone/RunAsTrustedInstaller/releases/latest/download/RunAsTI64.exe" | Out-Null

    $global:config = Get-NitroWinConfig
    if (-Not $config) {
        Show-Prompt -message "Config could not be loaded. Please connect to the internet and rerun NitroWin." -title "Could not load config" -buttons Ok -icon Error
        exit 1
    }
}