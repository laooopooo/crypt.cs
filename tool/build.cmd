@echo off
cd "%~dp0\.."
"C:\Program Files (x86)\MSBuild\12.0\Bin\MSBuild.exe" /nologo /property:Configuration=Release /verbosity:minimal
