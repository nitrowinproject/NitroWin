function Initialize-Environment {
    [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
    Set-ExecutionPolicy Unrestricted -Scope Process -Force
    Get-FileFromURL -url "https://live.sysinternals.com/PsExec64.exe"
}