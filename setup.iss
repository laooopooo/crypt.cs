#define AppBase "crypt"
#define AppExeName AppBase+".windows.exe"
#define AppName "Crypt.cs"
#define AppPublisher "Cédric Belin"
#define AppURL "https://bitbucket.org/cedx/crypt.cs"
#define AppVersion "0.4.3"

[Setup]
AppCopyright = Copyright (c) 2009-2015 {#AppPublisher}
AppId = {{CA46E55B-8587-4F14-81A5-699D4DED86EF}
AppName = {#AppName}
AppPublisher = {#AppPublisher}
AppPublisherURL = http://belin.io
AppSupportURL = {#AppURL}/issues
AppVerName = {#AppName} {#AppVersion}
AppVersion = {#AppVersion}

AllowNoIcons = yes
ArchitecturesInstallIn64BitMode = x64
Compression = lzma
DefaultDialogFontName = Segoe UI
DefaultDirName = {pf}\Belin\{#AppName}
DefaultGroupName = {#AppName}
LicenseFile = LICENSE.txt
OutputBaseFilename = {#LowerCase(AppName)}-{#AppVersion}
OutputDir = var
SetupIconFile = src\{#AppBase}.windows\resources\app.ico
SolidCompression = yes
UninstallDisplayIcon = {app}\{#AppExeName}
VersionInfoVersion = {#AppVersion}
WizardImageFile = compiler:WizModernImage-IS.bmp
WizardSmallImageFile = setup.bmp

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "french"; MessagesFile: "compiler:Languages\French.isl"

[CustomMessages]
CommandLineInterface = Command Line Interface
GraphicalUserInterface = Graphical User Interface
french.CommandLineInterface = Invite de commandes
french.GraphicalUserInterface = Interface graphique

[Components]
Name: "windows"; Description: "{cm:GraphicalUserInterface}"; Types: full compact; Flags: fixed
Name: "console"; Description: "{cm:CommandLineInterface}"; Types: full

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked; OnlyBelowVersion: 6.1

[Files]
{#DeleteFileNow("var\release\"+AppBase+".console.vshost.exe")}
{#DeleteFileNow("var\release\"+AppBase+".console.vshost.exe.config")}
{#DeleteFileNow("var\release\"+AppBase+".console.vshost.exe.manifest")}

{#DeleteFileNow("var\release\"+AppBase+".windows.vshost.exe")}
{#DeleteFileNow("var\release\"+AppBase+".windows.vshost.exe.config")}
{#DeleteFileNow("var\release\"+AppBase+".windows.vshost.exe.manifest")}

Source: "LICENSE.txt"; DestDir: "{app}"

Source: "var\release\*.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "var\release\addins\*"; DestDir: "{app}\addins"; Flags: ignoreversion
Source: "var\release\fr\miniframework.*"; DestDir: "{app}\fr"; Flags: ignoreversion
Source: "var\release\fr\{#AppBase}.encoders.*"; DestDir: "{app}\fr"; Flags: ignoreversion

Source: "var\release\{#AppBase}.console.*"; DestDir: "{app}"; Flags: ignoreversion; Components: console
Source: "var\release\fr\{#AppBase}.console.*"; DestDir: "{app}\fr"; Flags: ignoreversion; Components: console

Source: "var\release\{#AppBase}.windows.*"; DestDir: "{app}"; Flags: ignoreversion; Components: windows
Source: "var\release\fr\{#AppBase}.windows.*"; DestDir: "{app}\fr"; Flags: ignoreversion; Components: windows

[Icons]
Name: "{group}\{#AppName}"; Filename: "{app}\{#AppExeName}"; WorkingDir: "%HOMEDRIVE%%HOMEPATH%"; Components: windows
Name: "{group}\{cm:ProgramOnTheWeb,{#AppName}}"; Filename: "{#AppURL}"
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\{#AppName}"; Filename: "{app}\{#AppExeName}"; WorkingDir: "%HOMEDRIVE%%HOMEPATH%"; Tasks: quicklaunchicon; Components: windows
Name: "{userdesktop}\{#AppName}"; Filename: "{app}\{#AppExeName}"; WorkingDir: "%HOMEDRIVE%%HOMEPATH%"; Tasks: desktopicon; Components: windows

[Run]
Filename: "{app}\{#AppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(AppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent; Components: windows
