
git.exe add --all

git submodule init
git submodule update

git.exe submodule foreach "git.exe add --all"
set git.exe_STATUS=%ERRORLEVEL% 
if not %git.exe_STATUS%==0 goto eof 

git.exe submodule foreach "git.exe commit -m'Auto Update SubModules' || true"
set GIT_STATUS=%ERRORLEVEL% 
if not %GIT_STATUS%==0 goto eof 
git.exe commit -m"Auto Version Update"
set GIT_STATUS=%ERRORLEVEL% 
if not %GIT_STATUS%==0 goto eof 

git.exe push --recurse-submodules=on-demand
set git.exe_STATUS=%ERRORLEVEL% 
if not %GIT_STATUS%==0 goto eof

:PullRequest
call PullRequest.cmd

:eof
pause
exit /b 0