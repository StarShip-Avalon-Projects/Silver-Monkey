
:GitPullActive
git.exe pull --recurse-submodules=yes origin v2.19.x.Elta
set GIT_STATUS=%ERRORLEVEL% 
if not %GIT_STATUS%==0 goto fail 

:NuGetRestore
bin\nuget.exe restore
set GIT_STATUS=%ERRORLEVEL% 
if not %GIT_STATUS%==0 goto fail 

:eof
exit /b 0

:fail 
pause 
exit /b 1