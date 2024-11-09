<#
.SYNOPSIS
    Displays a message box and returns its response

.PARAMETER title
    The title of the message box

.PARAMETER message
    The message box message

.PARAMETER buttons
    The buttons that will appear

.PARAMETER icon
    The message box icon

.EXAMPLE
    Show-Prompt -message "Hello World" -title "Test" -buttons OK -icon Information
#>

function Show-Prompt {
    param (
        [string]$title,
        [string]$message,
        [System.Windows.Forms.MessageBoxButtons]$buttons,
        [System.Windows.Forms.MessageBoxIcon]$icon
    )

    $result = [System.Windows.Forms.MessageBox]::Show($message, $title, $buttons, $icon)
    return $result
}