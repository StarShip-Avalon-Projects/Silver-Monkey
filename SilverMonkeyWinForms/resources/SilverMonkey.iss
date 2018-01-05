[Dirs]

[Files]
Source: "C:\Users\Gerolkae\Documents\Visual Studio 2010\Projects\SilverMonkey2.0\bin\Debug\SilverMonkey.exe"; DestDir: {app}; 
Source: "C:\Users\Gerolkae\Documents\Visual Studio 2010\Projects\SilverMonkey2.0\bin\Debug\Monkeyspeak.dll"; DestDir: {app};
Source: "C:\Users\Gerolkae\Documents\Visual Studio 2010\Projects\SilverMonkey2.0\bin\Debug\FurcadiaLib.dll"; DestDir: {app};
Source: "C:\Users\Gerolkae\Documents\Visual Studio 2010\Projects\SilverMonkey2.0\bin\Debug\FurcadiaLib.xml"; DestDir: {app};
Source: "C:\Users\Gerolkae\Documents\Visual Studio 2010\Projects\SilverMonkey2.0\bin\Debug\Monkeyspeak.pdb"; DestDir: {app};
Source: "C:\Users\Gerolkae\Documents\Visual Studio 2010\Projects\SilverMonkey2.0\bin\Debug\Monkeyspeak.xml"; DestDir: {app};
Source: "C:\Users\Gerolkae\Documents\Visual Studio 2010\Projects\SilverMonkey2.0\bin\Debug\Nini.dll"; DestDir: {app};
Source: "C:\Users\Gerolkae\Documents\Visual Studio 2010\Projects\SilverMonkey2.0\bin\Debug\SilverMonkey.exe.config"; DestDir: {app};
Source: "C:\Users\Gerolkae\Documents\Visual Studio 2010\Projects\SilverMonkey2.0\bin\Debug\SilverMonkey.pdb"; DestDir: {app};
Source: "C:\Users\Gerolkae\Documents\Visual Studio 2010\Projects\SilverMonkey2.0\bin\Debug\SilverMonkey.vshost.exe"; DestDir: {app};
Source: "C:\Users\Gerolkae\Documents\Visual Studio 2010\Projects\SilverMonkey2.0\bin\Debug\SilverMonkey.xml"; DestDir: {app};
Source: "C:\Users\Gerolkae\Documents\Silver Monkey\Example.ms"; DestDir: {userdocs}\SilverMonkey; DestName: Example.ms; Flags: onlyifdoesntexist; 

[Setup]
WizardImageFile="C:\Users\Gerolkae\Documents\Visual Studio 2010\Projects\SilverMonkey2.0\resources\Monkey.png"
SetupIconFile=C:\Users\Gerolkae\Documents\Visual Studio 2010\Projects\SilverMonkey2.0\resources\Monkey.ico
AppName=DilverMonkey
PrivilegesRequired=poweruser
DefaultDirName={pf}\SilverMonkey
OutputDir=C:\Users\Gerolkae\Documents\Visual Studio 2010\Projects\SilverMonkey2.0\bin\Debug
Compression=lzma/Max
DefaultGroupName=SilverMonkey
SourceDir=C:\Users\Gerolkae\Documents\Visual Studio 2010\Projects\SilverMonkey2.0\bin\Debug
AppID={{5F21D645-395D-4F8B-82CB-5DCF40C9F50C}
VersionInfoVersion=2.0
OutputManifestFile=setup.exe

[Run]
MinVersion: 0,5.1.2600; Filename: {app}\SilverMonkey.exe; WorkingDir: {app}; Flags: RunAsCurrentUser;
