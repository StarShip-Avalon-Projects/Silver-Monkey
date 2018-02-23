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

"send update.cmd"
set GIT_STATUS=%ERRORLEVEL% 
if not %GIT_STATUS%==0 goto eof

:PullRest
call PullRequest.cmd

:eof
exit /b 0

:fail 

pause 
exit /b 1