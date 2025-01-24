<#
.SYNOPSIS
    Initializes the MainView to run correctly

.DESCRIPTION
    Runs automatically when the NitroWin.GUI package is imported.

.EXAMPLE
    Initialize-MainWindow
#>

function Initialize-MainView {
    $mainViewForm = Initialize-Form -xamlfile ".\src\NitroWin.GUI\GUI\MainView.xaml"

    $imagePath = Join-Path -Path (Get-Location) -ChildPath "assets/logo/NitroWin.png"
    $imageSource = New-Object System.Windows.Media.Imaging.BitmapImage
    $imageSource.BeginInit()
    $imageSource.UriSource = [Uri]::new("file:///$imagePath")
    $imageSource.EndInit()

    $mainViewForm.FindName("MainViewTopLogoImage").Source = $imageSource

    $mainViewForm.FindName("MainViewGPUOptionComboBox").SelectedIndex = 0

    return $mainViewForm
}