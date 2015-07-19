@echo off
REM Deletes all generated files and reset any saved state.

cd "%~dp0\.."

set file="var\*.exe"
if exist %file% del /q %file%

for %%d in ("src\crypt.console\obj" "src\crypt.core\obj" "src\crypt.encoders\obj" "src\crypt.windows\obj" "var\debug" "var\release") do (
  if exist %%d rmdir /q /s %%d
)
