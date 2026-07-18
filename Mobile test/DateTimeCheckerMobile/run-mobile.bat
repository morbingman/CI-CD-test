@echo off
setlocal EnableDelayedExpansion
title Mobile Testing - Topic 4

:: ============================================================
::  run-mobile.bat  —  Topic 4: Mobile Testing
::  Requires: Android Studio Quail 2 installed, Java 17+
::  First run installs Maestro CLI + downloads Sauce Labs APK
:: ============================================================

echo.
echo ========================================
echo   Topic 4 - Mobile Testing with Maestro
echo ========================================
echo.

:: ── STEP 0: Check Java ──────────────────────────────────────
echo [1/5] Checking Java...
java -version >nul 2>&1
if errorlevel 1 (
    echo  ERROR: Java not found. Maestro requires Java 17+.
    echo  Tip: Android Studio ships its own JDK. Add it to PATH:
    echo    e.g. C:\Program Files\Android\Android Studio\jbr\bin
    pause
    exit /b 1
)
echo  Java found.

:: ── STEP 1: Install Maestro CLI if missing ──────────────────
echo.
echo [2/5] Checking Maestro CLI...
where maestro >nul 2>&1
if errorlevel 1 (
    echo  Maestro not found. Installing now...
    echo  ^(This uses curl — make sure you have internet access^)
    echo.
    curl -Ls "https://get.maestro.mobile.dev" | bash
    if errorlevel 1 (
        echo.
        echo  ERROR: Maestro install failed.
        echo  Try manually: curl -Ls "https://get.maestro.mobile.dev" | bash
        echo  Then add ~/.maestro/bin to your PATH and re-run this script.
        pause
        exit /b 1
    )
    :: Add to PATH for this session
    set "PATH=%PATH%;%USERPROFILE%\.maestro\bin"
    echo.
    echo  Maestro installed successfully.
) else (
    echo  Maestro already installed.
)

:: Show version
maestro --version
ver >nul
:: ── STEP 2: Check ADB + emulator ────────────────────────────
echo.
echo [3/5] Checking emulator...

:: Locate adb from ANDROID_HOME or common default paths
set "ADB_PATH="
if defined ANDROID_HOME (
    set "ADB_PATH=%ANDROID_HOME%\platform-tools\adb.exe"
) else if exist "%LOCALAPPDATA%\Android\Sdk\platform-tools\adb.exe" (
    set "ADB_PATH=%LOCALAPPDATA%\Android\Sdk\platform-tools\adb.exe"
) else if exist "%USERPROFILE%\AppData\Local\Android\Sdk\platform-tools\adb.exe" (
    set "ADB_PATH=%USERPROFILE%\AppData\Local\Android\Sdk\platform-tools\adb.exe"
)

if not defined ADB_PATH (
    echo  WARNING: adb not found. Cannot auto-detect emulator.
    echo  Make sure your emulator is running, then press any key to continue.
    pause
    goto :skip_emulator_check
)

echo  adb found at: %ADB_PATH%
echo  Waiting for emulator to be ready...

:: Wait up to 60s for a device
set /a WAIT=0
:wait_loop
"%ADB_PATH%" devices 2>nul | findstr "emulator" >nul
if not errorlevel 1 goto :emulator_found
set /a WAIT+=5
if %WAIT% GEQ 60 (
    echo.
    echo  ERROR: No emulator detected after 60 seconds.
    echo  Please start your AVD in Android Studio ^> Device Manager,
    echo  then re-run this script.
    pause
    exit /b 1
)
echo  Still waiting... ^(%WAIT%s^)
timeout /t 5 /nobreak >nul
goto :wait_loop

:emulator_found
echo  Emulator detected and ready.

:skip_emulator_check

:: ── STEP 3: Download + install Sauce Labs APK ───────────────
echo.
echo [4/5] Checking Sauce Labs demo app...

set "APK_FILE=my-demo-app-android.apk"
set "APK_URL=https://github.com/saucelabs/my-demo-app-android/releases/download/v1.0.24/mda-1.0.24-15.apk"
set "APP_ID=com.saucelabs.mydemoapp.android"

if not exist "%APK_FILE%" (
    echo  APK not found. Downloading Sauce Labs demo app...
    curl -L -o "%APK_FILE%" "%APK_URL%"
    if errorlevel 1 (
        echo  ERROR: Download failed. Check your internet connection.
        echo  Or download manually from:
        echo    https://github.com/saucelabs/my-demo-app-android/releases
        echo  Save as: %APK_FILE%  in this folder, then re-run.
        pause
        exit /b 1
    )
    echo  Downloaded successfully.
) else (
    echo  APK already present.
)

if defined ADB_PATH (
    echo  Installing APK on emulator...
    "%ADB_PATH%" install -r "%APK_FILE%"
    if errorlevel 1 (
        echo  WARNING: APK install failed. Emulator may not be ready yet.
        echo  Try running:  adb install -r %APK_FILE%
    ) else (
        echo  App installed successfully.
    )
)

:: ── STEP 4: Run Maestro flows ────────────────────────────────
echo.
echo [5/5] Running Maestro test flows...
echo.

maestro test flows\ --format junit --output test-results.xml

echo.
echo ========================================
echo   Test run complete!
echo ========================================
echo.
echo  Results saved to: test-results.xml
echo.
echo  To view in Maestro Studio:
echo    Open Maestro Studio desktop app
echo    Connect to your emulator
echo    Open the flows\ folder
echo.
echo  To re-run tests only (skip setup):
echo    maestro test flows\
echo.
echo  To record a video of a flow:
echo    maestro record flows\01-login.yaml
echo.
pause
