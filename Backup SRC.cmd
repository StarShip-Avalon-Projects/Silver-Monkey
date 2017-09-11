
git pull
git submodule update -f --merge
IF "%~1"=="" GOTO BuildAll
IF "%~1"=="VersionBump" GOTO VersionBump

:VersionBump
msbuild /t:IncrementVersions;BuildAll  Solution.build
goto :End

:BuildAll
msbuild /t:BuildAll  Solution.build

:End
git add --all
git commit -m"Auto Version Update" --all
git push


git submodule foreach 'git commit -a' ;
     git submodule foreach 'git push --all' ;
     git commit -a && \
     git push --all --recurse-submodules=on-demand

git request-pull master https://github.com/StarShip-Avalon-Projects/Silver-Monkey.git

