﻿; example1.nsi
;
; This script is perhaps one of the simplest NSIs you can make. All of the
; optional settings are left to their default settings. The installer simply 
; prompts the user asking them where to install, and drops a copy of example1.nsi
; there. 

Unicode true

!define APP   "yPDFEditor"
!define TITLE "your PDF Editor"

!system 'MySign "bin\Release\${APP}.exe"'
!finalize 'MySign "%1"'

!system 'DefineAsmVer.exe "bin\Release\${APP}.exe" "!define VER ""[RAWFV]"" " > Appver.tmp'
!include "Appver.tmp"

!searchreplace APPVER "${VER}" "." "_"

!define MIME "application/pdf"

!define EXT ".pdf"

; bin\Release
; Release/Any CPU

XPStyle on

;--------------------------------

; The name of the installer
Name "${TITLE} ${VER}"

; The file to write
OutFile "Setup_${APP}_${APPVER}_user.exe"

; The default installation directory
InstallDir "$APPDATA\${APP}"

; Registry key to check for directory (so if you install again, it will
; overwrite the old one automatically)
InstallDirRegKey HKCU "Software\HIRAOKA HYPERS TOOLS, Inc.\${APP}" "Install_Dir"

; Request application privileges for Windows Vista
RequestExecutionLevel user

AutoCloseWindow true

AllowSkipFiles off

SetOverwrite ifdiff

!include LogicLib.nsh

;--------------------------------

; Pages

Page license
PageEx license
  LicenseText "変更履歴"
  LicenseData CHANGES.rtf
PageExEnd
Page directory
Page components
Page instfiles

LicenseData GNUGPL2.txt

UninstPage uninstConfirm
UninstPage instfiles

;--------------------------------

!ifdef SHCNE_ASSOCCHANGED
!undef SHCNE_ASSOCCHANGED
!endif
!define SHCNE_ASSOCCHANGED 0x08000000

!ifdef SHCNF_FLUSH
!undef SHCNF_FLUSH
!endif
!define SHCNF_FLUSH        0x1000

!ifdef SHCNF_IDLIST
!undef SHCNF_IDLIST
!endif
!define SHCNF_IDLIST       0x0000

!macro UPDATEFILEASSOC
  IntOp $1 ${SHCNE_ASSOCCHANGED} | 0
  IntOp $0 ${SHCNF_IDLIST} | ${SHCNF_FLUSH}
; Using the system.dll plugin to call the SHChangeNotify Win32 API function so we
; can update the shell.
  System::Call "shell32::SHChangeNotify(i,i,i,i) ($1, $0, 0, 0)"
!macroend

;--------------------------------

; The stuff to install
Section "${APP}" ;No components page, name is not important
  SectionIn ro

  ; Set output path to the installation directory.
  SetOutPath $INSTDIR

  ; Put file there
  File /r /x "*.vshost.*" "bin\Release\*.*"
  File ".\MAPISendMailSa.exe"
  File "1.ico"

  SetOutPath $INSTDIR

  WriteRegStr HKCU "Software\Classes\${APP}" "" "${TITLE}"
  WriteRegStr HKCU "Software\Classes\${APP}\DefaultIcon" "" "$INSTDIR\1.ico,0"
  WriteRegStr HKCU "Software\Classes\${APP}\shell\open\command" "" '"$INSTDIR\${APP}.exe" "%1"'

  WriteRegStr HKCU "Software\HIRAOKA HYPERS TOOLS, Inc.\${APP}" "Install_Dir" "$INSTDIR"

  ; Write the uninstall keys for Windows
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP}" "DisplayName" "${TITLE}"
  WriteRegStr HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP}" "UninstallString" '"$INSTDIR\uninstall.exe"'
  WriteRegDWORD HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP}" "NoModify" 1
  WriteRegDWORD HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP}" "NoRepair" 1
  WriteUninstaller "uninstall.exe"
  
SectionEnd ; end the section

Section /o "PDFへ強く関連付け"
  WriteRegStr HKCU "Software\Classes\${EXT}" "" "${APP}"
  WriteRegStr HKCU "Software\Classes\${EXT}" "Content Type" "${MIME}"
SectionEnd

Section ""
  WriteRegStr HKCU "Software\Classes\${EXT}\OpenWithProgids" "${APP}" ""

  WriteRegStr HKCU "Software\Classes\Applications\${APP}.exe\shell\open\command" "" '"$INSTDIR\${APP}.exe" "%1"'

  DeleteRegValue HKCU "Software\Microsoft\Windows\CurrentVersion\Explorer\FileExts\${EXT}" ""
  DeleteRegValue HKCU "Software\Microsoft\Windows\CurrentVersion\Explorer\FileExts\${EXT}" "Progid"
  DeleteRegValue HKCU "Software\Microsoft\Windows\CurrentVersion\Explorer\FileExts\${EXT}" "Application"

  DetailPrint "関連付け更新中です。お待ちください。"
  !insertmacro UPDATEFILEASSOC
SectionEnd

Section "スタートメニューへ登録"
  CreateDirectory "$SMPROGRAMS\${TITLE}"
  CreateShortCut "$SMPROGRAMS\${TITLE}\Uninstall.lnk" "$INSTDIR\uninstall.exe" "" "$INSTDIR\uninstall.exe" 0
  CreateShortCut "$SMPROGRAMS\${TITLE}\起動.lnk" "$INSTDIR\${APP}.exe" "" "$INSTDIR\${APP}.exe" 0
SectionEnd

Section "起動"
  SetOutPath $INSTDIR
  Exec "$INSTDIR\${APP}.exe"
SectionEnd

;--------------------------------

; Uninstaller

Section "Uninstall"

  ; Remove registry keys
  DeleteRegKey HKCU "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APP}"
  DeleteRegKey HKCU "Software\HIRAOKA HYPERS TOOLS, Inc.\${APP}"
  
  DeleteRegValue HKCU "Software\Classes\${EXT}\OpenWithProgids" "${APP}"

  DeleteRegKey HKCU "Software\Classes\Applications\${APP}.exe\shell\open\command"
  DeleteRegKey HKCU "Software\Classes\Applications\${APP}.exe\shell\open"
  DeleteRegKey HKCU "Software\Classes\Applications\${APP}.exe\shell"
  DeleteRegKey HKCU "Software\Classes\Applications\${APP}.exe"

  ReadRegStr $0 HKCU "Software\Classes\${EXT}" ""
  ${If} $0 == "${APP}"
    ReadRegStr $0 HKLM "Software\Classes\${EXT}" ""
    WriteRegStr   HKCU "Software\Classes\${EXT}" "" "$0"
  ${EndIf}

  ; Remove files and uninstaller
  Delete "$INSTDIR\freetype6.dll"
  Delete "$INSTDIR\libgcc_s_dw2-1.dll"
  Delete "$INSTDIR\libiconv2.dll"
  Delete "$INSTDIR\libjpeg-8.dll"
  Delete "$INSTDIR\libpng14-14.dll"
  Delete "$INSTDIR\libpoppler-13.dll"
  Delete "$INSTDIR\libstdc++-6.dll"
  Delete "$INSTDIR\pdfinfo.exe"
  Delete "$INSTDIR\pdftoppm.exe"
  Delete "$INSTDIR\zlib1.dll"

  RMDir /r "$INSTDIR\license_gpl_pdftk"

  Delete "$INSTDIR\cyggcc_s-1.dll"
  Delete "$INSTDIR\cygiconv-2.dll"
  Delete "$INSTDIR\cygwin1.dll"
  Delete "$INSTDIR\cygz.dll"
  Delete "$INSTDIR\pdftk.exe"
  Delete "$INSTDIR\libiconv2.dll"

  Delete "$INSTDIR\1.ico"
  Delete "$INSTDIR\${APP}.exe"
  Delete "$INSTDIR\${APP}.pdb"
  Delete "$INSTDIR\MAPISendMailSa.exe"

  RMDir /r "$INSTDIR\share\poppler"
  RMDir    "$INSTDIR\share"

  RMDir /r "$INSTDIR\GPL"
  
  Delete "$INSTDIR\nl\PdfiumViewer.resources.dll"
  RMDir  "$INSTDIR\nl"
  Delete "$INSTDIR\x64\pdfium.dll"
  RMDir  "$INSTDIR\x64"
  Delete "$INSTDIR\x86\pdfium.dll"
  RMDir  "$INSTDIR\x86"

  DetailPrint "関連付け更新中です。お待ちください。"
  !insertmacro UPDATEFILEASSOC

  Delete "$INSTDIR\uninstall.exe"

  ; Remove shortcuts, if any
  Delete "$SMPROGRAMS\${TITLE}\Uninstall.lnk"
  Delete "$SMPROGRAMS\${TITLE}\起動.lnk"

  ; Remove directories used
  RMDir "$SMPROGRAMS\${TITLE}"
  RMDir "$INSTDIR"

SectionEnd
