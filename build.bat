@echo off
setlocal enabledelayedexpansion

set PROJ=src\QuickMathCLI.csproj
set BUILD=build

if not exist %BUILD% mkdir %BUILD%

echo ============================================
echo Building QuickMath CLI for all platforms...
echo ============================================

echo.
echo [1/3] Building for Windows (win-x64)...
dotnet publish %PROJ% -r win-x64 --self-contained true -c Release ^
  -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true ^
  -o %BUILD%\win-x64

if errorlevel 1 (
    echo [ERROR] Windows build failed.
    exit /b 1
)

echo.
echo [2/3] Building for Linux (linux-x64)...
dotnet publish %PROJ% -r linux-x64 --self-contained true -c Release ^
  -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true ^
  -o %BUILD%\linux-x64

if errorlevel 1 (
    echo [ERROR] Linux build failed.
    exit /b 1
)

echo.
echo [3/3] Building for macOS (osx-x64)...
dotnet publish %PROJ% -r osx-x64 --self-contained true -c Release ^
  -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true ^
  -o %BUILD%\osx-x64

if errorlevel 1 (
    echo [ERROR] macOS build failed.
    exit /b 1
)

echo.
echo ============================================
echo All builds complete!
echo   build\win-x64\   - Windows
echo   build\linux-x64\ - Linux
echo   build\osx-x64\   - macOS
echo ============================================
