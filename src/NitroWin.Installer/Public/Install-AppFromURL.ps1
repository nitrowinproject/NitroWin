<#
.SYNOPSIS
    Downloads and executes a file from the Internet

.PARAMETER url
    The URL of the executable

.PARAMETER name
    The name of the app

.PARAMETER arguments
    Arguments to give to the installer

.EXAMPLE
    Install-AppFromURL -url "https://example.com/program.exe" -name "Example" -arguments "/quiet"
#>

function Install-AppFromURL {
    param (
        [Parameter(Mandatory=$true)]
        [string]$url,

        [Parameter(Mandatory=$true)]
        [string]$name,

        [string]$arguments = $null
    )

    try {
        $download = Get-FileFromURL -url $url -outpath $(Get-DownloadFolder)
        Start-Process $download -ArgumentList $arguments -Wait
    }
    catch {
        $message = "Error while installing $($name). Continue without installing?"
        $title = "Error while installing $($name)"

        $prompt = Show-Prompt -message $message -title $title -buttons YesNo -icon Error
        if (-Not ($prompt -eq 'Yes')) {
            Exit 0
        }
    }
}