<#
.SYNOPSIS
    Reads and returns a XAML GUI file as a Form

.EXAMPLE
    Initialize-Form -xamlfile "C:\gui.xaml"
#>

using namespace System.PresentationFramework
using namespace System.Windows.Markup

function Initialize-Form {
    param (
        [string]$xamlfile
    )

    [xml]$xaml = (Get-Content $xamlfile -Raw) -replace 'x:Name', 'Name'
    if ($xaml.Window) {
        $xaml.Window.RemoveAttribute('x:Class')
        $xaml.Window.RemoveAttribute('mc:Ignorable')
    }
    elseif ($xaml.UserControl) {
        $xaml.UserControl.RemoveAttribute('x:Class')
        $xaml.UserControl.RemoveAttribute('mc:Ignorable')
    }
        
    $xaml.SelectNodes("//*") | ForEach-Object {
        $_.RemoveAttribute('d:LayoutOverrides')
    }

    $reader = (New-Object System.Xml.XmlNodeReader $xaml)
    try {
        $Form = [Windows.Markup.XamlReader]::Load( $reader )
        $xaml.SelectNodes("//*[@Name]") | ForEach-Object {
            Set-Variable -Name ($_.Name) -Value $Form.FindName($_.Name) -Scope Global
        }
    }
    catch {
        Write-Host "Unable to load Windows.Markup.XamlReader";
        Write-host -f red "Encountered Error:"$_.Exception.Message
        exit
    }

    return $Form
}
