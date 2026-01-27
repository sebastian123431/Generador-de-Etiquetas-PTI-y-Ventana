using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
	/// <summary>
	/// Formulario generador de etiquetas tipo VENTANA para productos CÍTRICOS
	/// Genera etiquetas especializadas para productos cítricos con información específica del sector
	/// Utiliza formato 8045 para códigos de barras
	/// </summary>
	public partial class frm_generadorVentana_Citricos : Form
	{
		/// <summary>
		/// Constructor del formulario generador de etiquetas de ventana para cítricos
		/// Inicializa todos los componentes visuales del formulario
		/// </summary>
		public frm_generadorVentana_Citricos()
		{
			this.InitializeComponent();
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

		/// <summary>
		/// Evento al salir del campo de fecha
		/// Valida que la fecha ingresada esté en formato correcto dd-MM-yyyy
		/// Si el formato es incorrecto, muestra un mensaje y devuelve el foco al campo
		/// </summary>
		/// <param name="sender">Control que disparó el evento</param>
		/// <param name="e">Argumentos del evento</param>
		private void txt_date_Leave(object sender, EventArgs e)
		{
			if (this.txt_date.Text.Trim() != "")
			{
				try
				{
					// Intenta parsear la fecha en formato dd-MM-yyyy
					DateTime dateTime = DateTime.ParseExact(this.txt_date.Text.Trim(), "dd-MM-yyyy", null);
				}
				catch
				{
					MessageBox.Show("Formato de Fecha Incorrecto.");
					this.txt_date.Focus();
				}
			}
		}

		/// <summary>
		/// Evento de carga del formulario de cítricos
		/// Inicializa los datos cargando variedades, tipos de embalaje y estableciendo valores predeterminados
		/// </summary>
		/// <param name="sender">Objeto que disparó el evento</param>
		/// <param name="e">Argumentos del evento</param>
		private void Form1_Load(object sender, EventArgs e)
		{
			// Llena el combo de variedades de cítricos
			this.LlenaVariedad();
			// Llena el combo de tipos de embalaje
			this.LlenaEmbalaje();
			// Limpia la imagen de vista previa
			this.pb_etiqueta.Image = null;
			// Establece valores predeterminados en los combos
			this.cmb_titulo2.SelectedIndex = 0;
			this.cmb_Recibidor.SelectedIndex = 0;
			this.cmb_packing.SelectedIndex = 0;
			this.cmb_calibre.SelectedIndex = 0;
			this.cmb_cat1.SelectedIndex = 0;
			this.cmb_productor.SelectedIndex = 0;
			this.cbx_pallets.SelectedIndex = 1;
		}

		/// <summary>
		/// Evento clic del botón Copiar
		/// Copia la imagen de la etiqueta al portapapeles rotándola 90 grados para orientación correcta
		/// Después de copiar, restaura la rotación original de la imagen
		/// </summary>
		/// <param name="sender">Botón que disparó el evento</param>
		/// <param name="e">Argumentos del evento</param>
		private void btn_copiar_Click(object sender, EventArgs e)
		{
			try
			{
				// Rota la imagen 90 grados para orientación de impresión
				this.pb_etiqueta.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
				// Copia al portapapeles
				Clipboard.SetDataObject(this.pb_etiqueta.Image, true);
				// Restaura la rotación original (270 grados = -90 grados)
				this.pb_etiqueta.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
			}
			catch (Exception)
			{
				// Silenciosamente ignora errores de copia
			}
		}

		/// <summary>
		/// Evento clic del botón Imprimir
		/// Envía la etiqueta generada a una impresora Zebra específica
		/// Configura el tamaño de papel personalizado (315x168 puntos) y sin márgenes
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
						// Guarda la imagen en formato JPEG
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
		/// Llena el combo box de variedades de cítricos
		/// Incluye variedades: ORONULE, OROGRANDE, CLEMENULE, TANGO
		/// Cada variedad tiene un código numérico asociado
		/// </summary>
		private void LlenaVariedad()
		{
			List<frm_generadorVentana_Citricos.Item> list = new List<frm_generadorVentana_Citricos.Item>();
			// Agrega las diferentes variedades de cítricos con sus códigos
			list.Add(new frm_generadorVentana_Citricos.Item("01 1 ORONULE", 1));
			list.Add(new frm_generadorVentana_Citricos.Item("02 2 OROGRANDE", 1));
			list.Add(new frm_generadorVentana_Citricos.Item("03 3 CLEMENULE", 1));
			list.Add(new frm_generadorVentana_Citricos.Item("04 4 TANGO", 1));
			// Configura el combo box con la lista de variedades
			this.cmb_variedad.DisplayMember = "Name";
			this.cmb_variedad.ValueMember = "Value";
			this.cmb_variedad.DataSource = list;
		}

		/// <summary>
		/// Llena el combo box de tipos de embalaje para cítricos
		/// Incluye diferentes formatos CG (Citrus Grower) con variantes A, S y PM
		/// CG145A/CG150A/CG163A = Cajas estándar de diferentes tamaños
		/// Sufijo A = Abierto, S = Sellado, APM/SPM = variantes especiales
		/// Nota: Actualmente el checkbox peso fijo no cambia los tipos disponibles
		/// </summary>
		private void LlenaEmbalaje()
		{
			List<frm_generadorVentana_Citricos.Item> list = new List<frm_generadorVentana_Citricos.Item>();
			if (!this.chb_pesofijo.Checked)
			{
				// Tipos de embalaje para peso variable
				list.Add(new frm_generadorVentana_Citricos.Item("CG145A", 1));
				list.Add(new frm_generadorVentana_Citricos.Item("CG150A", 1));
				list.Add(new frm_generadorVentana_Citricos.Item("CG163A", 1));
				list.Add(new frm_generadorVentana_Citricos.Item("CG150S", 1));
				list.Add(new frm_generadorVentana_Citricos.Item("CG163S", 1));
				list.Add(new frm_generadorVentana_Citricos.Item("CG145APM", 1));
				list.Add(new frm_generadorVentana_Citricos.Item("CG150APM", 1));
				list.Add(new frm_generadorVentana_Citricos.Item("CG163APM", 1));
				list.Add(new frm_generadorVentana_Citricos.Item("CG150SPM", 1));
				list.Add(new frm_generadorVentana_Citricos.Item("CG163SPM", 1));
			}
			else
			{
				// Tipos de embalaje para peso fijo (actualmente iguales)
				list.Add(new frm_generadorVentana_Citricos.Item("CG145A", 1));
				list.Add(new frm_generadorVentana_Citricos.Item("CG150A", 1));
				list.Add(new frm_generadorVentana_Citricos.Item("CG163A", 1));
				list.Add(new frm_generadorVentana_Citricos.Item("CG150S", 1));
				list.Add(new frm_generadorVentana_Citricos.Item("CG163S", 1));
				list.Add(new frm_generadorVentana_Citricos.Item("CG145APM", 1));
				list.Add(new frm_generadorVentana_Citricos.Item("CG150APM", 1));
				list.Add(new frm_generadorVentana_Citricos.Item("CG163APM", 1));
				list.Add(new frm_generadorVentana_Citricos.Item("CG150SPM", 1));
				list.Add(new frm_generadorVentana_Citricos.Item("CG163SPM", 1));
			}
			// Configura el combo box con la lista de tipos de embalaje
			this.cmb_tipo_embalaje.DisplayMember = "Name";
			this.cmb_tipo_embalaje.ValueMember = "Value";
			this.cmb_tipo_embalaje.DataSource = list;
			this.cmb_tipo_embalaje.SelectedIndex = 0;
		}

		/// <summary>
		/// Evento de cambio de selección en el combo de título 1
		/// Limpia la vista previa de la etiqueta al cambiar la selección
		/// </summary>
		private void cmb_titulo1_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.pb_etiqueta.Image = null;
		}

		/// <summary>
		/// Evento de cambio de selección en el combo de GTIN
		/// Limpia la vista previa de la etiqueta al cambiar la selección
		/// </summary>
		private void cmb_gtin_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.pb_etiqueta.Image = null;
		}

		/// <summary>
		/// Evento de cambio de selección en el combo de variedad
		/// Recarga los calibres disponibles para la variedad seleccionada
		/// y limpia la vista previa de la etiqueta
		/// </summary>
		private void cmb_variedad_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.LlenaCalibres();
			this.pb_etiqueta.Image = null;
		}

		/// <summary>
		/// Llena el combo box de calibres disponibles para cítricos
		/// Los calibres representan el tamaño/clasificación de los cítricos
		/// Incluye: 1, 2, 3, 4, 5, 5A, 1XX, 1X (ordenados por valor numérico asociado)
		/// </summary>
		private void LlenaCalibres()
		{
			List<frm_generadorVentana_Citricos.Item> list = new List<frm_generadorVentana_Citricos.Item>();
			// Agrega calibres con sus valores de orden
			list.Add(new frm_generadorVentana_Citricos.Item("1", 8));
			list.Add(new frm_generadorVentana_Citricos.Item("2", 1));
			list.Add(new frm_generadorVentana_Citricos.Item("3", 2));
			list.Add(new frm_generadorVentana_Citricos.Item("4", 3));
			list.Add(new frm_generadorVentana_Citricos.Item("5", 4));
			list.Add(new frm_generadorVentana_Citricos.Item("5A", 6));
			list.Add(new frm_generadorVentana_Citricos.Item("1XX", 5));
			list.Add(new frm_generadorVentana_Citricos.Item("1X", 7));
			// Configura el combo box con la lista de calibres
			this.cmb_calibre.DisplayMember = "Name";
			this.cmb_calibre.ValueMember = "Value";
			this.cmb_calibre.DataSource = list;
		}

		/// <summary>
		/// Evento de cambio de texto en el campo productor
		/// Limpia la vista previa de la etiqueta al modificar el productor
		/// </summary>
		private void txt_productor_TextChanged(object sender, EventArgs e)
		{
			this.pb_etiqueta.Image = null;
		}

		/// <summary>
		/// Evento de cambio de texto en el campo lote
		/// Limpia la vista previa de la etiqueta al modificar el lote
		/// </summary>
		private void txt_lote_TextChanged(object sender, EventArgs e)
		{
			this.pb_etiqueta.Image = null;
		}

		/// <summary>
		/// Evento de cambio de selección en el combo de título 2
		/// Limpia la vista previa de la etiqueta al cambiar la selección
		/// </summary>
		private void cmb_titulo2_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.pb_etiqueta.Image = null;
		}

		/// <summary>
		/// Evento de cambio de selección en el combo de fecha
		/// Limpia la vista previa de la etiqueta al cambiar la selección
		/// </summary>
		private void cmb_fecha_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.pb_etiqueta.Image = null;
		}

		/// <summary>
		/// Evento de validación de tecla presionada en el campo productor
		/// Solo permite la entrada de dígitos, teclas de control y separadores
		/// </summary>
		/// <param name="sender">Control que disparó el evento</param>
		/// <param name="e">Argumentos con la tecla presionada</param>
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

		/// <summary>
		/// Evento de cambio de texto en el campo fecha agrícola
		/// Limpia la vista previa de la etiqueta al modificar la fecha
		/// </summary>
		private void txt_fecha_agricola_TextChanged(object sender, EventArgs e)
		{
			this.pb_etiqueta.Image = null;
		}

		/// <summary>
		/// Evento de cambio de texto en el campo línea
		/// Limpia la vista previa de la etiqueta al modificar la línea de producción
		/// </summary>
		private void txt_linea_TextChanged(object sender, EventArgs e)
		{
			this.pb_etiqueta.Image = null;
		}

		/// <summary>
		/// Evento de cambio de selección en el combo de tipo de embalaje
		/// Limpia la vista previa de la etiqueta al cambiar el tipo de embalaje
		/// </summary>
		private void cmb_tipo_embalaje_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.pb_etiqueta.Image = null;
		}

		/// <summary>
		/// Evento de cambio de selección en el combo de packing
		/// Limpia la vista previa de la etiqueta al cambiar la planta empacadora
		/// </summary>
		private void cmb_packing_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.pb_etiqueta.Image = null;
		}

		/// <summary>
		/// Evento de cambio de selección en el combo de calibre
		/// Limpia la vista previa de la etiqueta al cambiar el calibre
		/// </summary>
		private void cmb_calibre_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.pb_etiqueta.Image = null;
		}

		/// <summary>
		/// Evento de cambio de selección en el combo de categoría 1
		/// Limpia la vista previa de la etiqueta al cambiar la categoría
		/// </summary>
		private void cmb_cat1_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.pb_etiqueta.Image = null;
		}

		/// <summary>
		/// Evento clic del botón Generar (button5)
		/// Valida todos los campos requeridos y genera la etiqueta completa de cítricos
		/// Realiza múltiples validaciones: formato de fecha, lote, variedad, etc.
		/// Si todas las validaciones pasan, llama a DibujaEtiquetaCOMPLETA() para crear la imagen
		/// </summary>
		/// <param name="sender">Botón que disparó el evento</param>
		/// <param name="e">Argumentos del evento</param>
		private void button5_Click(object sender, EventArgs e)
		{
			// Actualiza el título de la ventana con el número de línea
			this.Text = "L-" + this.txt_linea.Text + " - Generador PTI";
			try
			{
				// Validación 1: Verificar formato de fecha agrícola
				if (this.txt_fecha_agricola.Text.Trim() != "")
				{
					try
					{
						DateTime dateTime = DateTime.ParseExact(this.txt_fecha_agricola.Text.Trim(), "dd-MM-yyyy", null);
					}
					catch
					{
						MessageBox.Show("Formato de Fecha Incorrecto.");
						this.pb_etiqueta.Image = null;
						this.txt_fecha_agricola.Focus();
						return;
					}
				}
				
				// Validación 2: Verificar que el lote no esté vacío
				if (this.txt_lote.Text.Trim() == "")
				{
					MessageBox.Show("Favor ingresar Lote correcto.");
					this.pb_etiqueta.Image = null;
					this.txt_lote.Focus();
				}
				// Validación 3: Verificar longitud mínima del lote (4 caracteres)
				else if (this.txt_lote.Text.Trim().Length < 4)
				{
					MessageBox.Show("Favor ingresar Lote de 4 o 5 caracteres.");
					this.pb_etiqueta.Image = null;
					this.txt_lote.Focus();
				}
				// Validación 4: Verificar que la variedad esté seleccionada
				else if (this.cmb_variedad.Text.Trim() == "")
				{
					MessageBox.Show("Favor ingresar Variedad correcta.");
					this.pb_etiqueta.Image = null;
					this.cmb_variedad.Focus();
				}
				// Validación 5: Verificar longitud de fecha agrícola
				else if (this.txt_fecha_agricola.Text.Trim().Length < 4)
				{
					MessageBox.Show("Favor ingresar Fecha Agricola de 4 caracteres.");
					this.pb_etiqueta.Image = null;
					this.txt_fecha_agricola.Focus();
				}
				else
				{
					// Todas las validaciones pasaron
					string empty = string.Empty;
					// Construye el código compuesto: CSG (6 dígitos) + Variedad (2 dígitos) + Lote
					string text = this.cmb_productor.Text.Trim().Substring(0, 6).ToString() + this.cmb_variedad.Text.Trim().Substring(0, 2).ToString() + this.txt_lote.Text.Trim();
					
					// Maneja la fecha de empaque (opcional)
					if (this.txt_date.Text.Trim() == "")
					{
						DateTime dateTime2 = Convert.ToDateTime("01-01-2001"); // Fecha por defecto si no se ingresa
					}
					else
					{
						DateTime dateTime2 = Convert.ToDateTime(this.txt_date.Text.Trim());
					}
					
					// Genera la etiqueta completa
					this.DibujaEtiquetaCOMPLETA();
				}
			}
			catch (Exception)
			{
				MessageBox.Show("Error al generar el Voice Pick Code");
				this.pb_etiqueta.Image = null;
			}
		}

		/// <summary>
		/// Dibuja la etiqueta completa de cítricos con todos sus elementos
		/// Crea una imagen de 1600x800 píxeles con:
		/// - Tabla de información (Province/Township y Code)
		/// - Calibre (tamaño grande en esquina superior izquierda)
		/// - Número de pallets
		/// - Código de variedad
		/// - Nombre del recibidor (ajusta tamaño de fuente automáticamente)
		/// - Códigos CSG (productor) y CSP (packing)
		/// - Ubicación (ELQUI, COQUIMBO)
		/// - Fecha, lote y tipo de embalaje
		/// Guarda la imagen como "etiqueta.jpg" y la muestra en el PictureBox
		/// </summary>
		private void DibujaEtiquetaCOMPLETA()
		{
			// Inicializa generadores de códigos de barras
			//BarcodeGenerator barcodeGenerator = new BarcodeGenerator();
			//BarcodeGenerator_PLU barcodeGenerator_PLU = new BarcodeGenerator_PLU();
			
			// Crea contextos gráficos temporales
			Graphics g = Graphics.FromImage(new Bitmap(1, 1));
			Graphics graphics = Graphics.FromImage(new Bitmap(1, 1));
			Bitmap image = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
			g = Graphics.FromImage(image);
			graphics = Graphics.FromImage(image);
			
			string empty = string.Empty;
			SizeF sizeF = default(SizeF);
			string empty2 = string.Empty;
			
			// Genera código de barras temporal
			Image image2 = null; //barcodeGenerator.DrawCode128(g, "123456789", 0, 0);
			
			// Obtiene valores de los controles
			string text = this.cmb_titulo2.Text.Trim();
			// Construye código compuesto: Productor (6) + Variedad (2) + Lote
			string text2 = this.cmb_productor.Text.Trim().Substring(0, 6).ToString() + this.cmb_variedad.Text.Trim().Substring(0, 2).ToString() + this.txt_lote.Text.Trim();
			string empty3 = string.Empty;
			
			// Define ubicación geográfica
			string str = "ELQUI";
			string str2 = "COQUIMBO";
			
			// Dimensiones de la etiqueta
			int width = 1600;
			int height = 800;
			Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
			
			// SECCIÓN 1: Dibuja el fondo blanco y las líneas de la tabla
			for (int i = 0; i < bitmap.Width; i++)
			{
				for (int j = 0; j < bitmap.Height; j++)
				{
					// Establece fondo blanco
					bitmap.SetPixel(i, j, Color.White);
					
					// Dibuja líneas horizontales de la tabla
					if (i > 390 & i < 928 & j > 210 & j < 213)
					{
						bitmap.SetPixel(i, j, Color.Black); // Línea superior
					}
					if (i > 390 & i < 928 & j > 250 & j < 253)
					{
						bitmap.SetPixel(i, j, Color.Black); // Línea 2
					}
					if (i > 390 & i < 928 & j > 300 & j < 303)
					{
						bitmap.SetPixel(i, j, Color.Black); // Línea 3
					}
					if (i > 390 & i < 928 & j > 350 & j < 353)
					{
						bitmap.SetPixel(i, j, Color.Black); // Línea inferior
					}
					
					// Dibuja líneas verticales de la tabla
					if (i > 390 & i < 393 & j > 210 & j < 353)
					{
						bitmap.SetPixel(i, j, Color.Black); // Borde izquierdo
					}
					if (i > 735 & i < 738 & j > 210 & j < 353)
					{
						bitmap.SetPixel(i, j, Color.Black); // Línea divisoria
					}
					if (i > 925 & i < 928 & j > 210 & j < 353)
					{
						bitmap.SetPixel(i, j, Color.Black); // Borde derecho
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
			font = new Font("arial", 22f, FontStyle.Regular);
			StringFormat stringFormat3 = new StringFormat();
			stringFormat3.Alignment = StringAlignment.Center;
			SolidBrush brush3 = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
			graphics2.DrawString("Province and Township", font, brush3, new Point(570, 215), stringFormat3);
			font = new Font("arial", 22f, FontStyle.Regular);
			StringFormat stringFormat4 = new StringFormat();
			stringFormat4.Alignment = StringAlignment.Center;
			SolidBrush brush4 = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
			graphics2.DrawString("Code", font, brush4, new Point(830, 215), stringFormat4);
			font = new Font("arial", 180f, FontStyle.Bold);
			StringFormat stringFormat5 = new StringFormat();
			stringFormat5.Alignment = StringAlignment.Center;
			SolidBrush brush5 = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
			graphics2.DrawString(this.cmb_variedad.Text.Trim().Substring(3, 1).ToString(), font, brush5, new Point(1280, 100), stringFormat5);
			string text3 = this.cmb_Recibidor.Text.Trim().ToString();
			string[] array2;
			if (this.cmb_Recibidor.Text.Trim() == "10 T Seedless" || this.cmb_Recibidor.Text.Trim() == "16 Iniagrape-one cv.")
			{
				array2 = text3.Split(new char[]
				{
					'X'
				});
			}
			else if (this.cmb_Recibidor.Text.Trim() == "AM FRESH_NORTH_AMERICA" || this.cmb_Recibidor.Text.Trim() == "AM FRESH_NORTH_AMERICA" || this.cmb_Recibidor.Text.Trim() == "GUAN CHAN_INTERNATIONAL_(IPG)" || this.cmb_Recibidor.Text.Trim() == "DEREK L_WANG_(IPG)")
			{
				array2 = text3.Split(new char[]
				{
					'_'
				});
			}
			else
			{
				array2 = text3.Split(new char[]
				{
					' '
				});
			}
			int[] array3 = new int[]
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
			if (array2.Length.ToString() == "1")
			{
				try
				{
					font = new Font("arial", 36f, FontStyle.Bold);
					StringFormat stringFormat6 = new StringFormat();
					stringFormat6.Alignment = StringAlignment.Center;
					SolidBrush brush6 = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
					graphics2.DrawString("", font, brush6, new Point(670, 520), stringFormat6);
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
					StringFormat stringFormat7 = new StringFormat();
					stringFormat7.Alignment = StringAlignment.Center;
					SolidBrush brush7 = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
					graphics2.DrawString(array2[0], font, brush7, new Point(670, 550), stringFormat7);
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
						if ((ushort)graphics2.MeasureString(array2[1], font).Width < 350)
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
					StringFormat stringFormat6 = new StringFormat();
					stringFormat6.Alignment = StringAlignment.Center;
					SolidBrush brush6 = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
					graphics2.DrawString(array2[0], font, brush6, new Point(670, 450), stringFormat6);
				}
				catch (Exception)
				{
				}
				try
				{
					font = new Font("arial", (float)array3[num3], FontStyle.Bold);
					StringFormat stringFormat7 = new StringFormat();
					stringFormat7.Alignment = StringAlignment.Center;
					SolidBrush brush7 = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
					graphics2.DrawString(array2[1], font, brush7, new Point(670, 550), stringFormat7);
				}
				catch (Exception)
				{
				}
				try
				{
					font = new Font("arial", (float)array3[num3], FontStyle.Bold);
					StringFormat stringFormat8 = new StringFormat();
					stringFormat8.Alignment = StringAlignment.Center;
					SolidBrush brush8 = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
					graphics2.DrawString(array2[2], font, brush8, new Point(670, 650), stringFormat8);
				}
				catch (Exception)
				{
				}
			}
			font = new Font("arial", 22f, FontStyle.Bold);
			StringFormat stringFormat9 = new StringFormat();
			stringFormat9.Alignment = StringAlignment.Center;
			SolidBrush brush9 = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
			graphics2.DrawString("CSG " + this.cmb_productor.Text.Trim().Substring(0, 6).ToString(), font, brush9, new Point(832, 260), stringFormat9);
			if (this.cmb_packing.Text.Trim().ToString() == "146 P. Los Pimientos Terreno                      3101432")
			{
				font = new Font("arial", 22f, FontStyle.Bold);
				StringFormat stringFormat10 = new StringFormat();
				stringFormat10.Alignment = StringAlignment.Center;
				SolidBrush brush10 = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
				graphics2.DrawString("CSP " + this.cmb_packing.Text.Trim().Substring(50, 7).ToString(), font, brush10, new Point(832, 310), stringFormat10);
			}
			else
			{
				font = new Font("arial", 22f, FontStyle.Bold);
				StringFormat stringFormat10 = new StringFormat();
				stringFormat10.Alignment = StringAlignment.Center;
				SolidBrush brush10 = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
				graphics2.DrawString("CSP " + this.cmb_packing.Text.Trim().Substring(50, 6).ToString(), font, brush10, new Point(832, 310), stringFormat10);
			}
			font = new Font("arial", 17f, FontStyle.Bold);
			StringFormat stringFormat11 = new StringFormat();
			stringFormat11.Alignment = StringAlignment.Center;
			SolidBrush brush11 = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
			graphics2.DrawString(str + ", " + str2, font, brush11, new Point(563, 265), stringFormat11);
			font = new Font("arial", 17f, FontStyle.Bold);
			StringFormat stringFormat12 = new StringFormat();
			stringFormat12.Alignment = StringAlignment.Center;
			SolidBrush brush12 = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
			graphics2.DrawString(str + ", " + str2, font, brush12, new Point(563, 315), stringFormat12);
			font = new Font("arial", 34f, FontStyle.Bold);
			StringFormat stringFormat13 = new StringFormat();
			stringFormat13.Alignment = StringAlignment.Near;
			SolidBrush brush13 = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
			graphics2.DrawString("DATE: " + this.txt_fecha_agricola.Text, font, brush13, new Point(380, 95), stringFormat13);
			font = new Font("arial", 34f, FontStyle.Bold);
			StringFormat stringFormat14 = new StringFormat();
			stringFormat14.Alignment = StringAlignment.Near;
			SolidBrush brush14 = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
			graphics2.DrawString("LOTE: " + this.txt_lote.Text, font, brush14, new Point(380, 145), stringFormat14);
			font = new Font("arial", 34f, FontStyle.Bold);
			StringFormat stringFormat15 = new StringFormat();
			stringFormat15.Alignment = StringAlignment.Near;
			SolidBrush brush15 = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
			graphics2.DrawString("INNER PACK: " + this.cmb_tipo_embalaje.Text, font, brush15, new Point(380, 45), stringFormat15);
			image2 = bitmap;
			image2.Save("etiqueta.jpg", ImageFormat.Jpeg);
			graphics2.Dispose();
			image2.Dispose();
			this.pb_etiqueta.ImageLocation = "etiqueta.jpg";
		}

		/// <summary>
		/// Evento de cambio de selección en el combo de productor
		/// (Actualmente sin implementación)
		/// </summary>
		private void cmb_productor_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// Evento de cambio del checkbox de peso fijo
		/// Recarga los tipos de embalaje disponibles según el modo seleccionado
		/// </summary>
		private void chb_pesofijo_CheckedChanged(object sender, EventArgs e)
		{
			this.LlenaEmbalaje();
		}

		/// <summary>
		/// Evento al salir del campo de fecha agrícola
		/// Valida que la fecha esté en formato correcto dd-MM-yyyy
		/// Si el formato es incorrecto, muestra un mensaje y devuelve el foco al campo
		/// </summary>
		private void txt_fecha_agricola_Leave(object sender, EventArgs e)
		{
			if (this.txt_fecha_agricola.Text.Trim() != "")
			{
				try
				{
					DateTime dateTime = DateTime.ParseExact(this.txt_fecha_agricola.Text.Trim(), "dd-MM-yyyy", null);
				}
				catch
				{
					MessageBox.Show("Formato de Fecha Incorrecto.");
					this.txt_fecha_agricola.Focus();
				}
			}
		}

		/// <summary>
		/// Clase estática para calcular Voice Pick Codes SIN fecha
		/// Genera un código de 4 dígitos basado en GTIN y lote usando CRC16
		/// </summary>
		public static class VoiceCodesinFecha
		{
			/// <summary>
			/// Calcula el Voice Pick Code basado en GTIN y lote (sin incluir fecha)
			/// Usa algoritmo CRC16 para generar un código único de 4 dígitos
			/// </summary>
			/// <param name="GTIN">Código GTIN del producto</param>
			/// <param name="lot">Número de lote</param>
			/// <param name="packDate">Fecha de empaque (no se usa en este método)</param>
			/// <returns>Código de 4 dígitos (0000-9999)</returns>
			public static string Compute(string GTIN, string lot, DateTime? packDate)
			{
				// Calcula CRC16 de la concatenación GTIN + lote
				ushort num = frm_generadorVentana_Citricos.Crc16.ComputeChecksum(Encoding.ASCII.GetBytes(string.Format("{0}{1}{2}", GTIN, lot, string.Empty)));
				// Retorna los últimos 4 dígitos del CRC16 (módulo 10000)
				return string.Format("{0:0000}", (int)(num % 10000));
			}
		}

		/// <summary>
		/// Clase estática para calcular Voice Pick Codes CON fecha
		/// Genera un código de 4 dígitos basado en GTIN, lote y fecha usando CRC16
		/// </summary>
		public static class VoiceCodeconFecha
		{
			/// <summary>
			/// Calcula el Voice Pick Code basado en GTIN, lote y fecha de empaque
			/// Usa algoritmo CRC16 para generar un código único de 4 dígitos
			/// </summary>
			/// <param name="GTIN">Código GTIN del producto</param>
			/// <param name="lot">Número de lote</param>
			/// <param name="packDate">Fecha de empaque en formato yyMMdd</param>
			/// <returns>Código de 4 dígitos (0000-9999)</returns>
			public static string Compute(string GTIN, string lot, DateTime? packDate)
			{
				// Calcula CRC16 de la concatenación GTIN + lote + fecha (yyMMdd)
				ushort num = frm_generadorVentana_Citricos.Crc16.ComputeChecksum(Encoding.ASCII.GetBytes(string.Format("{0}{1}{2}", GTIN, lot, (packDate != null) ? packDate.Value.ToString("yyMMdd") : string.Empty)));
				// Retorna los últimos 4 dígitos del CRC16 (módulo 10000)
				return string.Format("{0:0000}", (int)(num % 10000));
			}
		}

		/// <summary>
		/// Clase estática para cálculo de CRC16 (Cyclic Redundancy Check de 16 bits)
		/// Implementa el algoritmo CRC16 con polinomio 0xA001 (40961 decimal)
		/// Usado para generar checksums únicos para Voice Pick Codes
		/// </summary>
		public static class Crc16
		{
			/// <summary>
			/// Constructor estático - Inicializa la tabla de lookup para CRC16
			/// La tabla precalcula valores para acelerar el cálculo del checksum
			/// Se ejecuta una sola vez cuando se carga la clase
			/// </summary>
			static Crc16()
			{
				// Genera tabla de 256 entradas para CRC16
				ushort num = 0;
				while ((int)num < frm_generadorVentana_Citricos.Crc16.table.Length)
				{
					ushort num2 = 0;
					ushort num3 = num;
					// Procesa cada bit (8 bits por entrada)
					for (byte b = 0; b < 8; b += 1)
					{
						if (0 != ((num2 ^ num3) & 1))
						{
							// Aplica el polinomio CRC16 (0xA001)
							num2 = (ushort)(num2 >> 1 ^ 40961);
						}
						else
						{
							num2 = (ushort)(num2 >> 1);
						}
						num3 = (ushort)(num3 >> 1);
					}
					frm_generadorVentana_Citricos.Crc16.table[(int)num] = num2;
					num += 1;
				}
			}

			/// <summary>
			/// Calcula el checksum CRC16 de un array de bytes
			/// Procesa cada byte usando la tabla de lookup precalculada
			/// </summary>
			/// <param name="bytes">Array de bytes para calcular el checksum</param>
			/// <returns>Valor CRC16 de 16 bits (0-65535)</returns>
			public static ushort ComputeChecksum(byte[] bytes)
			{
				ushort num = 0; // Inicializa el CRC en 0
				// Procesa cada byte del array
				for (int i = 0; i < bytes.Length; i++)
				{
					// XOR del byte actual con los 8 bits inferiores del CRC
					byte b = (byte)(num ^ (ushort)bytes[i]);
					// Actualiza el CRC usando la tabla de lookup
					num = (ushort)(num >> 8 ^ (int)frm_generadorVentana_Citricos.Crc16.table[(int)b]);
				}
				return num;
			}

			// Polinomio CRC16: 0xA001 (40961 en decimal)
			private const ushort polynomial = 40961;

			// Tabla de lookup precalculada para acelerar el cálculo
			private static ushort[] table = new ushort[256];
		}

		/// <summary>
		/// Clase auxiliar para almacenar items de ComboBox
		/// Representa un par nombre-valor para usar en listas desplegables
		/// </summary>
		public class Item
		{
			/// <summary>
			/// Nombre visible del item en el ComboBox
			/// </summary>
			public string Name { get; set; }

			/// <summary>
			/// Valor asociado al item (para procesamiento interno)
			/// </summary>
			public int Value { get; set; }

			/// <summary>
			/// Constructor para crear un nuevo item
			/// </summary>
			/// <param name="name">Nombre del item</param>
			/// <param name="value">Valor asociado</param>
			public Item(string name, int value)
			{
				this.Name = name;
				this.Value = value;
			}

			/// <summary>
			/// Retorna la representación en texto del item (su nombre)
			/// Usado por el ComboBox para mostrar el item
			/// </summary>
			/// <returns>Nombre del item</returns>
			public override string ToString()
			{
				return this.Name;
			}
		}

		/// <summary>
		/// Evento clic en label16 (sin implementación)
		/// </summary>
        private void label16_Click(object sender, EventArgs e)
        {

        }

		/// <summary>
		/// Evento clic en label2 (sin implementación)
		/// </summary>
        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
