# Firma Digital de Ejecutables

## ¿Qué es esto?

Este certificado se utiliza para firmar digitalmente el ejecutable de la aplicación **Generador de Etiquetas PTI y Ventana**, evitando que sea bloqueado por políticas de seguridad como **AppLocker** en otros computadores.

## Archivos

- **GeneradorEtiquetas.pfx**: Certificado auto-firmado (válido por 10 años)
- **Contraseña**: `etiquetas123`

## ¿Cómo funciona?

### Automático (Recomendado)

Después de compilar en Visual Studio, el proyecto ejecuta automáticamente un script PowerShell (`Sign-Executable.ps1`) que firma el ejecutable con este certificado.

**No necesitas hacer nada, todo ocurre automáticamente después de la compilación.**

### Manual (si el script no funciona)

Si necesitas firmar manualmente desde PowerShell:

```powershell
# Abrir PowerShell como Administrador
cd "D:\escritorio\vercion mejorada\generador de etiquetas"

# Ejecutar el script de firma
.\Sign-Executable.ps1 -ExePath ".\bin\Debug\Generador de Etiquetas PTI y Ventana.exe"
```

## Verificar que está firmado

```powershell
Get-AuthenticodeSignature "D:\ruta\ejecutable.exe"
```

Si ves **Status: Valid**, está correctamente firmado ✅

## Para Producción

Este certificado es **auto-firmado**, lo que significa:

- ✅ Funciona en desarrollo/testing
- ⚠️ Puede provocar advertencias en algunos sistemas Windows
- ❌ No es reconocido como "confiable" en todas las máquinas

**Para producción profesional**, considera:

1. **Comprar un certificado de código**: Sectigo, DigiCert, etc. (recomendado)
2. **Usar ClickOnce**: Configurar en las propiedades del proyecto para deploying
3. **Usar un instalador MSI**: Más reconocible que un .exe suelto

## Regenerar el certificado

Si necesitas crear uno nuevo:

```powershell
# Eliminar el certificado viejo del almacén
Get-ChildItem "Cert:\CurrentUser\My" | Where-Object { $_.FriendlyName -eq "GeneradorEtiquetasPTI" } | Remove-Item

# Crear uno nuevo (ver script Sign-Executable.ps1)
$cert = New-SelfSignedCertificate -CertStoreLocation "Cert:\CurrentUser\My" `
    -Subject "CN=Generador de Etiquetas PTI" `
    -FriendlyName "GeneradorEtiquetasPTI" `
    -NotAfter (Get-Date).AddYears(10) `
    -Type CodeSigningCert

# Exportar
Export-PfxCertificate -Cert $cert -FilePath "GeneradorEtiquetas.pfx" `
    -Password (ConvertTo-SecureString -String "etiquetas123" -AsPlainText -Force)
```

## Troubleshooting

### Error: signtool.exe no encontrado
**Solución**: Instala Windows SDK desde Visual Studio Installer
- Visual Studio → Modificar → Detalles → marcar "Herramientas de firma de código"

### Error: Certificado no encontrado
**Solución**: El certificado necesita estar en el almacén Windows (no es suficiente tener solo el archivo .pfx)

### El script no se ejecuta automáticamente
**Solución**: Ejecuta manualmente:
```powershell
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
```

---

**Última actualización**: Configuración realizada automáticamente
**Certificado válido hasta**: 10 años desde la creación
