# Script PowerShell para inspeccionar la base de datos SQLite

$dbPath = "D:\escritorio\vercion mejorada\generador de etiquetas\bin\Debug\etiquetas.db"

# Cargar ensamblados
Add-Type -AssemblyName System.Data

# Buscar la DLL de SQLite
$sqliteDll = Get-ChildItem -Path "D:\escritorio\vercion mejorada\generador de etiquetas" -Filter "System.Data.SQLite.dll" -Recurse | Select-Object -First 1

if ($sqliteDll) {
    Write-Host "✓ Encontrado: $($sqliteDll.FullName)"
    [System.Reflection.Assembly]::LoadFrom($sqliteDll.FullName)
} else {
    Write-Host "✗ No se encontró System.Data.SQLite.dll"
    exit 1
}

# Conectar a la base de datos
$connectionString = "Data Source=$dbPath;Version=3;"
$connection = New-Object System.Data.SQLite.SQLiteConnection($connectionString)

try {
    $connection.Open()
    Write-Host "✓ Conectado a la base de datos`n"
    
    # Obtener todas las tablas
    $cmd = $connection.CreateCommand()
    $cmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table' ORDER BY name;"
    $reader = $cmd.ExecuteReader()
    
    Write-Host "TABLAS DISPONIBLES:"
    Write-Host "=================="
    $tables = @()
    while ($reader.Read()) {
        $tableName = $reader["name"]
        $tables += $tableName
        Write-Host "  - $tableName"
    }
    $reader.Close()
    
    Write-Host "`nESTRUCTURA DE TABLAS:"
    Write-Host "===================="
    
    foreach ($table in $tables) {
        Write-Host "`n📋 TABLA: $table"
        Write-Host ("-" * 60)
        
        # Obtener estructura
        $cmd = $connection.CreateCommand()
        $cmd.CommandText = "PRAGMA table_info($table);"
        $reader = $cmd.ExecuteReader()
        
        Write-Host "Columnas:"
        while ($reader.Read()) {
            $colName = $reader["name"]
            $colType = $reader["type"]
            Write-Host "  - $colName ($colType)"
        }
        $reader.Close()
        
        # Contar registros
        $cmd = $connection.CreateCommand()
        $cmd.CommandText = "SELECT COUNT(*) as total FROM $table;"
        $total = $cmd.ExecuteScalar()
        Write-Host "Total de registros: $total"
        
        # Mostrar primeros registros
        $cmd = $connection.CreateCommand()
        $cmd.CommandText = "SELECT * FROM $table LIMIT 3;"
        $reader = $cmd.ExecuteReader()
        
        if ($reader.HasRows) {
            Write-Host "Primeros registros:"
            $rowNum = 0
            while ($reader.Read() -and $rowNum -lt 3) {
                $row = ""
                for ($i = 0; $i -lt $reader.FieldCount; $i++) {
                    $colName = $reader.GetName($i)
                    $value = $reader.GetValue($i)
                    $row += "$colName='$value' | "
                }
                Write-Host "  [$($rowNum+1)] $row"
                $rowNum++
            }
        } else {
            Write-Host "La tabla está vacía"
        }
        $reader.Close()
    }
    
} catch {
    Write-Host "✗ Error: $_"
} finally {
    $connection.Close()
}
