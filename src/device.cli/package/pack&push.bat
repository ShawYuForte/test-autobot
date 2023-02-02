rem packs device-cli solution and pushes it to nuget feed
rem you need to change version in device-cli.nuspec first and build solution in "release" mode

choco pack
nuget sources remove -name forte-approved
nuget sources add -name forte-approved -Source https://pkgs.dev.azure.com/forte-fit/_packaging/forte-approved/nuget/v3/index.json -username NugetUser -password frzyl6gstd6o7kwuxsygfpj6ksio32gpjqj43tu5s3bqipgebpbq

FOR %%f in (*.nupkg) DO (
nuget push -Source forte-approved -ApiKey az %%f
del %%f /s /f /q
)