# VERIFICACIÓN DE CONGRUENCIA: API Django vs BD SQLite

## ❌ PROBLEMAS ENCONTRADOS

### 1. **GTIN - Campo adicional faltante en BD**
**Django Model:**
```python
class GTIN(models.Model):
    gtin = models.CharField(...)
    plu = models.CharField(...)
    cod_bolsa = models.CharField(...)
    activo = models.BooleanField(default=True)
    peso_fijo = models.BooleanField(default=False)
```

**API Response esperado:**
```json
{
  "id": 1,
  "gtin": "1234567890",
  "plu": "456",
  "cod_bolsa": "BOLSA123",
  "activo": true,
  "peso_fijo": false
}
```

**BD SQLite (InitializeDatabase):** ✅ CORRECTO
```sql
CREATE TABLE IF NOT EXISTS GTIN (
    id INTEGER PRIMARY KEY,
    gtin TEXT DEFAULT '',
    plu TEXT DEFAULT '',
    cod_bolsa TEXT DEFAULT '',
    activo INTEGER DEFAULT 1,
    peso_fijo INTEGER DEFAULT 0
)
```

---

### 2. **Trazabilidad - Problema con los campos que vienen de FK**
**Django Model:**
```python
class Trazabilidad(models.Model):
    productor = models.ForeignKey('Productor', ...)
    variedad = models.ForeignKey('Variedad', ...)
    lote = models.ForeignKey('Lote', ...)
    sdp = models.ForeignKey('SDP', ...)
    color = models.ForeignKey('Color', null=True, blank=True)
    variedad_imprime = models.ForeignKey('VariedadImprime', ...)
    creado_en = models.DateTimeField(auto_now_add=True)
    actualizado_en = models.DateTimeField(auto_now=True)
```

**API Response esperado (Django DRF por defecto):**
```json
{
  "id": 1,
  "productor": 5,          ← ID de la FK
  "variedad": 3,           ← ID de la FK
  "lote": 2,               ← ID de la FK
  "sdp": 1,                ← ID de la FK
  "color": null,           ← ID o null
  "variedad_imprime": 4,   ← ID de la FK
  "creado_en": "2024-01-15T10:30:00Z",
  "actualizado_en": "2024-01-15T10:30:00Z"
}
```

**Llamado en C# (HttpSyncClient):** ✅ CORRECTO
```csharp
case "trazabilidades":
    db.UpsertTrazabilidad(
        item["id"].Value<int>(),
        item["productor"].Value<int>(),          // ✅ Correcto
        item["variedad"].Value<int>(),           // ✅ Correcto
        item["lote"].Value<int>(),               // ✅ Correcto
        item["sdp"].Value<int>(),                // ✅ Correcto
        item["color"] != null && item["color"].Type != JTokenType.Null 
            ? item["color"].Value<int>() 
            : (int?)null,                         // ✅ Correcto (nullable)
        item["variedad_imprime"].Value<int>(),   // ✅ Correcto
        GetString(item, "creado_en", "created_at", "creado"),
        GetString(item, "actualizado_en", "updated_at", "actualizado")
    );
    break;
```

**BD SQLite:** ✅ CORRECTO
```sql
CREATE TABLE IF NOT EXISTS Trazabilidad (
    id INTEGER PRIMARY KEY,
    productor_id INTEGER,
    variedad_id INTEGER,
    lote_id INTEGER,
    sdp_id INTEGER,
    color_id INTEGER,
    variedad_imprime_id INTEGER,
    creado_en TEXT,
    actualizado_en TEXT
)
```

---

### 3. **VariedadImprime_GTIN - Tabla intermedia**
**Django Model:**
```python
class VariedadImprime_GTIN(models.Model):
    variedad_imprime = models.ForeignKey('VariedadImprime', ...)
    gtin = models.ForeignKey('GTIN', ...)
    creado_en = models.DateTimeField(auto_now_add=True)
    actualizado_en = models.DateTimeField(auto_now=True)
```

**API Response esperado:**
```json
{
  "id": 1,
  "variedad_imprime": 5,
  "gtin": 3,
  "creado_en": "2024-01-15T10:30:00Z",
  "actualizado_en": "2024-01-15T10:30:00Z"
}
```

**Llamado en C#:** ✅ CORRECTO
```csharp
case "variedad_imprime_gtin":
    db.UpsertVariedadImprimeGTIN(
        item["id"].Value<int>(),
        item["variedad_imprime"].Value<int>(),
        item["gtin"].Value<int>(),
        GetString(item, "creado_en", "created_at", "creado"),
        GetString(item, "actualizado_en", "updated_at", "actualizado")
    );
    break;
```

**BD SQLite:** ✅ CORRECTO

---

### 4. **VariedadVariedadImprime_variedad - Tabla intermedia**
**Django Model:**
```python
class VariedadVariedadImprime_variedad(models.Model):
    variedad = models.ForeignKey('Variedad', ...)
    variedad_imprime = models.ForeignKey('VariedadImprime', ...)
```

**API Response esperado:**
```json
{
  "id": 1,
  "variedad": 2,
  "variedad_imprime": 5
}
```

**Llamado en C#:** ✅ CORRECTO
```csharp
case "variedades_variedad_imprimir":
    db.UpsertVariedadVariedadImprime(
        item["id"].Value<int>(),
        item["variedad"].Value<int>(),
        item["variedad_imprime"].Value<int>()
    );
    break;
```

---

## ✅ RESUMEN DE COINCIDENCIAS

| Tabla | Django | API | C# | BD |
|-------|--------|-----|----|----|
| **Ubicacion** | 5 campos | id, region, provincia, comuna, gln, ggn, activo, creado_en, actualizado_en | ✅ | ✅ |
| **Productor** | dato, ubicacion(FK), activo | id, dato, ubicacion(FK), activo | ✅ | ✅ |
| **Variedad** | dato, activo | id, dato, activo | ✅ | ✅ |
| **Lote** | dato, activo | id, dato, activo | ✅ | ✅ |
| **VariedadImprime** | var_interno, dato, activo, peso_fijo | id, var_interno, dato, activo, peso_fijo | ✅ | ✅ |
| **TipoEmbalaje** | dato, activo, peso_fijo | id, dato, activo, peso_fijo | ✅ | ✅ |
| **GTIN** | gtin, plu, cod_bolsa, activo, peso_fijo | id, gtin, plu, cod_bolsa, activo, peso_fijo | ✅ | ✅ |
| **Calibre** | dato, activo | id, dato, activo | ✅ | ✅ |
| **SDP** | dato, activo, ubicacion(FK) | id, dato, activo, ubicacion(FK) | ✅ | ✅ |
| **Packing** | dato, activo, csp, ubicacion(FK) | id, dato, activo, csp, ubicacion(FK) | ✅ | ✅ |
| **Peso** | dato, activo | id, dato, activo | ✅ | ✅ |
| **CategoriaSAG** | nombre | id, nombre | ✅ | ✅ |
| **Color** | nombre, descripcion, activo, creado_en, actualizado_en | id, nombre, descripcion, activo, creado_en, actualizado_en | ✅ | ✅ |
| **Trazabilidad** | FK: productor, variedad, lote, sdp, color, variedad_imprime + timestamps | ✅ | ✅ | ✅ |
| **VariedadImprime_GTIN** | FK: variedad_imprime, gtin + timestamps | ✅ | ✅ | ✅ |
| **VariedadVariedadImprime_variedad** | FK: variedad, variedad_imprime | ✅ | ✅ | ✅ |

---

## ⚠️ COSAS A VERIFICAR EN LA API DJANGO

1. **¿Los timestamps vienen como `creado_en` o `created_at`?**
   - El código ya maneja ambos: `GetString(item, "creado_en", "created_at", "creado")`
   
2. **¿Las relaciones foráneas vienen como IDs o como objetos?**
   - Se espera que vengan como IDs (ejemplo: `"productor": 5`)
   - El código maneja esto correctamente

3. **¿El campo `ubicacion` en Productor/Packing/SDP viene anidado?**
   - Si viene como: `"ubicacion": {"id": 5, "region": "..."}` → PROBLEMA
   - Si viene como: `"ubicacion": 5` → OK (como está ahora)

---

## 🔧 RECOMENDACIÓN

**Antes de sincronizar, prueba un endpoint manualmente con:**
```bash
curl http://192.168.3.30:8001/api/gtin/
curl http://192.168.3.30:8001/api/trazabilidad/
curl http://192.168.3.30:8001/api/color/
```

Y verifica que la estructura JSON coincida con lo esperado.
