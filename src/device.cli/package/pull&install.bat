rem run lines below only once on machine, password is stored elsewhere
nuget sources remove -name forte-approved
nuget sources add -name forte-approved -Source https://pkgs.dev.azure.com/forte-fit/_packaging/forte-approved/nuget/v3/index.json -username NugetUser -password <???>

rem this script needs to be run on device to update device-cli
cd /D c:\forte\repo
nuget install device-cli
choco upgrade device-cli --force -y