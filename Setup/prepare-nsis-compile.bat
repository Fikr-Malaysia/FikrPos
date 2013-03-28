@echo off
del source\*.* /S /F /Q
mkdir source
copy "..\FikrPos\bin\release\FikrPos.exe" source
mkdir source\images
copy "..\FikrPos\bin\release\images\cash.ico" source\images


