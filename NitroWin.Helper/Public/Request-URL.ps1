function Request-URL {
    param (
        [string]$url
    )
    
    # Create request
    $request = [System.Net.WebRequest]::Create($url)

    # Get response
    $response = $request.GetResponse()

    # Save HTTP code as an integer
    $status = [int]$response.StatusCode

    # Close request
    if ($null -ne $reponse) { 
        $response.Close() 
    }

    # Return HTTP code
    return $status
}