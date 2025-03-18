function Add-MusicVideosToHome {
    $o = New-Object -ComObject Shell.Application
    $currentPins = $o.Namespace('shell:::{679f85cb-0220-4080-b29b-5540cc05aab6}').Items() | ForEach-Object { $_.Path }

    $folders = @(
        [System.Environment]::GetFolderPath('MyVideos'),
        [System.Environment]::GetFolderPath('MyMusic')
    )

    foreach ($path in $folders) {
        if ($currentPins -notcontains $path) {
            $o.Namespace($path).Self.InvokeVerb('pintohome')
        }
    }
}