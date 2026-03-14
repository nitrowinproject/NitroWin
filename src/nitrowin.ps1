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
        [string]$arguments = ""
    )

    $destinationPath = Get-FileFromURL -url $url

    Write-Host "Installing $name..."

    Start-Process -FilePath $destinationPath -Wait -Verb RunAs -ArgumentList $arguments
}
function Install-AppFromWinGet {
    <#
    .SYNOPSIS
        Installs an app using WinGet.

    .PARAMETER id
        The package ID of the desired app.

    .PARAMETER arguments
        Optional arguments to pass to WinGet.
    #>

    param (
        [Parameter(Mandatory = $true)]
        [string]$id,

        [Parameter(Mandatory = $false)]
        [string]$arguments = ""
    )

    Write-Host "Installing $id via WinGet..."
    Start-Process -FilePath "winget.exe" -Wait -Verb RunAs -ArgumentList "install --id $id --exact --accept-package-agreements --accept-source-agreements $arguments"
}
function Install-Apps {
    <#
    .SYNOPSIS
        Installs applications based on the configuration defined in "NitroWin.json".
        The configuration file is searched on all local drives. If not found locally,
        it will be downloaded from the NitroWin GitHub repository.
    #>

    foreach ($app in $config.apps.web) {
        if (-Not (Confirm-ProcessorArchitecture($app.arch))) { continue }

        $arguments = if ($app.args) { $app.args -join " " } else { "" }
        Install-App -name $app.name -url $app.url -arguments $arguments
    }

    foreach ($app in $config.apps.winget) {
        $arguments = if ($app.args) { $app.args -join " " } else { "" }
        Install-AppFromWinGet -id $app.id -arguments $arguments
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

        switch ($env:PROCESSOR_ARCHITECTURE) {
            "ARM64" {
                $wingetDepsArch = "arm64"
            }
            default {
                $wingetDepsArch = "x64"
            }
        }

        $files = Get-ChildItem (Join-Path -Path (Get-DownloadFolder) -ChildPath $wingetDepsArch)
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
    #>

    param (
        [Parameter(Mandatory = $true)]
        [string]$url
    )

    try {
        $fileName = [System.IO.Path]::GetFileName($url)
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
function Get-NitroWinConfig {
    <#
    .SYNOPSIS
        Loads the NitroWin JSON configuration from local disk or downloads it from GitHub.
        Returns the parsed configuration object.
    #>

    $jsonFileName = "NitroWin.json"

    foreach ($drive in (Get-PsDrive -PsProvider FileSystem)) {
        $configPath = Join-Path -Path "$($drive.Name):" -ChildPath $jsonFileName
        if (Test-Path -Path $configPath -PathType Leaf) {
            Write-Host "Found config under $configPath! Continuing with this configuration..." -ForegroundColor Green
            return Get-Content -Path $configPath -Raw | ConvertFrom-Json
        }
    }

    Write-Host "No configuration found. Downloading from GitHub..."
    try {
        $config = $global:httpClient.GetStringAsync("https://raw.githubusercontent.com/nitrowinproject/NitroWin/main/assets/Configuration/NitroWin.json").Result | ConvertFrom-Json
        Write-Host "The configuration was downloaded successfully!" -ForegroundColor Green
        return $config
    }
    catch {
        Show-InstallError -name $jsonFileName
        return $null
    }
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

    Get-FileFromURL -url "https://github.com/fafalone/RunAsTrustedInstaller/releases/latest/download/RunAsTI64.exe" | Out-Null

    $global:config = Get-NitroWinConfig
    if (-Not $config) {
        Show-Prompt -message "Config could not be loaded. Please connect to the internet and rerun NitroWin." -title "Could not load config" -buttons Ok -icon Error
        exit 1
    }
}
function Confirm-ProcessorArchitecture {
    <#
    .SYNOPSIS
        Checks if an app should be installed based on the processor architecture
    #>

    param (
        [Parameter(Mandatory = $true)]
        [object]$architectures
    )

    switch ($env:PROCESSOR_ARCHITECTURE) {
        "AMD64" {
            if ($null -ne $architectures.x64) {
                return $architectures.x64
            }
            else {
                return $true
            }
        }
        "ARM64" {
            if ($null -ne $architectures.arm64) {
                return $architectures.arm64
            }
            else {
                return $true
            }
        }
        default {
            return $true
        }
    }
}
function Clear-DesktopFolders {
    <#
    .SYNOPSIS
        This deletes everything in the current user's and the public desktop folder.
    #>

    $desktopFolder = (New-Object -ComObject Shell.Application).NameSpace('shell:Desktop').Self.Path
    $publicDesktopFolder = [Environment]::GetFolderPath("CommonDesktopDirectory")

    foreach ($path in @($desktopFolder, $publicDesktopFolder)) {
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
                $runAsTIExe = Join-Path -Path (Get-DownloadFolder) -ChildPath "RunAsTI64.exe"
                $regArgs = "$env:windir\System32\reg.exe import `"$file`""
                Start-Process -FilePath $runAsTIExe -ArgumentList $regArgs -NoNewWindow -Wait
                Write-Host "System registry tweaks imported successfully!" -ForegroundColor Green
            }
            { $_.EndsWith("System.ps1") } {
                Write-Host "Executing system PowerShell script from $file..."
                $runAsTIExe = Join-Path -Path (Get-DownloadFolder) -ChildPath "RunAsTI64.exe"
                $psArgs = "$env:windir\System32\WindowsPowerShell\v1.0\powershell.exe -ExecutionPolicy Bypass -NoProfile -File `"$file`""
                Start-Process -FilePath $runAsTIExe -ArgumentList $psArgs -NoNewWindow -Wait
                Write-Host "System PowerShell script executed successfully!" -ForegroundColor Green
            }
        }
    }

    if ($config.config.drivers) {
        Enable-AutomaticDriverInstallation
    }

    if ($config.config.store) {
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
if ($config.config.winget) {
    Install-WinGet
}
else {
    Write-Host "Not installing WinGet, as it was disabled in the config file..." -ForegroundColor Gray
}

Write-Host "`n[4/5] Installing Apps..." -ForegroundColor Cyan
Install-Apps

Write-Host "`n[5/5] Cleaning up..." -ForegroundColor Cyan
Clear-DownloadFolder
Clear-DesktopFolders
