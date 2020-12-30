Try
{
	Write-Host "Issuing shutdown command to local server"
	# Assuming process is running, issue shutdown command through the API
	# NOTE: port is configurable, though here it is hard-coded, TODO to fix
	Invoke-WebRequest -Uri http://localhost:9000/api/device/shutdown -Method POST
	Write-Host "Shutdown command issued successfully"
}
Catch
{
	#Write-Host "Could not issue command, the server might already be shut down"
}