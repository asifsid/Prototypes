param (
    [Parameter(Mandatory=$true)]
    [string]$Org,

    [Parameter(Mandatory=$true)]
    [string]$ManagedIdentityId,

    [string]$TenantId = "common",

    [switch]$Rollback
)

# Check if running as administrator
if (-not ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)) {
    Write-Output "Please run in Administrator mode"
    exit
}

if (-not (Get-Module -ListAvailable -Name MSAL.PS)) {
    Write-Host "MSAL.PS module not found. Installing..."
    try
    {
        Install-Module MSAL.PS -Scope CurrentUser -Force
    }
    catch 
    {
        Write-Host "Failed to install MSAL.PS module: $_" -ForegroundColor Red
        exit 1
    }
}

$clientId = "04b07795-8ddb-461a-bbee-02f9e1bf7b46"
$scopes = "https://$Org.crm.dynamics.com/.default"

# Interactive login
$authResult = Get-MsalToken -ClientId $clientId -TenantId $TenantId -Interactive -Scopes $scopes
$accessToken = $authResult.AccessToken

$uri =  "https://$Org.crm.dynamics.com/api/data/v9.2/managedidentities($ManagedIdentityId)"

$headers = @{
    "Authorization" = "Bearer $accessToken"
    "Content-Type" = "application/json"
    "OData-MaxVersion" = "4.0"
    "OData-Version" = "4.0"
}

$body = @{
    version = if ($Rollback) { "0" } else { "1" }
} | ConvertTo-Json -Depth 3

#Write-Output "Uri: $uri"
#Write-Output "Headers: $headers"
#Write-Output "Body: $body"

try
{
    $response = Invoke-RestMethod -Uri $uri -Method Patch -Headers $headers -Body $body
    Write-Output "Update successful."
}
catch 
{
    Write-Host "Failed to update MI Record: $_" -ForegroundColor Red
    exit 1
}