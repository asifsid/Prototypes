. ".\MigrationUtil.ps1"

Write-Host "Welcome to Plugin MI Migration"
Write-Host "******************************"
Write-Host ""
Write-Host "This script will help you migrate your V0 Plugin Managed Identity (MI) to V1."
Write-Host ""
Write-Host "Please ensure you have the following prerequisites before proceeding:"
Write-Host "1. Have access the Dataverse Environment that has the MI configured."
Write-Host "2. Ensure you have the necessary permissions to update the App Registration."
Write-Host "3. The script must be run in Administrator mode."
Write-Host ""
Write-Host "Press Enter to continue or Ctrl+C to abort..."
Read-Host
Write-Host ""

Check-AdminMode

Install-Prereq

Write-Host "Step 1: Gathering Required Information"
Write-Host "--------------------------------------"
$Org = Read-Host "Enter the Dataverse Environment URL (e.g., org.crm.dynamics.com) or just OrgName"

$Org = Get-OrgUrl $Org
Write-Host $Org


$clientId = "04b07795-8ddb-461a-bbee-02f9e1bf7b46"
$scopes = "https://$Org/.default"

# Interactive login
$authResult = Get-MsalToken -ClientId $clientId -Interactive -Scopes $scopes
$accessToken = $authResult.AccessToken

# Extract the tenant ID
$tenantId = Get-TenantId $accessToken

$uri =  "https://$Org/api/data/v9.2/WhoAmI"
$response = Invoke-DVGet -Uri $uri
$environmentId = $response.OrganizationId

# Get Managed Identity details
$uri =  "https://$Org/api/data/v9.2/managedidentities?`$filter=version eq 0"
try
{
    $response = Invoke-RestMethod -Uri $uri -Method Get -Headers $headers
    if ($response.value.Count -eq 0) {
        Write-Host "No Managed Identities found in this environment to migrate."
        exit
    }
        
    for ($i = 0; $i -lt $response.value.Count ; $i++) {
        $mi = $response.value[$i]
        $managedIdentityId = $mi.managedidentityid
        $appId = $mi.applicationid
        $tenantid = $mi.tenantid

        Write-Host "$mi"
        Write-Host "$i"
        Write-Host "Managed Identity ID: $managedIdentityId"
        Write-Host "App ID: $appId"
        Write-Host "Tenant ID: $tenantid"
    }

    $selection = Read-Host "Enter the number of the Managed Identity to migrate"
    if ($selection -lt 0 -or $selection -ge $response.value.Count) {
        Write-Host "Invalid selection."
        exit 1
    }
    $selectedMi = $response.value[$selection]
    $managedIdentityId = $selectedMi.managedidentityid
    $appId = $selectedMi.applicationid
    $tenantid = $selectedMi.tenantid
    Write-Host ""

    #get app registration details

    $scopes = @(
        "Application.ReadWrite.All"
    )

    Connect-MgGraph -TenantId $tenantid -Scopes $scopes
    $app = Get-MgApplication -Filter "appId eq '$appId'"

    if (-not $app) {
        Write-Host "App Registration with App ID $appId not found in tenant $tenantid" -ForegroundColor Red
        exit 1
    }
    $appObjectId = $app.Id
    Write-Host "App Registration found: $($app.DisplayName) (Object ID: $appObjectId)" -ForegroundColor Green
}
catch 
{
    Write-Host "$_" -ForegroundColor Red
    exit 1
}