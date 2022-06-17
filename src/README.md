* About The Project
  Autobot helps automate the act of Streaming a particular class.
 * Built With
  C# .NET Framework 4.8
 * Installation
 
    1. From Remote nuget
	
	. Run pull&install.bat file from package folder on corresponding machine
	
	2. For Local nuget package
	  . Run the below command on the developement machine
	  
		choco pack

		FOR %%f in (*.nupkg) DO (
		nuget add %%f -source C:\local-nuget-packages\
		del %%f /s /f /q
		)
		
	  . Copy package to corresponding machine and run below command to install
	  
	  taskkill /F /FI "IMAGENAME eq device-cli.exe"
		rem run lines below only once on machine, password is stored elsewhere
		nuget sources remove -name forte-approved
		nuget sources add -name forte-approved -Source "C:\local-nuget-packages" -username userName -password password

		rem this script needs to be run on device to update device-cli
		cd /D c:\forte\repo
		nuget locals http-cache -clear
		nuget install device-cli
		choco upgrade device-cli --force -y
		rem finish
	  
	3. To Push tested package to remote nuget
	 . Run pack&push.bat file 
	
	
 * Troubleshooting
   https://fortefit.atlassian.net/wiki/spaces/PD/pages/2454847493/Autobot+Prod+Issue+debugging+steps
 * Maintainers