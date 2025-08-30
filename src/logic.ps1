#Requires -RunAsAdministrator
Clear-Host

Write-Host ""
Write-Host " _   _ _ _          __        ___       "
Write-Host "| \ | (_) |_ _ __ __\ \      / (_)_ __  "
Write-Host "|  \| | | __| '__/ _ \ \ /\ / /| | '_ \ "
Write-Host "| |\  | | |_| | | (_) \ V  V / | | | | |"
Write-Host "|_| \_|_|\__|_|  \___/ \_/\_/  |_|_| |_|"
Write-Host "`n"
Write-Host "Watch how this script will heavily modify your Windows installation..."

Write-Host "`n[1/5] Initializing environment..." -ForegroundColor Cyan
Initialize-Environment

Write-Host "`n[2/5] Applying tweaks..." -ForegroundColor Cyan
Invoke-Tweaks

Write-Host "`n[3/5] Installing WinGet..." -ForegroundColor Cyan
Install-WinGet

Write-Host "`n[4/5] Installing Apps..." -ForegroundColor Cyan
Install-Apps

Write-Host "`n[5/5] Cleaning up..." -ForegroundColor Cyan
Clear-DownloadFolder
Clear-DesktopFolders