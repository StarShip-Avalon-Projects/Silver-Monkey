cd

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

:End
git.exe add --all
git.exe submodule foreach "git.exe add --all"
set git.exe_STATUS=%ERRORLEVEL% 
if not %git.exe_STATUS%==0 goto eof 

git.exe submodule foreach "git.exe commit -m'Auto Update SubModules'"


git.exe commit -m"Auto Version Update"
set git.exe_STATUS=%ERRORLEVEL% 
if not %git.exe_STATUS%==0 goto eof 

git.exe push --recurse-submodules=check
set git.exe_STATUS=%ERRORLEVEL% 
if not %git.exe_STATUS%==0 goto eof

:pull
git.exe request-pull v2.19.x_Elta https://git.exehub.com/StarShip-Avalon-Projects/Silver-Monkey.git.exe v2.19.x_Elta 

:eof
exit /b 0