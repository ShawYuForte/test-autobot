rem packs device-cli solution and pushes it to nuget feed
rem you need to change version in device-cli.nuspec first and build solution in "release" mode

choco pack
nuget sources add -name forte-approved -Source https://pkgs.dev.azure.com/forte-fit/_packaging/forte-approved/nuget/v3/index.json

FOR %%f in (*.nupkg) DO (
nuget push -Source forte-approved -ApiKey az %%f
del %%f /s /f /q
)