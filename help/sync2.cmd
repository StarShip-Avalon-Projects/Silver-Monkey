@echo off

"C:\Program Files (x86)\WinSCP\WinSCP.com" ^
  /log="C:\writable\path\to\log\WinSCP.log" /ini=nul ^
  /command ^
    "open ftp://tsprojects:NaEhRdjsor2pCF03@www.tsprojects.org/ -passive=0" ^
    "cd /domains/silvermonkey.tsprojects.org/www/help"
    "Your command 2" ^
/script=example.txt /parameter // %1 "/domains/silvermonkey.tsprojects.org/www/help/%~n1%~x1" ^
    "exit"

set WINSCP_RESULT=%ERRORLEVEL%
if %WINSCP_RESULT% equ 0 (
  echo Success
) else (
  echo Error
)

exit /b %WINSCP_RESULT%
