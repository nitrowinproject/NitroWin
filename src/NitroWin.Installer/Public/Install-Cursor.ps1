<#
.SYNOPSIS
    Installs a cursor from a specified directory

.DESCRIPTION
    The cursor directory must contain a Install.inf file for this function to work properly.

.PARAMETER cursordir
    The directory where the cursor is located

.EXAMPLE
    Install-Cursor -cursordir "C:\mycursor"
#>

function Install-Cursor {
    param (
        [string]$cursordir
    )

    try {
        Get-ChildItem -Path $cursordir -Name "Install.inf" | ForEach-Object { Start-Process -FilePath "pnputil.exe" -ArgumentList "/add-driver `"$_.FullName`" /install" -Verb RunAs }
    }
    catch {
        $prompt = Show-Prompt -message "Error while installing custom cursor. Continue without installing?" -title "Error while installing custom cursor" -buttons YesNo -icon Error
        if (-Not ($prompt -eq 'Yes')) {
            Exit 0
        }
    }
}