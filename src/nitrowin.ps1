function Install-App {
    <#
    .SYNOPSIS
        Installs an app from a given URL.

    .PARAMETER name
        The name of the app which should be installed.

    .PARAMETER url
        The URL of the installer for the desired app.

    .PARAMETER arguments
        Optional arguments to pass to the installer.
    #>

    param (
        [Parameter(Mandatory = $true)]
        [string]$name,

        [Parameter(Mandatory = $true)]
        [string]$url,

        [Parameter(Mandatory = $false)]
        [string]$arguments
    )

    $destinationPath = Get-FileFromURL -url $url

    Write-Host "Installing $name..."

    if ($arguments) {
        Start-Process -FilePath $destinationPath -Wait -Verb RunAs -ArgumentList $arguments
    }
    else {
        Start-Process -FilePath $destinationPath -Wait -Verb RunAs
    }
}
function Install-AppFromWinGet {
    <#
    .SYNOPSIS
        Installs an app using WinGet.

    .PARAMETER name
    The name of the app which should be installed.

    .PARAMETER id
        The package ID of the desired app.

    .EXAMPLE
        Install-AppFromWinGet -id "Example.Example"
    #>

    param (
        [Parameter(Mandatory = $true)]
        [string]$name,

        [Parameter(Mandatory = $true)]
        [string]$id,

        [Parameter(Mandatory = $false)]
        [string]$arguments
    )

    Write-Host "Installing $name via WinGet..."
    Start-Process -FilePath "winget.exe" -Wait -Verb RunAs -ArgumentList "install --id $($id) --exact --accept-package-agreements --accept-source-agreements $($arguments)"
}
function Install-Apps {
    <#
    .SYNOPSIS
        Installs applications based on the configuration defined in "NitroWin.json".
        The configuration file is searched on all local drives. If not found locally,
        it will be downloaded from the NitroWin GitHub repository.
    #>

    $jsonFileName = "NitroWin.json"

    foreach ($drive in (Get-PsDrive -PsProvider FileSystem)) {
        $configPath = Join-Path -Path "$($drive.Name):" -ChildPath $jsonFileName
        if (Test-Path -Path $configPath -PathType Leaf) {
            Write-Host "Found config under $configPath! Continuing with this configuration..." -ForegroundColor Green
            $config = Get-Content -Path $configPath -Raw | ConvertFrom-Json
            break
        }
    }

    if (-Not $config) {
        Write-Host "No configuration found. Downloading from GitHub..."
        try {
            $config = $httpClient.GetStringAsync("https://raw.githubusercontent.com/nitrowinproject/NitroWin/main/assets/Configuration/NitroWin.json").Result | ConvertFrom-Json
            Write-Host "The configuration was downloaded successfully!" -ForegroundColor Green
        }
        catch {
            Show-InstallError -name $jsonFileName
        }
    }

    foreach ($app in $config.apps) {
        if (app.arch -notcontains $arch) { continue }

        switch ($app.source) {
            "web" {
                $arguments = $app.args -join " "
                Install-App -name $app.name -url $app.url -arguments $arguments
            }
            "winget" {
                $arguments = if ($app.args) { "$($app.args)" } else { "" }
                Install-AppFromWinGet -name $app.name -id $app.id -arguments $arguments
            }
        }
    }
}
function Install-WinGet {
    <#
    .SYNOPSIS
        It installs WinGet, if it isn't already installed.
    #>

    if (-Not (Get-Command winget -ErrorAction SilentlyContinue)) {
        $dependencies = "https://github.com/microsoft/winget-cli/releases/latest/download/DesktopAppInstaller_Dependencies.zip"
        $dependenciesArchive = Get-FileFromURL -url $dependencies

        try {
            Write-Host "Extracting WinGet dependencies..."
            Expand-Archive -Path $dependenciesArchive -DestinationPath (Get-DownloadFolder)
            Write-Host "Extracted WinGet dependencies!" -ForegroundColor Green
        }
        catch {
            Show-InstallError -name "WinGet dependencies"
        }

        $files = Get-ChildItem (Join-Path -Path (Get-DownloadFolder) -ChildPath $arch)
        foreach ($file in $files) {
            try {
                Write-Host "Installing $file..."
                Add-AppxPackage -Path $file
                Write-Host "Installed $file!" -ForegroundColor Green
            }
            catch {
                Show-InstallError -name "WinGet dependencies"
            }
        }

        Write-Host "Installed WinGet dependencies!" -ForegroundColor Green

        $winget = "https://github.com/microsoft/winget-cli/releases/latest/download/Microsoft.DesktopAppInstaller_8wekyb3d8bbwe.msixbundle"
        $wingetInstaller = Get-FileFromURL -url $winget

        try {
            Write-Host "Installing WinGet..."
            Add-AppxPackage $wingetInstaller
            Write-Host "Installed WinGet!" -ForegroundColor Green
        }
        catch {
            Show-InstallError -name "WinGet"
        }
    }
    else {
        Write-Host "WinGet is already installed..." -ForegroundColor Gray
    }
}
function Clear-DesktopFolders {
    <#
    .SYNOPSIS
        This deletes everything in the current user's and the public desktop folder.
    #>

    foreach ($path in @((Get-DesktopFolder), (Get-PublicDesktopFolder))) {
        Get-ChildItem -Path $path -File -Recurse | Remove-Item -ErrorAction SilentlyContinue -Force

        Get-ChildItem -Path $path -Directory -Recurse |
        Where-Object { !(Get-ChildItem -Path $_.FullName -Force) } |
        Remove-Item -Force -Recurse -ErrorAction SilentlyContinue
    }
}
function Clear-DownloadFolder {
    <#
    .SYNOPSIS
        This deletes everything in the current user's download folder.
    #>

    Get-ChildItem -Path (Get-DownloadFolder) -File -Recurse | Remove-Item -ErrorAction SilentlyContinue -Force

    Get-ChildItem -Path (Get-DownloadFolder) -Directory -Recurse |
    Where-Object { !(Get-ChildItem -Path $_.FullName -Force) } |
    Remove-Item -Force -Recurse -ErrorAction SilentlyContinue
}
function Get-DesktopFolder {
    <#
    .SYNOPSIS
        This returns the current user's desktop folder.
    #>

    $value = (New-Object -ComObject Shell.Application).NameSpace('shell:Desktop').Self.Path
    return $value
}
function Get-DownloadFolder {
    <#
    .SYNOPSIS
        This returns the current user's download folder.
    #>

    $value = (New-Object -ComObject Shell.Application).NameSpace('shell:Downloads').Self.Path
    return $value
}
function Get-FileFromURL {
    <#
    .SYNOPSIS
        Downloads a file from the Internet and returns its path after downloading.

    .PARAMETER url
        The URL of the file to be downloaded.

    .EXAMPLE
        Get-FileFromURL -url "https://example.com/example.txt"
    #>

    param (
        [Parameter(Mandatory = $true)]
        [string]$url
    )

    try {
        $global:fileName = [System.IO.Path]::GetFileName($url)
        $destinationPath = Join-Path -Path (Get-DownloadFolder) -ChildPath $fileName

        Write-Host "Downloading: $fileName..."

        $response = $httpClient.GetAsync($url).Result
        [System.IO.File]::WriteAllBytes($destinationPath, $response.Content.ReadAsByteArrayAsync().Result)

        Write-Host "Downloaded: $fileName!" -ForegroundColor Green

        return $destinationPath
    }
    catch {
        Show-InstallError -name $fileName
    }
}
function Get-PublicDesktopFolder {
    <#
    .SYNOPSIS
        This returns the public desktop folder.
    #>

    $value = [Environment]::GetFolderPath("CommonDesktopDirectory")
    return $value
}
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
function Set-RegistryValue {
    <#
    .SYNOPSIS
        Sets a specific registry value.

    .PARAMETER Path
        Path to the registry value

    .PARAMETER Name
        Name of the registry value

    .PARAMETER Value
        Registry value
    #>

    param (
        [Parameter(Mandatory = $true)]
        [string]$Path,

        [Parameter(Mandatory = $true)]
        [string]$Name,

        [Parameter(Mandatory = $true)]
        [UInt32]$Value
    )

    if (-not (Test-Path $Path)) {
        New-Item -Path $Path -Force | Out-Null
    }
    Set-ItemProperty -Path $Path -Name $Name -Value $Value -Type DWord
}
function Show-InstallError {
    <#
    .SYNOPSIS
        Throws an error if something fails to install.
    #>

    param (
        [Parameter(Mandatory = $true)]
        [string]$name
    )

    $message = "Error while installing $name. Continue without installing?"
    $title = "Error while installing $name"

    Write-Host "Error while installing $name." -ForegroundColor Red

    $prompt = Show-Prompt -message $message -title $title -buttons YesNo -icon Error
    if ($prompt -eq 'No') {
        Write-Host "Quitting..." -ForegroundColor Red
        Exit 0
    }

    Write-Host "Continuing..." -ForegroundColor Yellow
}
function Show-Prompt {
    <#
    .SYNOPSIS
        Displays a message box and returns its response.

    .PARAMETER title
        The title of the message box.

    .PARAMETER message
        The message box message.

    .PARAMETER buttons
        The buttons that will appear.

    .PARAMETER icon
        The message box icon.

    .EXAMPLE
        Show-Prompt -message "Hello World" -title "Test" -buttons OK -icon Information
    #>

    param (
        [Parameter(Mandatory = $true)]
        [string]$title,

        [Parameter(Mandatory = $true)]
        [string]$message,

        [Parameter(Mandatory = $true)]
        [System.Windows.Forms.MessageBoxButtons]$buttons,

        [System.Windows.Forms.MessageBoxIcon]$icon
    )

    $result = [System.Windows.Forms.MessageBox]::Show($message, $title, $buttons, $icon)
    return $result
}
function Enable-AutomaticDriverInstallation {
    <#
    .SYNOPSIS
        Enables the automatic installation of drivers via Windows Update.
    #>

    try {
        Write-Host "Enabling automatic driver installation..."

        Set-RegistryValue "HKLM:\SOFTWARE\Microsoft\PolicyManager\current\device\Update" "ExcludeWUDriversInQualityUpdate" 0
        Set-RegistryValue "HKLM:\SOFTWARE\Microsoft\PolicyManager\default\Update" "ExcludeWUDriversInQualityUpdate" 0
        Set-RegistryValue "HKLM:\SOFTWARE\Microsoft\WindowsUpdate\UX\Settings" "ExcludeWUDriversInQualityUpdate" 0
        Set-RegistryValue "HKLM:\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate" "ExcludeWUDriversInQualityUpdate" 0
        Set-RegistryValue "HKLM:\SOFTWARE\Microsoft\PolicyManager\default\Update\ExcludeWUDriversInQualityUpdate" "value" 0
        Set-RegistryValue "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Device Metadata" "PreventDeviceMetadataFromNetwork" 0

        Set-RegistryValue "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\DriverSearching" "SearchOrderConfig" 1
        Set-RegistryValue "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\DriverSearching" "DontSearchWindowsUpdate" 0

        Write-Host "Enabled automatic driver installation successfully!" -ForegroundColor Green
    }
    catch {
        Show-InstallError -name "automatic driver installation"
    }
}
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
function Invoke-Tweaks {
    <#
    .SYNOPSIS
        Downloads and invokes all tweaks from NitroWin. Also checks config for extra tweaks.
    #>

    $jsonFileName = "NitroWin.json"

    foreach ($drive in (Get-PsDrive -PsProvider FileSystem)) {
        $configPath = Join-Path -Path "$($drive.Name):" -ChildPath $jsonFileName
        if (Test-Path -Path $configPath -PathType Leaf) {
            Write-Host "Found config under $configPath! Continuing with this configuration..." -ForegroundColor Green
            $config = Get-Content -Path $configPath -Raw | ConvertFrom-Json
            break
        }
    }

    if (-Not $config) {
        Write-Host "No configuration found. Downloading from GitHub..."
        try {
            $config = $httpClient.GetStringAsync("https://raw.githubusercontent.com/nitrowinproject/NitroWin/main/assets/Configuration/NitroWin.json").Result | ConvertFrom-Json
            Write-Host "The configuration was downloaded successfully!" -ForegroundColor Green
        }
        catch {
            Show-InstallError -name $jsonFileName
        }
    }

    $urls = @(
        "https://raw.githubusercontent.com/nitrowinproject/Tweaks/main/NitroWin.Tweaks.User.reg",
        "https://raw.githubusercontent.com/nitrowinproject/Tweaks/main/NitroWin.Tweaks.User.ps1",
        "https://raw.githubusercontent.com/nitrowinproject/Tweaks/main/NitroWin.Tweaks.System.reg",
        "https://raw.githubusercontent.com/nitrowinproject/Tweaks/main/NitroWin.Tweaks.System.ps1"
    )

    foreach ($url in $urls) {
        $file = Get-FileFromURL -url $url
        switch ($file) {
            { $_.EndsWith("User.reg") } {
                Write-Host "Importing user registry tweaks from $file..."
                Start-Process -FilePath "reg" -ArgumentList "import `"$file`"" -NoNewWindow -Wait
                Write-Host "User registry tweaks imported successfully!" -ForegroundColor Green
            }
            { $_.EndsWith("User.ps1") } {
                Write-Host "Executing user PowerShell script from $file..."
                Start-Process -FilePath "powershell.exe" -ArgumentList "-ExecutionPolicy Bypass -File `"$file`"" -NoNewWindow -Wait
                Write-Host "User PowerShell script executed successfully!" -ForegroundColor Green
            }
            { $_.EndsWith("System.reg") } {
                Write-Host "Importing system registry tweaks from $file..."
                Start-Process -FilePath (Join-Path -Path (Get-DownloadFolder) -ChildPath "RunAsTI$runAsTIBitness.exe") -ArgumentList "$env:windir\System32\reg.exe import ""$file""" -NoNewWindow -Wait
                Write-Host "System registry tweaks imported successfully!" -ForegroundColor Green
            }
            { $_.EndsWith("System.ps1") } {
                Write-Host "Executing system PowerShell script from $file..."
                Start-Process -FilePath (Join-Path -Path (Get-DownloadFolder) -ChildPath "RunAsTI$runAsTIBitness.exe") -ArgumentList "$env:windir\System32\WindowsPowerShell\v1.0\powershell.exe -ExecutionPolicy Bypass -NoProfile -File ""$file""" -NoNewWindow -Wait
                Write-Host "System PowerShell script executed successfully!" -ForegroundColor Green
            }
        }
    }

    if ($config.drivers) {
        Enable-AutomaticDriverInstallation
    }

    if ($config.store) {
        Install-MicrosoftStore
    }
}
#Requires -RunAsAdministrator
$host.ui.RawUI.WindowTitle = "NitroWin"
Clear-Host

Write-Host ""
Write-Host " _   _ _ _          __        ___       "
Write-Host "| \ | (_) |_ _ __ __\ \      / (_)_ __  "
Write-Host "|  \| | | __| '__/ _ \ \ /\ / /| | '_ \ "
Write-Host "| |\  | | |_| | | (_) \ V  V / | | | | |"
Write-Host "|_| \_|_|\__|_|  \___/ \_/\_/  |_|_| |_|"
Write-Host "`n"
Write-Host "Watch how this script will improve your Windows installation..."

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
