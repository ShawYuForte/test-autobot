choco pack
nuget sources add -name fortefeed -Source https://pkgs.dev.azure.com/forte-fit/_packaging/forte-approved/nuget/v3/index.json

FOR %%f in (*.nupkg) DO (
nuget push -Source fortefeed -ApiKey az %%f
del %%f /s /f /q
)

pause;