function Invoke-WinUtil {
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