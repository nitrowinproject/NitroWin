# Path to manifest
$ManifestPath = Join-Path -Path $PSScriptRoot -ChildPath 'NitroWin.Helper.psd1'

# Read manifest as hashtable
$HelperManifest = Import-PowerShellDataFile -Path $ManifestPath

# Set global variables
$Global:NitroWinVersion = $HelperManifest.ModuleVersion

# Import all functions from the Public and Private folders
Get-ChildItem -Path (Join-Path -Path $PSScriptRoot -ChildPath 'Public') -Filter *.ps1 | ForEach-Object { . $_.FullName }
Get-ChildItem -Path (Join-Path -Path $PSScriptRoot -ChildPath 'Private') -Filter *.ps1 | ForEach-Object { . $_.FullName }

# Export all public functions
$functionNames = Get-ChildItem -Path $PSScriptRoot -Filter *.ps1 -Recurse | ForEach-Object { $_.BaseName }
Export-ModuleMember -Function $functionNames

# Run initialization script
Initialize-Helper