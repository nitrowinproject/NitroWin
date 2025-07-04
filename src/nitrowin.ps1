function Install-App {
    <#
    .SYNOPSIS
        Installs an app from a given URL.

    .PARAMETER url
        The URL of the installer for the desired app.

    .PARAMETER arguments
        Optional arguments to pass to the installer.
    #>

    param (
        [Parameter(Mandatory=$true)]
        [string]$url,

        [Parameter(Mandatory=$false)]
        [string]$arguments
    )

    try {
        $destinationPath = Get-FileFromURL -url $url

        Write-Host "Installing $fileName..."
        Start-Process -FilePath $destinationPath -Wait -Verb RunAs -ArgumentList $arguments
        Write-Host "Installed $fileName!" -ForegroundColor Green
    }
    catch {
        Show-InstallError -name $fileName
    }
}
function Install-AppFromWinGet {
    <#
    .SYNOPSIS
        Installs an app using WinGet.

    .PARAMETER id
        The package ID of the desired app.

    .EXAMPLE
        Install-AppFromWinGet -id "Example.Example"
    #>

    param (
        [Parameter(Mandatory=$true)]
        [string]$id,

        [Parameter(Mandatory=$false)]
        [string]$arguments
    )

    try {
        Write-Host "Installing $id via WinGet..."
        Start-Process -FilePath "winget.exe" -Wait -Verb RunAs -ArgumentList "install --id $($id) --exact --accept-package-agreements --accept-source-agreements $($arguments)"
        Write-Host "Installed $id!" -ForegroundColor Green
    }
    catch {
        Show-InstallError -name $id
    }
}
function Install-Apps {
    <#
    .SYNOPSIS
        Installs applications based on the configuration defined in "NitroWin.Apps.json".
        The configuration file is searched on all local drives. If not found locally,
        it will be downloaded from the NitroWin GitHub repository.
    #>

    $jsonFileName = "NitroWin.Apps.json"

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
            $config = $httpClient.GetStringAsync("https://raw.githubusercontent.com/nitrowinproject/NitroWin/main/assets/Configuration/NitroWin.Apps.json").Result | ConvertFrom-Json
            Write-Host "The configuration was downloaded successfully!" -ForegroundColor Green
        }
        catch {
            Show-InstallError -name $jsonFileName
        }
    }

    foreach ($app in $config.apps) {
        if ($app.arch -notcontains $arch) { continue }

        switch ($app.source) {
            "web" {
                $arguments = $app.args -join " "
                Install-App -url $app.url -arguments $arguments
            }
            "winget" {
                $arguments = if ($app.args) { "$($app.args)" } else { "" }
                Install-AppFromWinGet -id $app.id -arguments $arguments
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
        [Parameter(Mandatory=$true)]
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

    $global:psExecBitness = switch ($env:PROCESSOR_ARCHITECTURE) {
        "AMD64" { "64" }
        "x86"   { "" }
        "ARM64" { "64" }
        "ARM"   { "" }
        default { "" }
    }
    Get-FileFromURL -url "https://live.sysinternals.com/PsExec$psExecBitness.exe" | Out-Null

    $global:arch = switch ($env:PROCESSOR_ARCHITECTURE) {
        "AMD64" { "x64" }
        "x86"   { "x86" }
        "ARM64" { "arm64" }
        "ARM"   { "arm" }
        default { "unknown" }
    }
}
function Show-InstallError {
    <#
    .SYNOPSIS
        Throws an error if something fails to install.
    #>

    param (
        [Parameter(Mandatory=$true)]
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
        [Parameter(Mandatory=$true)]
        [string]$title,

        [Parameter(Mandatory=$true)]
        [string]$message,

        [Parameter(Mandatory=$true)]
        [System.Windows.Forms.MessageBoxButtons]$buttons,

        [System.Windows.Forms.MessageBoxIcon]$icon
    )

    $result = [System.Windows.Forms.MessageBox]::Show($message, $title, $buttons, $icon)
    return $result
}
function Invoke-Tweaks {
    <#
    .SYNOPSIS
        Downloads and invokes all tweaks from NitroWin.
    #>

    $urls = @(
        "https://raw.githubusercontent.com/nitrowinproject/Tweaks/main/NitroWin.Tweaks.User.reg",
        "https://raw.githubusercontent.com/nitrowinproject/Tweaks/main/NitroWin.Tweaks.User.ps1",
        "https://raw.githubusercontent.com/nitrowinproject/Tweaks/main/NitroWin.Tweaks.System.reg",
        "https://raw.githubusercontent.com/nitrowinproject/Tweaks/main/NitroWin.Tweaks.System.ps1"
    )

    foreach ($url in $urls) {
        try {
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
                    Start-Process -FilePath (Join-Path -Path (Get-DownloadFolder) -ChildPath "PsExec$psExecBitness.exe") -ArgumentList "-accepteula -s -i reg.exe import $file" -NoNewWindow -Wait
                    Write-Host "System registry tweaks imported successfully!" -ForegroundColor Green
                }
                { $_.EndsWith("System.ps1") } {
                    Write-Host "Executing system PowerShell script from $file..."
                    Start-Process -FilePath (Join-Path -Path (Get-DownloadFolder) -ChildPath "PsExec$psExecBitness.exe") -ArgumentList "-accepteula -s -i powershell.exe -ExecutionPolicy Bypass -File $file" -NoNewWindow -Wait
                    Write-Host "System PowerShell script executed successfully!" -ForegroundColor Green
                }
            }
        }
        catch {
            Show-InstallError -name $file
        }
    }
}
function Invoke-WinUtil {
    <#
    .SYNOPSIS
        Runs WinUtil with an automatic configuration for either laptops or desktops.
    #>

    $chassis = Get-CimInstance -ClassName Win32_SystemEnclosure
    $laptopTypes = @(8, 9, 10, 14, 30, 31)

    $isLaptop = $false
    foreach ($type in $chassis.ChassisTypes) {
        if ($laptopTypes -contains $type) {
            $isLaptop = $true
            break
        }
    }

    $configUrl = if ($isLaptop) {
        "https://raw.githubusercontent.com/nitrowinproject/NitroWin/main/assets/Configuration/WinUtil_Laptop.json"
    } else {
        "https://raw.githubusercontent.com/nitrowinproject/NitroWin/main/assets/Configuration/WinUtil_Desktop.json"
    }

    try {
        $configPath = Get-FileFromURL -url $configUrl
        $command = 'Invoke-Expression "& { $(Invoke-RestMethod ''https://christitus.com/win'') } -Config `"' + $configPath + '`" -Run"'
        Start-Process -FilePath "powershell.exe" -ArgumentList "-NoProfile", "-ExecutionPolicy", "Bypass", "-Command", $command -Wait -Verb RunAs
    }
    catch {
        Show-InstallError -name "WinUtil"
    }
}
Clear-Host

Write-Host ""
Write-Host " _   _ _ _          __        ___       "
Write-Host "| \ | (_) |_ _ __ __\ \      / (_)_ __  "
Write-Host "|  \| | | __| '__/ _ \ \ /\ / /| | '_ \ "
Write-Host "| |\  | | |_| | | (_) \ V  V / | | | | |"
Write-Host "|_| \_|_|\__|_|  \___/ \_/\_/  |_|_| |_|"
Write-Host "`n"
Write-Host "Watch how this script will heavily modify your Windows installation..."

Write-Host "`n[1/6] Initializing environment..." -ForegroundColor Cyan
Initialize-Environment

Write-Host "`n[2/6] Running WinUtil..." -ForegroundColor Cyan
Invoke-WinUtil

Write-Host "`n[3/6] Applying tweaks..." -ForegroundColor Cyan
Invoke-Tweaks

Write-Host "`n[4/6] Installing WinGet..." -ForegroundColor Cyan
Install-WinGet

Write-Host "`n[5/6] Installing Apps..." -ForegroundColor Cyan
Install-Apps

Write-Host "`n[6/6] Cleaning up..." -ForegroundColor Cyan
Clear-DownloadFolder
