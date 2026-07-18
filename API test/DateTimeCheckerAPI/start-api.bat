@echo off
echo.
echo ============================================================
echo   DateTimeChecker API
echo   Swagger UI: http://localhost:5000/swagger
echo   Endpoint:   POST http://localhost:5000/validate-date
echo ============================================================
echo.
dotnet run --urls "http://localhost:5000"
pause
