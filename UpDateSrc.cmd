:GitPullCurrent

git.exe pull --recurse-submodules=on-demand
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


