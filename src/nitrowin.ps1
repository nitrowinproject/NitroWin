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
        Get-FileFromURL -url $url
        Start-Process $destinationPath
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

    Start-Process -FilePath "winget.exe" -Wait -NoNewWindow -Verb RunAs -ArgumentList "download --id $($id) --exact --skip-license --scope machine --accept-package-agreements --accept-source-agreements --interactive"
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
            $config = Get-Content -Path $configPath
            break
        }
    }

    if (-Not $config) {
        $config = (Invoke-WebRequest -Uri "https://raw.githubusercontent.com/nitrowinproject/NitroWin/main/assets/Configuration/NitroWin.Apps.txt").Content
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
        $arch = switch ($env:PROCESSOR_ARCHITECTURE) {
            "AMD64" { "x64" }
            "x86"   { "x86" }
            "ARM64" { "arm64" }
            "ARM"   { "arm" }
            default { "unknown" }
        }

        $dependencies = "https://github.com/microsoft/winget-cli/releases/latest/download/DesktopAppInstaller_Dependencies.zip"
        $dependenciesArchive = Get-FileFromURL -url $dependencies

        Expand-Archive -Path $dependenciesArchive -DestinationPath (Get-DownloadFolder)
        $files = Get-ChildItem (Join-Path -Path (Get-DownloadFolder) -ChildPath $arch)
        foreach ($file in $files) {
            Add-AppxPackage -Path $file
        }

        $winget = "https://github.com/microsoft/winget-cli/releases/latest/download/Microsoft.DesktopAppInstaller_8wekyb3d8bbwe.msixbundle"
        $wingetInstaller = Get-FileFromURL -url $winget

        Add-AppxPackage $wingetInstaller
    }
}
function Clear-DownloadFolder {
    Get-ChildItem -Path (Get-DownloadFolder) -Include *.* -File -Recurse | ForEach-Object { $_.Delete() }
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
        $filename = [System.IO.Path]::GetFileName($url)
        $destinationPath = Join-Path -Path (Get-DownloadFolder) -ChildPath $fileName

        Invoke-WebRequest -Uri $url -OutFile $destinationPath
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
    
    [System.Reflection.Assembly]::LoadWithPartialName("System.Windows.Forms")
    [System.Windows.Forms.Application]::EnableVisualStyles();
    [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
    Set-ExecutionPolicy Unrestricted -Scope Process -Force
    Get-FileFromURL -url "https://live.sysinternals.com/PsExec64.exe"
}
function Request-Network {
    while (Test-Connection -ComputerName github.com -Count 1 -Quiet) {
        Show-Prompt -message "NitroWin requires an active and unblocked network connection. Please connect to the internet and press OK to retry." -title "No network connection" -buttons OK -icon Error 
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

    $prompt = Show-Prompt -message $message -title $title -buttons YesNo -icon Error
    if ($prompt -eq 'No') {
        Exit 0
    }
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
                Start-Process -FilePath (Join-Path -Path (Get-DownloadFolder) -ChildPath "PsExec64.exe") -ArgumentList "-accepteula -s -i reg.exe import $file" -NoNewWindow
            }
            elseif ($file.EndsWith("System.ps1")) {
                Start-Process -FilePath (Join-Path -Path (Get-DownloadFolder) -ChildPath "PsExec64.exe") -ArgumentList "-accepteula -s -i powershell.exe -ExecutionPolicy Bypass -File $file" -NoNewWindow
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

    foreach ($type in $chassis.ChassisTypes) {
        if ($laptopTypes -contains $type) {
            try {
                Invoke-Expression "& { $(Invoke-RestMethod "https://christitus.com/win") } -Config $(Get-FileFromURL -url "https://raw.githubusercontent.com/nitrowinproject/NitroWin/main/assets/Configuration/WinUtil_Laptop.json") -Run"
            }
            catch {
                Show-InstallError -name "WinUtil"
            }
            return
        }
    }

    try {
        Invoke-Expression "& { $(Invoke-RestMethod "https://christitus.com/win") } -Config $(Get-FileFromURL -url "https://raw.githubusercontent.com/nitrowinproject/NitroWin/main/assets/Configuration/WinUtil_Desktop.json") -Run"
    }
    catch {
        Show-InstallError -name "WinUtil"
    }
}
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
