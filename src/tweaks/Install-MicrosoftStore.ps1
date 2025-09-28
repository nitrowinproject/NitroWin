function Install-MicrosoftStore {
    <#
    .SYNOPSIS
        Installs the Microsoft Store.
    #>
    try {
        Write-Host "Starting Microsoft Store installation..."
        Start-Process -FilePath "wsreset.exe" -Verb RunAs -ArgumentList "-i"
        Write-Host "Started Microsoft Store installation successfully!" -ForegroundColor Green
    }
    catch {
        Show-InstallError -name "Microsoft Store"
    }
}