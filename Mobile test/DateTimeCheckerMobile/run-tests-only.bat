@echo off
title Mobile Testing - Run Tests

:: Quick runner — skips all setup, just runs the flows
:: Use this after first-time setup with run-mobile.bat is complete

echo.
echo ========================================
echo   Mobile Tests - Quick Run
echo ========================================
echo.

:: Add Maestro to PATH for this session if needed
set "PATH=%PATH%;%USERPROFILE%\.maestro\bin"

maestro test flows\ --format junit --output test-results.xml

echo.
echo Done. Results: test-results.xml
echo.
pause
