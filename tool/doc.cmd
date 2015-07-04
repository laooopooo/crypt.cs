@echo off
cd "%~dp0\.."

"C:\Program Files\Doxygen\bin\doxygen.exe" doc\api.doxyfile
copy /y web\favicon.ico doc\api
