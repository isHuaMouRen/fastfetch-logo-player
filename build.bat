@echo off
setlocal EnableDelayedExpansion

set PROJECT=FastfetchLogoPlayer\FastfetchLogoPlayer.csproj
set BUILD=build

echo.
echo ==========================================
echo       FastfetchLogoPlayer Publish
echo ==========================================
echo.

if exist "%BUILD%" (
    echo [INFO] Cleaning build directory...
    rmdir /s /q "%BUILD%"
)

mkdir "%BUILD%"

for %%T in (
    "win-x64|win.x64|exe"
    "win-x86|win.x86|exe"
    "win-arm64|win.arm64|exe"
    "linux-x64|linux.x64|"
    "linux-arm64|linux.arm64|"
    "osx-x64|osx.x64|"
    "osx-arm64|osx.arm64|"
) do (

    for /f "tokens=1,2,3 delims=|" %%A in (%%T) do (

        set RID=%%A
        set NAME=%%B
        set EXT=%%C

        echo.
        echo ===== !RID! =====

        rem ==========================================
        rem Framework-dependent
        rem ==========================================

        dotnet publish "%PROJECT%" ^
            -c Release ^
            -r !RID! ^
            --self-contained false ^
            -p:PublishSingleFile=true ^
            -o "%BUILD%"

        if defined EXT (
            ren "%BUILD%\fflp.!EXT!" "fflp.!NAME!.NoFramework.!EXT!"
        ) else (
            ren "%BUILD%\fflp" "fflp.!NAME!.NoFramework"
        )

        rem ==========================================
        rem Self-contained
        rem ==========================================

        dotnet publish "%PROJECT%" ^
            -c Release ^
            -r !RID! ^
            --self-contained true ^
            -p:PublishSingleFile=true ^
            -p:PublishTrimmed=true ^
            -p:TrimMode=partial ^
            -p:EnableCompressionInSingleFile=true ^
            -o "%BUILD%"

        if defined EXT (
            ren "%BUILD%\fflp.!EXT!" "fflp.!NAME!.!EXT!"
        ) else (
            ren "%BUILD%\fflp" "fflp.!NAME!"
        )
    )
)

echo.
echo ==========================================
echo              Publish Complete
echo ==========================================
echo.

echo Output:
dir /b "%BUILD%"

echo.
endlocal
pause