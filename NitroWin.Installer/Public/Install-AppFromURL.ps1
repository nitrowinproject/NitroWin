<#
.SYNOPSIS
    Downloads and executes a file from the Internet

.PARAMETER url
    The URL of the executable

.EXAMPLE
    Install-FromURL -url "https://example.com/program.exe" -name "Example"
#>

function Install-AppFromURL {
    param (
        [string]$url,
        [string]$name
    )

    if (Request-URL($url) -eq 200) {
        try {
            $download = Get-FileFromURL -url $url -outpath $(Get-DownloadFolder)
        }
        catch {
            $message = "Error while downloading {0}. Continue without installing?" -f $name
            $title = "Error while downloading {0}" -f $name

            $prompt = Show-Prompt -message $message -title $title -buttons YesNo -icon Error
            if (-Not ($prompt -eq 'Yes')) {
                Exit 0
            }
        }
        if ($null -eq $download) {
            try {
                Invoke-Expression $download
            }
            catch {
                $message = "Error while installing {0}. Continue without installing?" -f $name
                $title = "Error while installing {0}" -f $name
    
                $prompt = Show-Prompt -message $message -title $title -buttons YesNo -icon Error
                if (-Not ($prompt -eq 'Yes')) {
                    Exit 0
                }
            }
        }
    }
    else {
        $message = "Error while downloading {0}. Continue without installing?" -f $name
        $title = "Error while downloading {0}" -f $name

        $prompt = Show-Prompt -message $message -title $title -buttons YesNo -icon Error
        if (-Not ($prompt -eq 'Yes')) {
            Exit 0
        }
    }
}