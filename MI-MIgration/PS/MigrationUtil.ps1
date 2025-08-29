
function Install-Prereq {
    Write-Host "Installing prerequisites"
    try
    {
        if (-not (Get-Module -ListAvailable -Name MSAL.PS)) {
            Write-Host "MSAL.PS module not found. Installing..."
                Install-Module MSAL.PS -Scope CurrentUser -Force
        }

        if (-not (Get-Module -ListAvailable -Name Microsoft.Graph.Applications)) {
            Write-Host "Microsoft.Graph.Applications module not found. Installing..."
            Install-Module -Name Microsoft.Graph.Applications -Scope CurrentUser -Repository PSGallery -Force
        }


    }
    catch 
    {
        Write-Host "Failed to install prerequisites: $_" -ForegroundColor Red
        exit 1
    }
    Write-Host "Prerequisites installed."
}

function Check-AdminMode {
    # Check if running as administrator
    if (-not ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)) {
        Write-Output "Please run in Administrator mode"
        exit
    }
}

function Get-OrgUrl {
    param ([string]$Org)

    if ($Org -notmatch "\.") {
        $Org = "$Org.crm.dynamics.com"
    }
    
    return $Org
}

function Login-Dataverse {
    param ([string]$Org)
    
    $clientId = "04b07795-8ddb-461a-bbee-02f9e1bf7b46"
    $scopes = "https://$Org/.default"

    $authResult = Get-MsalToken -ClientId $clientId -Interactive -Scopes $scopes
    $accessToken = $authResult.AccessToken

    return $accessToken
}

function Get-TenantId {
    param ([string]$AccessToken)

    $tokenParts = $AccessToken -split '\.'
    $payload = $tokenParts[1]

    # Add padding if needed
    switch ($payload.Length % 4) {
        2 {$payload += '=='}
        3 {$payload += '='}
    }

    $decodedBytes = [System.Convert]::FromBase64String($payload)
    $decodedJson = [System.Text.Encoding]::UTF8.GetString($decodedBytes)
    $tokenPayload = $decodedJson | ConvertFrom-Json

    return $tokenPayload.tid
}

function Invoke-DVGet {
    param (
        [string]$AccessToken,
        [string]$Url
    )

    $headers = @{
        "Authorization" = "Bearer $accessToken"
        "Content-Type" = "application/json"
        "OData-MaxVersion" = "4.0"
        "OData-Version" = "4.0"
    }

    try
    {
        $response = Invoke-RestMethod -Uri $uri -Method Get -Headers $headers
        return $response
    }
    catch 
    {
        Write-Host "$_" -ForegroundColor Red
        exit 1
    }

}