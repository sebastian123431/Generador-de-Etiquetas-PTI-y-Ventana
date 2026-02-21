-- Script para diagnosticar estructura de tablas en etiquetas.db

-- Ver estructura de tabla Ubicacion
PRAGMA table_info(Ubicacion);

-- Ver estructura de tabla Productor
PRAGMA table_info(Productor);

-- Ver estructura de tabla Variedad
PRAGMA table_info(Variedad);

-- Ver estructura de tabla Lote
PRAGMA table_info(Lote);

-- Ver estructura de tabla SDP
PRAGMA table_info(SDP);

-- Ver primeros registros de cada tabla
SELECT '=== UBICACION ===' as info;
SELECT * FROM Ubicacion LIMIT 3;

SELECT '=== PRODUCTOR ===' as info;
SELECT * FROM Productor LIMIT 3;

SELECT '=== VARIEDAD ===' as info;
SELECT * FROM Variedad LIMIT 3;

SELECT '=== LOTE ===' as info;
SELECT * FROM Lote LIMIT 3;

SELECT '=== SDP ===' as info;
SELECT * FROM SDP LIMIT 3;
