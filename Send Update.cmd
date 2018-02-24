
git.exe add --all
git.exe submodule foreach "git.exe add --all || true"
rem set git.exe_STATUS=%ERRORLEVEL% 
rem if not %git.exe_STATUS%==0 goto eof 

git.exe submodule foreach "git.exe commit -am'Auto Update SubModules' || true"
git commit -am"Auto Update SubModules"
set GIT_STATUS=%ERRORLEVEL% 
if not %GIT_STATUS%==0 goto eof 

git push --recurse-submodules=on-demand
set git.exe_STATUS=%ERRORLEVEL% 
if not %GIT_STATUS%==0 goto eof

rem git submodule foreach "git push || true"


:eof
pause
exit /b 0