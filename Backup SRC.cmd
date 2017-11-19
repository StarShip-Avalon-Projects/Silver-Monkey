call UpdateSrc.cmd


set BUILD_STATUS=%ERRORLEVEL% 

if not %BUILD_STATUS%==0 goto fail 


IF "%~1"=="" GOTO BuildAll

IF "%~1"=="VersionBump" GOTO VersionBump



:VersionBump


msbuild /t:IncrementVersions  Solution.build

set BUILD_STATUS=%ERRORLEVEL%
 
if not %BUILD_STATUS%==0 goto fail 
 

:BuildAll

msbuild /t:BuildAll  Solution.build

set BUILD_STATUS=%ERRORLEVEL% 

if not %BUILD_STATUS%==0 goto fail 



:Build
Release
msbuild /t:Build /property:Configuration=Release Solution.build
 
set BUILD_STATUS=%ERRORLEVEL% 

if %BUILD_STATUS%==0 goto end 

if not %BUILD_STATUS%==0 goto fail  
 



:End


git.exe add --all

git.exe submodule foreach "git.exe add --all"

set GIT_STATUS=%ERRORLEVEL% 

if not %GIT_STATUS%==0 goto eof 



git.exe submodule foreach "git.exe commit -m'Auto Update SubModules'"



git.exe commit -m"Auto Version Update"

set GIT_STATUS=%ERRORLEVEL% 

if not %GIT_STATUS%==0 goto eof 



git.exe push --recurse-submodules=on-demand

set GIT_STATUS=%ERRORLEVEL% 
if not %GIT_STATUS%==0 goto eof



:pull

git request-pull v2.19.x_Elta https://github.com/StarShip-Avalon-Projects/Silver-Monkey.git v2.19.x_Elta 



:eof

exit /b 0

:fail 

pause 
exit /b 1

