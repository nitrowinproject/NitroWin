@echo off
@title ONLY RUN THIS AS TRUSTEDINSTALLER
echo Only run this script as TrustedInstaller, or else it might not work. You can use ExecTI for that.
echo ExecTI: https://winaero.com/download-execti-run-as-trustedinstaller/
echo.
echo If you are running this script as TrustedInstaller, press any key to continue.
pause >nul

echo Downloading tweaks that require TrustedInstaller privileges....
powershell -Command "Invoke-WebRequest https://raw.githubusercontent.com/Nitro4542/NitroWin/main/src/NitroWin.Tweaks/Tweaks/Privacy/Disable-ErrorReportingExtended.reg -OutFile Disable-ErrorReportingExtended.reg"
powershell -Command "Invoke-WebRequest https://raw.githubusercontent.com/Nitro4542/NitroWin/main/src/NitroWin.Tweaks/Tweaks/Privacy/Disable-TelemetryExtended.reg -OutFile Disable-TelemetryExtended.reg"

echo Importing registry files...
reg import "Disable-ErrorReportingExtended.reg"
reg import "Disable-TelemetryExtended.reg"

echo Done. Press any key to exit.
pause >nul
