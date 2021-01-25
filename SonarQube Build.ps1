dotnet sonarscanner begin /o:archanox /k:archanox_RGBDS2CIL /d:sonar.host.url=https://sonarcloud.io /d:sonar.login=22114d9f2c56bb0dd5de62c06b147690de9899ec
dotnet build .\RGBDS2CIL.sln
dotnet sonarscanner end /d:sonar.login=22114d9f2c56bb0dd5de62c06b147690de9899ec