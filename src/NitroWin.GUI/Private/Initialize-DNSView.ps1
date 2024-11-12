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

    $DNSCloudflareNormal.Add_Checked({
        $DNSContinueButton.isEnabled = $true
    })

    $DNSCloudflareMalware.Add_Checked({
        $DNSContinueButton.isEnabled = $true
    })

    $DNSCloudflareMalwareAdult.Add_Checked({
        $DNSContinueButton.isEnabled = $true
    })

    $DNSQuad9.Add_Checked({
        $DNSContinueButton.isEnabled = $true
    })

    return $dnsViewForm
}