# Ensure there is a forte folder
if (!(Test-Path c:\forte\ -PathType Container))
{
	Write-Host "Creating forte folder path..."
	New-Item -Path c:\ -Name forte -ItemType Directory
}

# Ensure there is a data folder
if (!(Test-Path c:\forte\data -PathType Container))
{
	Write-Host "Creating data path..."
	New-Item -Path c:\forte\ -Name data -ItemType Directory
}

# Ensure there is a logs folder
if (!(Test-Path c:\forte\logs -PathType Container))
{
	Write-Host "Creating log path..."
	New-Item -Path c:\forte\ -Name logs -ItemType Directory
}

if (Test-Path c:\forte\device-cli -PathType Container){
	Write-Host "Removing previous version..."
	Remove-Item c:\forte\device-cli -Recurse:$true -Force:$true -Confirm:$false
}

# Copy files into the forte folder
Write-Host "Copying forte device-cli files..."
Move-Item "$($env:chocolateyPackageFolder)\tools\cli" "c:\forte\device-cli" -Confirm:$false
Install-ChocolateyPath c:\forte\device-cli

Write-Host "Scheduling automatic upgrade and run tasks..."

# Create scheduled upgrade task
$scheduledtaskscript = "$($env:chocolateyPackageFolder)\tools\scheduleupgrade.ps1"
Invoke-Expression $scheduledtaskscript
Write-Host "Scheduled upgrade task created!"

# Create scheduled upgrade task
$scheduledtaskscript = "$($env:chocolateyPackageFolder)\tools\schedulerestartdevicecli.ps1"
Invoke-Expression $scheduledtaskscript
Write-Host "Scheduled device-cli restart task created!"

# Create scheduled run task
$scheduledtaskscript = "$($env:chocolateyPackageFolder)\tools\schedulerun.ps1"
Invoke-Expression $scheduledtaskscript
Write-Host "Scheduled run task created!"

# Create scheduled task to lock workstation on automatic logon
$scheduledtaskscript = "$($env:chocolateyPackageFolder)\tools\schedulelockworkstation.ps1"
Invoke-Expression $scheduledtaskscript
Write-Host "Scheduled lock workstation task created!"

Update-SessionEnvironment

try 
{
	$app = "c:\forte\device-cli\device-cli.exe"
	$args = "run -b"
	Start-Process $app $args
}
catch
{
	$ErrorMessage = $_.Exception.Message
	Write-Host "Installed successfully, but couldn't run due to: $ErrorMessage"
}
