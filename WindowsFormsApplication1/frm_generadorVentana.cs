using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;

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
			this.LeerXML();
			this.LeerXML2();
			this.LlenaEmbalaje();
			this.Llena_Productor();
			this.Llena_Recibidor();
			this.pb_etiqueta.Image = null;
			this.cmb_titulo2.SelectedIndex = 0;
			this.cmb_Recibidor.SelectedIndex = 0;
			this.cmb_packing.SelectedIndex = 0;
			this.cmb_cat1.SelectedIndex = 0;
			this.cmb_productor.SelectedIndex = 0;
			this.cbx_pallets.SelectedIndex = 1;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x0000ABB0 File Offset: 0x00008DB0
		private void Llena_Recibidor()
		{
			try
			{
				this.Tabla_Recibidor.ReadXml("Recibidor.xml");
				EnumerableRowCollection<DataRow> source = from contact in this.Tabla_Recibidor.AsEnumerable()
				select contact;
				DataView dataView = source.AsDataView<DataRow>();
				dataView.Sort = "Recibidor asc";
				this.cmb_Recibidor.DataSource = dataView.ToTable(true, new string[]
				{
					"Recibidor"
				});
				this.cmb_Recibidor.DisplayMember = "Recibidor";
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
				this.Tabla_Productores.ReadXml("Catrastro.xml");
				EnumerableRowCollection<DataRow> source = from contact in this.Tabla_Productores.AsEnumerable()
				select contact;
				DataView dataView = source.AsDataView<DataRow>();
				dataView.Sort = "Productor DESC";
				this.cmb_productor.DataSource = dataView.ToTable(true, new string[]
				{
					"Productor"
				});
				this.cmb_productor.DisplayMember = "Productor";
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
			List<frm_generadorVentana.Item> list = new List<frm_generadorVentana.Item>();
			if (!this.chb_pesofijo.Checked)
			{
				list.Add(new frm_generadorVentana.Item("BP", 1));
				list.Add(new frm_generadorVentana.Item("BSUAC", 1));
				list.Add(new frm_generadorVentana.Item("BSUBI", 1));
				list.Add(new frm_generadorVentana.Item("BSUCH", 1));
				list.Add(new frm_generadorVentana.Item("BSUDR", 1));
				list.Add(new frm_generadorVentana.Item("BSUG2", 1));
				list.Add(new frm_generadorVentana.Item("BSUGF", 1));
				list.Add(new frm_generadorVentana.Item("BSU01", 1));
				list.Add(new frm_generadorVentana.Item("BSU02", 1));
				list.Add(new frm_generadorVentana.Item("BSUPF", 1));
				list.Add(new frm_generadorVentana.Item("BSURD", 1));
				list.Add(new frm_generadorVentana.Item("BSUSC", 1));
				list.Add(new frm_generadorVentana.Item("BSUSF", 1));
				list.Add(new frm_generadorVentana.Item("BSUSG", 1));
				list.Add(new frm_generadorVentana.Item("BSUSS", 1));
				list.Add(new frm_generadorVentana.Item("BSUSW", 1));
				list.Add(new frm_generadorVentana.Item("BZUAC", 1));
				list.Add(new frm_generadorVentana.Item("BZUALDI", 1));
				list.Add(new frm_generadorVentana.Item("BZUG1", 1));
				list.Add(new frm_generadorVentana.Item("BZUG2", 1));
				list.Add(new frm_generadorVentana.Item("BZUHB", 1));
				list.Add(new frm_generadorVentana.Item("BZUNF", 1));
				list.Add(new frm_generadorVentana.Item("BZU01", 1));
				list.Add(new frm_generadorVentana.Item("BZU01KR", 1));
				list.Add(new frm_generadorVentana.Item("BZUTI", 1));
				list.Add(new frm_generadorVentana.Item("PPS", 1));
				list.Add(new frm_generadorVentana.Item("PPZ", 1));
				list.Add(new frm_generadorVentana.Item("SL", 1));
			}
			else
			{
				list.Add(new frm_generadorVentana.Item("CL15", 1));
				list.Add(new frm_generadorVentana.Item("CL27", 1));
				list.Add(new frm_generadorVentana.Item("CL29", 1));
				list.Add(new frm_generadorVentana.Item("CL38BI", 1));
				list.Add(new frm_generadorVentana.Item("CL38BIW", 1));
				list.Add(new frm_generadorVentana.Item("CL38C", 1));
				list.Add(new frm_generadorVentana.Item("CL38D", 1));
				list.Add(new frm_generadorVentana.Item("CL38GF", 1));
				list.Add(new frm_generadorVentana.Item("CL38KR", 1));
				list.Add(new frm_generadorVentana.Item("CL38S", 1));
				list.Add(new frm_generadorVentana.Item("CL38W", 1));
				list.Add(new frm_generadorVentana.Item("CL38WE", 1));
				list.Add(new frm_generadorVentana.Item("PG15", 1));
			}
			this.cmb_tipo_embalaje.DisplayMember = "Name";
			this.cmb_tipo_embalaje.ValueMember = "Value";
			this.cmb_tipo_embalaje.DataSource = list;
			this.cmb_tipo_embalaje.SelectedIndex = 0;
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
			List<frm_generadorVentana.Item> list = new List<frm_generadorVentana.Item>();
			try
			{
				if (this.cmb_variedad.Text.ToString() == "ALLISON")
				{
					list.Add(new frm_generadorVentana.Item("15 Sheegene 20 -_Allison™", 1));
					list.Add(new frm_generadorVentana.Item("15 Sheegene 20", 1));
					list.Add(new frm_generadorVentana.Item("00 Red Seedless_'Unknown Variety'", 1));
				}
				if (this.cmb_variedad.Text.ToString() == "ARRA 15")
				{
					list.Add(new frm_generadorVentana.Item("11 Arrafifteen -_Arra15", 1));
					list.Add(new frm_generadorVentana.Item("00 Green Seedless_'Unknown Variety'", 1));
				}
				if (this.cmb_variedad.Text.ToString() == "ARRA 29")
				{
					list.Add(new frm_generadorVentana.Item("27 Arratwentynine -_Arra29", 1));
					list.Add(new frm_generadorVentana.Item("00 Red Seedless_'Unknown Variety'", 1));
				}
				if (this.cmb_variedad.Text.ToString() == "ARRA 35")
				{
					list.Add(new frm_generadorVentana.Item("51 Arrathirtyfive -_Arra35", 1));
					list.Add(new frm_generadorVentana.Item("00 Red Seedless_'Unknown Variety'", 1));
				}
				if (this.cmb_variedad.Text.ToString() == "AUTUMN CRISP")
				{
					list.Add(new frm_generadorVentana.Item("38 Sugrathirtyfive -_Autumn Crisp®", 1));
					list.Add(new frm_generadorVentana.Item("38 Sugrathirtyfive", 1));
					list.Add(new frm_generadorVentana.Item("00 Green Seedless_'Unknown Variety'", 1));
				}
				if (this.cmb_variedad.Text.ToString() == "AUTUMN ROYAL")
				{
					list.Add(new frm_generadorVentana.Item("09 Autumn Royal", 1));
					list.Add(new frm_generadorVentana.Item("00 Black Seedless_'Unknown Variety'", 1));
				}
				if (this.cmb_variedad.Text.ToString() == "CANDY HEARTS")
				{
					list.Add(new frm_generadorVentana.Item("35 IFG Nineteen -_Candy Hearts™", 1));
					list.Add(new frm_generadorVentana.Item("00 Red Seedless_'Unknown Variety'", 1));
				}
				if (this.cmb_variedad.Text.ToString() == "FLAME SEEDLESS")
				{
					list.Add(new frm_generadorVentana.Item("03 Flame Seedless", 1));
					list.Add(new frm_generadorVentana.Item("00 Red Seedless_'Unknown Variety'", 1));
				}
				if (this.cmb_variedad.Text.ToString() == "GREAT GREEN")
				{
					list.Add(new frm_generadorVentana.Item("30 Sheegene 17 -_Great Green™", 1));
					list.Add(new frm_generadorVentana.Item("30 Sheegene 17", 1));
					list.Add(new frm_generadorVentana.Item("00 Green Seedless_'Unknown Variety'", 1));
				}
				if (this.cmb_variedad.Text.ToString() == "INIAGRAPE-ONE (MAYLEN)")
				{
					list.Add(new frm_generadorVentana.Item("91 Maylen®_(Iniagrape-one cv.)", 1));
					list.Add(new frm_generadorVentana.Item("16 Iniagrape-one cv.", 1));
					list.Add(new frm_generadorVentana.Item("00 Black Seedless_'Unknown Variety'", 1));
				}
				if (this.cmb_variedad.Text.ToString() == "IVORY")
				{
					list.Add(new frm_generadorVentana.Item("34 Sheegene 21 -_Ivory™", 1));
					list.Add(new frm_generadorVentana.Item("34 Sheegene 21", 1));
					list.Add(new frm_generadorVentana.Item("00 Green Seedless_'Unknown Variety'", 1));
				}
				if (this.cmb_variedad.Text.ToString() == "KRISSY")
				{
					list.Add(new frm_generadorVentana.Item("21 Sheegene 12 -_Krissy™", 1));
					list.Add(new frm_generadorVentana.Item("21 Sheegene 12", 1));
					list.Add(new frm_generadorVentana.Item("00 Red Seedless_'Unknown Variety'", 1));
				}
				if (this.cmb_variedad.Text.ToString() == "MELODY")
				{
					list.Add(new frm_generadorVentana.Item("22 Blagratwo -_Melody™", 1));
					list.Add(new frm_generadorVentana.Item("22 Blagratwo", 1));
					list.Add(new frm_generadorVentana.Item("00 Black Seedless_'Unknown Variety'", 1));
				}
				if (this.cmb_variedad.Text.ToString() == "PRIME SEEDLESS")
				{
					list.Add(new frm_generadorVentana.Item("08 Prime Seedless", 1));
					list.Add(new frm_generadorVentana.Item("00 Green Seedless_'Unknown Variety'", 1));
				}
				if (this.cmb_variedad.Text.ToString() == "RED GLOBE")
				{
					list.Add(new frm_generadorVentana.Item("04 Red Globe", 1));
				}
				if (this.cmb_variedad.Text.ToString() == "SUGRA 53")
				{
					list.Add(new frm_generadorVentana.Item("00 Sugra53", 1));
					list.Add(new frm_generadorVentana.Item("00 Red Seedless_'Unknown Variety'", 1));
				}
				if (this.cmb_variedad.Text.ToString() == "SUGRAONE")
				{
					list.Add(new frm_generadorVentana.Item("05 Sugraone", 1));
					list.Add(new frm_generadorVentana.Item("00 Green Seedless_'Unknown Variety'", 1));
				}
				if (this.cmb_variedad.Text.ToString() == "SWEET CELEBRATION")
				{
					list.Add(new frm_generadorVentana.Item("32 IFG Three -_Sweet Celebration™", 1));
					list.Add(new frm_generadorVentana.Item("00 Red Seedless_'Unknown Variety'", 1));
				}
				if (this.cmb_variedad.Text.ToString() == "SWEET FAVORS")
				{
					list.Add(new frm_generadorVentana.Item("31 IFG Sixteen -_Sweet Favors™", 1));
					list.Add(new frm_generadorVentana.Item("00 Black Seedless_'Unknown Variety'", 1));
				}
				if (this.cmb_variedad.Text.ToString() == "SWEET GLOBE")
				{
					list.Add(new frm_generadorVentana.Item("36 IFG Ten -_Sweet Globe™", 1));
					list.Add(new frm_generadorVentana.Item("00 Green Seedless_'Unknown Variety'", 1));
				}
				if (this.cmb_variedad.Text.ToString() == "SWEET NECTAR")
				{
					list.Add(new frm_generadorVentana.Item("50 IFG Eighteen -_Sweet Nectar™", 1));
					list.Add(new frm_generadorVentana.Item("00 Red Seedless_'Unknown Variety'", 1));
				}
				if (this.cmb_variedad.Text.ToString() == "THOMPSON SEEDLESS")
				{
					list.Add(new frm_generadorVentana.Item("02 Thompson Seedless", 1));
					list.Add(new frm_generadorVentana.Item("00 Green Seedless_'Unknown Variety'", 1));
				}
				if (this.cmb_variedad.Text.ToString() == "TIMCO")
				{
					list.Add(new frm_generadorVentana.Item("17 Sheegene 13 -_Timco™", 1));
					list.Add(new frm_generadorVentana.Item("17 Sheegene 13", 1));
					list.Add(new frm_generadorVentana.Item("00 Red Seedless_'Unknown Variety'", 1));
				}
				if (this.cmb_variedad.Text.ToString() == "TIMPSON")
				{
					list.Add(new frm_generadorVentana.Item("23 Sheegene 2 -_Timpson™", 1));
					list.Add(new frm_generadorVentana.Item("23 Sheegene 2", 1));
					list.Add(new frm_generadorVentana.Item("00 Green Seedless_'Unknown Variety'", 1));
				}
				if (this.cmb_variedad.Text.ToString() == "BLACK SEEDLESS")
				{
					list.Add(new frm_generadorVentana.Item("00 Black Seedless 'Unknown Variety'", 1));
				}
				if (this.cmb_variedad.Text.ToString() == "RED SEEDLESS")
				{
					list.Add(new frm_generadorVentana.Item("00 Red Seedless 'Unknown Variety'", 1));
				}
				if (this.cmb_variedad.Text.ToString() == "GREEN SEEDLESS")
				{
					list.Add(new frm_generadorVentana.Item("00 Green Seedless 'Unknown Variety'", 1));
				}
				if (this.cmb_variedad.Text.ToString() == "SUGRAFIFTYTHREE (RUBY RUSH)")
				{
					list.Add(new frm_generadorVentana.Item("00 Red Seedless_'Unknown Variety'", 1));
				}
				if (this.cmb_variedad.Text.ToString() == "ARDTHITYFIVE (FIRE CRUNCH)")
				{
					list.Add(new frm_generadorVentana.Item("00 Red Seedless_'Unknown Variety'", 1));
				}
				this.cmb_variedad_Imprime.DisplayMember = "Name";
				this.cmb_variedad_Imprime.ValueMember = "Value";
				this.cmb_variedad_Imprime.DataSource = list;
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x0000BC54 File Offset: 0x00009E54
	private void Llena_lotes()
	{
		try
		{
			EnumerableRowCollection<DataRow> source = from contact in this.Tabla_Productores.AsEnumerable()
			where contact.Field<string>("Productor") == this.cmb_productor.Text.ToString() && contact.Field<string>("Variedad") == this.cmb_variedad.Text.ToString()
			select contact;
				DataView dataView = source.AsDataView<DataRow>();
				this.cmb_lote.DataSource = dataView.ToTable(true, new string[]
				{
					"Lote"
				});
				this.cmb_lote.DisplayMember = "Lote";
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x0000BCE0 File Offset: 0x00009EE0
		private void LlenaCalibres()
		{
			List<frm_generadorVentana.Item> list = new List<frm_generadorVentana.Item>();
			if (this.cmb_variedad_Imprime.Text.Trim() == "00" || this.cmb_variedad_Imprime.Text.Trim() == "00")
			{
				list.Add(new frm_generadorVentana.Item("XJ", 1));
				list.Add(new frm_generadorVentana.Item("J", 2));
				list.Add(new frm_generadorVentana.Item("D", 3));
				list.Add(new frm_generadorVentana.Item("V", 4));
				list.Add(new frm_generadorVentana.Item("XXL", 6));
				list.Add(new frm_generadorVentana.Item("XL", 7));
				this.cmb_calibre.DisplayMember = "Name";
				this.cmb_calibre.ValueMember = "Value";
				this.cmb_calibre.DataSource = list;
			}
			else
			{
				list.Add(new frm_generadorVentana.Item("XXJ", 1));
				list.Add(new frm_generadorVentana.Item("XJ", 2));
				list.Add(new frm_generadorVentana.Item("J", 3));
				list.Add(new frm_generadorVentana.Item("D", 4));
				list.Add(new frm_generadorVentana.Item("V", 5));
				list.Add(new frm_generadorVentana.Item("A", 6));
				list.Add(new frm_generadorVentana.Item("R", 7));
				list.Add(new frm_generadorVentana.Item("T", 8));
				list.Add(new frm_generadorVentana.Item("XXL", 9));
				list.Add(new frm_generadorVentana.Item("XL", 10));
				list.Add(new frm_generadorVentana.Item("L", 11));
				list.Add(new frm_generadorVentana.Item("M", 12));
				list.Add(new frm_generadorVentana.Item("JJ", 13));
				list.Add(new frm_generadorVentana.Item("DD", 14));
				list.Add(new frm_generadorVentana.Item("VV", 15));
				list.Add(new frm_generadorVentana.Item("AA", 16));
				list.Add(new frm_generadorVentana.Item("RR", 17));
				list.Add(new frm_generadorVentana.Item("R10", 18));
				list.Add(new frm_generadorVentana.Item("AA11", 19));
				list.Add(new frm_generadorVentana.Item("A22", 20));
				list.Add(new frm_generadorVentana.Item("VV33", 21));
				list.Add(new frm_generadorVentana.Item("V44", 22));
				list.Add(new frm_generadorVentana.Item("DD55", 23));
				list.Add(new frm_generadorVentana.Item("D66", 24));
				list.Add(new frm_generadorVentana.Item("JJ77", 25));
				list.Add(new frm_generadorVentana.Item("J88", 26));
				this.cmb_calibre.DisplayMember = "Name";
				this.cmb_calibre.ValueMember = "Value";
				this.cmb_calibre.DataSource = list;
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
			//BarcodeGenerator barcodeGenerator = new BarcodeGenerator();
			//BarcodeGenerator_PLU barcodeGenerator_PLU = new BarcodeGenerator_PLU();
			Graphics g = Graphics.FromImage(new Bitmap(1, 1));
			Graphics graphics = Graphics.FromImage(new Bitmap(1, 1));
			Bitmap image = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
			g = Graphics.FromImage(image);
			graphics = Graphics.FromImage(image);
			string empty = string.Empty;
			SizeF sizeF = default(SizeF);
			string empty2 = string.Empty;
			Image image2 = null;// barcodeGenerator.DrawCode128(g, "123456789", 0, 0);
			string text3 = this.cmb_titulo2.Text.Trim();
			string text4 = this.cmb_productor.Text.Trim().Substring(0, 6).ToString() + this.cmb_variedad_Imprime.Text.Trim().Substring(0, 2).ToString() + this.cmb_lote.Text.Trim();
			string empty3 = string.Empty;
			string empty4 = string.Empty;
			string str;
			string str2;
			if (this.cmb_packing.Text.Trim().Substring(0, 3).ToString() == "126" || this.cmb_packing.Text.Trim().Substring(0, 3).ToString() == "127" || this.cmb_packing.Text.Trim().Substring(0, 3).ToString() == "128" || this.cmb_packing.Text.Trim().Substring(0, 3).ToString() == "129" || this.cmb_packing.Text.Trim().Substring(0, 3).ToString() == "130" || this.cmb_packing.Text.Trim().Substring(0, 3).ToString() == "131" || this.cmb_packing.Text.Trim().Substring(0, 3).ToString() == "132" || this.cmb_packing.Text.Trim().Substring(0, 3).ToString() == "146" || this.cmb_packing.Text.Trim().Substring(0, 3).ToString() == "147")
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
			if (this.cmb_productor.Text.Trim().ToString() == "106957 HUANCARA" || this.cmb_productor.Text.Trim().ToString() == "106958 MAITENCILLO" || this.cmb_productor.Text.Trim().ToString() == "106955 SANTA BERNARDITA" || this.cmb_productor.Text.Trim().ToString() == "106956 SANTA ADRIANA" || this.cmb_productor.Text.Trim().ToString() == "87197 LA COMPAÑÍA" || this.cmb_productor.Text.Trim().ToString() == "89323 LOS PIMIENTOS")
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
			for (int i = 0; i < bitmap.Width; i++)
			{
				for (int j = 0; j < bitmap.Height; j++)
				{
					bitmap.SetPixel(i, j, Color.White);
					if (i > 390 & i < 928 & j > 200 & j < 203)
					{
						bitmap.SetPixel(i, j, Color.Black);
					}
					if (i > 390 & i < 928 & j > 250 & j < 253)
					{
						bitmap.SetPixel(i, j, Color.Black);
					}
					if (i > 390 & i < 928 & j > 300 & j < 303)
					{
						bitmap.SetPixel(i, j, Color.Black);
					}
					if (this.cbx_sdp.Text.ToString() != "")
					{
						if (i > 390 & i < 928 & j > 350 & j < 353)
						{
							bitmap.SetPixel(i, j, Color.Black);
						}
						if (i > 390 & i < 393 & j > 303 & j < 353)
						{
							bitmap.SetPixel(i, j, Color.Black);
						}
						if (i > 735 & i < 738 & j > 303 & j < 353)
						{
							bitmap.SetPixel(i, j, Color.Black);
						}
						if (i > 925 & i < 928 & j > 303 & j < 353)
						{
							bitmap.SetPixel(i, j, Color.Black);
						}
					}
					if (i > 390 & i < 393 & j > 200 & j < 303)
					{
						bitmap.SetPixel(i, j, Color.Black);
					}
					if (i > 735 & i < 738 & j > 200 & j < 303)
					{
						bitmap.SetPixel(i, j, Color.Black);
					}
					if (i > 925 & i < 928 & j > 200 & j < 303)
					{
						bitmap.SetPixel(i, j, Color.Black);
					}
				}
			}
			Graphics graphics2 = Graphics.FromImage(bitmap);
			graphics2.SmoothingMode = SmoothingMode.AntiAlias;
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
			string text5 = this.cmb_variedad_Imprime.Text.Trim().Substring(3).ToString();
			string[] array2;
			if (this.cmb_variedad_Imprime.Text.Trim() == "10 T Seedless" || this.cmb_variedad_Imprime.Text.Trim() == "16 Iniagrape-one cv." || this.cmb_variedad_Imprime.Text.Trim() == "23 Sheegene 2" || this.cmb_variedad_Imprime.Text.Trim() == "34 Sheegene 21" || this.cmb_variedad_Imprime.Text.Trim() == "30 Sheegene 17" || this.cmb_variedad_Imprime.Text.Trim() == "21 Sheegene 12" || this.cmb_variedad_Imprime.Text.Trim() == "17 Sheegene 13" || this.cmb_variedad_Imprime.Text.Trim() == "15 Sheegene 20")
			{
				array2 = text5.Split(new char[]
				{
					'X'
				});
			}
			else if (this.cmb_variedad_Imprime.Text.Trim() == "31 IFG Sixteen -_Sweet Favors™" || this.cmb_variedad_Imprime.Text.Trim() == "36 IFG Ten -_Sweet Globe™" || this.cmb_variedad_Imprime.Text.Trim() == "30 Sheegene 17 -_Great Green™" || this.cmb_variedad_Imprime.Text.Trim() == "23 Sheegene 2 -_Timpson™" || this.cmb_variedad_Imprime.Text.Trim() == "34 Sheegene 21 -_Ivory™" || this.cmb_variedad_Imprime.Text.Trim() == "11 Arrafifteen -_Arra15" || this.cmb_variedad_Imprime.Text.Trim() == "27 Arratwentynine -_Arra29" || this.cmb_variedad_Imprime.Text.Trim() == "32 IFG Three -_Sweet Celebration™" || this.cmb_variedad_Imprime.Text.Trim() == "21 Sheegene 12 -_Krissy™" || this.cmb_variedad_Imprime.Text.Trim() == "17 Sheegene 13 -_Timco™" || this.cmb_variedad_Imprime.Text.Trim() == "38 Sugrathirtyfive -_Autumn Crisp®" || this.cmb_variedad_Imprime.Text.Trim() == "02 IFG Eighteen -_Sweet Nectar™" || this.cmb_variedad_Imprime.Text.Trim() == "51 Arrathirtyfive -_Arra35" || this.cmb_variedad_Imprime.Text.Trim() == "35 IFG Nineteen -_Candy Hearts™" || this.cmb_variedad_Imprime.Text.Trim() == "15 Sheegene 20 -_Allison™" || this.cmb_variedad_Imprime.Text.Trim() == "00 Red Seedless_'Unknown Variety'" || this.cmb_variedad_Imprime.Text.Trim() == "00 Green Seedless_'Unknown Variety'" || this.cmb_variedad_Imprime.Text.Trim() == "00 Black Seedless_'Unknown Variety'" || this.cmb_variedad_Imprime.Text.Trim() == "50 IFG Eighteen -_Sweet Nectar™" || this.cmb_variedad_Imprime.Text.Trim() == "91 Maylen®_(Iniagrape-one cv.)" || this.cmb_variedad_Imprime.Text.Trim() == "22 Blagratwo -_Melody™")
			{
				array2 = text5.Split(new char[]
				{
					'_'
				});
			}
			else
			{
				array2 = text5.Split(new char[]
				{
					' '
				});
			}
			int[] array3 = new int[]
			{
				80,
				76,
				72,
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
			if (array2.Length.ToString() == "1")
			{
				try
				{
					font = new Font("arial", 40f, FontStyle.Bold);
					StringFormat stringFormat3 = new StringFormat();
					stringFormat3.Alignment = StringAlignment.Center;
					SolidBrush brush3 = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
					graphics2.DrawString("", font, brush3, new Point(1280, 70), stringFormat3);
				}
				catch (Exception)
				{
				}
				try
				{
					for (int k = 0; k < 12; k++)
					{
						font = new Font("arial", (float)array3[k], FontStyle.Bold);
						if ((ushort)graphics2.MeasureString(array2[0], font).Width < 400)
						{
							break;
						}
					}
					StringFormat stringFormat4 = new StringFormat();
					stringFormat4.Alignment = StringAlignment.Center;
					SolidBrush brush4 = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
					graphics2.DrawString(array2[0], font, brush4, new Point(1280, 180), stringFormat4);
				}
				catch (Exception)
				{
				}
			}
			else
			{
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				try
				{
					for (int k = 0; k < 12; k++)
					{
						font = new Font("arial", (float)array3[k], FontStyle.Bold);
						if ((ushort)graphics2.MeasureString(array2[0], font).Width < 400)
						{
							break;
						}
						num = k;
					}
					for (int k = 0; k < 12; k++)
					{
						font = new Font("arial", (float)array3[k], FontStyle.Bold);
						if ((ushort)graphics2.MeasureString(array2[1], font).Width < 400)
						{
							break;
						}
						num2 = k;
					}
					if (num > num2)
					{
						num3 = num;
					}
					else
					{
						num3 = num2;
					}
				}
				catch (Exception)
				{
				}
				try
				{
					font = new Font("arial", (float)array3[num3], FontStyle.Bold);
					StringFormat stringFormat3 = new StringFormat();
					stringFormat3.Alignment = StringAlignment.Center;
					SolidBrush brush3 = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
					graphics2.DrawString(array2[0], font, brush3, new Point(1280, 70), stringFormat3);
				}
				catch (Exception)
				{
				}
				try
				{
					font = new Font("arial", (float)array3[num3], FontStyle.Bold);
					StringFormat stringFormat4 = new StringFormat();
					stringFormat4.Alignment = StringAlignment.Center;
					SolidBrush brush4 = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
					graphics2.DrawString(array2[1], font, brush4, new Point(1280, 180), stringFormat4);
				}
				catch (Exception)
				{
				}
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
							if ((ushort)graphics2.MeasureString(array4[1], font).Width < 350)
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
							if ((ushort)graphics2.MeasureString(array4[2], font).Width < 350)
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
					font = new Font("arial", (float)array3[num3], FontStyle.Bold);
					StringFormat stringFormat3 = new StringFormat();
					stringFormat3.Alignment = StringAlignment.Center;
					SolidBrush brush3 = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
					graphics2.DrawString(array4[0], font, brush3, new Point(670, 450), stringFormat3);
				}
				catch (Exception)
				{
				}
				try
				{
					font = new Font("arial", (float)array3[num3], FontStyle.Bold);
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
					font = new Font("arial", (float)array3[num3], FontStyle.Bold);
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
			graphics2.DrawString("CSG " + this.cmb_productor.Text.Trim().Substring(0, 6).ToString(), font, brush7, new Point(832, 260), stringFormat7);
			if (this.cmb_packing.Text.Trim().ToString() == "146 P. Los Pimientos Terreno                      3101432")
			{
				font = new Font("arial", 22f, FontStyle.Bold);
				StringFormat stringFormat8 = new StringFormat();
				stringFormat8.Alignment = StringAlignment.Center;
				SolidBrush brush8 = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
				graphics2.DrawString("CSP " + this.cmb_packing.Text.Trim().Substring(50, 7).ToString(), font, brush8, new Point(832, 210), stringFormat8);
			}
			else
			{
				font = new Font("arial", 22f, FontStyle.Bold);
				StringFormat stringFormat8 = new StringFormat();
				stringFormat8.Alignment = StringAlignment.Center;
				SolidBrush brush8 = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
				graphics2.DrawString("CSP " + this.cmb_packing.Text.Trim().Substring(50, 6).ToString(), font, brush8, new Point(832, 210), stringFormat8);
			}
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
			image2 = bitmap;
			image2.Save("etiqueta.jpg", ImageFormat.Jpeg);
			graphics2.Dispose();
			image2.Dispose();
			this.pb_etiqueta.ImageLocation = "etiqueta.jpg";
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x0000DFC4 File Offset: 0x0000C1C4
		private void chb_pesofijo_CheckedChanged(object sender, EventArgs e)
		{
			this.LlenaEmbalaje();
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
			EnumerableRowCollection<DataRow> source = from contact in this.Tabla_Productores.AsEnumerable()
			where contact.Field<string>("Productor") == this.cmb_productor.Text.ToString()
			select contact;
				DataView dataView = source.AsDataView<DataRow>();
				this.cmb_variedad.DataSource = dataView.ToTable(true, new string[]
				{
					"Variedad"
				});
				this.cmb_variedad.DisplayMember = "Variedad";
			}
			catch (Exception)
			{
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
			EnumerableRowCollection<DataRow> source = from contact in this.Tabla_Productores.AsEnumerable()
			where contact.Field<string>("Productor") == this.cmb_productor.Text.ToString() && contact.Field<string>("Variedad") == this.cmb_variedad.Text.ToString() && contact.Field<string>("Lote") == this.cmb_lote.Text.ToString()
			select contact;
				DataView dataView = source.AsDataView<DataRow>();
				this.cbx_sdp.DataSource = dataView.ToTable(true, new string[]
				{
					"SDP"
				});
				this.cbx_sdp.DisplayMember = "SDP";
			}
			catch (Exception)
			{
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

		// Token: 0x02000021 RID: 33
		public class Item
		{
			// Token: 0x17000025 RID: 37
			// (get) Token: 0x06000105 RID: 261 RVA: 0x00010228 File Offset: 0x0000E428
			// (set) Token: 0x06000106 RID: 262 RVA: 0x0001023F File Offset: 0x0000E43F
			public string Name { get; set; }

			// Token: 0x17000026 RID: 38
			// (get) Token: 0x06000107 RID: 263 RVA: 0x00010248 File Offset: 0x0000E448
			// (set) Token: 0x06000108 RID: 264 RVA: 0x0001025F File Offset: 0x0000E45F
			public int Value { get; set; }

			// Token: 0x06000109 RID: 265 RVA: 0x00010268 File Offset: 0x0000E468
			public Item(string name, int value)
			{
				this.Name = name;
				this.Value = value;
			}

			// Token: 0x0600010A RID: 266 RVA: 0x00010284 File Offset: 0x0000E484
			public override string ToString()
			{
				return this.Name;
			}
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
