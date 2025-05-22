@echo off
:: Check if running as admin
NET SESSION >nul 2>&1
IF %ERRORLEVEL% NEQ 0 (
    echo Requesting admin privileges...
    powershell -Command "Start-Process '%~f0' -Verb runAs"
    exit /b
)

:: Now we're running as admin, restart the service
powershell -Command "Restart-Service -Name 'LinkedinBot' -Force"
