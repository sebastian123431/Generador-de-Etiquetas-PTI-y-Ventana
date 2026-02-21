# 🔄 DIAGRAMA DE CORRESPONDENCIA: Django ↔ SQLite ↔ C# ↔ UI

```
┌─────────────────────────────────────────────────────────────────────────┐
│                           DJANGO BACKEND                               │
│                    (models.py - API Endpoints)                         │
├─────────────────────────────────────────────────────────────────────────┤
│
│  ✅ Ubicacion           → /api/ubicacion/
│     (region, provincia, comuna, gln, ggn, activo, ...)
│     
│  ✅ Productor           → /api/productor/
│     (id, dato, CSG, ubicacion_fk, activo)
│     
│  ✅ Variedad            → /api/variedad/
│     (id, dato, activo)
│     
│  ✅ Lote                → /api/lote/
│     (id, dato, activo)
│     
│  ✅ VariedadImprime     → /api/variedad-imprime/
│     (id, var_interno, dato, activo, peso_fijo)
│     
│  ✅ GTIN                → /api/gtin/
│     (id, gtin, plu, cod_bolsa, activo, peso_fijo)
│     
│  ✅ SDP                 → /api/sdp/
│     (id, dato, activo, ubicacion_fk)
│     
│  ✅ Recibidor           → /api/recibidor/
│     (id, dato, activo, creado_en, actualizado_en)
│     
│  ✅ Packing             → /api/packing/
│     (id, dato, activo, csp, ubicacion_fk)
│     
│  ✅ TipoEmbalaje        → /api/tipo-embalaje/
│     (id, dato, activo, peso_fijo)
│     
│  ✅ Calibre             → /api/calibre/
│     (id, dato, activo)
│     
│  ✅ Peso                → /api/peso/
│     (id, dato, activo)
│     
│  ✅ Color               → /api/color/
│     (id, nombre, descripcion, activo, ...)
│     
│  ✅ CategoriaSAG        → /api/categoria-sag/
│     (id, nombre)
│     
│  ✅ CantidadCajas       → /api/cantidad-cajas/
│     (id, cantidad, activo)
│
└─────────────────────────────────────────────────────────────────────────┘
         ⬇️  HttpSyncClient.cs: SincronizarTodoAsync()  ⬇️
┌─────────────────────────────────────────────────────────────────────────┐
│                    SQLITE LOCAL DATABASE                               │
│               (bin\Debug\etiquetas.db)                                  │
├─────────────────────────────────────────────────────────────────────────┤
│
│  ✅ UpsertUbicacion(id, region, provincia, ...)
│  ✅ UpsertProductor(id, dato, csg, activo, ubicacion_id)
│  ✅ UpsertVariedad(id, dato, activo)
│  ✅ UpsertLote(id, dato, activo)
│  ✅ UpsertVariedadImprime(id, var_interno, dato, ...)
│  ✅ UpsertGTIN(id, gtin, plu, cod_bolsa, ...)
│  ✅ UpsertSDP(id, dato, activo, ubicacion_id)
│  ✅ UpsertRecibidor(id, dato, activo, creado_en, actualizado_en)
│  ✅ UpsertPacking(id, dato, activo, csp, ubicacion_id)
│  ✅ UpsertCalibre(id, dato, activo)
│  ✅ UpsertPeso(id, dato, activo)
│  ✅ UpsertColor(id, nombre, descripcion, activo)
│  ✅ UpsertCategoriaSAG(id, nombre)
│  ✅ UpsertCantidadCajas(id, cantidad, activo)
│
└─────────────────────────────────────────────────────────────────────────┘
         ⬇️  frm_generadorVentana.cs: Métodos Llena_*()  ⬇️
┌─────────────────────────────────────────────────────────────────────────┐
│                    WINDOWS FORMS CONTROLS                              │
│                   (ComboBox en la UI)                                   │
├─────────────────────────────────────────────────────────────────────────┤
│
│  ✅ cmb_productor        ← SELECT id || ' ' || dato FROM Productor
│                           WHERE activo = 1
│
│  ✅ cmb_Recibidor        ← SELECT dato FROM Recibidor
│                           WHERE activo = 1
│
│  ✅ cmb_variedad         ← SELECT dato FROM Variedad
│                           WHERE activo = 1
│
│  ✅ cmb_lote             ← SELECT dato FROM Lote
│                           WHERE activo = 1
│
│  ✅ cbx_sdp              ← SELECT dato FROM SDP
│                           WHERE activo = 1
│
│  ✅ cmb_variedad_Imprime ← SELECT var_interno || ' ' || dato 
│                           FROM VariedadImprime WHERE activo = 1
│
│  ✅ cmb_calibre          ← Llenado manualmente (LlenaCalibres)
│
│  ✅ cmb_tipo_embalaje    ← SELECT dato FROM TipoEmbalaje
│                           WHERE peso_fijo = [bool]
│
└─────────────────────────────────────────────────────────────────────────┘
```

## 📋 FLUJO DE DATOS

### Cuando se abre la aplicación (Form1_Load):

```
1. Llena_Productor()
   └─→ SELECT id || ' ' || dato FROM Productor WHERE activo = 1
       └─→ Muestra en cmb_productor

2. Llena_Recibidor()  ✅ FIXED (ahora usa tabla correcta)
   └─→ SELECT dato FROM Recibidor WHERE activo = 1
       └─→ Muestra en cmb_Recibidor

3. LlenaEmbalaje()
   └─→ Llena cmb_tipo_embalaje basado en chb_pesofijo

4. Setea SelectedIndex = 0 (solo si Items.Count > 0)
```

### Cuando se selecciona un Productor (cmb_productor_SelectedIndexChanged):

```
cmb_productor.SelectedIndexChanged
  └─→ LlenaVariedad()
      └─→ SELECT dato FROM Variedad WHERE activo = 1
          └─→ Muestra en cmb_variedad
          └─→ Llama Llena_variedad_imprime()
              └─→ Llena cmb_variedad_Imprime según Variedad seleccionada
```

### Cuando se selecciona una Variedad (cmb_variedad_SelectedIndexChanged):

```
cmb_variedad.SelectedIndexChanged
  └─→ Llena_lotes()
      └─→ SELECT dato FROM Lote WHERE activo = 1
          └─→ Muestra en cmb_lote
  └─→ Llena_variedad_imprime()
      └─→ Llena cmb_variedad_Imprime
```

### Cuando se selecciona un Lote (cmb_lote_SelectedIndexChanged):

```
cmb_lote.SelectedIndexChanged
  └─→ Llena_SDP()
      └─→ SELECT dato FROM SDP WHERE activo = 1
          └─→ Muestra en cbx_sdp
```

### Cuando se selecciona Variedad Imprime (cmb_variedad_Imprime_SelectedIndexChanged):

```
cmb_variedad_Imprime.SelectedIndexChanged
  └─→ LlenaCalibres()
      └─→ Llena cmb_calibre basado en variedad seleccionada
```

## 🟢 ESTADO: TODO CORRECTO

- ✅ Modelos Django consistentes
- ✅ Métodos Upsert correctos
- ✅ Llamadas API correctas
- ✅ Consultas SQL correctas
- ✅ Validaciones en UI correctas
- ✅ Tabla Recibidor fixed (era Ubicacion)
