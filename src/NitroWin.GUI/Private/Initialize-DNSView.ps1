<#
.SYNOPSIS
    Initializes the DNSView to run correctly

.DESCRIPTION
    Runs automatically when the NitroWin.GUI package is imported.

.EXAMPLE
    Initialize-DNSView
#>

function Initialize-DNSView {
    $dnsViewForm = Initialize-Form -xamlfile ".\src\NitroWin.GUI\GUI\DNSView.xaml"

    return $dnsViewForm
}