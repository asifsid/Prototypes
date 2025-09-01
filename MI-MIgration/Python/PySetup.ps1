$scriptName = "MigrateMI"
$argsEx = "-o <org-name> -m <managed-identity-id> <...other-args>"

# Check if Python is installed
$python = Get-Command python -ErrorAction SilentlyContinue
if (-not $python) {
    Write-Host "Python not found. Installing via winget..."
    winget install --id Python.Python.3 --source winget
    # Refresh environment variables
    $env:Path = [System.Environment]::GetEnvironmentVariable("Path","Machine")
    $python = Get-Command python -ErrorAction SilentlyContinue
    if (-not $python) {
        Write-Error "Python installation failed or not added to PATH."
        exit 1
    }
}

# Define virtual environment folder
$venvPath = ".\venv"

# Create virtual environment if it doesn't exist
if (-not (Test-Path $venvPath)) {
    Write-Host "Creating virtual environment..."
    python -m venv $venvPath
}

# Create a temporary script to activate venv and install requirements
$tempScript = @"
`$env:VIRTUAL_ENV='$venvPath'
`$env:PATH='$venvPath\Scripts;' + `$env:PATH
cd `"$PWD`"
if (Test-Path 'requirements.txt') {
    pip install -r requirements.txt
} else {
    Write-Host 'requirements.txt not found.'
}
cls
Write-Host 'Virtual environment is ready. You can now run your script.'
Write-Host '----------------------------------------------------------------'
Write-Host 'Run the script using the following command ...'
Write-Host "python .\$scriptName.py $argsEx"
Write-Host '----------------------------------------------------------------'
"@

# Save the script to a temp file
$tempFile = "$env:TEMP\activate_venv.ps1"
$tempScript | Out-File -Encoding UTF8 -FilePath $tempFile

# Open a new PowerShell window with the script
Start-Process powershell -ArgumentList "-NoExit", "-File", $tempFile