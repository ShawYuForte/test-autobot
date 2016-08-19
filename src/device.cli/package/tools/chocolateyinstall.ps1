﻿# Ensure there is a forte folder
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
	Remove-Item c:\forte\device-cli -Recurse:$true -Force:$true -Confirm:$false
}

Write-Host "Copying forte device-cli files..."
Move-Item "$($env:chocolateyPackageFolder)\tools\cli" "c:\forte\device-cli" -Confirm:$false
Install-ChocolateyPath c:\forte\device-cli

#$action = New-ScheduledTaskAction -Execute 'Powershell.exe' -Argument '-NoProfile -WindowStyle Hidden -command "& {get-eventlog -logname Application -After ((get-date).AddDays(-1)) | Export-Csv -Path c:\fso\applog.csv -Force -NoTypeInformation}"'
#$trigger = New-ScheduledTaskTrigger -Daily -At 2am

#Register-ScheduledTask -Action $action -Trigger $trigger -TaskName "Forte.Device-CLI.Upgrade" -Description "Forte Device CLI Version Upgrade (if available)."

Update-SessionEnvironment