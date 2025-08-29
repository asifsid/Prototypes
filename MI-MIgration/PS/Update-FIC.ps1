 . ".\MigrationUtil.ps1"

 param (
    [Parameter(Mandatory=$true)]
    [string]$Org,

    [Parameter(Mandatory=$true)]
    [string]$ManagedIdentityId,
)

 [Parameter(Mandatory=$true)]
    [string]$EnvironmentId,

    [Parameter(Mandatory=$true)]
    [string]$CertPath,
    
    [Parameter(Mandatory=$false)]
    [ValidateSet("Test", "PreProd", "Prod")]
    [string]$ClusterCat = "Prod",

    [Parameter(Mandatory=$false)]
    [string]$UpdateAppObjectId,

    [Parameter(Mandatory=$false)]
    [string]$UpdateAppInTenantId = $TenantID  

$Org = Get-OrgUrl $Org

$acessToken = Login-Dataverse $Ord
$tenantId = Get-TenantId $acessToken

$response = Invoke-DVGet -Uri "https://$Org/api/data/v9.2/WhoAmI"
$environmentId = $response.OrganizationId



$clusterAppIds = @{
    "Test" = "L5f3f5fVhEuUXYRgAT1Q4w"  # Test
    "PreProd" = "CQSGf3JJtEi27nY2ePL7UQ"  # PreProd
    "Prod" = "qzXoWDkuqUa3l6zM5mM0Rw"  # Prod
}

$encodedTenantId = [System.Web.HttpUtility]::UrlEncode($TenantId)
$encodedClusterAppId = $clusterAppIds[$ClusterCat]

try
{
    # Determine file extension
    $certFolder = [System.IO.Path]::GetDirectoryName($CertPath)
    $certFileName = [System.IO.Path]::GetFileName($CertPath)
    $certFileExt = [System.IO.Path]::GetExtension($CertPath)

    if ($certFileExt -eq ".pfx") {
        # Extract .cer from .pfx
        $cert = Get-PfxCertificate -FilePath $CertPath
        $CertPath = "$certFolder\$certFileName.cer"
        $cert.RawData | Set-Content -Encoding Byte -Path $CertPath
    }
    elseif ($certFileExt -ne ".cer") {
        Write-Host "Unsupported file type. Please provide a .cer or .pfx certificate."
        exit 1
    }

    # Load and show cert details
    $cert = New-Object System.Security.Cryptography.X509Certificates.X509Certificate2
    $cert.Import($CertPath)

    $subject = $cert.Subject
    $issuer = $cert.Issuer
    $isSelfSigned = $cert.Subject -eq $cert.Issuer
    
    if ($isSelfSigned) {
        $hashOutput = CertUtil -hashfile $CertPath SHA256
        $lines = $hashOutput -split "`r`n"
        $certHash = $lines[1].Trim()    
    }
}
catch 
{
    Write-Host "Failed to process certificate: $_" -ForegroundColor Red
    exit 1
}

$ficIss = "https://login.microsoftonline.com/$TenantID/v2.0"
$ficSub = if ($isSelfSigned) {
            "/eid1/c/pub/t/$encodedTenantId/a/$encodedClusterAppId/n/plugin/e/$EnvironmentId/h/$certHash"
          } else {
            "/eid1/c/pub/t/$encodedTenantId/a/$encodedClusterAppId/n/plugin/e/$environmentId/i/$issuer/s/$subject"
          }

if (-not $UpdateAppObjectId) {
    Write-Host "-----------------------------------------------------------------------------------------------"
    Write-Host "Add a new FIC with the following values for your App Registration or UAMI through Azure Portal:"
    Write-Host "-----------------------------------------------------------------------------------------------"
    Write-Host "Issuer:"
    Write-Host "$ficIss" -ForegroundColor Blue
    Write-Host "Subject:"
    Write-Host "$ficSub" -ForegroundColor Blue
    Write-Host "Use an ppropriate name. Leave audience as the default value." -ForegroundColor DarkGray
}
else {
    # Ensure Microsoft.Graph module is installed
    if (-not (Get-Module -ListAvailable -Name Microsoft.Graph.Applications)) {
        Write-Host "Microsoft.Graph.Applications module not found. Installing..."
        Install-Module -Name Microsoft.Graph.Applications -Scope CurrentUser -Repository PSGallery -Force
    }

    # Import the module
    Import-Module Microsoft.Graph.Applications

    # Connect to Microsoft Graph with required delegated scopes
    # Replace scopes with the ones needed for your scenario
    $scopes = @(
        "Application.ReadWrite.All"
    )

    try {
        Write-Host "Connecting to Microsoft Graph..."
        Connect-MgGraph -Scopes $scopes -TenantId $UpdateAppInTenantId

        Write-Host "Updating FIC for App Object ID: $UpdateAppObjectId"

        $ficName = "PPMI_FIC_V1"
        $ficExists = Get-MgApplicationFederatedIdentityCredential -ApplicationId $UpdateAppObjectId | Where-Object { $_.Name -eq $ficName }
        Write-Host "FIC Exists: $ficExists"

        if (-not $ficExists) {
            $currentDate = Get-Date -Format "yyyy-MM-dd"
            $description = "PPMI FIC V1 Added using migration script on $currentDate"
            $audience = "api://AzureADTokenExchange"

            New-MgApplicationFederatedIdentityCredential -ApplicationId $UpdateAppObjectId -Name $ficName -Issuer $ficIss -Subject $ficSub -Audiences $audience -Description $description
            Write-Host "Federated Identity Credential '$ficName' added."
        }
        else {
            Write-Host "FIC '$ficName' already exists."
        }
        
    }
    catch {
        Write-Host "Failed to add FIC: $_" -ForegroundColor Red
        exit 1
    }
}
