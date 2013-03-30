OutFile "Setup.exe"

installDir "$PROGRAMFILES\Fikr Malaysia\FikrPos"
!define shortcutDir "$SMPROGRAMS\FikrPos"
RequestExecutionLevel admin

section
	SetShellVarContext all
	setOutPath $INSTDIR
	File /r "source\*"
	writeUninstaller $INSTDIR\uninstaller.exe
	CreateDirectory "${shortcutDir}" 
	createShortCut "${shortcutDir}\Fikr POS.lnk" "$INSTDIR\FikrPos.exe"  "$INSTDIR\images\cash.ico" 
	createShortCut "${shortcutDir}\Fikr POS User Guide.lnk" "$INSTDIR\Fikr Point of Sales.pdf"
	createShortCut "${shortcutDir}\Uninstall FikrPos.lnk" "$INSTDIR\uninstaller.exe"
sectionEnd
	section "Uninstall"
	SetShellVarContext all
	delete "$INSTDIR\uninstaller.exe"
	RMDir /r "$INSTDIR"
	RMDir /r "${shortcutDir}"
sectionEnd