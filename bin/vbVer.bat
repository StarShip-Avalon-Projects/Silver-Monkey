@echo off
rem call "C:\Program Files\Microsoft Visual Studio 10.0\VC\vcvarsall.bat" x86
set WORKING_DIRECTORY=%cd%
set PATH=C:\Program Files\NSIS;%PATH%;..\..\bin
set mpath=%~2
IF "%mpath%" == "" (
set mpath=..\..\
) 
echo mpath = %mpath%
	
if exist {%mpath%Properties\AssemblyInfo.cs} (
	echo Found C Sharp AessemblyInfo
	set ASMINFO=%mpath%Properties\AssemblyInfo.cs
	FINDSTR /C:"[Assembly: AssemblyVersion(" "%ASMINFO%" | sed.exe "s/[Assembly: AssemblyVersion(\"/set CURRENT_VERSION=/g;s/\")]//g;s/\.\*//g">SetCurrVer.bat
)

if exist {%mpath%AssemblyFileVersion.vb} (
	echo Found VB AessemblyInfo
	set ASMINFO=%mpath%AssemblyFileVersion.vb
	FINDSTR /C:"<Assembly: AssemblyVersion(" "%ASMINFO%" | sed.exe "s/<Assembly: AssemblyVersion(\"/set CURRENT_VERSION=/g;s/\")>//g;s/\.\*//g">SetCurrVer.bat
 )

CALL SetCurrVer.bat
echo CURRENT_VERSION %CURRENT_VERSION%
del SetCurrVer.bat
makensis.exe -dVERSION=%CURRENT_VERSION% %1
