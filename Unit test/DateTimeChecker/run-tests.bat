@echo off
setlocal

echo.
echo ============================================================
echo   DateTimeChecker - Unit Test Runner
echo ============================================================
echo.

echo [1/3] Restoring packages and tools...
dotnet tool restore
if errorlevel 1 ( echo TOOL RESTORE FAILED && pause && exit /b 1 )
dotnet restore
if errorlevel 1 ( echo PACKAGE RESTORE FAILED && pause && exit /b 1 )

echo.
echo [2/3] Building...
dotnet build DateTimeChecker.Tests\DateTimeChecker.Tests.csproj -c Debug --no-restore -v quiet
if errorlevel 1 ( echo BUILD FAILED && pause && exit /b 1 )

echo.
echo [3/3] Running tests...
echo.

set DLL=DateTimeChecker.Tests\bin\Debug\net10.0\DateTimeChecker.Tests.dll

dotnet tool run nunit %DLL% --noresult --labels=After | powershell -NoProfile -ExecutionPolicy Bypass -File color-output.ps1

echo.
echo ============================================================
echo   Done.
echo ============================================================
echo.
pause
