$commands = @"
CREATE TABLE IF NOT EXISTS TipoEmbalaje (id INTEGER PRIMARY KEY, dato TEXT, descripcion TEXT, activo INTEGER, peso_fijo INTEGER);
CREATE TABLE IF NOT EXISTS Variedad (id INTEGER PRIMARY KEY, dato TEXT, activo INTEGER);
CREATE TABLE IF NOT EXISTS Lote (id INTEGER PRIMARY KEY, dato TEXT, activo INTEGER);
CREATE TABLE IF NOT EXISTS VariedadImprime (id INTEGER PRIMARY KEY, dato TEXT, activo INTEGER);
CREATE TABLE IF NOT EXISTS Packing (id INTEGER PRIMARY KEY, dato TEXT, activo INTEGER);
CREATE TABLE IF NOT EXISTS Calibre (id INTEGER PRIMARY KEY, dato TEXT, activo INTEGER);
CREATE TABLE IF NOT EXISTS Peso (id INTEGER PRIMARY KEY, dato TEXT, activo INTEGER);
CREATE TABLE IF NOT EXISTS CategoriaSAG (id INTEGER PRIMARY KEY, nombre TEXT, activo INTEGER);
CREATE TABLE IF NOT EXISTS Color (id INTEGER PRIMARY KEY, nombre TEXT, descripcion TEXT, activo INTEGER);
CREATE TABLE IF NOT EXISTS GTIN (id INTEGER PRIMARY KEY, dato TEXT, activo INTEGER, peso_fijo INTEGER);
CREATE TABLE IF NOT EXISTS SDP (id INTEGER PRIMARY KEY, dato TEXT, descripcion TEXT, activo INTEGER);
CREATE TABLE IF NOT EXISTS VariedadVariedadImprime_variedad (id INTEGER PRIMARY KEY, variedad_id INTEGER, variedad_imprime_id INTEGER, activo INTEGER);
CREATE TABLE IF NOT EXISTS VariedadImprime_GTIN (id INTEGER PRIMARY KEY, variedad_imprime_id INTEGER, gtin_id INTEGER, activo INTEGER, creado_en TEXT, actualizado_en TEXT);
CREATE TABLE IF NOT EXISTS Trazabilidad (id INTEGER PRIMARY KEY, productor_id INTEGER, variedad_id INTEGER, lote_id INTEGER, sdp_id INTEGER, variedad_imprime_id INTEGER, creado_en TEXT, actualizado_en TEXT);
CREATE TABLE IF NOT EXISTS ConfiguracionSync (Clave TEXT PRIMARY KEY, Valor TEXT);
"@

sqlite3 etiquetas_local.db $commands