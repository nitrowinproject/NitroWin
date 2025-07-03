function Install-App {
    <#
    .SYNOPSIS
        Installs an app from a given URL.

    .PARAMETER url
        The URL of the installer for the desired app.
    #>

    param (
        [Parameter(Mandatory=$true)]
        [string]$url
    )

    try {
        $destinationPath = Get-FileFromURL -url $url

        Write-Host "Installing..."
        Start-Process -FilePath $destinationPath -Wait -Verb RunAs
        Write-Host "Installed!"
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
        [string]$id
    )

    try {
        Write-Host "Installing $id via WinGet..."
        Start-Process -FilePath "winget.exe" -Wait -Verb RunAs -ArgumentList "install --id $($id) --exact --scope machine --accept-package-agreements --accept-source-agreements"
        Write-Host "Installed $id!"
    }
    catch {
        Show-InstallError -name $id
    }
}
function Install-Apps {
    <#
    .SYNOPSIS
        Installs applications based on the configuration defined in "NitroWin.Apps.txt".
        The configuration file is searched on all local drives. If not found locally,
        it will be downloaded from the NitroWin GitHub repository.
    #>

    foreach ($drive in (Get-PsDrive -PsProvider FileSystem)) {
        $configPath = Join-Path -Path "$($drive.Name):" -ChildPath "NitroWin.Apps.txt"
        if (Test-Path -Path $configPath -PathType Leaf) {
            Write-Host "Found config under $configPath! Continuing with this configuration..."
            $config = Get-Content -Path $configPath
            break
        }
    }

    if (-Not $config) {
        Write-Host "No configuration found. Downloading from GitHub..."
        try {
            $config = $httpClient.GetStringAsync("https://raw.githubusercontent.com/nitrowinproject/NitroWin/main/assets/Configuration/NitroWin.Apps.txt").Result -split "`r?`n"
            Write-Host "The configuration was downloaded successfully!"
        }
        catch {
            Show-InstallError -name "NitroWin.Apps.txt"
        }
    }

    foreach ($app in $config) {
        if ($app.Split(";")[0] -eq "web") {
            Install-App -url $app.Split(";")[1]
        }
        elseif ($app.Split(";")[0] -eq "winget") {
            Install-AppFromWinGet -id $app.Split(";")[1]
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
            Write-Host "Extracted WinGet dependencies!"
        }
        catch {
            Show-InstallError -name "WinGet dependencies"
        }

        $files = Get-ChildItem (Join-Path -Path (Get-DownloadFolder) -ChildPath $arch)
        foreach ($file in $files) {
            try {
                Write-Host "Installing $file..."
                Add-AppxPackage -Path $file
                Write-Host "Installed $file!"
            }
            catch {
                Show-InstallError -name "WinGet dependencies"
            }
        }

        Write-Host "Installed WinGet dependencies!"

        $winget = "https://github.com/microsoft/winget-cli/releases/latest/download/Microsoft.DesktopAppInstaller_8wekyb3d8bbwe.msixbundle"
        $wingetInstaller = Get-FileFromURL -url $winget

        try {
            Write-Host "Installing WinGet..."
            Add-AppxPackage $wingetInstaller
            Write-Host "Installed WinGet!"
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
        $global:filename = [System.IO.Path]::GetFileName($url)
        $destinationPath = Join-Path -Path (Get-DownloadFolder) -ChildPath $fileName

        Write-Host "Downloading: $fileName..."

        $response = $httpClient.GetAsync($url).Result
        [System.IO.File]::WriteAllBytes($destinationPath, $response.Content.ReadAsByteArrayAsync().Result)

        Write-Host "Downloaded: $fileName..."

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

    Write-Host "Error while installing $name."

    $prompt = Show-Prompt -message $message -title $title -buttons YesNo -icon Error
    if ($prompt -eq 'No') {
        Write-Host "Quitting..."
        Exit 0
    }

    Write-Host "Continuing..."
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
            if ($file.EndsWith("System.reg")) {
                Start-Process -FilePath (Join-Path -Path (Get-DownloadFolder) -ChildPath "PsExec$psExecBitness.exe") -ArgumentList "-accepteula -s -i reg.exe import $file" -NoNewWindow -Wait
            }
            elseif ($file.EndsWith("System.ps1")) {
                Start-Process -FilePath (Join-Path -Path (Get-DownloadFolder) -ChildPath "PsExec$psExecBitness.exe") -ArgumentList "-accepteula -s -i powershell.exe -ExecutionPolicy Bypass -File $file" -NoNewWindow -Wait
            }
            elseif ($file.EndsWith(".reg")) {
                Start-Process -FilePath "reg" -ArgumentList "import `"$file`"" -NoNewWindow
            }
            elseif ($file.EndsWith(".ps1")) {
                Invoke-Expression $file
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
        $command = 'Invoke-Expression "& { $(Invoke-RestMethod ''https://christitus.com/win'') } -Config "' + $configPath + '" -Run"'
        Start-Process -FilePath "powershell.exe" -ArgumentList "-NoProfile", "-ExecutionPolicy", "Bypass", "-Command", $command -Wait -Verb RunAs
    }
    catch {
        Show-InstallError -name "WinUtil"
    }
}
Write-Host "Initializing environment..."
Initialize-Environment

Write-Host "Running WinUtil..."
Invoke-WinUtil

Write-Host "Applying tweaks..."
Invoke-Tweaks

Write-Host "Installing WinGet..."
Install-WinGet

Write-Host "Installing Apps..."
Install-Apps

Write-Host "Cleaning up..."
Clear-DownloadFolder
