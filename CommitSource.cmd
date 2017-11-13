git.exe add -A

git.exe submodule foreach "git.exe add -A"
set git.exe_STATUS=%ERRORLEVEL% 
if not %git.exe_STATUS%==0 goto eof 

git.exe submodule foreach "git.exe commit -m'Auto Update SubModules'"


git.exe commit -m"Auto Version Update"
set git.exe_STATUS=%ERRORLEVEL% 
if not %git.exe_STATUS%==0 goto eof 

git.exe push --recurse-submodules=check
set git.exe_STATUS=%ERRORLEVEL% 
if not %git.exe_STATUS%==0 goto eof

:pull
git.exe request-pull v2.19.x_Elta https://git.exehub.com/StarShip-Avalon-Projects/Silver-Monkey.git.exe v2.19.x_Elta 

exit /b 0
:eof
exit /b 0
pause