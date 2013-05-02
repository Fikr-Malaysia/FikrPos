@echo on
del source\*.* /S /F /Q
mkdir source
copy "..\FikrPos\bin\release\FikrPos.exe" source
copy "..\FikrPos\bin\release\NAppUpdate.Framework.dll" source
copy "..\Documentation\Fikr Point of Sales.pdf" source
mkdir source\images
copy "..\FikrPos\bin\release\images\cash.ico" source\images


