Dim shell, systempath

set shell = WScript.CreateObject( "WScript.Shell" )

systempath = shell.ExpandEnvironmentStrings("%SystemRoot%")

shell.Run Chr(34) & systempath & "\system32\msiexec.exe" & Chr(34) & "  /x{56CDD99D-2E2D-4597-8B76-837743487B04}"

WScript.Quit