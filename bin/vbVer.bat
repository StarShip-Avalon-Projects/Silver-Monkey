@echo off
set WORKING_DIRECTORY=%cd%
	echo Found VB AessemblyInfo
	set ASMINFO=..\..\SilverMonkey\AssemblyFileVersion.vb
	FINDSTR /C:"<Assembly: AssemblyVersion(" %ASMINFO% | ..\..\bin\sed.exe "s/<Assembly: AssemblyVersion(\"/set CURRENT_VERSION=/g;s/\")>//g;s/\.\*//g">SetCurrVer.bat


CALL SetCurrVer.bat
echo CURRENT_VERSION %CURRENT_VERSION%
makensis.exe -dVERSION=%CURRENT_VERSION% -dCONFIGURATION=%2 %1

exit /b 0
