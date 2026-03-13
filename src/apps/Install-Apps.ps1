function Install-Apps {
    <#
    .SYNOPSIS
        Installs applications based on the configuration defined in "NitroWin.json".
        The configuration file is searched on all local drives. If not found locally,
        it will be downloaded from the NitroWin GitHub repository.
    #>

    $config = Get-NitroWinConfig
    if (-Not $config) { return }

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