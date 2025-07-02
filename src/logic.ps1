Write-Host "Initializing environment..."
Initialize-Environment

Write-Host "Running WinUtil..."
Invoke-WinUtil

Write-Host "Applying tweaks..."
Invoke-Tweaks

Write-Host "Installing Apps..."
Install-Apps

Write-Host "Cleaning up..."
Clear-DownloadFolder