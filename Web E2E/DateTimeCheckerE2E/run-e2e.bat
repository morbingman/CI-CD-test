@echo off
echo.
echo ============================================================
echo   DateTimeChecker E2E — Playwright
echo   Make sure start-api.bat is running first!
echo ============================================================
echo.

echo [1/3] Installing dependencies...
call npm install
if errorlevel 1 ( echo NPM INSTALL FAILED && pause && exit /b 1 )

echo.
echo [2/3] Installing Playwright browsers...
call npx playwright install chromium
if errorlevel 1 ( echo PLAYWRIGHT INSTALL FAILED && pause && exit /b 1 )

echo.
echo [3/3] Running tests...
echo.
call npx playwright test --reporter=list

echo.
echo ============================================================
echo   Done. Run 'npx playwright show-report' to open HTML report.
echo ============================================================
echo.
pause
