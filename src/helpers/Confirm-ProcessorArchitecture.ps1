function Confirm-ProcessorArchitecture {
    <#
    .SYNOPSIS
        Checks if an app should be installed based on the processor architecture
    #>

    param (
        [Parameter(Mandatory = $true)]
        [object]$architectures
    )

    switch ($env:PROCESSOR_ARCHITECTURE) {
        "AMD64" {
            if ($null -ne $architectures.x64) {
                return $architectures.x64
            }
            else {
                return $true
            }
        }
        "ARM64" {
            if ($null -ne $architectures.arm64) {
                return $architectures.arm64
            }
            else {
                return $true
            }
        }
        default {
            return $true
        }
    }
}