@echo off
cd "%~dp0\.."

"C:\Program Files\Doxygen\bin\doxygen.exe" docs\api.doxyfile
copy /y web\favicon.ico docs\api
