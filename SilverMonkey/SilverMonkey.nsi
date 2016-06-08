;!include "DotNetChecker.nsh"
!include "FileAssociation.nsh"
##{{NSIS_PLUS_BEGIN_PROJECT_SETTINGS}}##
#NAME "Release"
#	CMD /CD
#	FLAGS 2
##{{NSIS_PLUS_END_PROJECT_SETTINGS}}##

############################################################################################
#      NSIS Installation Script created by NSIS Quick Setup Script Generator v1.09.18
#               Entirely Edited with NullSoft Scriptable Installation System                
#              by Vlasis K. Barkas aka Red Wine red_wine@freemail.gr Sep 2006               
############################################################################################

!define APP_NAME "Silver Monkey"
!define COMP_NAME "TS-Projects"
;!define WEB_SITE "http://silvermonkey.tsprojects.org/"
!ifndef VERSION
!define VERSION "2.20.0.0"
!endif


!define COPYRIGHT "Author Gerolkae © 2014"
!define DESCRIPTION "Application"
!define INSTALLER_NAME "SM_Setup(AnyCPU)-${VERSION}.exe"
!define MAIN_APP_EXE "SilverMonkey.exe"
!define INSTALL_TYPE "SetShellVarContext all"
!define REG_ROOT "HKLM"
!define REG_APP_PATH "Software\Microsoft\Windows\CurrentVersion\App Paths\${MAIN_APP_EXE}"
!define UNINSTALL_PATH "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP_NAME}"
!define Bin_Directory "."
!define REG_START_MENU "Start Menu Folder"

Var SM_Folder 

!define LICENSE_TXT "SM Lisence.txt"

######################################################################

VIProductVersion  "${VERSION}"
VIAddVersionKey "ProductName"  "${APP_NAME}"
VIAddVersionKey "CompanyName"  "${COMP_NAME}"
VIAddVersionKey "LegalCopyright"  "${COPYRIGHT}"
VIAddVersionKey "FileDescription"  "${DESCRIPTION}"
VIAddVersionKey "FileVersion"  "${VERSION}"

######################################################################

SetCompressor LZMA
Name "${APP_NAME}"
Caption "${APP_NAME}"
OutFile "${INSTALLER_NAME}"

BrandingText "${APP_NAME}"
XPStyle on
RequestExecutionLevel admin


InstallDirRegKey "${REG_ROOT}" "${REG_APP_PATH}" ""
InstallDir "$PROGRAMFILES\SilverMonkey"

######################################################################
!include "nsProcess.nsh"
!include "Sections.nsh"
!include "MUI2.nsh"
!define MUI_ICON "metal.ico"

!define MUI_ABORTWARNING
!define MUI_UNABORTWARNING

!insertmacro MUI_PAGE_WELCOME

!ifdef LICENSE_TXT
!insertmacro MUI_PAGE_LICENSE "${LICENSE_TXT}"
!endif
!insertmacro MUI_PAGE_DIRECTORY

!ifdef REG_START_MENU
!define MUI_STARTMENUPAGE_NODISABLE
!define MUI_STARTMENUPAGE_DEFAULTFOLDER "SilverMonkey"
!define MUI_STARTMENUPAGE_REGISTRY_ROOT "${REG_ROOT}"
!define MUI_STARTMENUPAGE_REGISTRY_KEY "${UNINSTALL_PATH}"
!define MUI_STARTMENUPAGE_REGISTRY_VALUENAME "${REG_START_MENU}"
!insertmacro MUI_PAGE_STARTMENU Application "$SM_Folder"
!endif



!insertmacro MUI_PAGE_INSTFILES

!define MUI_FINISHPAGE_RUN "$INSTDIR\${MAIN_APP_EXE}"
!insertmacro MUI_PAGE_FINISH

!insertmacro MUI_UNPAGE_CONFIRM



!insertmacro MUI_UNPAGE_INSTFILES


!insertmacro MUI_UNPAGE_FINISH

!insertmacro MUI_LANGUAGE "English"

Section -KillProcess

	${nsProcess::CloseProcess} "SilverMonkey.exe" $R0
	${nsProcess::CloseProcess} "MonkeySpeakEditor.exe" $R0
	${nsProcess::CloseProcess} "Verbot Knowledgebase Editor.exe" $R0
	${nsProcess::Unload}
SectionEnd


######################################################################

Section -MainProgram
${INSTALL_TYPE}
SetOverwrite ifnewer
SetOutPath "$INSTDIR"
;!insertmacro CheckNetFramework "40Full"

ExecWait '"$INSTDIR\uninstall.exe" /S _?=$INSTDIR'

File "${Bin_Directory}\FastColoredTextBox.dll"
File "${Bin_Directory}\Furcadialib.dll"
File "${Bin_Directory}\Interfaces.dll"
File "${Bin_Directory}\MonkeySpeak.dll"
File "${Bin_Directory}\Verbot5Library.dll"
File "${Bin_Directory}\RTFLib.dll"

File "${Bin_Directory}\System.Data.SQLite.dll"
File "${Bin_Directory}\x86\SQLite.Interop.dll"
File "${Bin_Directory}\x64\SQLite.Interop.dll"

File "${Bin_Directory}\Keys-ms.ini"
File "${Bin_Directory}\keys.ini"
File "${Bin_Directory}\keyshelp.ini"
File "${Bin_Directory}\keyshelp-ms.ini"
File "${Bin_Directory}\MonkeySpeakEditor.exe"
File "${Bin_Directory}\Data Monkey.exe"
File "${Bin_Directory}\Silver Monkey.chm"
File "${Bin_Directory}\SilverMonkey.exe"
File "${Bin_Directory}\Verbot Knowledgebase Editor.exe"
File "${Bin_Directory}\MapSearch.exe"
File "${Bin_Directory}\*.txt"

${registerExtension} "$INSTDIR\MonkeySpeakEditor.exe" ".ms" "Monkeyspeak File"
${registerExtension} "$INSTDIR\SilverMonkey.exe" ".bini" "SM Bot Information File"

SectionEnd

######################################################################

Section -Additional
SetOutPath "$INSTDIR\Scripts"
File "${Bin_Directory}\Scripts\*.ini"
;File "${Bin_Directory}\Scripts\*.txt"
SectionEnd


Section -Additional2
SetOutPath "$INSTDIR\Templates"
File "${Bin_Directory}\Templates\*.ds"
;File "${Bin_Directory}\Scripts\*.txt"
SectionEnd

Section -Additional3
SetOutPath "$INSTDIR\Scripts-MS"
File "${Bin_Directory}\Scripts-MS\*.ini"
;File "${Bin_Directory}\Scripts-MS\*.txt"
SectionEnd


Section -Additional4
SetOutPath "$INSTDIR\Templates-MS"
File "${Bin_Directory}\Templates-MS\*.ms"
;File "${Bin_Directory}\Templates-MS\*.zip"
;File "${Bin_Directory}\Scripts\*.txt"
SectionEnd

Section -plugins
SetOutPath "$INSTDIR\Plugins"
File /nonfatal "${Bin_Directory}\Plugins\TheClaaaw.ini"
File /nonfatal "${Bin_Directory}\Plugins\TheClaaaw.dll"
SectionEnd

######################################################################

Section -Icons_Reg
SetOutPath "$INSTDIR"
WriteUninstaller "$INSTDIR\uninstall.exe"

!ifdef REG_START_MENU
!insertmacro MUI_STARTMENU_WRITE_BEGIN Application
CreateDirectory "$SMPROGRAMS\$SM_Folder"
CreateShortCut "$SMPROGRAMS\$SM_Folder\${APP_NAME}.lnk" "$INSTDIR\${MAIN_APP_EXE}"
CreateShortCut "$SMPROGRAMS\$SM_Folder\Monkey Speak Editor.lnk" "$INSTDIR\MonkeySpeakEditor.exe"
CreateShortCut "$SMPROGRAMS\$SM_Folder\Data Monkey.lnk" "$INSTDIR\Data Monkey.exe"
CreateShortCut "$SMPROGRAMS\$SM_Folder\Verbot Knowledgebase Editor.lnk" "$INSTDIR\Verbot Knowledgebase Editor.exe"
CreateShortCut "$SMPROGRAMS\$SM_Folder\Scripts.lnk" "$INSTDIR\Scripts"
CreateShortCut "$DESKTOP\${APP_NAME}.lnk" "$INSTDIR\${MAIN_APP_EXE}"
CreateShortCut "$SMPROGRAMS\$SM_Folder\${APP_NAME}.lnk" "$INSTDIR\${MAIN_APP_EXE}"
!ifdef WEB_SITE
WriteIniStr "$INSTDIR\${APP_NAME} website.url" "InternetShortcut" "URL" "${WEB_SITE}"
CreateShortCut "$SMPROGRAMS\$SM_Folder\${APP_NAME} Website.lnk" "$INSTDIR\${APP_NAME} website.url"
!endif
!insertmacro MUI_STARTMENU_WRITE_END
!endif

!ifndef REG_START_MENU
CreateDirectory "$SMPROGRAMS\$SM_Folder"
CreateShortCut "$SMPROGRAMS\$SM_Folder\${APP_NAME}.lnk" "$INSTDIR\${MAIN_APP_EXE}"
CreateShortCut "$SMPROGRAMS\$SM_Folder\Monkey Speak Editor.lnk" "$INSTDIR\MonkeySpeakEditor.exe"
CreateShortCut "$DESKTOP\${APP_NAME}.lnk" "$INSTDIR\${MAIN_APP_EXE}"
!ifdef WEB_SITE
WriteIniStr "$INSTDIR\${APP_NAME} website.url" "InternetShortcut" "URL" "${WEB_SITE}"
CreateShortCut "$SMPROGRAMS\$SM_Folder\${APP_NAME} Website.lnk" "$INSTDIR\${APP_NAME} website.url"
!endif
!endif

WriteRegStr ${REG_ROOT} "${REG_APP_PATH}" "" "$INSTDIR\${MAIN_APP_EXE}"
WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "DisplayName" "${APP_NAME}"
WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "UninstallString" "$INSTDIR\uninstall.exe"
WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "DisplayIcon" "$INSTDIR\${MAIN_APP_EXE}"
WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "DisplayVersion" "${VERSION}"
WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "Publisher" "${COMP_NAME}"

!ifdef WEB_SITE
WriteRegStr ${REG_ROOT} "${UNINSTALL_PATH}"  "URLInfoAbout" "${WEB_SITE}"
!endif
SectionEnd

######################################################################

Section Uninstall
${INSTALL_TYPE}

Delete "$INSTDIR\FastColoredTextBox.dll"
Delete "$INSTDIR\Furcadialib.dll"
Delete "$INSTDIR\Interfaces.dll"
Delete "$INSTDIR\MonkeySpeak.dll"
Delete "$INSTDIR\Verbot5Library.dll"
Delete "$INSTDIR\RTFLib.dll"
Delete "$INSTDIR\System.Data.SQLite.dll"

Delete "$INSTDIR\x86\SQLite.Interop.dll"
Delete "$INSTDIR\x64\SQLite.Interop.dll"
RmDir "$INSTDIR\x86"
RmDir "$INSTDIR\x64"

Delete "$INSTDIR\MSVCR100.dll"
Delete "$INSTDIR\Keys-ms.ini"
Delete "$INSTDIR\keys.ini"
Delete "$INSTDIR\MonkeySpeakEditor.exe"
Delete "$INSTDIR\Data Monkey.exe"
Delete "$INSTDIR\*.chm"
Delete "$INSTDIR\SilverMonkey.exe"
Delete "$INSTDIR\Verbot Knowledgebase Editor.exe"
Delete "$INSTDIR\MapSearch.exe"
Delete "$INSTDIR\*.txt"

Delete "$INSTDIR\Scripts\*.*"
Delete "$INSTDIR\Templates\*.*"
Delete "$INSTDIR\Scripts-MS\*.*"
Delete "$INSTDIR\Templates-MS\*.*"
Delete "$INSTDIR\Plugins\TheClaaaw.ini"
Delete "$INSTDIR\Plugins\TheClaaaw.dll"
RmDir "$INSTDIR\Scripts"
RmDir "$INSTDIR\Templates"
RmDir "$INSTDIR\Scripts-MS"
RmDir "$INSTDIR\Templates-MS"
RmDir "$INSTDIR\Plugins"
${unregisterExtension} ".bini" "SM Bot Information File"
${unregisterExtension} ".ms" "MonkeySpeak File"
!ifdef WEB_SITE
Delete "$INSTDIR\${APP_NAME} website.url"
!endif
RmDir "$INSTDIR"

!ifndef NEVER_UNINSTALL
RmDir "$DOCUMENTS\$SM_Folder"
!endif

!ifdef REG_START_MENU
!insertmacro MUI_STARTMENU_GETFOLDER "Application" $SM_Folder
Delete "$SMPROGRAMS\$SM_Folder\${APP_NAME}.lnk"
Delete "$SMPROGRAMS\$SM_Folder\Monkey Speak Editor.lnk"
Delete "$SMPROGRAMS\$SM_Folder\Data Monkey.lnk"
Delete "$SMPROGRAMS\$SM_Folder\Verbot Knowledgebase Editor.lnk"

!ifdef WEB_SITE
Delete "$SMPROGRAMS\$SM_Folder\${APP_NAME} Website.lnk"
!endif
Delete "$DESKTOP\${APP_NAME}.lnk"

RmDir "$SMPROGRAMS\${APP_NAME}"
!endif

!ifndef REG_START_MENU
Delete "$SMPROGRAMS\$SM_Folder}\${APP_NAME}.lnk"
Delete "$SMPROGRAMS\$SM_Folder\${APP_NAME} Help.lnk"
!ifdef WEB_SITE
Delete "$SMPROGRAMS\$SM_Folder\${APP_NAME} Website.lnk"
!endif
Delete "$DESKTOP\${APP_NAME}.lnk"

RmDir "$SMPROGRAMS\${APP_NAME}"
!endif

DeleteRegKey ${REG_ROOT} "${REG_APP_PATH}"
DeleteRegKey ${REG_ROOT} "${UNINSTALL_PATH}"
SectionEnd

######################################################################

