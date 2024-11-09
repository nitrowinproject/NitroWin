function Initialize-Installer {
    if ($Global:NitroWinVersion -ge $env:NitroWinVersion) {
        $result = Show-Prompt -message "Latest version already installed. Continue?" -title "Latest version already installed" -buttons YesNo -icon Question

        if (-Not ($result -eq 'Yes')) {
            Exit 0
        }
    }
}