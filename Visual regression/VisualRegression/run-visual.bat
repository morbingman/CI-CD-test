@echo off
echo.
echo ============================================================
echo   DateTimeChecker - Visual Regression
echo ============================================================
echo.

:: Always run from the folder this .bat file lives in
cd /d "%~dp0"

echo [1/2] Installing dependencies...
pip install pillow pygetwindow --quiet
if errorlevel 1 ( echo INSTALL FAILED && pause && exit /b 1 )

echo.
echo [2/2] Choose an action:
echo.
echo   1. Capture BASELINE  (run this first, before any UI changes)
echo   2. Capture CURRENT   (run this after making a UI change)
echo   3. Run DIFF          (compare baseline vs current)
echo.
set /p choice="Enter 1, 2, or 3: "

if "%choice%"=="1" (
    echo.
    echo Make sure DateTimeChecker.exe is running, then press any key...
    pause >nul
    python capture.py baseline
)

if "%choice%"=="2" (
    echo.
    echo Make sure the MODIFIED DateTimeChecker.exe is running, then press any key...
    pause >nul
    python capture.py current
)

if "%choice%"=="3" (
    echo.
    python diff.py
)

echo.
pause
