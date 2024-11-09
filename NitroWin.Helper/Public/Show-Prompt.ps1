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