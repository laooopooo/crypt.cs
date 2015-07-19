@echo off
REM Creates a distribution file for this program.

cd "%~dp0\.."
"C:\Program Files (x86)\Inno Setup\ISCC.exe" /qp setup.iss
