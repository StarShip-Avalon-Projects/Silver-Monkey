
git.exe add --all

rem git.exe submodule foreach "git.exe add --all"
rem set git.exe_STATUS=%ERRORLEVEL% 
rem if not %git.exe_STATUS%==0 goto eof 

git.exe submodule foreach "git.exe commit -am'Auto Update SubModules' || true"
git.exe commit -am"Auto Version Update"
set GIT_STATUS=%ERRORLEVEL% 
if not %GIT_STATUS%==0 goto eof 

git push  --set-upstream origin V3.0 --recurse-submodules=on-demand
set git.exe_STATUS=%ERRORLEVEL% 
if not %GIT_STATUS%==0 goto eof

rem git submodule foreach "git push || true"


:PullRequest
call PullRequest.cmd



:eof
pause
exit /b 0