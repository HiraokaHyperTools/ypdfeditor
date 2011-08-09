
#include <windows.h>

#include <mapi.h>
#include <tchar.h>

static HMODULE s_hInstMail;

typedef ULONG (PASCAL *SendMail)(ULONG, ULONG_PTR, MapiMessage*, FLAGS, ULONG);
static SendMail s_lpfnSendMail;

static MapiFileDesc s_fileDesc;
static MapiMessage s_message;

static char s_szPathName[1024];
static char s_szFileName[1024];

int main(int argc, char **argv) {
	if (argc < 2) {
		return 1;
	}

	s_hInstMail = LoadLibrary("MAPI32.DLL");
	if (s_hInstMail == NULL) {
		return 1;
	}

	s_lpfnSendMail = (SendMail)GetProcAddress(s_hInstMail, "MAPISendMail");
	if (s_lpfnSendMail == NULL) {
		return 1;
	}

	GetFullPathName(argv[1], sizeof(s_szPathName) / sizeof(TCHAR), s_szPathName, NULL);

	LPSTR psz = _tcsrchr(s_szPathName, _T('\\'));

	_tcscpy_s(s_szFileName, sizeof(s_szFileName), (psz == NULL) ? _T("") : &psz[1]);

	ZeroMemory(&s_fileDesc, sizeof(s_fileDesc));

	s_fileDesc.nPosition = (ULONG)-1;
	s_fileDesc.lpszPathName = s_szPathName;
	s_fileDesc.lpszFileName = 0;

	ZeroMemory(&s_message, sizeof(s_message));

	s_message.nFileCount = 1;
	s_message.lpFiles = &s_fileDesc;

	s_lpfnSendMail(0, NULL, &s_message, MAPI_LOGON_UI|MAPI_DIALOG, 0);

	return 0;
}
