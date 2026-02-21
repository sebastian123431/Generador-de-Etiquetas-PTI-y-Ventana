# Reparacion del Disenador - Version Simplificada

Write-Host "`n=== REPARACION DEL DISENADOR ===" -ForegroundColor Cyan
Write-Host "Este script debe ejecutarse con Visual Studio CERRADO`n" -ForegroundColor Yellow

# Verificar si VS esta abierto
$vsProcess = Get-Process devenv -ErrorAction SilentlyContinue
if ($vsProcess) {
    Write-Host "[ERROR] Visual Studio esta abierto!" -ForegroundColor Red
    Write-Host "Por favor, CIERRA Visual Studio y ejecuta nuevamente este script.`n" -ForegroundColor Yellow
    Read-Host "Presiona Enter para salir"
    exit 1
}

Write-Host "[OK] Visual Studio esta cerrado - Continuando...`n" -ForegroundColor Green

# Paso 1: Backup
Write-Host "[1/5] Creando backup..." -ForegroundColor Yellow
$timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
Copy-Item "WindowsFormsApplication1\frm_generadorVentana.Designer.cs" "WindowsFormsApplication1\frm_generadorVentana.Designer.cs.backup_$timestamp" -ErrorAction SilentlyContinue
Write-Host "OK`n" -ForegroundColor Green

# Paso 2: Eliminar carpeta .vs
Write-Host "[2/5] Eliminando cache de Visual Studio..." -ForegroundColor Yellow
if (Test-Path ".vs") {
    Remove-Item -Recurse -Force ".vs" -ErrorAction SilentlyContinue
    Write-Host "OK`n" -ForegroundColor Green
} else {
    Write-Host "No existe`n" -ForegroundColor Green
}

# Paso 3: Limpiar bin y obj
Write-Host "[3/5] Limpiando directorios de compilacion..." -ForegroundColor Yellow
Remove-Item -Recurse -Force "bin" -ErrorAction SilentlyContinue
Remove-Item -Recurse -Force "obj" -ErrorAction SilentlyContinue
Remove-Item -Recurse -Force "WindowsFormsApplication1\bin" -ErrorAction SilentlyContinue
Remove-Item -Recurse -Force "WindowsFormsApplication1\obj" -ErrorAction SilentlyContinue
Write-Host "OK`n" -ForegroundColor Green

# Paso 4: Eliminar archivos .suo y .user
Write-Host "[4/5] Eliminando archivos temporales de VS..." -ForegroundColor Yellow
Get-ChildItem -Recurse -Filter "*.suo" -Force -ErrorAction SilentlyContinue | Remove-Item -Force -ErrorAction SilentlyContinue
Get-ChildItem -Recurse -Filter "*.user" -ErrorAction SilentlyContinue | Remove-Item -Force -ErrorAction SilentlyContinue
Write-Host "OK`n" -ForegroundColor Green

# Paso 5: Restaurar archivo desde Git
Write-Host "[5/5] Restaurando archivo desde repositorio..." -ForegroundColor Yellow
& git.exe checkout HEAD -- 'WindowsFormsApplication1/frm_generadorVentana.cs' 2>&1 | Out-Null
& git.exe checkout HEAD -- 'WindowsFormsApplication1/frm_generadorVentana.Designer.cs' 2>&1 | Out-Null
Write-Host "OK`n" -ForegroundColor Green

Write-Host "=== PROCESO COMPLETADO ===" -ForegroundColor Green
Write-Host "`nAhora sigue estos pasos:" -ForegroundColor Cyan
Write-Host "1. Abre Visual Studio" -ForegroundColor White
Write-Host "2. Abre la solucion (File -> Open -> Project/Solution)" -ForegroundColor White
Write-Host "3. Espera a que cargue completamente (hasta que diga 'Ready')" -ForegroundColor White
Write-Host "4. En Solution Explorer, busca: WindowsFormsApplication1 > frm_generadorVentana.cs" -ForegroundColor White
Write-Host "5. Haz clic derecho -> View Designer (o presiona Shift+F7)" -ForegroundColor White
Write-Host "`nSi no funciona:" -ForegroundColor Yellow
Write-Host "  - Build -> Clean Solution" -ForegroundColor Gray
Write-Host "  - Build -> Rebuild Solution" -ForegroundColor Gray
Write-Host "  - Cierra VS completamente" -ForegroundColor Gray
Write-Host "  - Ejecuta nuevamente este script" -ForegroundColor Gray
Write-Host "`n"

Read-Host "Presiona Enter para salir"
