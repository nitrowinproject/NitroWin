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