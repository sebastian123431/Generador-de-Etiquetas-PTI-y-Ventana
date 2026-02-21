using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LabelKit_8045;
using LabelKit_PLU_8045;
using WindowsFormsApplication1.Data;
using Item = WindowsFormsApplication1.Data.Item;

namespace WindowsFormsApplication1
{
	/// <summary>
	/// Formulario generador de etiquetas tipo VENTANA
	/// Genera etiquetas de precio y descripción para exhibición en ventanas de tienda
	/// Utiliza formato 8045 para códigos de barras
	/// </summary>
	public partial class frm_generadorVentana : Form
	{
		/// <summary>
		/// Constructor del formulario generador de etiquetas de ventana
		/// Inicializa todos los componentes visuales del formulario
		/// </summary>
		public frm_generadorVentana()
		{
			this.InitializeComponent();
		}

// Extrae el nombre del packing, eliminando prefijos numéricos e identificadores CSP al final.
private static string ExtractPackingName(string s)
{
	if (string.IsNullOrWhiteSpace(s)) return "";
	var t = s.Trim();
	// si el texto contiene la marca "CSP:" o un token numérico al final, los eliminamos
	// quitar sufijo CSP si existe
	int idx = t.IndexOf("CSP:", StringComparison.OrdinalIgnoreCase);
	if (idx >= 0)
	{
		t = t.Substring(0, idx).Trim();
	}

	// quitar tokens numéricos al inicio (IDs)
	var parts = t.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
	int skip = 0;
	for (int i = 0; i < parts.Count; i++)
	{
		if (parts[i].All(c => char.IsDigit(c))) { skip++; continue; }
		// tokens muy cortos que no parecen nombre (ej: "001") -> skip
		if (parts[i].Length <= 3 && parts[i].All(c => char.IsDigit(c) || c == '0')) { skip++; continue; }
		break;
	}

	if (skip >= parts.Count) return string.Join(" ", parts);
	return string.Join(" ", parts.Skip(skip));
}

// Overload that accepts strongly-typed list returned by DatabaseManager
private static void BindComboItems(ComboBox cmb, List<WindowsFormsApplication1.Data.Item> items)
{
	// Forward to the IEnumerable implementation
	BindComboItems(cmb, (System.Collections.IEnumerable)items);
}

// New unambiguous helper that accepts an enumerable of objects
private static void PopulateCombo(ComboBox cmb, IEnumerable<object> items)
{
	if (cmb == null) return;
	try
	{
		cmb.DataSource = null;
		cmb.DisplayMember = "Dato";
		cmb.ValueMember = "Id";
		cmb.DataSource = items?.ToList() ?? new List<object>();
	}
	catch
	{
		try
		{
			cmb.Items.Clear();
			if (items != null)
			{
				foreach (var it in items)
					cmb.Items.Add(it);
			}
		}
		catch { }
	}
}

private static void PopulateCombo(ComboBox cmb, List<WindowsFormsApplication1.Data.Item> items)
{
	PopulateCombo(cmb, items?.Cast<object>().ToList());
}

		/// <summary>
		/// Convierte una fecha agrícola al formato PTI para etiquetas de ventana
		/// Transforma el formato interno (MMDD) a formato legible con año completo (MMM/DD/YYYY)
		/// Ejemplos: "0115" -> "Nov/15/2025", "0220" -> "Dec/20/2025", "0310" -> "Jan/10/2026"
		/// </summary>
		/// <returns>Fecha formateada en formato PTI con año completo</returns>
		public string Busca_Fecha_PTI()
		{
			string text = this.txt_fecha_agricola.Text.Trim().Substring(0, 2);
			switch (text)
			{
			case "01":
				return "Nov/" + this.txt_fecha_agricola.Text.Trim().Substring(2, 2) + "/2025";
			case "02":
				return "Dec/" + this.txt_fecha_agricola.Text.Trim().Substring(2, 2) + "/2025";
			case "03":
				return "Jan/" + this.txt_fecha_agricola.Text.Trim().Substring(2, 2) + "/2026";
			case "04":
				return "Feb/" + this.txt_fecha_agricola.Text.Trim().Substring(2, 2) + "/2026";
			case "05":
				return "Mar/" + this.txt_fecha_agricola.Text.Trim().Substring(2, 2) + "/2026";
			case "06":
				return "Apr/" + this.txt_fecha_agricola.Text.Trim().Substring(2, 2) + "/2026";
			case "07":
				return "May/" + this.txt_fecha_agricola.Text.Trim().Substring(2, 2) + "/2026";
			}
			return "ERROR";
		}


		/// <summary>
		/// Convierte una fecha agrícola al formato YYMMDD estándar
		/// Transforma el formato interno (MMDD) agregando el año correspondiente
		/// Maneja la transición de temporada (Nov-Dic del año anterior, Ene-Mayo del año actual)
		/// Ejemplos: "0115" -> "221115", "0220" -> "221220", "0310" -> "230110"
		/// </summary>
		/// <returns>Fecha en formato YYMMDD (año, mes, día)</returns>
		public string Busca_Fecha_YYMMDD()
		{
			string text = this.txt_fecha_agricola.Text.Trim().Substring(0, 2);
			switch (text)
			{
			case "01":
				return "2211" + this.txt_fecha_agricola.Text.Trim().Substring(2, 2);
			case "02":
				return "2212" + this.txt_fecha_agricola.Text.Trim().Substring(2, 2);
			case "03":
				return "2301" + this.txt_fecha_agricola.Text.Trim().Substring(2, 2);
			case "04":
				return "2302" + this.txt_fecha_agricola.Text.Trim().Substring(2, 2);
			case "05":
				return "2303" + this.txt_fecha_agricola.Text.Trim().Substring(2, 2);
			case "06":
				return "2304" + this.txt_fecha_agricola.Text.Trim().Substring(2, 2);
			case "07":
				return "2305" + this.txt_fecha_agricola.Text.Trim().Substring(2, 2);
			}
			return "ERROR";
		}

		/// <summary>
		/// Evento de validación de tecla presionada en el campo GTIN
		/// Solo permite la entrada de dígitos, teclas de control y separadores
		/// </summary>
		/// <param name="sender">Control que disparó el evento</param>
		/// <param name="e">Argumentos con la tecla presionada</param>
		private void txt_gtin_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (char.IsDigit(e.KeyChar))
			{
				e.Handled = false;
			}
			else if (char.IsControl(e.KeyChar))
			{
				e.Handled = false;
			}
			else if (char.IsSeparator(e.KeyChar))
			{
				e.Handled = false;
			}
			else
			{
				e.Handled = true;
			}
		}


		// Token: 0x060000D6 RID: 214 RVA: 0x0000AB10 File Offset: 0x00008D10
		private void Form1_Load(object sender, EventArgs e)
		{

// ==========================
// XML (LEGACY) - DEJAR COMENTADO
// ==========================
// this.LeerXML();
// this.LeerXML2();

// ==========================
// DESDE BD (NUEVO)
// ==========================
this.LlenaEmbalaje();
this.Llena_Productor();
this.Llena_Recibidor();
this.Llena_Packing();
this.Llena_Peso();
this.Llena_Calibre();
this.Llena_CategoriaSAG();

// Cargar variedades / lotes / etc.
SafeSelectFirst(this.cmb_productor);
this.LlenaVariedad();
SafeSelectFirst(this.cmb_variedad);
this.Llena_lotes();
SafeSelectFirst(this.cmb_lote);
this.Llena_variedad_imprime();
SafeSelectFirst(this.cmb_variedad_Imprime);
this.Llena_SDP();

this.pb_etiqueta.Image = null;

// Selecciones iniciales (solo si hay datos)
SafeSelectFirst(this.cmb_titulo2);
SafeSelectFirst(this.cmb_Recibidor);
SafeSelectFirst(this.cmb_packing);
SafeSelectFirst(this.cmb_cat1);

if (this.cbx_pallets.Items.Count > 1) this.cbx_pallets.SelectedIndex = 1;
else if (this.cbx_pallets.Items.Count > 0) this.cbx_pallets.SelectedIndex = 0;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x0000ABB0 File Offset: 0x00008DB0
		private void Llena_Recibidor()
		{

try
{
    // ==========================
    // XML (LEGACY) - DEJAR COMENTADO
    // ==========================
    /*
    this.Tabla_Recibidor.ReadXml("Recibidor.xml");
    EnumerableRowCollection<DataRow> source = from contact in this.Tabla_Recibidor.AsEnumerable()
        select contact;
    DataView dataView = source.AsDataView<DataRow>();
    dataView.Sort = "Recibidor asc";
    this.cmb_Recibidor.DataSource = dataView.ToTable(true, new string[] { "Recibidor" });
    this.cmb_Recibidor.DisplayMember = "Recibidor";
    */

    // ==========================
    // DESDE BD (NUEVO)
    // ==========================
    var db = DatabaseManager.Instance;
	PopulateCombo(this.cmb_Recibidor, db.GetRecibidoresItems().Cast<object>().ToList());
}
catch (Exception)
{
    throw;
}
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x0000AC74 File Offset: 0x00008E74
		private void Llena_Productor()
		{

try
{
    // ==========================
    // XML (LEGACY) - DEJAR COMENTADO
    // ==========================
    /*
    this.Tabla_Productores.ReadXml("Catrastro.xml");
    EnumerableRowCollection<DataRow> source = from contact in this.Tabla_Productores.AsEnumerable()
        select contact;
    DataView dataView = source.AsDataView<DataRow>();
    dataView.Sort = "Productor DESC";
    this.cmb_productor.DataSource = dataView.ToTable(true, new string[] { "Productor" });
    this.cmb_productor.DisplayMember = "Productor";
    */

    // ==========================
    // DESDE BD (NUEVO)
    // ==========================
    var db = DatabaseManager.Instance;

	// Mostrar sólo el nombre del productor (sin prefijos numéricos como CSG/ID)
	var originales = db.GetProductoresItems();
	var formateados = new List<WindowsFormsApplication1.Data.Item>();
	foreach (var it in originales)
	{
		string raw = it.Dato ?? "";
		string name = ExtractProductorName(raw);
		if (string.IsNullOrWhiteSpace(name)) name = raw.Trim();
		formateados.Add(new WindowsFormsApplication1.Data.Item(name, it.Id));
	}

	PopulateCombo(this.cmb_productor, formateados.Cast<object>().ToList());
}
catch (Exception)
{
    throw;
}
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x0000AD24 File Offset: 0x00008F24
		private void LeerXML()
		{
			try
			{
				this.datos.ReadXml("Catrastro.xml", XmlReadMode.Auto);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060000DA RID: 218 RVA: 0x0000AD60 File Offset: 0x00008F60
		private void LeerXML2()
		{
			try
			{
				this.datos.ReadXml("Recibidor.xml", XmlReadMode.Auto);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060000DB RID: 219 RVA: 0x0000AD9C File Offset: 0x00008F9C
		private void btn_copiar_Click(object sender, EventArgs e)
		{
			try
			{
				this.pb_etiqueta.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
				Clipboard.SetDataObject(this.pb_etiqueta.Image, true);
				this.pb_etiqueta.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060000DC RID: 220 RVA: 0x0000ADFC File Offset: 0x00008FFC
		private void btn_imprimir_Click(object sender, EventArgs e)
		{
			try
			{
				PrintDocument printDocument = new PrintDocument();
				printDocument.PrintPage += this.documentoAimprimir;
				printDocument.PrinterSettings.PrinterName = "ZDesigner S4M-203dpi ZPL";
				PageSettings pageSettings = new PageSettings();
				pageSettings.Margins = new Margins(0, 0, 0, 0);
				printDocument.DefaultPageSettings.Margins = pageSettings.Margins;
				PaperSize paperSize = new PaperSize("Custom", 315, 168);
				printDocument.DefaultPageSettings.PaperSize = paperSize;
				printDocument.Print();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Impresion: " + ex.Message);
			}
		}

		// Token: 0x060000DD RID: 221 RVA: 0x0000AEB8 File Offset: 0x000090B8
		private void documentoAimprimir(object sender, PrintPageEventArgs e)
		{
			try
			{
				using (Graphics graphics = e.Graphics)
				{
					graphics.DrawImage(this.pb_etiqueta.Image, 0, 0);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Impresion:" + ex.Message);
			}
		}

		// Token: 0x060000DE RID: 222 RVA: 0x0000AF34 File Offset: 0x00009134
		private void btn_guardar_Click(object sender, EventArgs e)
		{
			try
			{
				if (this.sfd.ShowDialog(this) == DialogResult.OK)
				{
					ImageFormat jpeg = ImageFormat.Jpeg;
					try
					{
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

		// Token: 0x060000DF RID: 223 RVA: 0x0000AFC4 File Offset: 0x000091C4
		private void LlenaEmbalaje()
		{

try
{
    // ==========================
    // DATOS HARDCODEADOS (LEGACY) - DEJAR COMENTADO
    // ==========================
    /*
    List<Item> list = new List<Item>();
    if (!this.chb_pesofijo.Checked)
    {
        list.Add(new Item("BP", 1));
        list.Add(new Item("BSUAC", 1));
        list.Add(new Item("BUSAL", 1));
        list.Add(new Item("CLAM", 1));
        list.Add(new Item("Clam", 1));
        list.Add(new Item("TORE", 1));
        list.Add(new Item("TORE COV", 1));
        list.Add(new Item("TORE COV BT", 1));
        list.Add(new Item("TORE COV LD", 1));
    }
    else
    {
        list.Add(new Item("BUSAL", 1));
        list.Add(new Item("BUSAL LD", 1));
        list.Add(new Item("CLAM", 1));
        list.Add(new Item("Clam", 1));
        list.Add(new Item("GCBUSS", 1));
        list.Add(new Item("GCBUSS LD", 1));
        list.Add(new Item("TORE COV BT", 1));
        list.Add(new Item("TORE COV LD", 1));
    }
    this.cmb_tipo_embalaje.DisplayMember = "Name";
    this.cmb_tipo_embalaje.ValueMember = "Value";
    this.cmb_tipo_embalaje.DataSource = list;
    */

    // ==========================
    // DESDE BD (NUEVO)
    // ==========================
    var db = DatabaseManager.Instance;
	bool pesoFijo = this.chb_pesofijo.Checked;
	PopulateCombo(this.cmb_tipo_embalaje, db.GetTipoEmbalajePorPesoFijo(pesoFijo).Cast<object>().ToList());
}
catch (Exception)
{
    // mantener comportamiento antiguo: no cortar el flujo si falla la carga
}
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x0000B311 File Offset: 0x00009511
		private void cmb_titulo1_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.pb_etiqueta.Image = null;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x0000B321 File Offset: 0x00009521
		private void cmb_gtin_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.pb_etiqueta.Image = null;
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x0000B331 File Offset: 0x00009531
		private void cmb_variedad_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.Llena_lotes();
			this.Llena_variedad_imprime();
			this.pb_etiqueta.Image = null;
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x0000B350 File Offset: 0x00009550
		private void Llena_variedad_imprime()
		{

try
{
    // ==========================
    // DATOS HARDCODEADOS (LEGACY) - DEJAR COMENTADO
    // ==========================
    /*
    List<Item> list = new List<Item>();
    // ... (mapeo gigante por texto de variedad)
    this.cmb_variedad_Imprime.DisplayMember = "Name";
    this.cmb_variedad_Imprime.ValueMember = "Value";
    this.cmb_variedad_Imprime.DataSource = list;
    */

    // ==========================
    // DESDE BD (NUEVO)
    // ==========================
    var db = DatabaseManager.Instance;

    // Si aún no hay variedad seleccionada, limpiamos
    int? variedadId = TryGetSelectedId(this.cmb_variedad);
    if (!variedadId.HasValue || variedadId.Value <= 0)
    {
    BindComboItems(this.cmb_variedad_Imprime, (System.Collections.IEnumerable)new List<WindowsFormsApplication1.Data.Item>());
        return;
    }

    bool pesoFijo = this.chb_pesofijo.Checked;
    var items = db.GetVariedadesImprimePorVariedadYPesoFijo(variedadId.Value, pesoFijo);

    PopulateCombo(this.cmb_variedad_Imprime, items.Cast<object>().ToList());
}
catch (Exception)
{
    // mantener comportamiento antiguo
}
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x0000BC54 File Offset: 0x00009E54
	private void Llena_lotes()
	{

try
{
    // ==========================
    // XML (LEGACY) - DEJAR COMENTADO
    // ==========================
    /*
    EnumerableRowCollection<DataRow> source = from contact in this.Tabla_Productores.AsEnumerable()
        where contact.Field<string>("Productor") == this.cmb_productor.Text.ToString() && contact.Field<string>("Variedad") == this.cmb_variedad.Text.ToString()
        select contact;
    DataView dataView = source.AsDataView<DataRow>();
    this.cmb_lote.DataSource = dataView.ToTable(true, new string[] { "Lote" });
    this.cmb_lote.DisplayMember = "Lote";
    */

    // ==========================
    // DESDE BD (NUEVO)
    // ==========================
    var db = DatabaseManager.Instance;
    int? variedadId = TryGetSelectedId(this.cmb_variedad);

    if (variedadId.HasValue && variedadId.Value > 0)
        PopulateCombo(this.cmb_lote, db.GetLotePorVariedad(variedadId.Value).Cast<object>().ToList());
	else
		PopulateCombo(this.cmb_lote, db.GetLotesItems().Cast<object>().ToList());
}
catch (Exception)
{
    // mantener comportamiento antiguo
}
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x0000BCE0 File Offset: 0x00009EE0
		private void LlenaCalibres()
		{

try
{
    // ==========================
    // DATOS HARDCODEADOS (LEGACY) - DEJAR COMENTADO
    // ==========================
    /*
    List<Item> list = new List<Item>();
    if (this.cmb_variedad_Imprime.Text.Trim() == "00" || this.cmb_variedad_Imprime.Text.Trim() == "00")
    {
        list.Add(new Item("XJ", 1));
        // ...
    }
    else
    {
        list.Add(new Item("XXJ", 1));
        // ...
    }
    this.cmb_calibre.DisplayMember = "Name";
    this.cmb_calibre.ValueMember = "Value";
    this.cmb_calibre.DataSource = list;
    */

    // ==========================
    // DESDE BD (NUEVO)
    // ==========================
    var db = DatabaseManager.Instance;
	PopulateCombo(this.cmb_calibre, db.GetCalibresItems().Cast<object>().ToList());
}
catch (Exception)
{
    // mantener comportamiento antiguo
}
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x0000BFF1 File Offset: 0x0000A1F1
		private void txt_productor_TextChanged(object sender, EventArgs e)
		{
			this.pb_etiqueta.Image = null;
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x0000C001 File Offset: 0x0000A201
		private void txt_lote_TextChanged(object sender, EventArgs e)
		{
			this.pb_etiqueta.Image = null;
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x0000C011 File Offset: 0x0000A211
		private void cmb_titulo2_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.pb_etiqueta.Image = null;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x0000C021 File Offset: 0x0000A221
		private void cmb_fecha_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.pb_etiqueta.Image = null;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x0000C034 File Offset: 0x0000A234
		private void txt_productor_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (char.IsDigit(e.KeyChar))
			{
				e.Handled = false;
			}
			else if (char.IsControl(e.KeyChar))
			{
				e.Handled = false;
			}
			else if (char.IsSeparator(e.KeyChar))
			{
				e.Handled = false;
			}
			else
			{
				e.Handled = true;
			}
		}

		// Token: 0x060000EB RID: 235 RVA: 0x0000C0A6 File Offset: 0x0000A2A6
		private void txt_fecha_agricola_TextChanged(object sender, EventArgs e)
		{
			this.pb_etiqueta.Image = null;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x0000C0B6 File Offset: 0x0000A2B6
		private void txt_linea_TextChanged(object sender, EventArgs e)
		{
			this.pb_etiqueta.Image = null;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x0000C0C6 File Offset: 0x0000A2C6
		private void cmb_tipo_embalaje_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.pb_etiqueta.Image = null;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x0000C0D6 File Offset: 0x0000A2D6
		private void cmb_packing_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.pb_etiqueta.Image = null;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x0000C0E6 File Offset: 0x0000A2E6
		private void cmb_calibre_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.pb_etiqueta.Image = null;
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x0000C0F6 File Offset: 0x0000A2F6
		private void cmb_cat1_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.pb_etiqueta.Image = null;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x0000C108 File Offset: 0x0000A308
		private void button5_Click(object sender, EventArgs e)
		{
			this.Text = "L-" + this.txt_linea.Text + " - Generador PTI";
			try
			{
				if (this.cmb_variedad_Imprime.Text.Trim() == "")
				{
					MessageBox.Show("Favor ingresar Variedad correcta.");
					this.pb_etiqueta.Image = null;
					this.cmb_variedad_Imprime.Focus();
				}
				else if (this.txt_fecha_agricola.Text.Trim().Length < 4)
				{
					MessageBox.Show("Favor ingresar Fecha Agricola de 4 caracteres.");
					this.pb_etiqueta.Image = null;
					this.txt_fecha_agricola.Focus();
				}
				else if (this.txt_linea.Text.Trim().Length < 2)
				{
					MessageBox.Show("Favor ingresar Linea de 2 caracteres.");
					this.pb_etiqueta.Image = null;
					this.txt_linea.Focus();
				}
				else
				{
					this.DibujaEtiquetaCOMPLETA();
				}
			}
			catch (Exception)
			{
				MessageBox.Show("Error al generar el Voice Pick Code");
				this.pb_etiqueta.Image = null;
			}
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x0000C3B4 File Offset: 0x0000A5B4
		private void DibujaEtiquetaCOMPLETA()
		{
			string text = string.Empty;
			string text2 = string.Empty;
			text = this.Busca_Fecha_PTI();
			text2 = this.Busca_Fecha_YYMMDD();
			BarcodeGenerator barcodeGenerator = new BarcodeGenerator();
			BarcodeGenerator_PLU barcodeGenerator_PLU = new BarcodeGenerator_PLU();
			Graphics g = Graphics.FromImage(new Bitmap(1, 1));
			Graphics graphics = Graphics.FromImage(new Bitmap(1, 1));
			Bitmap image = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
			g = Graphics.FromImage(image);
			graphics = Graphics.FromImage(image);
			string empty = string.Empty;
			Image image2 = barcodeGenerator.DrawCode128(g, "123456789", 0, 0);
            string text3 = this.cmb_titulo2.Text.Trim();
			string productorCsg = this.GetSelectedProductorCsg();
			string variedadCode2 = this.GetVariedadImprimeCode2();
			int? packingId = TryGetSelectedId(this.cmb_packing);
			string packingCsp = string.Empty;
			if (packingId.HasValue)
			{
				var cspVal = WindowsFormsApplication1.Data.DatabaseManager.Instance.GetCspByPackingId(packingId.Value);
				if (cspVal.HasValue)
					packingCsp = cspVal.Value.ToString();
			}

string text4 = productorCsg + variedadCode2 + this.cmb_lote.Text.Trim();
string empty3 = string.Empty;
			string empty4 = string.Empty;
			string str;
			string str2;
			if (packingId.HasValue && (packingId.Value == 126 || packingId.Value == 127 || packingId.Value == 128 || packingId.Value == 129 || packingId.Value == 130 || packingId.Value == 131 || packingId.Value == 132 || packingId.Value == 146 || packingId.Value == 147))
			{
				str = "ELQUI";
				str2 = "VICUÑA";
			}
			else
			{
				str = "COPIAPO";
				str2 = "TIERRA AMARILLA";
			}
			string str3;
			string str4;
			if (IsProductorElqui(productorCsg))
			{
				str3 = "ELQUI";
				str4 = "VICUÑA";
			}
			else
			{
				str3 = "COPIAPO";
				str4 = "TIERRA AMARILLA";
			}
			int width = 1600;
			int height = 800;
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);

			// Replace slow per-pixel SetPixel loop with fast Graphics operations.
			// Clearing and drawing filled rectangles is much faster and avoids the bottleneck.
			Graphics graphics2 = Graphics.FromImage(bitmap);
			try
			{
				graphics2.Clear(Color.White);
				graphics2.SmoothingMode = SmoothingMode.AntiAlias;

				// horizontal thin lines (converted from pixel ranges)
				// original: i>390 && i<928 => x=391..927, width=537
				int hx = 391;
				int hwidth = 927 - 391 + 1; // 537
				// first three fixed horizontal lines
				graphics2.FillRectangle(Brushes.Black, hx, 201, hwidth, 2); // j>200 && j<203 -> y=201..202
				graphics2.FillRectangle(Brushes.Black, hx, 251, hwidth, 2); // j>250 && j<253
				graphics2.FillRectangle(Brushes.Black, hx, 301, hwidth, 2); // j>300 && j<303

				// optional SDP horizontal line
				if (this.cbx_sdp.Text.ToString() != "")
				{
					graphics2.FillRectangle(Brushes.Black, hx, 351, hwidth, 2); // j>350 && j<353

					// three small vertical blocks inside the SDP area
					graphics2.FillRectangle(Brushes.Black, 391, 304, 2, 49); // i>390 && i<393 && j>303 && j<353
					graphics2.FillRectangle(Brushes.Black, 736, 304, 2, 49); // i>735 && i<738
					graphics2.FillRectangle(Brushes.Black, 926, 304, 2, 49); // i>925 && i<928
				}

				// main vertical separators
				graphics2.FillRectangle(Brushes.Black, 391, 201, 2, 102); // i>390 && i<393 && j>200 && j<303
				graphics2.FillRectangle(Brushes.Black, 736, 201, 2, 102); // i>735 && i<738
				graphics2.FillRectangle(Brushes.Black, 926, 201, 2, 102); // i>925 && i<928
			}
			catch
			{
				// if something unexpected happens, ensure we still have a white background
				graphics2.Clear(Color.White);
			}
			int[] array = new int[]
			{
				36,
				34,
				32,
				30,
				28,
				26,
				24,
				22,
				20,
				18,
				16,
				14,
				12,
				10,
				8,
				6,
				4
			};
			Font font = null;
			font = new Font("arial", 120f, FontStyle.Bold);
			if (this.cmb_calibre.Text.Length > 3)
			{
				font = new Font("arial", 80f, FontStyle.Bold);
			}
			StringFormat stringFormat = new StringFormat();
			stringFormat.Alignment = StringAlignment.Center;
			SolidBrush brush = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
			graphics2.DrawString(this.cmb_calibre.Text, font, brush, new Point(190, 110), stringFormat);
			font = new Font("Impact", 160f, FontStyle.Regular);
			StringFormat stringFormat2 = new StringFormat();
			stringFormat2.Alignment = StringAlignment.Center;
			SolidBrush brush2 = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
			graphics2.DrawString(this.cbx_pallets.Text, font, brush2, new Point(190, 450), stringFormat2);
            // Use the full variety text (do not strip the first 3 chars) so the whole name can be displayed.
			string text5 = this.cmb_variedad_Imprime.Text.Trim();
			// Keep the full name as a single token so drawing logic will measure and scale the full string to fit.
			string[] array2 = new[] { text5 };
            // Draw the full variety name inside a constrained rectangle at the top-right of the label.
			// We'll auto-scale the font and allow wrapping so the full name fits the available area.
			string varietyText = text5; // full name
            // Try larger sizes first so short names will expand; shrink for long names until they fit wrapped in the box.
			float[] candidateSizes = new float[] { 120f, 100f, 80f, 76f, 72f, 68f, 64f, 60f, 56f, 52f, 48f, 44f, 40f, 36f, 32f, 28f, 24f, 20f, 16f, 14f, 12f };
			// Rectangle where the variety must fit (top-right area). Adjust if needed to exact template mm->px mapping.
			RectangleF varietyRect = new RectangleF(1080f, 40f, 400f, 260f);
			StringFormat varietyFormat = new StringFormat();
			// center horizontally and vertically inside the rectangle
			varietyFormat.Alignment = StringAlignment.Center;
			varietyFormat.LineAlignment = StringAlignment.Center;

			Font chosenFont = null;
			foreach (var fs in candidateSizes)
			{
				try
				{
					using (var f = new Font("arial", fs, FontStyle.Bold))
					{
						var size = graphics2.MeasureString(varietyText, f, (int)varietyRect.Width);
						if (size.Height <= varietyRect.Height)
						{
							chosenFont = new Font(f.FontFamily, f.Size, f.Style);
							break;
						}
					}
				}
				catch { }
			}
			if (chosenFont == null)
				chosenFont = new Font("arial", 14f, FontStyle.Bold);

			try
			{
				// Draw the text wrapped and centered within the rectangle
				var brushVar = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
				graphics2.DrawString(varietyText, chosenFont, brushVar, varietyRect, varietyFormat);
				brushVar.Dispose();
			}
			catch { }
			finally
			{
				try { chosenFont.Dispose(); } catch { }
			}
			if (this.cmb_variedad_Imprime.Text.Trim() == "06 Black Seedless" || this.cmb_variedad_Imprime.Text.Trim() == "10 T Seedless" || this.cmb_variedad_Imprime.Text.Trim() == "00 Green Seedless" || this.cmb_variedad_Imprime.Text.Trim() == "00 Red Seedless")
			{
				font = new Font("arial", 30f, FontStyle.Bold);
				StringFormat stringFormat5 = new StringFormat();
				stringFormat5.Alignment = StringAlignment.Center;
				SolidBrush brush5 = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
				graphics2.DrawString("''Unknown Variety''", font, brush5, new Point(1280, 290), stringFormat5);
			}
			string text6 = this.cmb_Recibidor.Text.Trim().ToString();
			string[] array4;
			if (this.cmb_Recibidor.Text.Trim() == "10 T Seedless" || this.cmb_Recibidor.Text.Trim() == "16 Iniagrape-one cv.")
			{
				array4 = text6.Split(new char[]
				{
					'X'
				});
			}
			else if (this.cmb_Recibidor.Text.Trim() == "AM FRESH_NORTH_AMERICA" || this.cmb_Recibidor.Text.Trim() == "AM FRESH_NORTH_AMERICA" || this.cmb_Recibidor.Text.Trim() == "GUAN CHAN_INTERNATIONAL_(IPG)" || this.cmb_Recibidor.Text.Trim() == "DEREK L_WANG_(IPG)" || this.cmb_Recibidor.Text.Trim() == "NEW_WORLD_CHILE S.A." || this.cmb_Recibidor.Text.Trim() == "GRUPO_FARTURA DE_HORTIFRUT S.A.")
			{
				array4 = text6.Split(new char[]
				{
					'_'
				});
			}
			else
			{
				array4 = text6.Split(new char[]
				{
					' '
				});
			}
			int[] array5 = new int[]
			{
				68,
				64,
				60,
				56,
				52,
				48,
				44,
				40,
				36,
				32,
				28,
				24,
				20
			};
			if (array4.Length.ToString() == "1")
			{
				try
				{
					font = new Font("arial", 36f, FontStyle.Bold);
					StringFormat stringFormat3 = new StringFormat();
					stringFormat3.Alignment = StringAlignment.Center;
					SolidBrush brush3 = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
					graphics2.DrawString("", font, brush3, new Point(670, 520), stringFormat3);
				}
				catch (Exception)
				{
				}
				try
				{
					for (int k = 0; k < 12; k++)
					{
						font = new Font("arial", (float)array5[k], FontStyle.Bold);
						if ((ushort)graphics2.MeasureString(array4[0], font).Width < 400)
						{
							break;
						}
					}
					StringFormat stringFormat4 = new StringFormat();
					stringFormat4.Alignment = StringAlignment.Center;
					SolidBrush brush4 = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
					graphics2.DrawString(array4[0], font, brush4, new Point(670, 550), stringFormat4);
				}
				catch (Exception)
				{
				}
			}
			else
			{
				int num = 0;
				int num2 = 0;
				int num4 = 0;
				int num3 = 0;
				try
				{
					try
					{
						for (int k = 0; k < 12; k++)
						{
							font = new Font("arial", (float)array5[k], FontStyle.Bold);
							if ((ushort)graphics2.MeasureString(array4[0], font).Width < 400)
							{
								break;
							}
							num = k;
						}
					}
					catch (Exception)
					{
					}
					try
					{
						for (int k = 0; k < 12; k++)
						{
							font = new Font("arial", (float)array5[k], FontStyle.Bold);
							if ((ushort)graphics2.MeasureString(array4[1], font).Width < 400)
							{
								break;
							}
							num2 = k;
						}
					}
					catch (Exception)
					{
					}
					try
					{
						for (int k = 0; k < 12; k++)
						{
							font = new Font("arial", (float)array5[k], FontStyle.Bold);
							if ((ushort)graphics2.MeasureString(array4[2], font).Width < 400)
							{
								break;
							}
							num4 = k;
						}
					}
					catch (Exception)
					{
					}
					if (num > num2)
					{
						num3 = num;
						if (num > num4)
						{
							num3 = num;
						}
						else
						{
							num3 = num4;
						}
					}
					else
					{
						num3 = num2;
						if (num2 > num4)
						{
							num3 = num2;
						}
						else
						{
							num3 = num4;
						}
					}
				}
				catch (Exception)
				{
				}
				try
				{
					font = new Font("arial", (float)array5[num3], FontStyle.Bold);
					StringFormat stringFormat3 = new StringFormat();
					stringFormat3.Alignment = StringAlignment.Center;
					SolidBrush brush3 = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
					graphics2.DrawString(array4[0], font, brush3, new Point(670, 520), stringFormat3);
				}
				catch (Exception)
				{
				}
				try
				{
					font = new Font("arial", (float)array5[num3], FontStyle.Bold);
					StringFormat stringFormat4 = new StringFormat();
					stringFormat4.Alignment = StringAlignment.Center;
					SolidBrush brush4 = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
					graphics2.DrawString(array4[1], font, brush4, new Point(670, 550), stringFormat4);
				}
				catch (Exception)
				{
				}
				try
				{
					font = new Font("arial", (float)array5[num3], FontStyle.Bold);
					StringFormat stringFormat6 = new StringFormat();
					stringFormat6.Alignment = StringAlignment.Center;
					SolidBrush brush6 = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
					graphics2.DrawString(array4[2], font, brush6, new Point(670, 650), stringFormat6);
				}
				catch (Exception)
				{
				}
			}
			font = new Font("arial", 22f, FontStyle.Bold);
			StringFormat stringFormat7 = new StringFormat();
			stringFormat7.Alignment = StringAlignment.Center;
			SolidBrush brush7 = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
			graphics2.DrawString("CSG " + productorCsg, font, brush7, new Point(832, 260), stringFormat7);
			font = new Font("arial", 22f, FontStyle.Bold);
StringFormat stringFormat8 = new StringFormat();
stringFormat8.Alignment = StringAlignment.Center;
SolidBrush brush8 = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
graphics2.DrawString("CSP " + packingCsp, font, brush8, new Point(832, 210), stringFormat8);
font = new Font("arial", 17f, FontStyle.Bold);
			StringFormat stringFormat9 = new StringFormat();
			stringFormat9.Alignment = StringAlignment.Center;
			SolidBrush brush9 = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
			graphics2.DrawString(str + ", " + str2, font, brush9, new Point(563, 215), stringFormat9);
			font = new Font("arial", 17f, FontStyle.Bold);
			StringFormat stringFormat10 = new StringFormat();
			stringFormat10.Alignment = StringAlignment.Center;
			SolidBrush brush10 = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
			graphics2.DrawString(str3 + ", " + str4, font, brush10, new Point(563, 265), stringFormat10);
			if (this.cbx_sdp.Text.ToString() != "")
			{
				font = new Font("arial", 17f, FontStyle.Bold);
				StringFormat stringFormat11 = new StringFormat();
				stringFormat11.Alignment = StringAlignment.Center;
				SolidBrush brush11 = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
				graphics2.DrawString(str3 + ", " + str4, font, brush11, new Point(563, 315), stringFormat11);
				font = new Font("arial", 22f, FontStyle.Bold);
				StringFormat stringFormat12 = new StringFormat();
				stringFormat12.Alignment = StringAlignment.Center;
				SolidBrush brush12 = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
				graphics2.DrawString("SdP " + this.cbx_sdp.Text.Trim().ToString(), font, brush12, new Point(832, 310), stringFormat7);
			}
			font = new Font("arial", 34f, FontStyle.Bold);
			StringFormat stringFormat13 = new StringFormat();
			stringFormat13.Alignment = StringAlignment.Near;
			SolidBrush brush13 = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
			graphics2.DrawString("DATE: " + this.txt_fecha_agricola.Text, font, brush13, new Point(380, 95), stringFormat13);
			font = new Font("arial", 34f, FontStyle.Bold);
			StringFormat stringFormat14 = new StringFormat();
			stringFormat14.Alignment = StringAlignment.Near;
			SolidBrush brush14 = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
			graphics2.DrawString("LOTE: " + this.cmb_lote.Text + this.txt_linea.Text, font, brush14, new Point(380, 145), stringFormat14);
			font = new Font("arial", 34f, FontStyle.Bold);
			StringFormat stringFormat15 = new StringFormat();
			stringFormat15.Alignment = StringAlignment.Near;
			SolidBrush brush15 = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
			graphics2.DrawString("INNER PACK: ", font, brush15, new Point(380, 45), stringFormat15);
			font = new Font("arial", 44f, FontStyle.Bold);
			StringFormat stringFormat16 = new StringFormat();
			stringFormat16.Alignment = StringAlignment.Near;
			SolidBrush brush16 = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
			graphics2.DrawString(this.cmb_tipo_embalaje.Text, font, brush16, new Point(685, 36), stringFormat15);
            // Assign generated bitmap directly to the PictureBox to avoid slow disk IO.
			try
			{
				Image old = this.pb_etiqueta.Image;
				// Clone so we can dispose the local bitmap and graphics safely
				this.pb_etiqueta.Image = (Image)bitmap.Clone();
				try { old?.Dispose(); } catch { }
			}
			catch
			{
				// Fallback: if cloning fails, set image location via temporary save
				try { bitmap.Save("etiqueta.jpg", ImageFormat.Jpeg); this.pb_etiqueta.ImageLocation = "etiqueta.jpg"; } catch { }
			}
			finally
			{
				graphics2.Dispose();
				try { bitmap.Dispose(); } catch { }
			}
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x0000DFC4 File Offset: 0x0000C1C4
		private void chb_pesofijo_CheckedChanged(object sender, EventArgs e)
		{

this.LlenaEmbalaje();
this.Llena_variedad_imprime();
this.pb_etiqueta.Image = null;
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x0000DFCE File Offset: 0x0000C1CE
		private void cmb_productor_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.LlenaVariedad();
			this.pb_etiqueta.Image = null;
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x0000E01C File Offset: 0x0000C21C
	private void LlenaVariedad()
	{

try
{
    // ==========================
    // XML (LEGACY) - DEJAR COMENTADO
    // ==========================
    /*
    EnumerableRowCollection<DataRow> source = from contact in this.Tabla_Productores.AsEnumerable()
        where contact.Field<string>("Productor") == this.cmb_productor.Text.ToString()
        select contact;
    DataView dataView = source.AsDataView<DataRow>();
    this.cmb_variedad.DataSource = dataView.ToTable(true, new string[] { "Variedad" });
    this.cmb_variedad.DisplayMember = "Variedad";
    */

    // ==========================
    // DESDE BD (NUEVO)
    // ==========================
    var db = DatabaseManager.Instance;

    // Preservar selección si es posible
	int? current = TryGetSelectedId(this.cmb_variedad);

	BindComboItems(this.cmb_variedad, (System.Collections.IEnumerable)db.GetVariedadesItems());

    if (current.HasValue)
    {
        try { this.cmb_variedad.SelectedValue = current.Value; } catch { }
    }
}
catch (Exception)
{
    // mantener comportamiento antiguo
}
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x0000E0A8 File Offset: 0x0000C2A8
		private void cmb_lote_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.Llena_SDP();
			this.pb_etiqueta.Image = null;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x0000E13C File Offset: 0x0000C33C
	private void Llena_SDP()
	{

try
{
    // ==========================
    // XML (LEGACY) - DEJAR COMENTADO
    // ==========================
    /*
    EnumerableRowCollection<DataRow> source = from contact in this.Tabla_Productores.AsEnumerable()
        where contact.Field<string>("Productor") == this.cmb_productor.Text.ToString()
            && contact.Field<string>("Variedad") == this.cmb_variedad.Text.ToString()
            && contact.Field<string>("Lote") == this.cmb_lote.Text.ToString()
        select contact;
    DataView dataView = source.AsDataView<DataRow>();
    this.cbx_sdp.DataSource = dataView.ToTable(true, new string[] { "SDP" });
    this.cbx_sdp.DisplayMember = "SDP";
    */

    // ==========================
    // DESDE BD (NUEVO)
    // ==========================
    var db = DatabaseManager.Instance;

    int? productorId = TryGetSelectedId(this.cmb_productor);
    int? variedadId = TryGetSelectedId(this.cmb_variedad);
    int? loteId = TryGetSelectedId(this.cmb_lote);
    int? variedadImprimeId = TryGetSelectedId(this.cmb_variedad_Imprime);

    BindComboItems(this.cbx_sdp, (System.Collections.IEnumerable)db.GetSDPsPorSeleccion(productorId, variedadId, loteId, variedadImprimeId));
}
catch (Exception)
{
    // mantener comportamiento antiguo
}
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x0000E1C8 File Offset: 0x0000C3C8
		private void cmb_Recibidor_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.pb_etiqueta.Image = null;
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x0000E1D8 File Offset: 0x0000C3D8
		private void cmb_variedad_Imprime_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.LlenaCalibres();
			this.pb_etiqueta.Image = null;
		}



// ==========================
// CARGA DE DATOS DESDE BD
// ==========================
private void Llena_Packing()
{
    try
    {
        var db = DatabaseManager.Instance;

        // Para mantener el comportamiento del formulario (dependía de que el texto partiera con el ID),
        // se arma un display "000 <dato> <csp>" cuando aplique.
        var originales = db.GetPackingItems();
        var formateados = new List<WindowsFormsApplication1.Data.Item>();

        foreach (var it in originales)
        {
            // Mostrar solo el nombre del packing (sin ID ni CSP en el texto)
			string raw = (it.Dato ?? "").Trim();
			string name = ExtractPackingName(raw);
			if (string.IsNullOrWhiteSpace(name)) name = raw;
			formateados.Add(new WindowsFormsApplication1.Data.Item(name, it.Id));
        }

        BindComboItems(this.cmb_packing, (System.Collections.IEnumerable)formateados);
    }
    catch { }
}

private void Llena_Peso()
{
    try
    {
        var db = DatabaseManager.Instance;
        BindComboItems(this.cmb_titulo2, (System.Collections.IEnumerable)db.GetPesoItems());
    }
    catch { }
}

private void Llena_Calibre()
{
    try
    {
        var db = DatabaseManager.Instance;
        BindComboItems(this.cmb_calibre, (System.Collections.IEnumerable)db.GetCalibresItems());
    }
    catch { }
}

private void Llena_CategoriaSAG()
{
    try
    {
        var db = DatabaseManager.Instance;
        BindComboItems(this.cmb_cat1, (System.Collections.IEnumerable)db.GetCategoriaSAGItems());
    }
    catch { }
}

// ==========================
// HELPERS
// ==========================
private static void SafeSelectFirst(ComboBox cmb)
{
    try
    {
        if (cmb == null) return;
        if (cmb.Items != null && cmb.Items.Count > 0)
            cmb.SelectedIndex = 0;
    }
    catch { }
}

private static void BindComboItems(ComboBox cmb, System.Collections.IEnumerable items)
{
    if (cmb == null) return;

    try
    {
        cmb.DataSource = null;
        cmb.DisplayMember = "Dato";
        cmb.ValueMember = "Id";
        cmb.DataSource = items ?? new object[0];
    }
    catch
    {
        // Si falla el binding (por estilo del control), dejamos el fallback por texto
        try
        {
            cmb.Items.Clear();
            if (items != null)
            {
                foreach (var it in items)
                    cmb.Items.Add(it);
            }
        }
        catch { }
    }
}

private static int? TryGetSelectedId(ComboBox cmb)
{
    if (cmb == null) return null;

    try
    {
        if (cmb.SelectedValue != null)
        {
            int id;
            if (int.TryParse(cmb.SelectedValue.ToString(), out id))
                return id;
        }

        // Fallback: intentar extraer Id/Value/ValueMember dinámicamente
		var sel = cmb.SelectedItem;
		if (sel != null)
		{
			// Si tiene propiedad "Id"
			var propId = sel.GetType().GetProperty("Id");
			if (propId != null)
			{
				try { return Convert.ToInt32(propId.GetValue(sel)); } catch { }
			}

			// Si tiene propiedad "Value"
			var propValue = sel.GetType().GetProperty("Value");
			if (propValue != null)
			{
				try { return Convert.ToInt32(propValue.GetValue(sel)); } catch { }
			}

			// Intentar usar ToString() parseable
			try
			{
				int v;
				if (int.TryParse(sel.ToString(), out v)) return v;
			}
			catch { }
		}
    }
    catch { }

    return null;
}

private static string OnlyDigits(string s)
{
    if (string.IsNullOrWhiteSpace(s)) return "";
    var sb = new System.Text.StringBuilder();
    foreach (char ch in s)
    {
        if (char.IsDigit(ch)) sb.Append(ch);
    }
    return sb.ToString();
}

private static string ExtractLeadingDigits(string s)
{
    if (string.IsNullOrWhiteSpace(s)) return "";
    s = s.TrimStart();
    var sb = new System.Text.StringBuilder();
    foreach (char ch in s)
    {
        if (char.IsDigit(ch)) sb.Append(ch);
        else break;
    }
    return sb.ToString();
}

// Extrae el nombre del productor eliminando prefijos numéricos o códigos al inicio.
private static string ExtractProductorName(string s)
{
	if (string.IsNullOrWhiteSpace(s)) return "";
	// Quitar guiones bajos y múltiplos espacios
	var t = s.Trim();
	// Si el texto comienza con dígitos o con patrón "123456 401 name...", saltar tokens iniciales que sean numéricos
	var parts = t.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
	int skip = 0;
	for (int i = 0; i < parts.Count; i++)
	{
		// token completamente numérico -> skip
		if (parts[i].All(c => char.IsDigit(c))) { skip++; continue; }
		// token con dígitos seguido de otros (ej: "105588401") -> if starts with digits, skip token
		if (parts[i].Length > 0 && char.IsDigit(parts[i][0]) && parts[i].Any(c => char.IsLetter(c)) == false)
		{
			skip++; continue;
		}
		// else consider first non-numeric token as start of name
		break;
	}

	if (skip >= parts.Count) return string.Join(" ", parts);
	return string.Join(" ", parts.Skip(skip));
}

private string GetSelectedProductorCsg()
{
    try
    {
        int? productorId = TryGetSelectedId(this.cmb_productor);
        if (productorId.HasValue)
        {
            string csg = DatabaseManager.Instance.GetProductorCsgById(productorId.Value);
            string digits = OnlyDigits(csg);
            if (!string.IsNullOrWhiteSpace(digits)) return digits;
        }
    }
    catch { }

    // fallback: extraer dígitos del texto mostrado
    try
    {
        string t = (this.cmb_productor.Text ?? "").Trim();
        string lead = ExtractLeadingDigits(t);
        if (!string.IsNullOrWhiteSpace(lead)) return lead;
        return OnlyDigits(t);
    }
    catch { }

    return "";
}

private string GetSelectedPackingCsp()
{
    try
    {
        int? packingId = TryGetSelectedId(this.cmb_packing);
        if (packingId.HasValue)
        {
            var csp = DatabaseManager.Instance.GetCspByPackingId(packingId.Value);
            if (csp.HasValue) return csp.Value.ToString();
        }
    }
    catch { }

    // fallback: último token numérico
    try
    {
        string t = (this.cmb_packing.Text ?? "").Trim();
        string[] parts = t.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length > 0)
        {
            long v;
            if (long.TryParse(parts[parts.Length - 1], out v)) return v.ToString();
        }
    }
    catch { }

    return "";
}

private string GetVariedadImprimeCode2()
{
    try
    {
        string t = (this.cmb_variedad_Imprime.Text ?? "").Trim();
        // el código suele venir al inicio
        string lead = ExtractLeadingDigits(t);
        if (lead.Length >= 2) return lead.Substring(0, 2);
        if (t.Length >= 2) return t.Substring(0, 2);
    }
    catch { }
    return "";
}

private bool IsProductorElqui(string productorCsg)
{
    try
    {
        int n;
        if (!int.TryParse(productorCsg, out n)) return false;

        return n == 106957
            || n == 106958
            || n == 106955
            || n == 106956
            || n == 87197
            || n == 89323;
    }
    catch { }
    return false;
}

		// Token: 0x040000AC RID: 172
		private DataSet datos = new DataSet("Catrastro");

		// Token: 0x040000AD RID: 173
		private DataTable Tabla_Productores = new DataTable("FEAL");

		// Token: 0x040000AE RID: 174
		private DataSet datos2 = new DataSet("Recibidor");

		// Token: 0x040000AF RID: 175
		private DataTable Tabla_Recibidor = new DataTable("FEAL_Recibidor");

		// Token: 0x0200001E RID: 30
		public static class VoiceCodesinFecha
		{
			// Token: 0x06000101 RID: 257 RVA: 0x000100AC File Offset: 0x0000E2AC
			public static string Compute(string GTIN, string lot, DateTime? packDate)
			{
				ushort num = frm_generadorVentana.Crc16.ComputeChecksum(Encoding.ASCII.GetBytes(string.Format("{0}{1}{2}", GTIN, lot, string.Empty)));
				return string.Format("{0:0000}", (int)(num % 10000));
			}
		}

		// Token: 0x0200001F RID: 31
		public static class VoiceCodeconFecha
		{
			// Token: 0x06000102 RID: 258 RVA: 0x000100F8 File Offset: 0x0000E2F8
			public static string Compute(string GTIN, string lot, DateTime? packDate)
			{
				ushort num = frm_generadorVentana.Crc16.ComputeChecksum(Encoding.ASCII.GetBytes(string.Format("{0}{1}{2}", GTIN, lot, (packDate != null) ? packDate.Value.ToString("yyMMdd") : string.Empty)));
				return string.Format("{0:0000}", (int)(num % 10000));
			}
		}

		// Token: 0x02000020 RID: 32
		public static class Crc16
		{
			// Token: 0x06000103 RID: 259 RVA: 0x00010164 File Offset: 0x0000E364
			static Crc16()
			{
				ushort num = 0;
				while ((int)num < frm_generadorVentana.Crc16.table.Length)
				{
					ushort num2 = 0;
					ushort num3 = num;
					for (byte b = 0; b < 8; b += 1)
					{
						if (0 != ((num2 ^ num3) & 1))
						{
							num2 = (ushort)(num2 >> 1 ^ 40961);
						}
						else
						{
							num2 = (ushort)(num2 >> 1);
						}
						num3 = (ushort)(num3 >> 1);
					}
					frm_generadorVentana.Crc16.table[(int)num] = num2;
					num += 1;
				}
			}

			// Token: 0x06000104 RID: 260 RVA: 0x000101E8 File Offset: 0x0000E3E8
			public static ushort ComputeChecksum(byte[] bytes)
			{
				ushort num = 0;
				for (int i = 0; i < bytes.Length; i++)
				{
					byte b = (byte)(num ^ (ushort)bytes[i]);
					num = (ushort)(num >> 8 ^ (int)frm_generadorVentana.Crc16.table[(int)b]);
				}
				return num;
			}

			// Token: 0x040000E0 RID: 224
			private const ushort polynomial = 40961;

			// Token: 0x040000E1 RID: 225
			private static ushort[] table = new ushort[256];
		}

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void sfd_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void cbx_pallets_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
