call UpdateSrc.cmd
set BUILD_STATUS=%ERRORLEVEL% 
if not %BUILD_STATUS%==0 goto fail 

IF "%~1"=="" GOTO BuildAll
IF "%~1"=="VersionBump" GOTO VersionBump

:VersionBump
call MsBuildSolution.cmd VersionBump
set GIT_STATUS=%ERRORLEVEL% 
if not %GIT_STATUS%==0 goto fail 
goto End

:BuildAll
call MsBuildSolution.cmd
set GIT_STATUS=%ERRORLEVEL% 
if not %GIT_STATUS%==0 goto fail 

:End

git add --all

git submodule foreach "git add --all"
set GIT_STATUS=%ERRORLEVEL% 
if not %GIT_STATUS%==0 goto fail 

git submodule foreach "git commit -m'Auto Update SubModules' || true"


git commit -m"Auto Version Update"
set GIT_STATUS=%ERRORLEVEL% 
if not %GIT_STATUS%==0 goto fail 

git push --recurse-submodules=on-demand
set GIT_STATUS=%ERRORLEVEL% 
if not %GIT_STATUS%==0 goto fail

:PullRest
call PullRequest.cmd

:eof
exit /b 0

:fail 

pause 
exit /b 1