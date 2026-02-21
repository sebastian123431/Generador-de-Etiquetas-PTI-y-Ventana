using System;
using System.Collections.Generic;
using System.Globalization;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using LabelKit_2022;
using LabelKit_PLU_2022;
using DB = WindowsFormsApplication1.Data;
using WindowsFormsApplication1.Data;

namespace WindowsFormsApplication1
{
    [System.ComponentModel.DesignerCategory("Form")]
    public partial class frm_generador : Form
    {
        /// <summary>
        /// Constructor del formulario generador PTI
        /// Inicializa todos los componentes visuales del formulario
        /// </summary>
        public frm_generador()
        {
            this.InitializeComponent();
        }

        // Evita re-entradas/duplicados al sincronizar (por eventos o múltiples aperturas)
        private bool _syncInProgress = false;
        private bool _syncMensajeMostrado = false;

        private bool BaseDeDatosTieneDatos()
        {
            try
            {
                var db = DatabaseManager.Instance;

                // Basta con que alguna tabla clave tenga datos para considerar la BD "cargada"
                var productores = db.GetProductoresItems();
                if (productores != null && productores.Count > 0) return true;

                var variedades = db.GetVariedadesItems();
                if (variedades != null && variedades.Count > 0) return true;

                var packings = db.GetPackingItems();
                if (packings != null && packings.Count > 0) return true;

                return false;
            }
            catch
            {
                return false;
            }
        }

        // OnShown override removed to avoid nested initialization issues during design-time.
        // Initialization is performed in Form1_Load which is invoked by the constructor or form events.

        private bool IsDesignMode()
        {
            return LicenseManager.UsageMode == LicenseUsageMode.Designtime || this.DesignMode;
        }

        // Código de búsqueda de código final: versiones antiguas eliminadas para evitar duplicados.


        /// <summary>
        /// Convierte una fecha agrícola en formato PTI (Produce Traceability Initiative)
        /// Transforma el formato de fecha interna (MMDD) a formato legible (MMM DD)
        /// Ejemplos: "0115" -> "Nov 15", "0220" -> "Dec 20"
        /// </summary>
        /// <returns>Fecha formateada en formato PTI (Mes abreviado + día)</returns>
        public string Busca_Fecha_PTI()
        {
            string text = this.txt_fecha_agricola.Text.Trim().Substring(0, 2);
            switch (text)
            {
                case "01":
                    return "Nov " + this.txt_fecha_agricola.Text.Trim().Substring(2, 2);
                case "02":
                    return "Dec " + this.txt_fecha_agricola.Text.Trim().Substring(2, 2);
                case "03":
                    return "Jan " + this.txt_fecha_agricola.Text.Trim().Substring(2, 2);
                case "04":
                    return "Feb " + this.txt_fecha_agricola.Text.Trim().Substring(2, 2);
                case "05":
                    return "Mar " + this.txt_fecha_agricola.Text.Trim().Substring(2, 2);
                case "06":
                    return "Apr " + this.txt_fecha_agricola.Text.Trim().Substring(2, 2);
                case "07":
                    return "May " + this.txt_fecha_agricola.Text.Trim().Substring(2, 2);
            }
            return "ERROR";
        }

        /// <summary>
        /// Convierte una fecha agrícola al formato YYMMDD estándar
        /// Transforma el formato interno (MMDD) agregando el año correspondiente
        /// Maneja la transición de temporada (Nov-Dic del año anterior, Ene-Mayo del año actual)
        /// Ejemplos: "0115" -> "251115", "0220" -> "251220", "0310" -> "260110"
        /// </summary>
        /// <returns>Fecha en formato YYMMDD (año, mes, día)</returns>
        public string Busca_Fecha_YYMMDD()
        {
            string text = this.txt_fecha_agricola.Text.Trim().Substring(0, 2);
            switch (text)
            {
                case "01":
                    return "2511" + this.txt_fecha_agricola.Text.Trim().Substring(2, 2);
                case "02":
                    return "2512" + this.txt_fecha_agricola.Text.Trim().Substring(2, 2);
                case "03":
                    return "2601" + this.txt_fecha_agricola.Text.Trim().Substring(2, 2);
                case "04":
                    return "2602" + this.txt_fecha_agricola.Text.Trim().Substring(2, 2);
                case "05":
                    return "2603" + this.txt_fecha_agricola.Text.Trim().Substring(2, 2);
                case "06":
                    return "2604" + this.txt_fecha_agricola.Text.Trim().Substring(2, 2);
                case "07":
                    return "2605" + this.txt_fecha_agricola.Text.Trim().Substring(2, 2);
            }
            return "ERROR";
        }

        /// <summary>
        /// Evento de validación de tecla presionada en el campo GTIN
        /// Solo permite la entrada de dígitos, teclas de control y separadores
        /// FORMATO GTIN: Solo acepta números (0-9)
        /// PROPÓSITO: Prevenir entrada inválida en tiempo real
        /// </summary>
        /// <param name="sender">Control que disparó el evento</param>
        /// <param name="e">Argumentos con la tecla presionada</param>
        private void txt_gtin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar))
            {
                // Permite dígitos (0-9)
                e.Handled = false;
            }
            else if (char.IsControl(e.KeyChar))
            {
                // Permite teclas de control (Backspace, Delete, etc.)
                e.Handled = false;
            }
            else if (char.IsSeparator(e.KeyChar))
            {
                // Permite separadores (espacio, tab, etc.)
                e.Handled = false;
            }
            else
            {
                // Bloquea cualquier otro carácter
                e.Handled = true;
            }
        }

        /// <summary>
        /// Evento de carga del formulario
        /// Inicializa los datos cargando la configuración desde XML y estableciendo valores predeterminados
        /// </summary>
        /// <param name="sender">Objeto que disparó el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private async void Form1_Load(object sender, EventArgs e)
        {
            if (IsDesignMode())
            {
                return;
            }
            // Carga la configuración desde el archivo XML (si aplica)
            this.LeerXML();

            // ==========================
            // SINCRONIZACIÓN (SOLO SI LA BD ESTÁ VACÍA)
            // Nota: normalmente la app sincroniza en frm_Sincronizacion antes de abrir PTI.
            // Aquí evitamos re-sincronizar (y borrar datos) cada vez que se abre el formulario.
            // ==========================
            try
            {
                if (!BaseDeDatosTieneDatos())
                {
                    System.Diagnostics.Debug.WriteLine("[FORM_LOAD] BD vacía, iniciando sincronización desde API...");
                    await SincronizarDatosDesdeAPI_Interno(mostrarMensaje: true);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("[FORM_LOAD] BD ya contiene datos, se omite sincronización automática.");
                }
            }
            catch
            {
                // si algo falla, seguimos y dejamos que los combos intenten cargar
            }

            // ==========================
            // CARGA DE COMBOS DESDE BD
            // ==========================
            try
            {
                System.Diagnostics.Debug.WriteLine("[FORM_LOAD] Iniciando carga de combos...");

                this.LlenaEmbalaje();
                System.Diagnostics.Debug.WriteLine($"[FORM_LOAD] Embalaje cargado: {this.cmb_tipo_embalaje.Items.Count} items");

                this.Llena_Productor();
                System.Diagnostics.Debug.WriteLine($"[FORM_LOAD] Productor cargado: {this.cmb_productor.Items.Count} items");

                this.Llena_Variedad();
                System.Diagnostics.Debug.WriteLine($"[FORM_LOAD] Variedad cargada: {this.cmb_variedad.Items.Count} items");

                this.Llena_lotes();
                System.Diagnostics.Debug.WriteLine($"[FORM_LOAD] Lotes cargados: {this.cmb_lote.Items.Count} items");

                this.Llena_variedad_imprime();
                System.Diagnostics.Debug.WriteLine($"[FORM_LOAD] Variedad Imprime cargada: {this.cmb_variedad_Imprime.Items.Count} items");

                this.Llena_GTIN();
                System.Diagnostics.Debug.WriteLine($"[FORM_LOAD] GTIN cargado: {this.cmb_gtin.Items.Count} items");

                this.Llena_Packing();
                System.Diagnostics.Debug.WriteLine($"[FORM_LOAD] Packing cargado: {this.cmb_packing.Items.Count} items");

                this.Llena_CategoriaSAG();
                System.Diagnostics.Debug.WriteLine($"[FORM_LOAD] CategoriaSAG cargada: {this.cmb_cat1.Items.Count} items");

                this.Llena_Calibre();
                System.Diagnostics.Debug.WriteLine($"[FORM_LOAD] Calibre cargado: {this.cmb_calibre.Items.Count} items");

                this.Llena_Peso();
                System.Diagnostics.Debug.WriteLine($"[FORM_LOAD] Peso cargado: {this.cmb_titulo2.Items.Count} items");

                this.Llena_Productos();
                this.Llena_Colores();
                System.Diagnostics.Debug.WriteLine($"[FORM_LOAD] Colores cargados: {this.cmb_titulo1.Items.Count} items");

                RefrescarFiltros();
                System.Diagnostics.Debug.WriteLine("[FORM_LOAD] Filtros refrescados");

                // DEBUG: Obtener colores desde la API y mostrar en consola
                // TestColoresDesdeAPI(); // Comentado - No mostrar mensajes de debug
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[FORM_LOAD ERROR] {ex.Message}\n{ex.StackTrace}");
                MessageBox.Show(this, $"Error al cargar los datos desde la base de datos: {ex.Message}\n\nAsegúrate de que la base de datos esté inicializada correctamente.", "Error de Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Limpia la imagen de vista previa
            this.pb_etiqueta.Image = null;

            // Establece valores predeterminados SOLO si hay elementos
            if (this.cmb_titulo2.Items.Count > 0) this.cmb_titulo2.SelectedIndex = 0;
            if (this.cmb_packing.Items.Count > 0) this.cmb_packing.SelectedIndex = 0;
            if (this.cmb_cat1.Items.Count > 0) this.cmb_cat1.SelectedIndex = 0;
        }


        /// <summary>
        /// Evento clic del botón Copiar
        /// Copia la imagen de la etiqueta generada al portapapeles de Windows
        /// </summary>
        /// <param name="sender">Botón que disparó el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void btn_copiar_Click(object sender, EventArgs e)
        {
            try
            {
                // Copia la imagen del PictureBox al portapapeles
                Clipboard.SetDataObject(this.pb_etiqueta.Image, true);
            }
            catch (Exception)
            {
                // Silenciosamente ignora errores de copia
            }
        }

        /// <summary>
        /// Evento clic del botón Imprimir
        /// Envía la etiqueta generada a una impresora Zebra específica
        /// Configura el tamaño de papel personalizado y sin márgenes
        /// </summary>
        /// <param name="sender">Botón que disparó el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void btn_imprimir_Click(object sender, EventArgs e)
        {
            try
            {
                // Crea el documento de impresión
                PrintDocument printDocument = new PrintDocument();
                // Asigna el manejador del evento de impresión
                printDocument.PrintPage += this.documentoAimprimir;
                // Especifica la impresora Zebra
                printDocument.PrinterSettings.PrinterName = "ZDesigner S4M-203dpi ZPL";
                // Configura márgenes en cero
                PageSettings pageSettings = new PageSettings();
                pageSettings.Margins = new Margins(0, 0, 0, 0);
                printDocument.DefaultPageSettings.Margins = pageSettings.Margins;
                // Define tamaño de papel personalizado (315x168 puntos)
                PaperSize paperSize = new PaperSize("Custom", 315, 168);
                printDocument.DefaultPageSettings.PaperSize = paperSize;
                // Ejecuta la impresión
                printDocument.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Impresion: " + ex.Message);
            }
        }

        /// <summary>
        /// Manejador del evento PrintPage - Renderiza la etiqueta en la página de impresión
        /// Dibuja la imagen de la etiqueta en las coordenadas (0,0) del área de impresión
        /// </summary>
        /// <param name="sender">Objeto PrintDocument que disparó el evento</param>
        /// <param name="e">Argumentos con el contexto gráfico de impresión</param>
        private void documentoAimprimir(object sender, PrintPageEventArgs e)
        {
            try
            {
                // Usa el contexto gráfico de la impresora
                using (Graphics graphics = e.Graphics)
                {
                    // Dibuja la imagen escalada a los márgenes de la página
                    graphics.DrawImage(this.pb_etiqueta.Image, e.MarginBounds);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Impresion:" + ex.Message);
            }
        }

        /// <summary>
        /// Evento clic del botón Guardar
        /// Abre un diálogo para guardar la imagen de la etiqueta generada como archivo JPEG
        /// </summary>
        /// <param name="sender">Botón que disparó el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void btn_guardar_Click(object sender, EventArgs e)
        {
            try
            {
                // Muestra el diálogo de guardar archivo
                if (this.sfd.ShowDialog(this) == DialogResult.OK)
                {
                    ImageFormat jpeg = ImageFormat.Jpeg;
                    try
                    {
                        // Guarda la imagen en el formato JPEG
                        this.pb_etiqueta.Image.Save(this.sfd.FileName, jpeg);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show(this, "Error al guardar la Imagen", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Llena el combo box de tipos de embalaje según el modo seleccionado
        /// Si el checkbox de peso fijo está marcado, carga tipos de embalaje para peso fijo (CL, PG)
        /// Si no, carga tipos de embalaje estándar (BP, BSU, BZU, PP, SL)
        /// </summary>
        private void LlenaEmbalaje()
        {
            System.Diagnostics.Debug.WriteLine($"\n[LlenaEmbalaje] ========== INICIO ==========");
            System.Diagnostics.Debug.WriteLine($"[LlenaEmbalaje] Peso fijo: {this.chb_pesofijo.Checked}");

            List<DB.Item> embalajes;

            // 1) Intento (compatibilidad): usar el método existente del DatabaseManager
            try
            {
                var db = DatabaseManager.Instance;
                embalajes = db.GetTipoEmbalajePorPesoFijo(this.chb_pesofijo.Checked);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[LlenaEmbalaje] ERROR llamando GetTipoEmbalajePorPesoFijo: {ex.Message}");
                embalajes = new List<DB.Item>();
            }

            // 2) Si viene vacío (o con textos vacíos), hacer consulta segura que usa DATO o DESCRIPCION
            if (embalajes == null)
                embalajes = new List<DB.Item>();

            bool todosVacios = embalajes.Count > 0;
            foreach (var it in embalajes)
            {
                if (it != null && !string.IsNullOrWhiteSpace(it.Dato))
                {
                    todosVacios = false;
                    break;
                }
            }

            if (embalajes.Count == 0 || todosVacios)
            {
                var listaSegura = GetTipoEmbalajeSeguro(this.chb_pesofijo.Checked);
                if (listaSegura.Count > 0)
                    embalajes = listaSegura;
            }

            // 3) Fallback final: todos los activos (para no dejar el combo vacío)
            if (embalajes.Count == 0)
                embalajes = GetTipoEmbalajeActivosSeguro();

            System.Diagnostics.Debug.WriteLine($"[LlenaEmbalaje] Items finales: {embalajes.Count}");

            if (embalajes.Count == 0)
            {
                System.Diagnostics.Debug.WriteLine("[LlenaEmbalaje] ⚠️  NO HAY DATOS EN BD (TipoEmbalaje)");
                System.Diagnostics.Debug.WriteLine("[LlenaEmbalaje] Revisa: sincronización API, endpoint tipo-embalaje/tipo_embalaje y tabla TipoEmbalaje.");
            }
            else
            {
                foreach (var e in embalajes)
                    System.Diagnostics.Debug.WriteLine($"[LlenaEmbalaje] ✓ Embalaje: '{e?.Dato}' (ID: {e?.Id})");
            }

            this.cmb_tipo_embalaje.DisplayMember = "Dato";
            this.cmb_tipo_embalaje.ValueMember = "Id";
            this.cmb_tipo_embalaje.DataSource = embalajes;
            this.cmb_tipo_embalaje.SelectedIndex = embalajes.Count > 0 ? 0 : -1;

            System.Diagnostics.Debug.WriteLine($"[LlenaEmbalaje] ========== FIN ==========\n");
        }

        /// <summary>
        /// Carga tipos de embalaje de forma "segura":
        /// - Soporta dato NULL/vacío usando descripcion como fallback
        /// - Filtra por peso_fijo con COALESCE
        /// </summary>
		private List<WindowsFormsApplication1.Data.Item> GetTipoEmbalajeSeguro(bool pesoFijo)
        {
            var result = new List<DB.Item>();

            try
            {
				using (var conn = DatabaseManager.Instance.GetConnection())
                {
                    conn.Open();
                    EnsureTipoEmbalajeTableExists(conn);

                    string query = @"
                SELECT id,
                       COALESCE(NULLIF(TRIM(dato),''), NULLIF(TRIM(descripcion),''), '') AS texto
                FROM TipoEmbalaje
                WHERE activo = 1
                  AND COALESCE(peso_fijo, 0) = @peso_fijo
                ORDER BY texto ASC";

                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@peso_fijo", pesoFijo ? 1 : 0);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                                string texto = reader.IsDBNull(1) ? "" : reader.GetString(1);
                                result.Add(new DB.Item(texto, id));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[GetTipoEmbalajeSeguro] ERROR: {ex.Message}\n{ex.StackTrace}");
            }

            return result;
        }

        /// <summary>
        /// Fallback: trae todos los embalajes activos (sin filtrar por peso_fijo),
        /// usando dato/descripcion como texto.
        /// </summary>
        private List<WindowsFormsApplication1.Data.Item> GetTipoEmbalajeActivosSeguro()
        {
            var result = new List<WindowsFormsApplication1.Data.Item>();

            try
            {
				using (var conn = DatabaseManager.Instance.GetConnection())
                {
                    conn.Open();
                    EnsureTipoEmbalajeTableExists(conn);

                    string query = @"
                SELECT id,
                       COALESCE(NULLIF(TRIM(dato),''), NULLIF(TRIM(descripcion),''), '') AS texto
                FROM TipoEmbalaje
                WHERE activo = 1
                ORDER BY texto ASC";

                    using (var cmd = new SQLiteCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                            string texto = reader.IsDBNull(1) ? "" : reader.GetString(1);
                            result.Add(new DB.Item(texto, id));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[GetTipoEmbalajeActivosSeguro] ERROR: {ex.Message}\n{ex.StackTrace}");
            }

            return result;
        }

        private void EnsureTipoEmbalajeTableExists(SQLiteConnection conn)
        {
            // Si no existe la tabla, la creamos para evitar fallos en primera ejecución
            const string createTable = @"
        CREATE TABLE IF NOT EXISTS TipoEmbalaje (
            id INTEGER PRIMARY KEY,
            dato TEXT,
            descripcion TEXT,
            activo INTEGER,
            peso_fijo INTEGER
        )";

            using (var cmd = new SQLiteCommand(createTable, conn))
            {
                cmd.ExecuteNonQuery();
            }
        }


        // Token: 0x06000118 RID: 280 RVA: 0x00014185 File Offset: 0x00012385
        private void cmb_titulo1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // cmb_titulo1 es ahora Color - al cambiar color, limpiar vista previa
            this.pb_etiqueta.Image = null;
        }

        private void cmb_gtin_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("\n[cmb_gtin_SelectedIndexChanged] INICIO");

            try
            {
                if (this.cmb_gtin.SelectedItem is DB.GTINItem gtin)
                {
                    _gtinSeleccionado = gtin;
                    System.Diagnostics.Debug.WriteLine($"[cmb_gtin_SelectedIndexChanged] ✓ GTIN capturado directo: {gtin.Gtin}, PLU: {gtin.Plu}, CodBolsa: {gtin.CodBolsa}");
                }
                else
                {
                    // Fallback: cuando el binding no entrega GTINItem, resolvemos por SelectedValue
                    int? gtinId = TryGetSelectedId(this.cmb_gtin);
                    if (gtinId.HasValue)
                    {
                        var gtinDb = DatabaseManager.Instance.GetGTINById(gtinId.Value);
                        if (gtinDb != null)
                        {
                            _gtinSeleccionado = gtinDb;
                            System.Diagnostics.Debug.WriteLine($"[cmb_gtin_SelectedIndexChanged] ✓ GTIN capturado por id={gtinId}: {gtinDb.Gtin}, PLU: {gtinDb.Plu}, CodBolsa: {gtinDb.CodBolsa}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[cmb_gtin_SelectedIndexChanged ERROR] {ex.Message}");
            }

            System.Diagnostics.Debug.WriteLine("[cmb_gtin_SelectedIndexChanged] FIN\n");
            this.pb_etiqueta.Image = null;
        }

        // Token: 0x0600011A RID: 282
        private void cmb_variedad_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Llena_lotes();
            this.Llena_variedad_imprime();
            RefrescarFiltros();
            this.pb_etiqueta.Image = null;
        }

        private void Llena_variedad_imprime()
        {
            // ==========================
            // HARDcode (DEJAR COMENTADO)
            // ==========================
            // List<frm_generador.Item> list = new List<frm_generador.Item>();
            // try
            // {
            //     if (this.cmb_variedad.Text.ToString() == "ALLISON")
            //     {
            //         list.Add(new frm_generador.Item("15 Sheegene 20 - Allison™", 1));
            //         list.Add(new frm_generador.Item("15 Sheegene 20", 1));
            //         list.Add(new frm_generador.Item("00 Red Seedless 'Unknown Variety'", 1));
            //     }
            //     ...
            //     this.cmb_variedad_Imprime.DisplayMember = "Name";
            //     this.cmb_variedad_Imprime.ValueMember = "Value";
            //     this.cmb_variedad_Imprime.DataSource = list;
            // }
            // catch (Exception)
            // {
            // }

            // ==========================
            // DESDE BD (NUEVO)
            // ==========================
            var db = DatabaseManager.Instance;

            // Intentamos tomar el Id desde el combo (si está enlazado a Item)
            int? variedadId = TryGetSelectedId(this.cmb_variedad);
            // Fallback: resolver por texto si el combo es string
            if (!variedadId.HasValue)
                variedadId = db.GetVariedadIdByDato(this.cmb_variedad.Text);

            // Nota: VariedadImprime NO debe filtrarse por peso_fijo -> siempre solicitar sin filtro de peso
            List<DB.Item> variedadesImprime = db.GetVariedadesImprimePorVariedadYPesoFijo(variedadId, /*pesoFijo*/ false);

            this.cmb_variedad_Imprime.DisplayMember = "Dato";
            this.cmb_variedad_Imprime.ValueMember = "Id";
            this.cmb_variedad_Imprime.DataSource = variedadesImprime;
            this.cmb_variedad_Imprime.SelectedIndex = variedadesImprime.Count > 0 ? 0 : -1;

            System.Diagnostics.Debug.WriteLine($"[Llena_variedad_imprime] variedad_id: {variedadId}, peso_fijo: false (no filtrar), items: {variedadesImprime.Count}");

            // Al cambiar la variedad imprime, refrescamos GTIN
            this.Llena_GTIN();
        }


        private void Llena_GTIN()
        {
            var db = DatabaseManager.Instance;

            // ✅ OJO: el filtro sale de Variedad Imprime (NO del combo GTIN)
            int? variedadImprimeId = TryGetSelectedId(this.cmb_variedad_Imprime);
            if (!variedadImprimeId.HasValue)
                variedadImprimeId = db.GetVariedadImprimeIdByDato(this.cmb_variedad_Imprime.Text);

            // Trae GTIN por (VariedadImprime + PesoFijo).
            // Si no hay relaciones en tabla intermedia, hace fallback a GTIN por peso_fijo.
            List<DB.GTINItem> gtins = db.GetGTINsPorVariedadImprimeYPesoFijo(variedadImprimeId, this.chb_pesofijo.Checked);

            System.Diagnostics.Debug.WriteLine($"[Llena_GTIN] variedad_imprime_id: {variedadImprimeId}, peso_fijo: {this.chb_pesofijo.Checked}, items: {gtins.Count}");
            if (gtins.Count == 0)
            {
                System.Diagnostics.Debug.WriteLine("[Llena_GTIN] ⚠️ No hay GTIN para los filtros actuales");
            }

            this.cmb_gtin.DisplayMember = "Gtin";
            this.cmb_gtin.ValueMember = "Id";
            this.cmb_gtin.DataSource = gtins;
            this.cmb_gtin.SelectedIndex = gtins.Count > 0 ? 0 : -1;

            // Mantener estado consistente para PLU/cod_bolsa en la generación
            _gtinSeleccionado = gtins.Count > 0 ? gtins[0] : null;
            if (_gtinSeleccionado != null)
            {
                System.Diagnostics.Debug.WriteLine($"[Llena_GTIN] ✓ Inicializado GTIN={_gtinSeleccionado.Gtin}, PLU={_gtinSeleccionado.Plu}, Bag={_gtinSeleccionado.CodBolsa}");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Validar que todos los campos necesarios estén seleccionados
            if (this.cmb_productor.SelectedItem == null ||
                this.cmb_variedad.SelectedItem == null ||
                this.cmb_lote.SelectedItem == null ||
                this.cmb_variedad_Imprime.SelectedItem == null ||
                this.cmb_gtin.SelectedItem == null ||
                this.cmb_tipo_embalaje.SelectedItem == null ||
                this.cmb_titulo2.SelectedItem == null ||
                this.cmb_packing.SelectedItem == null ||
                this.cmb_cat1.SelectedItem == null ||
                this.cmb_calibre.SelectedItem == null ||
                this.cmb_titulo1.SelectedItem == null ||
                string.IsNullOrWhiteSpace(this.txt_fecha_agricola.Text) ||
                string.IsNullOrWhiteSpace(this.txt_linea.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos requeridos antes de generar la etiqueta.", "Campos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ============================
            // PASO 3: Validar FechaAgrícola/Línea
            // ============================
            var fa = txt_fecha_agricola.Text.Trim();
            var li = txt_linea.Text.Trim();

            if (fa.Length != 4)
            {
                MessageBox.Show("Fecha Agrícola debe tener 4 dígitos (MMDD).");
                return;
            }
            if (li.Length < 2)
            {
                MessageBox.Show("Línea debe tener al menos 2 caracteres.");
                return;
            }

            // Generar la etiqueta
            try
            {
                this.pb_etiqueta.Image = this.GenerarEtiquetaPTI();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar la etiqueta: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ButtonSave_MouseLeave(object sender, EventArgs e)
        {
            // Implementar lógica si es necesario
        }

        private void ButtonSave_MouseEnter(object sender, EventArgs e)
        {
            // Implementar lógica si es necesario
        }


        // Handlers genéricos (están referenciados desde Designer)
        private void Button_MouseEnter(object sender, EventArgs e)
        {
            // Efecto hover opcional. Dejar vacío si no se usa.
        }

        private void Button_MouseLeave(object sender, EventArgs e)
        {
            // Efecto hover opcional. Dejar vacío si no se usa.
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Sin lógica
        }

        private void label8_Click(object sender, EventArgs e)
        {
            // Sin lógica
        }
        private void cmb_packing_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Actualiza el CSP (código packing) en base al valor seleccionado.
            // Importante: el combo puede venir con formato "Nombre .... 176864" (hardcode) o desde BD.
            ActualizarCspDesdePacking();
            this.pb_etiqueta.Image = null;
        }

        private void txt_fecha_agricola_TextChanged(object sender, EventArgs e)
        {
            // Implementar lógica si es necesario
        }

        private void txt_linea_TextChanged(object sender, EventArgs e)
        {
            // Implementar lógica si es necesario
        }

        private void cmb_tipo_embalaje_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Implementar lógica si es necesario
        }

        private void cmb_titulo2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Implementar lógica si es necesario
        }

        private void chb_pesofijo_CheckedChanged(object sender, EventArgs e)
        {
            // Al cambiar el modo (peso fijo / variable) se refrescan los combos dependientes
            this.LlenaEmbalaje();
            this.Llena_variedad_imprime();
            this.Llena_GTIN();
            this.RefrescarFiltros(); // Refrescar colores según peso_fijo
            this.pb_etiqueta.Image = null;
        }


        private void cmb_variedad_Imprime_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Llena_GTIN();
            this.pb_etiqueta.Image = null;
        }


        private void cmb_cat1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Implementar lógica si es necesario
        }

        private void cmb_lote_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefrescarFiltros();
            this.pb_etiqueta.Image = null;
        }

        private void cmb_productor_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefrescarFiltros();
            this.pb_etiqueta.Image = null;
        }

        private void cmb_sdp_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefrescarFiltros();
            this.pb_etiqueta.Image = null;
        }

        private void cmb_calibre_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Implementar lógica si es necesario
        }


        // =====================================================
        // Packing -> CSP (para imprimir en PTI)
        // =====================================================
        private long? _cspSeleccionado = null;
        public long? GetCspSeleccionado() => _cspSeleccionado;

        // =====================================================
        // GTIN Data (para vincular con la etiqueta)
        // =====================================================
        private DB.GTINItem _gtinSeleccionado = null;
        public DB.GTINItem GetGTINSeleccionado() => _gtinSeleccionado;

        /// <summary>
        /// Llena el combo de Packing. Si hay datos en SQLite, los usa.
        /// Si no hay datos o falla la consulta, deja el hardcode del Designer.
        /// </summary>
        private void Llena_Packing()
        {
            try
            {
                var db = DatabaseManager.Instance;
                var packings = db.GetPackingItems(); // List<DatabaseManager.Item> (Id, Dato)
                if (packings == null) packings = new List<DB.Item>();

                // Normalizar texto: quitar sufijo 'CSP:xxxxx' si está presente
                var cleaned = new List<DB.Item>();
                foreach (var p in packings)
                {
                    string dato = p?.Dato ?? "";
                    try
                    {
                        int idx = dato.IndexOf("CSP:", StringComparison.OrdinalIgnoreCase);
                        if (idx >= 0)
                            dato = dato.Substring(0, idx).Trim();
                    }
                    catch { }
                    cleaned.Add(new DB.Item(dato, p?.Id ?? 0));
                }

                // Siempre bindear para eliminar hardcode del Designer (aunque venga vacío)
                BindCombo(this.cmb_packing, cleaned);
            }
            catch
            {
                // Si falla la consulta, al menos limpiar el hardcode
                BindCombo(this.cmb_packing, new List<DB.Item>());
            }

            // Siempre intenta obtener el CSP desde lo seleccionado (BD o hardcode)
            ActualizarCspDesdePacking();
        }

        /// <summary>
        /// Actualiza el CSP seleccionado usando:
        /// 1) Si el combo viene desde BD: parsea CSP desde el texto si está pegado.
        /// 2) Si viene hardcode: toma el último token numérico.
        /// Nota: si ya separaste CSP en DB y lo traes como campo, lo ideal es mapearlo
        /// en el DataSource a un objeto que contenga Csp directamente.
        /// </summary>
        private void ActualizarCspDesdePacking()
        {
            try
            {
                int? packingId = TryGetSelectedId(this.cmb_packing);
                if (packingId.HasValue)
                {
                    var cspDb = DatabaseManager.Instance.GetCspByPackingId(packingId.Value);
                    if (cspDb.HasValue)
                    {
                        _cspSeleccionado = cspDb.Value;
                        return;
                    }
                }
            }
            catch
            {
                // seguimos con fallback textual
            }

            // Fallback (hardcode o texto libre)
            _cspSeleccionado = ExtraerCspDesdeTexto(this.cmb_packing.Text);
        }

        private long? ExtraerCspDesdeTexto(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            var parts = s.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) return null;

            // último token
            if (long.TryParse(parts[parts.Length - 1], NumberStyles.Integer, CultureInfo.InvariantCulture, out var csp))
                return csp;

            return null;
        }

        // =====================================================
        // SDP + Color (filtrados por Trazabilidad)
        // =====================================================
        /// <summary>
        /// Refresca SdP, Color y Producto según Productor/Variedad/Lote.
        /// Si no hay trazabilidad, hace fallback a listas completas activas.
        /// </summary>
        private void RefrescarFiltros()
        {
            try
            {
                var db = DatabaseManager.Instance;

                int? productorId = TryGetSelectedId(this.cmb_productor);
                int? variedadId = TryGetSelectedId(this.cmb_variedad);
                int? loteId = TryGetSelectedId(this.cmb_lote);
                int? variedadImprimeId = TryGetSelectedId(this.cmb_variedad_Imprime);

                // SdP filtrado por trazabilidad
                var sdps = db.GetSDPsPorSeleccion(productorId, variedadId, loteId, variedadImprimeId);
                if (sdps == null || sdps.Count == 0)
                    sdps = GetAllSdpsFromDb();
                BindCombo(this.cbx_sdp, sdps);
                this.cbx_sdp.Enabled = false;

                // Color filtrado por trazabilidad
                var colores = db.GetColoresPorSeleccion(productorId, variedadId, loteId, this.chb_pesofijo.Checked, variedadImprimeId);
                if (colores == null || colores.Count == 0)
                    colores = GetAllColoresFromDb();
                BindCombo(this.cmb_titulo1, colores);
                this.cmb_titulo1.Enabled = false;

                System.Diagnostics.Debug.WriteLine($"[RefrescarFiltros] SdP: {sdps?.Count ?? 0}, Colores: {colores?.Count ?? 0}");

                // Como el Designer no tiene evento para cbx_sdp, lo enganchamos aquí
                this.cbx_sdp.SelectedIndexChanged -= CbxSdpChanged_Data;
                this.cbx_sdp.SelectedIndexChanged += CbxSdpChanged_Data;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[RefrescarFiltros ERROR] {ex.Message}");
                // si falla, no cortamos el flujo del usuario
            }
        }

        // Fallback: traer todos los SdP/Colores desde tablas base (cuando Trazabilidad no tiene registros aún)
        private List<DB.Item> GetAllSdpsFromDb()
        {
            var result = new List<DB.Item>();
            try
            {
                using (var conn = DatabaseManager.Instance.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new System.Data.SQLite.SQLiteCommand("SELECT id, dato FROM SDP WHERE activo = 1 ORDER BY dato", conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new DB.Item(reader.GetString(1), reader.GetInt32(0)));
                        }
                    }
                }
            }
            catch
            {
                // si falla, devolvemos lista vacía
            }
            return result;
        }

        private List<DB.Item> GetAllColoresFromDb()
        {
            var result = new List<DB.Item>();
            try
            {
                using (var conn = DatabaseManager.Instance.GetConnection())
                {
                    conn.Open();
                    // Asegurar que la tabla existe
                    string createTable = @"CREATE TABLE IF NOT EXISTS Color (
				id INTEGER PRIMARY KEY,
				nombre TEXT,
				descripcion TEXT,
				activo INTEGER
			)";
                    using (var cmdCreate = new System.Data.SQLite.SQLiteCommand(createTable, conn))
                    {
                        cmdCreate.ExecuteNonQuery();
                    }
                    using (var cmd = new System.Data.SQLite.SQLiteCommand("SELECT id, nombre FROM Color WHERE activo = 1 ORDER BY nombre", conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new DB.Item(reader.GetString(1), reader.GetInt32(0)));
                        }
                    }
                }
            }
            catch
            {
                // si falla, devolvemos lista vacía
            }
            return result;
        }

        private void CbxSdpChanged_Data(object sender, EventArgs e)
        {
            RefrescarFiltros();
            this.pb_etiqueta.Image = null;
        }

        // =====================================================
        // Métodos que Form1_Load llama y faltaban
        // =====================================================
        private void Llena_CategoriaSAG()
        {
            var db = DatabaseManager.Instance;
            BindCombo(this.cmb_cat1, db.GetCategoriaSAGItems());
        }

        private void Llena_Calibre()
        {
            var db = DatabaseManager.Instance;
            BindCombo(this.cmb_calibre, db.GetCalibresItems());
        }

        private void Llena_Peso()
        {
            var db = DatabaseManager.Instance;
            BindCombo(this.cmb_titulo2, db.GetPesoItems());
        }

        /// <summary>
        ///
        /// </summary>
        private async Task<bool> SincronizarDatosDesdeAPI_Interno(bool mostrarMensaje)
        {
            if (_syncInProgress) return false;
            _syncInProgress = true;

            try
            {
                var httpClient = HttpSyncClient.Instance;
                var progress = new Progress<(int percentage, string message)>(report =>
                {
                    System.Diagnostics.Debug.WriteLine($"[SYNC] {report.percentage}% - {report.message}");
                });

                var (success, message) = await httpClient.SincronizarTodoAsync(progress);

                if (success)
                {
                    System.Diagnostics.Debug.WriteLine("[SYNC] ✓ Sincronización exitosa");

                    if (mostrarMensaje && !_syncMensajeMostrado)
                    {
                        _syncMensajeMostrado = true;
                        MessageBox.Show(
                            "Sincronización completada exitosamente.\n\nLos datos han sido actualizados.",
                            "Sincronización",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                    }

                    return true;
                }

                System.Diagnostics.Debug.WriteLine($"[SYNC] ✗ Error: {message}");
                MessageBox.Show($"Error en sincronización:\n{message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[SYNC ERROR] {ex.Message}");
                MessageBox.Show($"Error al sincronizar:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                _syncInProgress = false;
            }
        }

        public async void SincronizarDatosDesdeAPI()
        {
            // Mantener firma original por compatibilidad
            bool ok = await SincronizarDatosDesdeAPI_Interno(mostrarMensaje: true);
            if (ok)
            {
                // Recargar todos los combos después de sincronizar
                RecargarTodosCombos();
            }
        }

        private void RecargarTodosCombos()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[RELOAD] Recargando todos los combos...");

                this.LlenaEmbalaje();
                System.Diagnostics.Debug.WriteLine($"[RELOAD] Embalaje: {this.cmb_tipo_embalaje.Items.Count} items");

                this.Llena_Productor();
                System.Diagnostics.Debug.WriteLine($"[RELOAD] Productor: {this.cmb_productor.Items.Count} items");

                this.Llena_Variedad();
                System.Diagnostics.Debug.WriteLine($"[RELOAD] Variedad: {this.cmb_variedad.Items.Count} items");

                this.Llena_lotes();
                System.Diagnostics.Debug.WriteLine($"[RELOAD] Lotes: {this.cmb_lote.Items.Count} items");

                this.Llena_variedad_imprime();
                System.Diagnostics.Debug.WriteLine($"[RELOAD] Variedad Imprime: {this.cmb_variedad_Imprime.Items.Count} items");

                this.Llena_GTIN();
                System.Diagnostics.Debug.WriteLine($"[RELOAD] GTIN: {this.cmb_gtin.Items.Count} items");

                this.Llena_Packing();
                System.Diagnostics.Debug.WriteLine($"[RELOAD] Packing: {this.cmb_packing.Items.Count} items");

                this.Llena_CategoriaSAG();
                System.Diagnostics.Debug.WriteLine($"[RELOAD] CategoriaSAG: {this.cmb_cat1.Items.Count} items");

                this.Llena_Calibre();
                System.Diagnostics.Debug.WriteLine($"[RELOAD] Calibre: {this.cmb_calibre.Items.Count} items");

                this.Llena_Peso();
                System.Diagnostics.Debug.WriteLine($"[RELOAD] Peso: {this.cmb_titulo2.Items.Count} items");

                this.Llena_Colores();
                System.Diagnostics.Debug.WriteLine($"[RELOAD] Colores: {this.cmb_titulo1.Items.Count} items");

                RefrescarFiltros();
                System.Diagnostics.Debug.WriteLine("[RELOAD] ✓ Recarga completada");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[RELOAD ERROR] {ex.Message}");
            }
        }

        private void Llena_Productos()
        {
            // Productos ahora se cargan desde RefrescarFiltros()
            // Esta función se mantiene para compatibilidad pero cmb_titulo1 ahora usa colores
        }

        private void Llena_Colores()
        {
            try
            {
                var db = DatabaseManager.Instance;
                var colores = db.GetColoresItems();
                System.Diagnostics.Debug.WriteLine($"[Llena_Colores] Colores cargados desde BD: {colores?.Count ?? 0}");

                if (colores == null || colores.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("[Llena_Colores] No hay colores en BD, intentando desde fallback...");
                    colores = GetAllColoresFromDb();
                    System.Diagnostics.Debug.WriteLine($"[Llena_Colores] Colores desde fallback: {colores?.Count ?? 0}");
                }

                // cmb_titulo1 es ahora la lista de colores, mantener cmb_color vacío o usar para otros propósitos
                BindCombo(this.cmb_titulo1, colores);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ERROR Llena_Colores] {ex.Message}");
                MessageBox.Show($"Error al cargar colores: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Llena_lotes()
        {
            var db = DatabaseManager.Instance;
            int? variedadId = TryGetSelectedId(this.cmb_variedad);

            var lotes = variedadId.HasValue
                ? db.GetLotePorVariedad(variedadId.Value)
                : db.GetLotesItems();

            BindCombo(this.cmb_lote, lotes);
        }

        // =====================================================
        // Lectura XML (si aún aplica)
        // =====================================================
        private void LeerXML()
        {
            // Si aún usas XML para otras configuraciones, déjalo aquí.
            // Por ahora lo mantenemos vacío porque la mayoría de combos vienen desde BD.
        }

        // ==========================
        // Helpers de combos (BD)
        // ==========================
        private int? TryGetSelectedId(ComboBox cmb)
        {
            try
            {
                if (cmb == null) return null;
                if (cmb.SelectedValue is int id) return id;
                if (cmb.SelectedValue != null && int.TryParse(cmb.SelectedValue.ToString(), out var idParsed)) return idParsed;
                if (cmb.SelectedItem is DB.Item item) return item.Id;
                return null;
            }
            catch
            {
                return null;
            }
        }

        private void BindCombo(ComboBox cmb, List<DB.Item> items)
        {
            if (cmb == null) return;
            cmb.DataSource = null; // Forzar reset del DataSource
            cmb.Items.Clear(); // Limpiar items previos (por si hay hardcode en Designer)
            cmb.DisplayMember = "Dato";
            cmb.ValueMember = "Id";
            cmb.DataSource = items;
            cmb.SelectedIndex = items != null && items.Count > 0 ? 0 : -1;
        }

        private void Llena_Productor()
        {
            var db = DatabaseManager.Instance;
            var productores = db.GetProductoresItems();
            BindCombo(this.cmb_productor, productores);
        }

        private void Llena_Variedad()
        {
            var db = DatabaseManager.Instance;
            var variedades = db.GetVariedadesItems();
            BindCombo(this.cmb_variedad, variedades);
        }

        /// <summary>
        /// Genera la imagen de la etiqueta PTI 80x65 usando los datos seleccionados
        /// </summary>
        /// <returns>Bitmap con la etiqueta generada</returns>
        private Bitmap GenerarEtiquetaPTI()
        {
            // =========================
            // 0) Validaciones mínimas
            // =========================
            string fechaAgr = this.txt_fecha_agricola.Text.Trim();
            string linea = this.txt_linea.Text.Trim();
            if (fechaAgr.Length != 4) throw new Exception("Fecha Agricola inválida (debe ser MMDD, 4 dígitos).");
            if (linea.Length < 2) throw new Exception("Linea inválida (mínimo 2 caracteres).");

            // =========================
            // 1) Datos GTIN
            // =========================
            string gtin = "";
            string plu = "";
            string codBolsa = "";

            if (_gtinSeleccionado != null)
            {
                gtin = _gtinSeleccionado.Gtin ?? "";
                plu = _gtinSeleccionado.Plu ?? "";
                codBolsa = _gtinSeleccionado.CodBolsa ?? "";
                System.Diagnostics.Debug.WriteLine($"[GenerarEtiquetaPTI] Usando _gtinSeleccionado: GTIN={gtin}, PLU={plu}, CodBolsa={codBolsa}");
            }
            else if (this.cmb_gtin.SelectedItem is DB.GTINItem gtinItem)
            {
                gtin = gtinItem.Gtin ?? "";
                plu = gtinItem.Plu ?? "";
                codBolsa = gtinItem.CodBolsa ?? "";
                System.Diagnostics.Debug.WriteLine($"[GenerarEtiquetaPTI] Usando SelectedItem: GTIN={gtin}, PLU={plu}, CodBolsa={codBolsa}");
            }
            else
            {
                gtin = this.cmb_gtin.Text.Trim();
                System.Diagnostics.Debug.WriteLine($"[GenerarEtiquetaPTI] Usando cmb_gtin.Text: GTIN={gtin}");
            }

            if (string.IsNullOrWhiteSpace(gtin))
                throw new Exception("GTIN no seleccionado. Por favor selecciona un GTIN válido.");

            // =========================
// 2) Datos base desde UI
// =========================
string fechaPTI = Busca_Fecha_PTI();        // Ej: "Feb 06"
string fechaYYMMDD = Busca_Fecha_YYMMDD();  // Ej: "260206"
string loteTxt = this.cmb_lote.Text.Trim();

string productorDato = this.cmb_productor.Text.Trim();
string codProductor6 = ExtraerCodigoInicial(productorDato, 6);
int productorIdForCsg = TryGetSelectedId(this.cmb_productor) ?? 0;
string csg = DatabaseManager.Instance.GetProductorCsgById(productorIdForCsg);
if (string.IsNullOrWhiteSpace(csg))
    csg = codProductor6;

// =========================
// VARIEDAD IMPRIME - USAR var_interno
// =========================
string variedadImpDato = this.cmb_variedad_Imprime.Text.Trim();
int variedadImpId = TryGetSelectedId(this.cmb_variedad_Imprime) ?? 0;

// Obtener var_interno desde SQLite (ej: "13")
string codVarImp2 = DatabaseManager.Instance
                        .GetVariedadImprimeVarInternoById(variedadImpId);

// Respaldo por seguridad (si algo falla)
if (string.IsNullOrWhiteSpace(codVarImp2))
    codVarImp2 = ExtraerCodigoInicial(variedadImpDato, 2);

// Construcción correcta del lote GS1
string lot = $"{csg}{codVarImp2}{loteTxt}";

// =========================
// 3) Voice Pick Code
// =========================
string voice;
DateTime fechaVoice;
bool tieneFechaVoice = DateTime.TryParse(this.txt_date.Text.Trim(), out fechaVoice);

if (tieneFechaVoice)
    voice = frm_generadorVentana.VoiceCodeconFecha.Compute(gtin, lot, new DateTime?(fechaVoice));
else
    voice = frm_generadorVentana.VoiceCodesinFecha.Compute(gtin, lot, new DateTime?(new DateTime(2001, 1, 1)));

voice = (voice ?? "0000").Trim();
if (voice.Length < 4) voice = voice.PadLeft(4, '0');
if (voice.Length > 4) voice = voice.Substring(voice.Length - 4, 4);

this.lbl_codigo1.Text = voice.Substring(0, 2);
this.lbl_codigo2.Text = voice.Substring(2, 2);

// =========================
// 4) GS1 Code128 principal
// =========================
string gs1 = "01" + gtin +
             "13" + fechaYYMMDD +
             "10" + lot;

            // =========================
            // 5) PLU Bar Data + PLU #
            // =========================
            string pluBarData = DerivarPluBarDataDesdeGtin(gtin);
            if (pluBarData == "ERROR" && !string.IsNullOrWhiteSpace(plu))
                pluBarData = DerivarPluBarDataDesdeGtin(plu);

            if (string.IsNullOrWhiteSpace(pluBarData) || pluBarData == "ERROR")
                pluBarData = "0000000000000";

            string pluNumber = !string.IsNullOrWhiteSpace(plu) ? plu : DerivarPluNumberDesdePluBarData(pluBarData);
            if (string.IsNullOrWhiteSpace(pluNumber)) pluNumber = "----";

            // =========================
            // 6) Datos que se imprimen
            // =========================
            string especieColor = this.cmb_titulo1.Text.Trim();
            string pesoNeto = this.cmb_titulo2.Text.Trim();
            string calibre = this.cmb_calibre.Text.Trim();
            string tipoEmb = this.cmb_tipo_embalaje.Text.Trim();
            string cat = "Cat 1 / US #1";
            string sdp = this.cbx_sdp.Text.Trim();

            int packingIdForCsp = TryGetSelectedId(this.cmb_packing) ?? 0;
            long? csp = DatabaseManager.Instance.GetCspByPackingId(packingIdForCsp) ?? GetCspSeleccionado();

            // =========================
            // 7) Ubicación desde SQLite
            // =========================
            int packingId = packingIdForCsp;
            var loc = ObtenerUbicacionDesdeSQLite(packingId);

            string regionPacking = string.IsNullOrWhiteSpace(loc.region) ? "Atacama" : loc.region;
            string provinciaPacking = string.IsNullOrWhiteSpace(loc.provincia) ? "Copiapó" : loc.provincia;
            string comunaPacking = string.IsNullOrWhiteSpace(loc.comuna) ? "Tierra Amarilla" : loc.comuna;

            string ggn = string.IsNullOrWhiteSpace(loc.ggn) ? "GGN: 4049928844026" : "GGN: " + loc.ggn;
            string gln = string.IsNullOrWhiteSpace(loc.gln) ? "GLN: 7808771800002" : "GLN: " + loc.gln;

            // =========================
            // 8) Crear barcodes
            // =========================
            var bcGs1 = new BarcodeGenerator();
            var bcPlu = new BarcodeGenerator_PLU();

            using (Bitmap tmp1 = new Bitmap(1, 1))
            using (Bitmap tmp2 = new Bitmap(1, 1))
            using (Graphics gTmp = Graphics.FromImage(tmp1))
            using (Graphics gTmp2 = Graphics.FromImage(tmp2))
            using (Image imgGs1 = bcGs1.DrawCode128(gTmp, gs1, 0, 0))
            using (Image imgPlu = bcPlu.DrawCode128(gTmp2, pluBarData, 0, 0))
            {
                // =========================
                // 9) Bitmap final PTI 80x65 (203 dpi => 640x520)
                // =========================
                int W = 640;
                int H = 520;

                Bitmap bmp = new Bitmap(W, H, PixelFormat.Format32bppArgb);
                bmp.SetResolution(203, 203);

                // Fondo + líneas
                for (int x = 0; x < bmp.Width; x++)
                {
                    for (int y = 0; y < bmp.Height; y++)
                    {
                        // cuadro negro voice pick
                        if (x > 480 && x < 621 && y > 392 && y < 457) bmp.SetPixel(x, y, Color.Black);
                        else bmp.SetPixel(x, y, Color.White);

                        // caja pack date
                        if (x > 480 && x < 621 && y > 387 && y < 390) bmp.SetPixel(x, y, Color.Black);
                        if (x > 480 && x < 621 && y > 357 && y < 360) bmp.SetPixel(x, y, Color.Black);
                        if (x > 480 && x < 483 && y > 357 && y < 390) bmp.SetPixel(x, y, Color.Black);
                        if (x > 618 && x < 621 && y > 357 && y < 390) bmp.SetPixel(x, y, Color.Black);

                        // división central vertical
                        if (x > 295 && x < 298 && y > 273 && y < 463) bmp.SetPixel(x, y, Color.Black);

                        // tabla: verticales
                        if (x > 20 && x < 23 && y > 193 && y < 276) bmp.SetPixel(x, y, Color.Black);
                        if (x > 105 && x < 108 && y > 193 && y < 276) bmp.SetPixel(x, y, Color.Black);
                        if (x > 460 && x < 463 && y > 193 && y < 276) bmp.SetPixel(x, y, Color.Black);
                        if (x > 618 && x < 621 && y > 193 && y < 276) bmp.SetPixel(x, y, Color.Black);

                        // tabla: horizontales
                        if (x > 20 && x < 620 && y > 220 && y < 223) bmp.SetPixel(x, y, Color.Black);
                        if (x > 20 && x < 620 && y > 247 && y < 250) bmp.SetPixel(x, y, Color.Black);
                        if (x > 20 && x < 620 && y > 193 && y < 196) bmp.SetPixel(x, y, Color.Black);
                        if (x > 20 && x < 620 && y > 273 && y < 276) bmp.SetPixel(x, y, Color.Black);

                        // línea superior footer
                        if (x > 20 && x < 620 && y > 460 && y < 463) bmp.SetPixel(x, y, Color.Black);
                    }
                }

                using (Graphics gr = Graphics.FromImage(bmp))
                {
                    gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                    gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                    gr.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                    gr.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

                    // ===== BLOQUE SUPERIOR =====
                    var gs1Rect = new Rectangle(78, 8, 484, 55); // más ancho como referencia
                    gr.DrawImage(imgGs1, gs1Rect);

                    DrawText(gr, "(01)" + gtin + "(13)" + fechaYYMMDD + "(10)" + lot, 18, FontStyle.Bold, Color.Black, 320, 72, center: true);

                    // TITULOS: más gruesos y auto-ajuste de ancho
                    // Evitar quitar los primeros 3 chars incondicionalmente (causa que falten palabras).
                    // Si la variedad viene con un código numérico inicial (ej. "15 Sheegene 20 - Allison™")
                    // eliminamos sólo ese prefijo numérico y separadores. En otros casos, conservamos el texto completo.
                    string nombreVarImp = Regex.Replace(variedadImpDato ?? string.Empty, "^\\s*\\d+\\s*[-_.:]?\\s*", "");
                    nombreVarImp = nombreVarImp.Replace("_", " ");

                    DrawTextFitCentered(gr, especieColor, 320f, 90f, 600f, 34f, 24f, FontStyle.Bold, Color.Black, "Arial Black");
                    DrawTextFitCentered(gr, nombreVarImp, 320f, 126f, 560f, 32f, 22f, FontStyle.Bold, Color.Black, "Arial Black");
                    DrawTextFitCentered(gr, "Net weight when packed: " + pesoNeto, 320f, 164f, 560f, 24f, 17f, FontStyle.Regular, Color.Black, "Arial");

                    // ===== TABLA =====
                    DrawText(gr, "Grower", 20, FontStyle.Regular, Color.Black, 25, 199);
                    DrawText(gr, "Packing", 20, FontStyle.Regular, Color.Black, 25, 226);
                    DrawText(gr, "SdP", 20, FontStyle.Regular, Color.Black, 25, 253);

                    string locationText = $"{regionPacking}, {provinciaPacking}, {comunaPacking}";
                    DrawText(gr, locationText, 20, FontStyle.Bold, Color.Black, 282, 199, center: true);
                    DrawText(gr, locationText, 20, FontStyle.Bold, Color.Black, 282, 226, center: true);
                    DrawText(gr, locationText, 20, FontStyle.Bold, Color.Black, 282, 253, center: true);

                    DrawText(gr, "CSG:" + csg, 22, FontStyle.Bold, Color.Black, 539, 196, center: true);
                    DrawText(gr, "CSP:" + (csp?.ToString() ?? ""), 22, FontStyle.Bold, Color.Black, 539, 223, center: true);
                    DrawText(gr, "SdP: " + sdp, 22, FontStyle.Bold, Color.Black, 539, 250, center: true);

                    // ===== ZONA CENTRAL =====
                    DrawText(gr, "Produce of Chile", 30, FontStyle.Bold, Color.Black, 20, 282);
                    DrawText(gr, "Size:" + calibre + " / " + tipoEmb, 26, FontStyle.Bold, Color.Black, 20, 318);

                    DrawText(gr, $"{fechaAgr} / {loteTxt}{linea}", 28, FontStyle.Bold, Color.Black, 618, 278, far: true);

                    string catToPrint = string.IsNullOrWhiteSpace(cat) ? "Cat 1 / US #1" : cat;
                    DrawText(gr, catToPrint, 20, FontStyle.Bold, Color.Black, 610, 314, far: true);
                    DrawText(gr, "Pack Date", 16, FontStyle.Bold, Color.Black, 550, 335, center: true);
                    DrawText(gr, fechaPTI, 18, FontStyle.Bold, Color.Black, 550, 360, center: true);

                    // ===== ZONA IZQUIERDA BAJA =====
                    DrawText(gr, ggn, 22, FontStyle.Regular, Color.Black, 20, 352);
                    DrawText(gr, gln, 22, FontStyle.Regular, Color.Black, 20, 380);

                    DrawText(gr, "TREATED WITH SULFUR", 13, FontStyle.Bold, Color.Black, 20, 404);
                    DrawText(gr, "DIOXIDE FOR FUNGICIDE USE", 13, FontStyle.Bold, Color.Black, 20, 421);
                    DrawText(gr, "www.atacamagrapes.cl", 20, FontStyle.Regular, Color.Black, 20, 435);

                    // ===== ZONA PLU =====
                    int pluCenterX = 382;
                    int pluBarY = 375;
                    int pluBarWidth = 120;
                    int pluBarHeight = 52;
                    int pluTitleY = pluBarY - 24;
                    int bagCodeY = pluBarY + pluBarHeight + 6;

                    Rectangle pluRect = new Rectangle(pluCenterX - (pluBarWidth / 2), pluBarY, pluBarWidth, pluBarHeight);

                    DrawText(gr, "PLU #" + pluNumber, 22, FontStyle.Bold, Color.Black, pluCenterX, pluTitleY, center: true);

                    Rectangle srcPluRect = new Rectangle(0, 0, imgPlu.Width, imgPlu.Height);
                    if (imgPlu.Height > 40)
                    {
                        int cropTop = imgPlu.Height / 5;
                        srcPluRect = new Rectangle(0, cropTop, imgPlu.Width, imgPlu.Height - cropTop);
                    }
                    gr.DrawImage(imgPlu, pluRect, srcPluRect, GraphicsUnit.Pixel);

                    string bagCodeToPrint = !string.IsNullOrWhiteSpace(codBolsa) ? codBolsa : pluBarData;
                    if (!string.IsNullOrWhiteSpace(bagCodeToPrint) && bagCodeToPrint != "ERROR")
                        DrawText(gr, bagCodeToPrint, 19, FontStyle.Regular, Color.Black, pluCenterX, bagCodeY, center: true);

                    // ===== VOICE PICK =====
                    string vp1 = (this.lbl_codigo1.Text ?? "00").Trim();
                    string vp2 = (this.lbl_codigo2.Text ?? "00").Trim();
                    vp1 = vp1.PadLeft(2, '0');
                    vp2 = vp2.PadLeft(2, '0');

                    // Parecido a ref: izquierda más chico, derecha más grande
                    DrawText(gr, vp1, 34, FontStyle.Bold, Color.White, 523, 402, center: true);
                    DrawText(gr, vp2, 58, FontStyle.Bold, Color.White, 578, 394, center: true);

                    // ===== FOOTER INFERIOR (tamaño ajustado) =====
                    DrawText(gr, "EXPORTED BY FRUTICOLA Y EXPORTADORA ATACAMA LIMITADA", 17, FontStyle.Bold, Color.Black, 320, 468, center: true);
                    DrawText(gr, "Santiago  -  Chile  -  Phone:(56-2)25696000  -  Email:atc@atacamagrapes.cl", 13, FontStyle.Bold, Color.Black, 320, 488, center: true);
                }

                return bmp;
            }
        }

        /// <summary>
        /// Dibuja texto centrado con ajuste automático de tamaño para que respete un ancho máximo.
        /// Útil para títulos largos/cortos manteniendo “mismo ancho visual”.
        /// </summary>
        private void DrawTextFitCentered(
            Graphics gr,
            string text,
            float centerX,
            float y,
            float maxWidth,
            float maxSizePx,
            float minSizePx,
            FontStyle style,
            Color color,
            string fontName)
        {
            if (string.IsNullOrWhiteSpace(text)) return;

            // fallback por si Arial Black no existe en el equipo
            string family = string.IsNullOrWhiteSpace(fontName) ? "Arial" : fontName;

            using (var brush = new SolidBrush(color))
            using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Near })
            {
                float size = maxSizePx;

                while (size >= minSizePx)
                {
                    using (var f = new Font(family, size, style, GraphicsUnit.Pixel))
                    {
                        SizeF s = gr.MeasureString(text, f, 2000, StringFormat.GenericTypographic);
                        if (s.Width <= maxWidth)
                        {
                            gr.DrawString(text, f, brush, new PointF(centerX, y), sf);
                            return;
                        }
                    }
                    size -= 1f;
                }

                using (var f = new Font(family, minSizePx, style, GraphicsUnit.Pixel))
                {
                    gr.DrawString(text, f, brush, new PointF(centerX, y), sf);
                }
            }
        }


        private (string region, string provincia, string comuna, string gln, string ggn) ObtenerUbicacionDesdeSQLite(int packingId)
        {
            try
            {
                // Primero intenta usar el helper centralizado en DatabaseManager (lee Packing + Ubicacion_Detalle)
                try
                {
                    var pack = DatabaseManager.Instance.GetPackingById(packingId);
                    if (pack != null && pack.Ubicacion_Detalle != null)
                    {
                        var u = pack.Ubicacion_Detalle;
                        return (
                            u.Region ?? "",
                            u.Provincia ?? "",
                            u.Comuna ?? "",
                            u.Gln ?? "",
                            u.Ggn ?? ""
                        );
                    }
                }
                catch { /* seguir a la consulta directa si falla */ }

                using (var conn = DatabaseManager.Instance.GetConnection())
                {
                    conn.Open();

                    // Intentos de JOIN con distintos nombres de columna (compatibilidad)
                    var tried = new List<string>
                    {
                        // preferimos ubicacion_id (usado por el UI)
                        "SELECT u.region, u.provincia, u.comuna, u.gln, u.ggn FROM Packing p LEFT JOIN Ubicacion u ON u.id = p.ubicacion_id WHERE p.id = @pid LIMIT 1;",
                        // fallback a 'ubicacion'
                        "SELECT u.region, u.provincia, u.comuna, u.gln, u.ggn FROM Packing p LEFT JOIN Ubicacion u ON u.id = p.ubicacion WHERE p.id = @pid LIMIT 1;",
                        // fallback: si no hay columna, leer ubicacion desde Packing directamente (número), luego buscar Ubicacion por id
                        // esto lo hacemos manualmente abajo
                    };

                    foreach (var sql in tried)
                    {
                        try
                        {
                            using (var cmd = new System.Data.SQLite.SQLiteCommand(sql, conn))
                            {
                                cmd.Parameters.AddWithValue("@pid", packingId);
                                using (var r = cmd.ExecuteReader())
                                {
                                    if (!r.Read()) continue;
                                    return (
                                        r.IsDBNull(0) ? "" : r.GetString(0),
                                        r.IsDBNull(1) ? "" : r.GetString(1),
                                        r.IsDBNull(2) ? "" : r.GetString(2),
                                        r.IsDBNull(3) ? "" : r.GetString(3),
                                        r.IsDBNull(4) ? "" : r.GetString(4)
                                    );
                                }
                            }
                        }
                        catch
                        {
                            // intentar siguiente variante
                            continue;
                        }
                    }

                    // Último recurso: leer la columna 'ubicacion' desde Packing y consultar Ubicacion
                    try
                    {
                        using (var cmd = new System.Data.SQLite.SQLiteCommand("SELECT ubicacion FROM Packing WHERE id = @pid LIMIT 1", conn))
                        {
                            cmd.Parameters.AddWithValue("@pid", packingId);
                            var o = cmd.ExecuteScalar();
                            if (o != null && o != DBNull.Value && int.TryParse(o.ToString(), out var uid))
                            {
                                using (var cmd2 = new System.Data.SQLite.SQLiteCommand("SELECT region, provincia, comuna, gln, ggn FROM Ubicacion WHERE id = @uid LIMIT 1", conn))
                                {
                                    cmd2.Parameters.AddWithValue("@uid", uid);
                                    using (var r2 = cmd2.ExecuteReader())
                                    {
                                        if (r2.Read())
                                        {
                                            return (
                                                r2.IsDBNull(0) ? "" : r2.GetString(0),
                                                r2.IsDBNull(1) ? "" : r2.GetString(1),
                                                r2.IsDBNull(2) ? "" : r2.GetString(2),
                                                r2.IsDBNull(3) ? "" : r2.GetString(3),
                                                r2.IsDBNull(4) ? "" : r2.GetString(4)
                                            );
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch { }
                }
            }
            catch { }

            return ("", "", "", "", "");
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Método de DEBUG: Prueba obtener colores desde la API y los muestra en consola
        /// </summary>
        private async void TestColoresDesdeAPI()
        {
            try
            {
                var httpClient = HttpSyncClient.Instance;
                var colores = await httpClient.GetColoresDesdeAPI();

                System.Diagnostics.Debug.WriteLine("=== COLORES DESDE API ===");
                System.Diagnostics.Debug.WriteLine($"Total de colores: {colores.Count}");

                foreach (var color in colores)
                {
                    System.Diagnostics.Debug.WriteLine($"  ID: {color.Id}, Nombre: {color.Nombre}, Activo: {color.Activo}");
                }

                System.Diagnostics.Debug.WriteLine("========================");

                // Si hay colores, mostrar en MessageBox también
                if (colores.Count > 0)
                {
                    var colorStrings = new List<string>();
                    foreach (var c in colores)
                    {
                        colorStrings.Add($"- {c.Nombre} (ID: {c.Id})");
                    }
                    string colorList = string.Join("\n", colorStrings);
                    MessageBox.Show($"Se encontraron {colores.Count} colores en la API:\n\n{colorList}", "Colores desde API", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No se encontraron colores en la API o la API no está disponible.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en TestColoresDesdeAPI: {ex.Message}");
                MessageBox.Show($"Error al obtener colores desde API:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Extrae los primeros N caracteres de un texto, descartando separadores
        /// Ejemplo: "ABC_001" con length=3 devuelve "ABC"
        /// </summary>
        private string ExtraerCodigoInicial(string texto, int length)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return "";

            int count = 0;
            foreach (char c in texto)
            {
                if (c != '_' && c != ' ' && c != '-')
                {
                    count++;
                    if (count == length)
                        return texto.Substring(0, texto.IndexOf(c) + 1);
                }
            }
            return texto.Substring(0, Math.Min(length, texto.Length));
        }

        /// <summary>
        /// Derivar PLU Bar Data desde GTIN
        /// Si es GTIN-13, extrae los 10 dígitos medios (posición 2-12)
        /// Si es GTIN-12 (PLU), lo usa directamente
        /// </summary>
        private string DerivarPluBarDataDesdeGtin(string gtin)
        {
            if (string.IsNullOrWhiteSpace(gtin))
                return "ERROR";

            gtin = gtin.Trim();

            if (gtin.Length == 13)
            {
                // GTIN-13: tomar posiciones 2-12 (10 dígitos)
                return gtin.Substring(2, 11);
            }
            else if (gtin.Length == 12)
            {
                // GTIN-12 o PLU: usar como está
                return gtin;
            }

            return "ERROR";
        }

        /// <summary>
        /// Derivar PLU Number desde PLU Bar Data
        /// Si es de 13 dígitos, tomar los últimos 5
        /// Si es de otro formato, devolver como está
        /// </summary>
        private string DerivarPluNumberDesdePluBarData(string pluBarData)
        {
            if (string.IsNullOrWhiteSpace(pluBarData) || pluBarData == "ERROR")
                return "";

            if (pluBarData.Length >= 5)
            {
                return pluBarData.Substring(pluBarData.Length - 5);
            }

            return pluBarData;
        }

        /// <summary>
        /// Dibuja texto en un Graphics object con posicionamiento simple
        /// </summary>
        private void DrawText(Graphics gr, string text, float fontSize, FontStyle style, Color color, float x, float y, bool center = false, bool far = false)
        {
            if (string.IsNullOrWhiteSpace(text)) return;

            using (var brush = new SolidBrush(color))
            using (var font = new Font("Arial", fontSize, style, GraphicsUnit.Pixel))
            {
                SizeF size = gr.MeasureString(text, font);
                if (center)
                {
                    x = x - (size.Width / 2);
                }
                else if (far)
                {
                    x = x - size.Width;
                }
                gr.DrawString(text, font, brush, new PointF(x, y));
            }
        }
    }
}
