set signtool="c:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\signtool.exe"
set /p cert=Certification file path (*.p12):
set /p psw=Password:
%signtool% sign /f %cert% /p %psw% /t http://timestamp.comodoca.com/authenticode .\FigLeaf.UI\bin\Release\FigLeaf.Core.dll
%signtool% sign /f %cert% /p %psw% /t http://timestamp.comodoca.com/authenticode .\FigLeaf.UI\bin\Release\FigLeaf.Console.exe
%signtool% sign /f %cert% /p %psw% /t http://timestamp.comodoca.com/authenticode .\FigLeaf.UI\bin\Release\FigLeaf.UI.exe
