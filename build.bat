@echo off
echo ========================================
echo Building Hawalaty System
echo ========================================

REM Check if .NET 8 is installed
dotnet --version >nul 2>&1
if %errorlevel% neq 0 (
    echo Error: .NET 8 SDK is not installed.
    echo Please install .NET 8 SDK from https://dotnet.microsoft.com/download/dotnet/8.0
    pause
    exit /b 1
)

REM Restore dependencies
echo Restoring NuGet packages...
dotnet restore HawalatySystem.sln
if %errorlevel% neq 0 (
    echo Error: Failed to restore packages.
    pause
    exit /b 1
)

REM Build the application
echo Building application...
dotnet build HawalatySystem.sln --configuration Release --no-restore
if %errorlevel% neq 0 (
    echo Error: Build failed.
    pause
    exit /b 1
)

REM Publish the application
echo Publishing application...
dotnet publish HawalatySystem\HawalatySystem.csproj --configuration Release --output ".\publish" --self-contained false --framework net8.0-windows
if %errorlevel% neq 0 (
    echo Error: Publish failed.
    pause
    exit /b 1
)

echo ========================================
echo Build completed successfully!
echo Published files are in the 'publish' folder.
echo ========================================
pause