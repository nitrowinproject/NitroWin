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

    $global:runAsTIBitness = switch ($env:PROCESSOR_ARCHITECTURE) {
        "AMD64" { "64" }
        "x86"   { "32" }
        "ARM64" { "64" }
        "ARM"   { "32" }
        default { "32" }
    }
    Get-FileFromURL -url "https://github.com/fafalone/RunAsTrustedInstaller/releases/latest/download/RunAsTI$runAsTIBitness.exe" | Out-Null

    $global:arch = switch ($env:PROCESSOR_ARCHITECTURE) {
        "AMD64" { "x64" }
        "x86"   { "x86" }
        "ARM64" { "arm64" }
        "ARM"   { "arm" }
        default { "unknown" }
    }
}