IF "%~1"=="" GOTO BuildAll
IF "%~1"=="VersionBump" GOTO VersionBump

:VersionBump
call MsBuildSolution.cmd VersionBump
set GIT_STATUS=%ERRORLEVEL% 
if not %GIT_STATUS%==0 goto eof 
goto End

:BuildAll
call MsBuildSolution.cmd
set GIT_STATUS=%ERRORLEVEL% 
if not %GIT_STATUS%==0 goto eof 

:End

"send update.cmd"
set GIT_STATUS=%ERRORLEVEL% 
if not %GIT_STATUS%==0 goto eof

:PullRequest
call PullRequest.cmd

:eof
exit /b 0

:fail 
pause 
exit /b 1