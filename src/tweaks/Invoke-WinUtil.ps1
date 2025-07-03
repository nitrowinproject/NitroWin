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
        $command = "Invoke-Expression '& { $(Invoke-RestMethod https://christitus.com/win) } -Config `"$configPath`" -Run'"
        Start-Process -FilePath "powershell.exe" -ArgumentList "-NoProfile", "-ExecutionPolicy", "Bypass", "-Command", $command -Wait -Verb RunAs
    }
    catch {
        Show-InstallError -name "WinUtil"
    }
}