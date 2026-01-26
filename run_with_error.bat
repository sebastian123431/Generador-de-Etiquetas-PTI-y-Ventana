@echo off
cd "C:\Users\seba5010\Documents\analisis\Voice Pick Code\bin\Debug"
echo Iniciando Voice Pick Code...
"Voice Pick Code.exe"
if errorlevel 1 (
    echo ERROR: La aplicacion termino con codigo de error %errorlevel%
)
pause
