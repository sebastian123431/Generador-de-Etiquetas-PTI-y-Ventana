param(
    [string]$ExePath,
    [string]$CertPath = ".\certificado\GeneradorEtiquetas.pfx",
    [string]$Password = "etiquetas123"
)

# Validar que el ejecutable existe
if (-not (Test-Path $ExePath)) {
    Write-Host "❌ Archivo no encontrado: $ExePath" -ForegroundColor Red
    exit 1
}

# Buscar signtool
$signtoolPaths = @(
    "C:\Program Files (x86)\Windows Kits\10\bin\10.0.26100.0\x64\signtool.exe",
    "C:\Program Files (x86)\Windows Kits\10\App Certification Kit\signtool.exe",
    "C:\Program Files (x86)\Windows Kits\10\bin\10.0.22621.0\x64\signtool.exe",
    "C:\Program Files (x86)\Windows Kits\10\bin\10.0.19041.0\x64\signtool.exe",
    "C:\Program Files (x86)\Windows Kits\11\bin\11.0.0\x64\signtool.exe"
)

$signtool = $null
foreach ($path in $signtoolPaths) {
    if (Test-Path $path) {
        $signtool = $path
        break
    }
}

if (-not $signtool) {
    Write-Host "⚠️  signtool.exe no encontrado. Instala Windows SDK." -ForegroundColor Yellow
    exit 0
}

# Firmar el ejecutable
try {
    $certFullPath = (Resolve-Path $CertPath -ErrorAction SilentlyContinue).Path
    if (-not $certFullPath) {
        $certFullPath = $CertPath
    }

    & $signtool sign /f $certFullPath /p $Password /t "http://timestamp.sectigo.com" /fd SHA256 "$ExePath"

    if ($LASTEXITCODE -eq 0) {
        Write-Host "✅ Ejecutable firmado correctamente: $ExePath" -ForegroundColor Green
    } else {
        Write-Host "⚠️  Firma completada con código de salida: $LASTEXITCODE" -ForegroundColor Yellow
    }
}
catch {
    Write-Host "❌ Error al firmar: $_" -ForegroundColor Red
    exit 1
}
