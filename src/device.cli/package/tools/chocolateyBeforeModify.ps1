Try
{
	Invoke-WebRequest -Uri http://localhost:9000/api/device/shutdown -Method POST
}
Catch
{
	# Ignore
}