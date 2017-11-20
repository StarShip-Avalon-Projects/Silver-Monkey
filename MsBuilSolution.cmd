IF "%~1"=="" GOTO BuildAll
IF "%~1"=="VersionBump" GOTO VersionBump

:VersionBump
msbuild /t:IncrementVersions /property:Configuration=Debug Solution.build 
set BUILD_STATUS=%ERRORLEVEL% 
if not %BUILD_STATUS%==0 goto fail 

:BuildAll
msbuild /t:Clean;BackupAllSources;CopyFiles  /property:Configuration=Debug Solution.build /flp:logfile=MonkeySystemDebug.log;verbosity=diagnostic
set BUILD_STATUS=%ERRORLEVEL% 
if not %BUILD_STATUS%==0 goto fail 

:BuildRelease
msbuild /t:CopyFiles /property:Configuration=Release Solution.build  /flp:logfile=MonkeySystemRelease.log;verbosity=diagnostic
 set BUILD_STATUS=%ERRORLEVEL% 
if %BUILD_STATUS%==0 goto end 
if not %BUILD_STATUS%==0 goto fail  
 
:fail 
pause 
exit /b 1


:eof
exit /b 0