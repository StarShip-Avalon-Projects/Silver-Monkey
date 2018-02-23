:GitPullCurrent

rem git checkout V3.0
git.exe pull --recurse-submodules=on-demand 
rem --rebase --autostash
set GIT_STATUS=%ERRORLEVEL% 
if not %GIT_STATUS%==0 goto fail 


:NuGetRestore

bin\nuget.exe update -self
set GIT_STATUS=%ERRORLEVEL% 
if not %GIT_STATUS%==0 goto fail 

bin\nuget.exe restore
set GIT_STATUS=%ERRORLEVEL% 
if not %GIT_STATUS%==0 goto fail 

bin\nuget.exe  update "Silver Monkey.sln"
set GIT_STATUS=%ERRORLEVEL% 
if not %GIT_STATUS%==0 goto fail 

cd monkeyspeak

..\bin\nuget.exe restore
set GIT_STATUS=%ERRORLEVEL% 
if not %GIT_STATUS%==0 goto fail 

..\bin\nuget.exe  update Monkeyspeak.sln
set GIT_STATUS=%ERRORLEVEL% 
if not %GIT_STATUS%==0 goto fail 
cd ..

:eof

exit /b 0
:fail
pause
exit /b 0

