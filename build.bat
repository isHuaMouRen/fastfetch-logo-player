@echo off

dotnet publish "FastfetchLogoPlayer\FastfetchLogoPlayer.csproj" -c Release -r win-x64 --self-contained true -o build

pause