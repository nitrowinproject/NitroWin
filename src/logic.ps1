Write-Host "Checking network connection..."
Request-Network

Write-Host "Running WinUtil..."
Invoke-WinUtil

Write-Host "Applying tweaks..."
Invoke-Tweaks

Write-Host "Installing Apps..."
Install-Apps

Write-Host "Cleaning up..."
Clear-DownloadFolder