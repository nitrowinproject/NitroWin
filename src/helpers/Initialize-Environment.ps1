function Initialize-Environment {
    <#
    .SYNOPSIS
        Initializes the PowerShell environment for NitroWin.
    #>
    
    [System.Reflection.Assembly]::LoadWithPartialName("System.Windows.Forms")
    [System.Windows.Forms.Application]::EnableVisualStyles();

    [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
    
    Set-ExecutionPolicy Unrestricted -Scope Process -Force
    
    $psExecBitness = switch ($env:PROCESSOR_ARCHITECTURE) {
        "AMD64" { "64" }
        "x86"   { "" }
        "ARM64" { "64" }
        "ARM"   { "" }
        default { "" }
    }
    Get-FileFromURL -url "https://live.sysinternals.com/PsExec$psExecBitness.exe"

    $httpClient = [System.Net.Http.HttpClient]::new()
}