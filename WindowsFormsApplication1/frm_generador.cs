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
// using LabelKit;
// using LabelKit_2022;
// using LabelKit_8045;
// using LabelKit_PLU_2022;
// using LabelKit_PLU_8045;
using WindowsFormsApplication1.Data;

namespace WindowsFormsApplication1
{
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

		/// <summary>
		/// Busca y retorna el código final para etiquetas PLU con peso fijo
		/// Realiza validaciones específicas para diferentes combinaciones de producto y GTIN
		/// </summary>
		/// <returns>Código final ajustado según el producto y GTIN seleccionado</returns>
		//public string Busca_CodigoFinalCodigo_PLU_PESO_FIJO()
		// Método duplicado, usar la versión que consulta la base de datos al final del archivo
		// {
		//     var db = Data.DatabaseManager.Instance;
		//     string producto = this.cmb_titulo1.Text.Trim();
		//     string gtin = this.cmb_gtin.Text.Trim();
		//     // Para peso fijo, normalmente tipoEmbalaje y variedadImprime pueden ser null
		//     return db.GetCodigoFinalPLU(producto, gtin, null, null);
		// }

		/// <summary>
		/// Busca y retorna el código final para etiquetas PLU estándar
		/// Realiza validaciones cruzadas entre variedad, GTIN y tipo de embalaje
		/// para determinar el código correcto a usar en la etiqueta
		/// </summary>
		/// <returns>Código final específico para la combinación seleccionada</returns>
		//public string Busca_CodigoFinalCodigo_PLU()
		// Método duplicado, usar la versión que consulta la base de datos al final del archivo
		// {
		//     var db = Data.DatabaseManager.Instance;
		//     string producto = this.cmb_titulo1.Text.Trim();
		//     string gtin = this.cmb_gtin.Text.Trim();
		//     string tipoEmbalaje = this.cmb_tipo_embalaje.Text.Trim();
		//     string variedadImprime = this.cmb_variedad_Imprime.Text.Trim();
		//     return db.GetCodigoFinalPLU(producto, gtin, tipoEmbalaje, variedadImprime);
		// }

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
	private void Form1_Load(object sender, EventArgs e)
	{
		// Carga la configuración desde el archivo XML
		this.LeerXML();
		// Llena el combo de tipos de embalaje
		this.LlenaEmbalaje();
		// Llena el combo de productores
		this.Llena_Productor();
		// Limpia la imagen de vista previa
		this.pb_etiqueta.Image = null;
		// Establece valores predeterminados en los combos
		this.cmb_titulo2.SelectedIndex = 0;
		this.cmb_packing.SelectedIndex = 0;
		this.cmb_cat1.SelectedIndex = 0;
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
				// Dibuja la imagen de la etiqueta en la esquina superior izquierda
				graphics.DrawImage(this.pb_etiqueta.Image, 0, 0);
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
            // List<frm_generador.Item> list = new List<frm_generador.Item>();
            // if (!this.chb_pesofijo.Checked)
            // {
            //     // Tipos de embalaje para peso variable
            //     list.Add(new frm_generador.Item("BP", 1));
            //     list.Add(new frm_generador.Item("BSUAC", 1));
            //     list.Add(new frm_generador.Item("BSUBI", 1));
            //     list.Add(new frm_generador.Item("BSUCH", 1));
            //     list.Add(new frm_generador.Item("BSUDR", 1));
            //     list.Add(new frm_generador.Item("BSUG2", 1));
            //     list.Add(new frm_generador.Item("BSUGF", 1));
            //     list.Add(new frm_generador.Item("BSU01", 1));
            //     list.Add(new frm_generador.Item("BSU02", 1));
            //     list.Add(new frm_generador.Item("BSUPF", 1));
            //     list.Add(new frm_generador.Item("BSURD", 1));
            //     list.Add(new frm_generador.Item("BSUSC", 1));
            //     list.Add(new frm_generador.Item("BSUSF", 1));
            //     list.Add(new frm_generador.Item("BSUSG", 1));
            //     list.Add(new frm_generador.Item("BSUSS", 1));
            //     list.Add(new frm_generador.Item("BSUSW", 1));
            //     list.Add(new frm_generador.Item("BZUAC", 1));
            //     list.Add(new frm_generador.Item("BZUALDI", 1));
            //     list.Add(new frm_generador.Item("BZUG1", 1));
            //     list.Add(new frm_generador.Item("BZUG2", 1));
            //     list.Add(new frm_generador.Item("BZUHB", 1));
            //     list.Add(new frm_generador.Item("BZUNF", 1));
            //     list.Add(new frm_generador.Item("BZU01", 1));
            //     list.Add(new frm_generador.Item("BZU01KR", 1));
            //     list.Add(new frm_generador.Item("BZUTI", 1));
            //     list.Add(new frm_generador.Item("PPS", 1));
            //     list.Add(new frm_generador.Item("PPZ", 1));
            //     list.Add(new frm_generador.Item("SL", 1));
            // }
            // else
            // {
            //     // Tipos de embalaje para peso fijo (clamshells y bolsas)
            //     list.Add(new frm_generador.Item("CL15", 1));
            //     list.Add(new frm_generador.Item("CL27", 1));
            //     list.Add(new frm_generador.Item("CL29", 1));
            //     list.Add(new frm_generador.Item("CL38BI", 1));
            //     list.Add(new frm_generador.Item("CL38BIW", 1));
            //     list.Add(new frm_generador.Item("CL38C", 1));
            //     list.Add(new frm_generador.Item("CL38D", 1));
            //     list.Add(new frm_generador.Item("CL38GF", 1));
            //     list.Add(new frm_generador.Item("CL38KR", 1));
            //     list.Add(new frm_generador.Item("CL38S", 1));
            //     list.Add(new frm_generador.Item("CL38W", 1));
            //     list.Add(new frm_generador.Item("CL38WE", 1));
            //     list.Add(new frm_generador.Item("PG15", 1));
            // }
            // Configura el combo box con la lista de tipos de embalaje
            // Consulta la base de datos y filtra por el campo peso_fijo
            var db = DatabaseManager.Instance;
            var embalajes = db.GetTipoEmbalajePorPesoFijo(this.chb_pesofijo.Checked);
            this.cmb_tipo_embalaje.DisplayMember = "Dato";
            this.cmb_tipo_embalaje.ValueMember = "Id";
            this.cmb_tipo_embalaje.DataSource = embalajes;
            this.cmb_tipo_embalaje.SelectedIndex = embalajes.Count > 0 ? 0 : -1;
        }

		// Token: 0x06000118 RID: 280 RVA: 0x00014185 File Offset: 0x00012385
		private void cmb_titulo1_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.pb_etiqueta.Image = null;
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00014195 File Offset: 0x00012395
		private void cmb_gtin_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.pb_etiqueta.Image = null;
		}

		// Token: 0x0600011A RID: 282 RVA: 0x000141A5 File Offset: 0x000123A5
		private void cmb_variedad_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.Llena_lotes();
			this.Llena_variedad_imprime();
			this.pb_etiqueta.Image = null;
		}

		// Token: 0x0600011B RID: 283 RVA: 0x000141C4 File Offset: 0x000123C4
		private void Llena_variedad_imprime()
		{
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
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00014A14 File Offset: 0x00012C14
		private void Llena_GTIN()
		{
			if (!this.chb_pesofijo.Checked)
			{
				List<Item> list = new List<Item>();
				if (this.cmb_variedad_Imprime.Text.Trim() == "11 Arrafifteen - Arra15")
				{
					this.cmb_titulo1.SelectedItem = "Table Grapes, Green Seedless";
					list.Add(new Item("17808771834912", 1));
					list.Add(new Item("17808771844980", 2));
					list.Add(new Item("17808771840227", 3));
					list.Add(new Item("17808771833434", 4));
					list.Add(new Item("17808771842757", 1));
					list.Add(new Item("17808771840220", 3));
					list.Add(new Item("17808771844982", 2));
					list.Add(new Item("1780877184022K", 5));
					this.cmb_gtin.DisplayMember = "Code";
					this.cmb_gtin.ValueMember = "Id";
					this.cmb_gtin.DataSource = list;
				}
				else if (this.cmb_variedad_Imprime.Text.Trim() == "12 Arratwelve - Arra12")
				{
					this.cmb_titulo1.SelectedItem = "Table Grapes, Green Seedless";
					list.Add(new Item("17808771834929", 1));
					list.Add(new Item("17808771846359", 2));
					list.Add(new Item("17808771840234", 3));
					list.Add(new Item("17808771833441", 1));
					list.Add(new Item("17808771842757", 1));
					list.Add(new Item("17808771840230", 3));
					list.Add(new Item("17808771846350", 2));
					list.Add(new Item("1780877184023K", 5));
					this.cmb_gtin.DisplayMember = "Code";
					this.cmb_gtin.ValueMember = "Id";
					this.cmb_gtin.DataSource = list;
				}
				else if (this.cmb_variedad_Imprime.Text.Trim() == "13 Arrasixteen - Arra16")
				{
					this.cmb_titulo1.SelectedItem = "Table Grapes, Black Seedless";
					list.Add(new Item("17808771840562", 1));
					list.Add(new Item("17808771842757", 1));
					list.Add(new Item("1780877184056K", 3));
					this.cmb_gtin.DisplayMember = "Code";
					this.cmb_gtin.ValueMember = "Id";
					this.cmb_gtin.DataSource = list;
				}
				else if (this.cmb_variedad_Imprime.Text.Trim() == "14 Arraseventeen - Arra17")
				{
					this.cmb_titulo1.SelectedItem = "Table Grapes, Black Seedless";
					list.Add(new Item("17808771840562", 1));
					list.Add(new Item("17808771842757", 1));
					this.cmb_gtin.DisplayMember = "Code";
					this.cmb_gtin.ValueMember = "Id";
					this.cmb_gtin.DataSource = list;
				}
			}
		}

		private void button7_Click(object sender, EventArgs e)
		{
			// Implementar lógica si es necesario
		}

		private void ButtonSave_MouseLeave(object sender, EventArgs e)
		{
			// Implementar lógica si es necesario
		}

		private void ButtonSave_MouseEnter(object sender, EventArgs e)
		{
			// Implementar lógica si es necesario
		}

		private void cmb_packing_SelectedIndexChanged(object sender, EventArgs e)
		{
			// Implementar lógica si es necesario
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
			// Implementar lógica si es necesario
		}

		private void cmb_variedad_Imprime_SelectedIndexChanged(object sender, EventArgs e)
		{
			// Implementar lógica si es necesario
		}

		private void cmb_cat1_SelectedIndexChanged(object sender, EventArgs e)
		{
			// Implementar lógica si es necesario
		}

		private void cmb_lote_SelectedIndexChanged(object sender, EventArgs e)
		{
			// Implementar lógica si es necesario
		}

		private void cmb_calibre_SelectedIndexChanged(object sender, EventArgs e)
		{
			// Implementar lógica si es necesario
		}

		private void cmb_productor_SelectedIndexChanged(object sender, EventArgs e)
		{
			// Implementar lógica si es necesario
		}

        private void LeerXML() { }
        private void Llena_Productor() { }
        private void Llena_lotes() { }
        private void label1_Click(object sender, EventArgs e) { }
        private void Button_MouseEnter(object sender, EventArgs e) { }
        private void Button_MouseLeave(object sender, EventArgs e) { }
        private void label8_Click(object sender, EventArgs e) { }
	}
}
