function Request-Network {
    while (-Not (Test-Connection -ComputerName https://github.com -Count 1 -Quiet)) {
        Show-Prompt -message "NitroWin requires an active and unblocked network connection. Please connect to the internet and press OK to retry." -title "No network connection" -buttons OK -icon Error 
    }
}