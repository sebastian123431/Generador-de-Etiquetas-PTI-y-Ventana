using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace LabelKit_PLU_8045
{
	/// <summary>
	/// Generador de códigos de barras PLU (Price Look-Up) - Formato 8045
	/// Especializado en productos con peso variable para etiquetas de ventana
	/// Genera códigos PLU de tamaño mediano (280x130) con fuente estándar (12pt)
	/// Usado principalmente en etiquetas de cítricos y productos especiales
	/// </summary>
	internal class BarcodeGenerator_PLU
	{
		/// <summary>
		/// Constructor por defecto del generador de códigos PLU 8045
		/// Inicializa valores predeterminados: tamaño 280x130, fuente Courier New 12pt
		/// </summary>
		public BarcodeGenerator_PLU()
		{
			this.width = 280;
			this.height = 130;
			this.humanReadable = true;
			this.fontSize = 12;
			this.fontName = "Courier New";
			this.centered = false;
		}

		/// <summary>
		/// Indica si el código de barras PLU 8045 debe estar centrado horizontalmente
		/// Cuando es true, el código se centra en el ancho especificado
		/// IMPORTANTE EN 8045: Centrado crucial para ventanas de cítricos con diseño específico
		/// </summary>
		public bool Centered
		{
			get
			{
				return this.centered;
			}
			set
			{
				this.centered = value;
			}
		}

		/// <summary>
		/// Nombre de la fuente para el texto legible en etiquetas PLU 8045
		/// Por defecto: "Courier New" (fuente monoespaciada)
		/// FORMATO 8045: Usa fuente estándar 12pt (no grande como 2022)
		/// MOTIVO: Etiquetas de ventana tienen más espacio que etiquetas de balanza
		/// </summary>
		public string FontName
		{
			get
			{
				return this.fontName;
			}
			set
			{
				this.fontName = value;
			}
		}

		/// <summary>
		/// Ancho de la imagen del código de barras PLU 8045 en píxeles
		/// Por defecto: 280 píxeles
		/// DIFERENCIA CLAVE:
		/// - PLU 2022 (balanza): 225px (muy pequeño)
		/// - PLU 8045 (ventana): 280px (mediano)
		/// - Code 128 estándar: 640px (grande)
		/// MOTIVO: Equilibrio entre espacio y legibilidad en ventanas de cítricos
		/// </summary>
		public int Width
		{
			get
			{
				return this.width;
			}
			set
			{
				this.width = value;
			}
		}

		/// <summary>
		/// Altura de la imagen del código de barras PLU 8045 en píxeles
		/// Por defecto: 130 píxeles (estándar para etiquetas de productos)
		/// NOTA: La altura final recortada será de 60px para ventanas compactas
		/// </summary>
		public int Height
		{
			get
			{
				return this.height;
			}
			set
			{
				this.height = value;
			}
		}

		/// <summary>
		/// Indica si se debe mostrar el texto legible por humanos en ventanas 8045
		/// Por defecto: true
		/// FORMATO 8045: Texto estándar 12pt para ventanas de cítricos
		/// DIFERENCIA: No necesita fuente tan grande como PLU 2022 (30pt)
		/// </summary>
		public bool HumanReadable
		{
			get
			{
				return this.humanReadable;
			}
			set
			{
				this.humanReadable = value;
			}
		}

		/// <summary>
		/// Tamaño de la fuente para el texto legible en ventanas PLU 8045
		/// Por defecto: 12 puntos (ESTÁNDAR)
		/// COMPARACIÓN:
		/// - PLU 2022 (balanza): 30pt (MUY GRANDE - lectura a distancia)
		/// - PLU 8045 (ventana): 12pt (ESTÁNDAR - lectura normal)
		/// - Code 128 estándar: 12pt (ESTÁNDAR)
		/// MOTIVO: Ventanas de cítricos se leen de cerca, no necesitan fuente gigante
		/// </summary>
		public int FontSize
		{
			get
			{
				return this.fontSize;
			}
			set
			{
				this.fontSize = value;
			}
		}

		/// <summary>
		/// Dibuja un código de barras UPC-A adaptado para ventanas PLU formato 8045
		/// Versión especializada para productos de cítricos y ventanas de exportación
		/// 
		/// FORMATO 8045 - CARACTERÍSTICAS:
		/// - Ancho: 280px (intermedio entre balanza 225px y estándar 640px)
		/// - Fuente: 12pt ESTÁNDAR (no 30pt como balanza)
		/// - Uso: Ventanas de cítricos, etiquetas de cajas de exportación
		/// - Diseño: Más espacio que balanza, más compacto que estándar
		/// 
		/// DIFERENCIAS CON OTRAS VERSIONES:
		/// - vs PLU 2022: Ancho mayor (280px vs 225px), fuente estándar (12pt vs 30pt)
		/// - vs Estándar: Ancho menor (280px vs 640px), mismo tamaño fuente (12pt)
		/// 
		/// USO TÍPICO EN VENTANA 8045:
		/// - Etiquetas de cajas de cítricos (limones, naranjas, mandarinas)
		/// - Ventanas de productos de exportación
		/// - Códigos de rastreo de pallets
		/// - Identificación de lotes de fruta
		/// </summary>
		/// <param name="g">Contexto gráfico donde se dibujará el código</param>
		/// <param name="code">Código de 11 dígitos (el 12º se calcula automáticamente)</param>
		/// <param name="x">Posición X donde iniciar el dibujo</param>
		/// <param name="y">Posición Y donde iniciar el dibujo</param>
		/// <returns>Cadena vacía si es exitoso, mensaje de error si falla</returns>
		public string DrawUPCA(Graphics g, string code, int x, int y)
		{
			code = code.Trim();
			try
			{
				long.Parse(code);
			}
			catch
			{
				return "Code is not valid for a UPCA barcode: " + code;
			}
			while (code.Length < 11)
			{
				code = "0" + code;
			}
			code = code.Substring(0, 11);
			code = code.Trim() + this.CheckDigitUPCA(code);
			string text = "101";
			for (int i = 0; i < 6; i++)
			{
				int num = int.Parse(code.Substring(i, 1));
				text += BarcodeGenerator_PLU.left_UPCA[num];
			}
			text += "01010";
			for (int i = 6; i < 12; i++)
			{
				int num = int.Parse(code.Substring(i, 1));
				text += BarcodeGenerator_PLU.right_UPCA[num];
			}
			text += "101";
			Font font = new Font(this.fontName, (float)this.fontSize, FontStyle.Bold);
			SizeF sizeF = g.MeasureString(code.Substring(0, 1), font);
			int num2 = 0;
			if (this.humanReadable)
			{
				num2 = (int)sizeF.Width + 2;
			}
			int num3 = (int)Math.Floor((double)(this.width - 2 * num2) / (double)text.Length);
			if (num3 <= 0)
			{
				num3 = 1;
			}
			if (this.centered)
			{
				x -= (num3 * 95 + 2 * num2) / 2;
			}
			int num4 = x + num2;
			for (int i = 1; i <= text.Length; i++)
			{
				string a = text.Substring(i - 1, 1);
				if (a == "0")
				{
					g.FillRectangle(Brushes.White, num4, y, num3, this.height);
				}
				else
				{
					g.FillRectangle(Brushes.Black, num4, y, num3, this.height);
				}
				num4 += num3;
			}
			g.FillRectangle(Brushes.White, x, y + this.height - 8, this.width, 8);
			if (this.humanReadable)
			{
				g.FillRectangle(Brushes.White, x + num2 + num3 * 10, y + this.height - 20, num3 * 36, 20);
				g.FillRectangle(Brushes.White, x + num2 + num3 * 49, y + this.height - 20, num3 * 36, 20);
				g.DrawString(code.Substring(0, 1), font, Brushes.Black, (float)(x + 2), (float)(y + this.height - font.Height));
				int num5 = num3 * 36 / 5;
				for (int i = 1; i < 6; i++)
				{
					g.DrawString(code.Substring(i, 1), font, Brushes.Black, (float)(x + num3 * 10 + num2 + num5 * (i - 1)), (float)(y + this.height - font.Height));
				}
				for (int i = 6; i < 11; i++)
				{
					g.DrawString(code.Substring(i, 1), font, Brushes.Black, (float)(x + num3 * 49 + num2 + num5 * (i - 6)), (float)(y + this.height - font.Height));
				}
				g.DrawString(code.Substring(11, 1), font, Brushes.Black, (float)(x + num3 * 95 + num2), (float)(y + this.height - font.Height));
			}
			return "";
		}

		/// <summary>
		/// Calcula el dígito verificador para códigos UPC-A en ventanas formato 8045
		/// Usa el algoritmo estándar UPC-A adaptado para cítricos y exportación
		/// 
		/// ALGORITMO UPC-A (MISMO PARA TODOS LOS FORMATOS):
		/// 1. Suma dígitos en posiciones IMPARES (1,3,5,7,9,11) y multiplica por 3
		/// 2. Suma dígitos en posiciones PARES (2,4,6,8,10)
		/// 3. Suma ambos resultados
		/// 4. El dígito verificador es lo que falta para el próximo múltiplo de 10
		/// 
		/// APLICACIÓN FORMATO 8045:
		/// - Ventanas de cítricos: Validación de códigos de lote
		/// - Cajas de exportación: Verificación de pallets
		/// - Rastreo de productos: Integridad de datos
		/// </summary>
		/// <param name="code">Código UPC-A de 11 dígitos</param>
		/// <returns>Dígito verificador (0-9)</returns>
		private string CheckDigitUPCA(string code)
		{
			int num = 0;  // Suma de dígitos en posiciones impares
			int num2 = 0; // Suma de dígitos en posiciones pares
			
			// Suma dígitos en posiciones impares (índices 0,2,4,6,8,10)
			for (int i = 0; i < code.Length; i += 2)
			{
				num += int.Parse(code.Substring(i, 1));
			}
			
			// Suma dígitos en posiciones pares (índices 1,3,5,7,9)
			for (int i = 1; i < code.Length; i += 2)
			{
				num2 += int.Parse(code.Substring(i, 1));
			}
			
			// Fórmula: (10 - ((suma_impares * 3 + suma_pares) % 10)) % 10
			return ((10 - (num * 3 + num2) % 10) % 10).ToString().Trim();
		}

		/// <summary>
		/// Dibuja un código de barras Interleaved 2 of 5 sin checksum
		/// Versión simplificada para ventanas formato 8045
		/// Sobrecarga que llama al método principal con checksum=false
		/// </summary>
		/// <param name="g">Contexto gráfico donde se dibujará</param>
		/// <param name="code">Código numérico a codificar</param>
		/// <param name="x">Posición X inicial</param>
		/// <param name="y">Posición Y inicial</param>
		/// <returns>Cadena vacía si es exitoso, mensaje de error si falla</returns>
		public string DrawInterleaved2of5(Graphics g, string code, int x, int y)
		{
			return this.DrawInterleaved2of5(g, code, x, y, false);
		}

		/// <summary>
		/// Dibuja un código de barras Interleaved 2 of 5 (ITF) para ventanas 8045
		/// Versión optimizada para etiquetas de cítricos y cajas de exportación
		/// 
		/// CARACTERÍSTICAS FORMATO 8045:
		/// - Ancho: 280px (balance entre legibilidad y espacio)
		/// - Fuente: 12pt estándar (lectura normal, no a distancia)
		/// - Uso principal: Cajas de cítricos, pallets de exportación
		/// 
		/// APLICACIONES VENTANA 8045:
		/// - Códigos de lote de cítricos
		/// - Identificación de cajas de exportación
		/// - Rastreo de pallets completos
		/// - Códigos de contenedores
		/// </summary>
		/// <param name="g">Contexto gráfico donde se dibujará</param>
		/// <param name="code">Código numérico a codificar</param>
		/// <param name="x">Posición X inicial</param>
		/// <param name="y">Posición Y inicial</param>
		/// <param name="checksum">Si true, agrega dígito verificador</param>
		/// <returns>Cadena vacía si es exitoso, mensaje de error si falla</returns>
		public string DrawInterleaved2of5(Graphics g, string code, int x, int y, bool checksum)
		{
			code = code.Trim();
			try
			{
				long.Parse(code);
			}
			catch
			{
				return "Code is not valid for an Interleaved 2 of 5 barcode: " + code;
			}
			if ((checksum && this.IsEven(code.Length)) || (!checksum && this.IsOdd(code.Length)))
			{
				code = "0" + code;
			}
			if (checksum)
			{
				code += this.CheckDigitInterleaved(code);
			}
			string text = "1010";
			for (int i = 0; i < code.Length; i++)
			{
				int num = int.Parse(code.Substring(i, 1));
				string text2 = BarcodeGenerator_PLU.both_2of5[num];
				i++;
				int num2 = int.Parse(code.Substring(i, 1));
				string text3 = BarcodeGenerator_PLU.both_2of5[num2];
				for (int j = 0; j < 5; j++)
				{
					if (text2[j] == 'W')
					{
						text += "11";
					}
					else
					{
						text += "1";
					}
					if (text3[j] == 'W')
					{
						text += "00";
					}
					else
					{
						text += "0";
					}
				}
			}
			text += "1101";
			Font font = new Font(this.fontName, (float)this.fontSize, FontStyle.Bold);
			SizeF sizeF = g.MeasureString(code.Substring(0, 1), font);
			int num3 = 0;
			if (this.humanReadable)
			{
				num3 = (int)sizeF.Width + 2;
			}
			int num4 = (int)Math.Floor((double)(this.width - 2 * num3) / (double)text.Length);
			if (num4 <= 0)
			{
				num4 = 1;
			}
			if (this.centered)
			{
				x -= (num4 * text.Length + 2 * num3) / 2;
			}
			int num5 = x + num3;
			for (int i = 1; i <= text.Length; i++)
			{
				string a = text.Substring(i - 1, 1);
				if (a == "0")
				{
					g.FillRectangle(Brushes.White, num5, y, num4, this.height);
				}
				else
				{
					g.FillRectangle(Brushes.Black, num5, y, num4, this.height);
				}
				num5 += num4;
			}
			if (this.humanReadable)
			{
				int num6 = num4 * 36 / 5;
				for (int i = 0; i < code.Length; i++)
				{
					g.DrawString(code.Substring(i, 1), font, Brushes.Black, (float)(x + num4 * 10 + num3 + num6 * (i - 1)), (float)(y + this.height + 4));
				}
			}
			return "";
		}

		/// <summary>
		/// Calcula el dígito verificador para códigos Interleaved 2 of 5 en ventanas 8045
		/// Similar al algoritmo UPC-A pero con las posiciones invertidas
		/// 
		/// DIFERENCIA CON UPC-A:
		/// - UPC-A: Impares *3 + Pares
		/// - ITF: Pares + Impares *3 (invertido)
		/// 
		/// ALGORITMO ITF:
		/// 1. Suma dígitos en posiciones PARES (2,4,6,8...)
		/// 2. Suma dígitos en posiciones IMPARES (1,3,5,7...) y multiplica por 3
		/// 3. El dígito verificador es lo que falta para el próximo múltiplo de 10
		/// 
		/// APLICACIÓN FORMATO 8045:
		/// - Validación de lotes de cítricos
		/// - Verificación de códigos de pallet
		/// - Integridad de datos en ventanas de exportación
		/// </summary>
		/// <param name="code">Código numérico</param>
		/// <returns>Dígito verificador (0-9)</returns>
		private string CheckDigitInterleaved(string code)
		{
			int num = 0;  // Suma de posiciones impares (se multiplica por 3)
			int num2 = 0; // Suma de posiciones pares
			
			// Suma dígitos en posiciones pares (índices 0,2,4...)
			for (int i = 0; i < code.Length; i += 2)
			{
				num2 += int.Parse(code.Substring(i, 1));
			}
			
			// Suma dígitos en posiciones impares (índices 1,3,5...)
			for (int i = 1; i < code.Length; i += 2)
			{
				num += int.Parse(code.Substring(i, 1));
			}
			
			// Fórmula: (10 - ((suma_impares * 3 + suma_pares) % 10)) % 10
			return ((10 - (num * 3 + num2) % 10) % 10).ToString().Trim();
		}

		/// <summary>
		/// Dibuja un código de barras Code 128 optimizado para ventanas formato 8045
		/// Versión especializada para etiquetas de cítricos y productos de exportación
		/// 
		/// FORMATO 8045 - CARACTERÍSTICAS DISTINTIVAS:
		/// - Ancho: 280px (INTERMEDIO - balance entre espacio y legibilidad)
		/// - Fuente: 12pt (ESTÁNDAR - lectura normal, no a distancia)
		/// - Altura recorte: 60px (MÁS COMPACTO que PLU 2022 que usa 75px)
		/// - Uso: Ventanas de cítricos, cajas de exportación, pallets
		/// 
		/// COMPARACIÓN DE FORMATOS:
		/// | Formato      | Ancho | Fuente | Recorte | Uso Principal              |
		/// |--------------|-------|--------|---------|----------------------------|
		/// | PLU 2022     | 225px | 30pt   | 75px    | Balanza (peso variable)    |
		/// | PLU 8045     | 280px | 12pt   | 60px    | Ventana (cítricos/export) |
		/// | Estándar    | 640px | 12pt   | 120px   | General (PTI)              |
		/// 
		/// VENTAJAS FORMATO 8045:
		/// 1. Ancho mediano (280px): Equilibrio perfecto para ventanas de cajas
		/// 2. Fuente estándar (12pt): No ocupa tanto espacio como 30pt de balanza
		/// 3. Recorte compacto (60px): Máximo aprovechamiento de espacio vertical
		/// 4. Optimizado para: Etiquetas de cítricos con información adicional
		/// 
		/// APLICACIONES TÍPICAS VENTANA 8045:
		/// - Etiquetas de cajas de limones con datos de exportación
		/// - Ventanas de naranjas con información de productor
		/// - Códigos de rastreo de pallets de mandarinas
		/// - Identificación de lotes de cítricos para mercado internacional
		/// 
		/// PROCESO (10 PASOS):
		/// 1. Crear bitmap formato 8045 (280x130px)
		/// 2. Analizar texto y elegir subconjunto Code 128 (A/B/C)
		/// 3. Optimizar automáticamente a Code C para números
		/// 4. Agregar dígito de verificación
		/// 5. Convertir a patrones de barras
		/// 6. Calcular dimensiones (adaptado a 280px)
		/// 7. Ajustar posición si está centrado
		/// 8. Dibujar barras con grosor calibrado
		/// 9. Agregar texto legible ESTÁNDAR (12pt)
		/// 10. Recortar a 60px de altura (MÁXIMO COMPACTO)
		/// </summary>
		/// <param name="g">Contexto gráfico (se reemplaza internamente)</param>
		/// <param name="code">Texto a codificar (cualquier carácter ASCII)</param>
		/// <param name="x">Posición X inicial</param>
		/// <param name="y">Posición Y inicial</param>
		/// <returns>Imagen del código de barras formato 8045 recortada a 60px de altura</returns>
		public Image DrawCode128(Graphics g, string code, int x, int y)
		{
			Bitmap bitmap = new Bitmap(this.width, this.height, PixelFormat.Format32bppArgb);
			g = Graphics.FromImage(bitmap);
			List<int> list = new List<int>();
			BarcodeGenerator_PLU.Code128Modes code128Modes = BarcodeGenerator_PLU.Code128Modes.CodeUnset;
			for (int i = 0; i < code.Length; i++)
			{
				if (this.IsNumber(code[i]) && i + 1 < code.Length && this.IsNumber(code[i + 1]))
				{
					if (code128Modes != BarcodeGenerator_PLU.Code128Modes.CodeC)
					{
						if (code128Modes == BarcodeGenerator_PLU.Code128Modes.CodeUnset)
						{
							list.Add(105);
						}
						else
						{
							list.Add(99);
						}
						code128Modes = BarcodeGenerator_PLU.Code128Modes.CodeC;
					}
					list.Add(int.Parse(code.Substring(i, 2)));
					i++;
				}
				else
				{
					if (code128Modes != BarcodeGenerator_PLU.Code128Modes.CodeB)
					{
						if (code128Modes == BarcodeGenerator_PLU.Code128Modes.CodeUnset)
						{
							list.Add(104);
						}
						else
						{
							list.Add(100);
						}
						code128Modes = BarcodeGenerator_PLU.Code128Modes.CodeB;
					}
					list.Add(this.EncodeCodeB(code[i]));
				}
			}
			list.Add(this.CheckDigitCode128(list));
			string text = "";
			for (int i = 0; i < list.Count; i++)
			{
				text += BarcodeGenerator_PLU.Code128Encoding[list[i]];
			}
			text += BarcodeGenerator_PLU.Code128Stop;
			text += "11";
			int num = (int)Math.Floor((double)(this.width - 2) / (double)(text.Length + 20));
			if (num <= 0)
			{
				num = 1;
			}
			int num2 = num * 10;
			if (this.centered)
			{
				x -= (num * text.Length + num2 * 2) / 2;
			}
			int num3 = x + num2;
			for (int i = 1; i <= text.Length; i++)
			{
				string a = text.Substring(i - 1, 1);
				if (a == "0")
				{
					g.FillRectangle(Brushes.White, num3, y, num, this.height);
				}
				else
				{
					g.FillRectangle(Brushes.Black, num3, y, num, this.height);
				}
				num3 += num;
			}
			if (this.humanReadable)
			{
				Font font = new Font(this.fontName, (float)this.fontSize, FontStyle.Bold);
				SizeF sizeF = g.MeasureString(code, font);
				x += (num * text.Length + num2 * 2) / 2;
				x -= (int)sizeF.Width / 2;
				g.DrawString(code, font, Brushes.Black, (float)x, (float)(y + this.height + 4));
				g.Save();
			}
			int num4 = 0;
			int num5 = 0;
			for (int j = 0; j < bitmap.Width; j++)
			{
				for (int k = 0; k < bitmap.Height; k++)
				{
					Color pixel = bitmap.GetPixel(j, k);
					if (pixel == Color.FromArgb(0))
					{
						bitmap.SetPixel(j, k, Color.White);
						if (k == 1 && (num4 == 0 || num4 + 1 == j))
						{
							num4 = j;
						}
						else if (k == 1 && num5 == 0)
						{
							num5 = j;
						}
					}
				}
			}
			Rectangle cropArea = new Rectangle(num4, 0, num5, 60);
			return BarcodeGenerator_PLU.cropImage(bitmap, cropArea);
		}

		/// <summary>
		/// Recorta una imagen al área especificada para ventanas formato 8045
		/// Crea una copia de la imagen original recortada al rectángulo indicado
		/// 
		/// USO EN FORMATO 8045:
		/// - Elimina espacios blancos innecesarios de ventanas de cítricos
		/// - Optimiza el tamaño del archivo de imagen para exportación
		/// - Asegura que la ventana tenga exactamente 60px de altura (MÁS COMPACTO)
		/// - Importante para maximizar espacio en etiquetas con mucha información
		/// </summary>
		/// <param name="img">Imagen original a recortar</param>
		/// <param name="cropArea">Rectángulo que define el área de recorte</param>
		/// <returns>Nueva imagen recortada en formato 8045</returns>
		private static Image cropImage(Image img, Rectangle cropArea)
		{
			Bitmap bitmap = new Bitmap(img);
			return bitmap.Clone(cropArea, bitmap.PixelFormat);
		}

		/// <summary>
		/// Recorta una imagen PLU al área especificada - Formato 8045
		/// Versión especializada con nomenclatura PLU para ventanas de cítricos
		/// 
		/// NOTA: Esta función es idéntica a cropImage() pero con nombres de parámetros
		/// específicos de PLU para claridad en el código de ventanas 8045
		/// 
		/// DIFERENCIA CON PLU 2022:
		/// Aunque la función es igual, se usa con recorte de 60px (vs 75px en 2022)
		/// </summary>
		/// <param name="img_PLU">Imagen PLU original a recortar</param>
		/// <param name="cropArea_PLU">Rectángulo que define el área de recorte PLU</param>
		/// <returns>Nueva imagen PLU recortada formato 8045</returns>
		private static Image cropImage_PLU(Image img_PLU, Rectangle cropArea_PLU)
		{
			Bitmap bitmap = new Bitmap(img_PLU);
			return bitmap.Clone(cropArea_PLU, bitmap.PixelFormat);
		}

		/// <summary>
		/// Calcula el dígito de verificación para Code 128 en ventanas formato 8045
		/// Usa el algoritmo estándar Code 128 con checksum módulo 103
		/// 
		/// ALGORITMO CODE 128 (UNIVERSAL):
		/// 1. Toma el primer código (código de inicio: 103, 104 o 105) como base
		/// 2. Para cada código siguiente, lo multiplica por su POSICIÓN y suma
		/// 3. El resultado MÓDULO 103 es el dígito de verificación
		/// 
		/// APLICACIÓN FORMATO 8045:
		/// - Ventanas de cítricos: Validación de códigos de lote
		/// - Cajas de exportación: Verificación de integridad
		/// - Rastreo de pallets: Detección de errores
		/// </summary>
		/// <param name="codes">Lista de códigos Code 128</param>
		/// <returns>Dígito de verificación (0-102)</returns>
		private int CheckDigitCode128(List<int> codes)
		{
			// Inicia con el primer código (código de inicio)
			int num = codes[0];
			// Suma cada código multiplicado por su posición (empezando en 1)
			for (int i = 1; i < codes.Count; i++)
			{
				num += codes[i] * i;
			}
			// Retorna el resultado módulo 103
			return num % 103;
		}

		/// <summary>
		/// Verifica si un carácter es un dígito numérico (0-9)
		/// Usado para optimización automática a Code C en ventanas 8045
		/// 
		/// IMPORTANCIA EN FORMATO 8045:
		/// - Códigos de lote de cítricos suelen ser numéricos
		/// - Fechas de cosecha en formato YYYYMMDD
		/// - Números de pallet y contenedor
		/// - Code C reduce ancho del código en ventanas con espacio limitado
		/// </summary>
		/// <param name="value">Carácter a verificar</param>
		/// <returns>True si es dígito (0-9), False en caso contrario</returns>
		private bool IsNumber(char value)
		{
			return '0' == value || '1' == value || '2' == value || '3' == value || '4' == value || '5' == value || '6' == value || '7' == value || '8' == value || '9' == value;
		}

		/// <summary>
		/// Verifica si un número entero es PAR
		/// Usa operación bit a bit (AND con 1) para máxima eficiencia
		/// 
		/// TÉCNICA:
		/// - Número par: bit menos significativo = 0 (ej: 4 = 100b, 4 & 1 = 0)
		/// - Número impar: bit menos significativo = 1 (ej: 5 = 101b, 5 & 1 = 1)
		/// </summary>
		/// <param name="value">Número a verificar</param>
		/// <returns>True si es par, False si es impar</returns>
		private bool IsEven(int value)
		{
			return (value & 1) == 0;
		}

		/// <summary>
		/// Verifica si un número entero es IMPAR
		/// Usa operación bit a bit (AND con 1) para máxima eficiencia
		/// </summary>
		/// <param name="value">Número a verificar</param>
		/// <returns>True si es impar, False si es par</returns>
		private bool IsOdd(int value)
		{
			return (value & 1) == 1;
		}

		/// <summary>
		/// Codifica un carácter según la tabla Code 128 subset B para ventanas 8045
		/// Code B es el subconjunto más usado en ventanas de cítricos porque incluye:
		/// - Letras MAYÚSCULAS (nombres de variedades: "LIMÓN", "NARANJA")
		/// - Letras minúsculas (descripciones: "orgánico", "exportación")
		/// - Dígitos 0-9 (códigos de lote, fechas)
		/// - Caracteres especiales ($, /, kg, etc.)
		/// 
		/// PROCESO DE CODIFICACIÓN:
		/// 1. Busca el carácter en Code128ComboAB (64 caracteres comunes A y B)
		/// 2. Si no está, busca en Code128B (31 letras minúsculas exclusivas)
		/// 3. Retorna el índice correspondiente (0-94)
		/// 4. Si no existe, lanza excepción
		/// 
		/// EJEMPLOS VENTANA 8045:
		/// - 'L' (mayúscula) → índice 44 (está en ComboAB) - "LIMÓN"
		/// - 'o' (minúscula) → índice 79 (64 de ComboAB + 15 de Code128B) - "orgánico"
		/// - '/' (especial) → índice 15 (está en ComboAB) - "kg/caja"
		/// - '2' (dígito) → índice 18 (está en ComboAB) - "2024"
		/// </summary>
		/// <param name="value">Carácter a codificar</param>
		/// <returns>Índice del carácter en la tabla de codificación (0-94)</returns>
		/// <exception cref="Exception">Se lanza si el carácter no es válido para Code 128</exception>
		private int EncodeCodeB(char value)
		{
			// PASO 1: Busca en la tabla ComboAB (64 caracteres comunes)
			for (int i = 0; i < BarcodeGenerator_PLU.Code128ComboAB.Length; i++)
			{
				if (BarcodeGenerator_PLU.Code128ComboAB[i] == value)
				{
					return i;  // Retorna índice directo (0-63)
				}
			}
			
			// PASO 2: Busca en la tabla específica de Code B (31 caracteres)
			for (int i = 0; i < BarcodeGenerator_PLU.Code128B.Length; i++)
			{
				if (BarcodeGenerator_PLU.Code128B[i] == value)
				{
					// Retorna el índice ajustado: 64 (tamaño ComboAB) + posición en Code128B
					return i + BarcodeGenerator_PLU.Code128ComboAB.Length;
				}
			}
			
			// PASO 3: Carácter no válido, lanza excepción
			throw new Exception("Invalid Character");
		}

		// ==================== MIEMBROS PRIVADOS DE CONFIGURACIÓN FORMATO 8045 ====================
		
		/// <summary>
		/// Altura del código de barras formato 8045 en píxeles
		/// Valor por defecto: 130 píxeles (estándar para etiquetas de productos)
		/// NOTA: La altura final recortada será de 60px (MÁS COMPACTO que 2022 con 75px)
		/// VENTAJA 8045: Maximiza espacio vertical en ventanas de cítricos con mucha info
		/// </summary>
		private int height;

		/// <summary>
		/// Indica si se debe mostrar el texto legible por humanos en ventanas 8045
		/// Valor por defecto: true
		/// FORMATO 8045: Texto estándar 12pt (no grande como PLU 2022 de 30pt)
		/// MOTIVO: Ventanas se leen de cerca, no necesitan fuente gigante
		/// </summary>
		private bool humanReadable;

		/// <summary>
		/// Ancho del código de barras formato 8045 en píxeles
		/// Valor por defecto: 280 píxeles
		/// DIFERENCIACIÓN:
		/// - PLU 2022 (balanza): 225px (PEQUEÑO - espacio muy limitado)
		/// - PLU 8045 (ventana): 280px (MEDIANO - equilibrio perfecto)
		/// - Estándar (PTI): 640px (GRANDE - uso general)
		/// MOTIVO: Ventanas de cítricos tienen más espacio que balanza pero menos que PTI
		/// </summary>
		private int width;

		/// <summary>
		/// Nombre de la fuente utilizada para el texto legible en ventanas 8045
		/// Valor por defecto: "Courier New" (fuente monoespaciada)
		/// FORMATO 8045: Usa misma fuente que estándar
		/// </summary>
		private string fontName;

		/// <summary>
		/// Tamaño en puntos de la fuente para el texto legible en formato 8045
		/// Valor por defecto: 12 puntos (ESTÁNDAR)
		/// COMPARATIVA COMPLETA:
		/// - PLU 2022 (balanza): 30pt (2.5x más grande) - lectura a distancia en mostrador
		/// - PLU 8045 (ventana): 12pt (estándar) - lectura normal de cerca
		/// - Code 128 estándar: 12pt (estándar) - lectura normal
		/// VENTAJA 8045: Fuente estándar ahorra espacio para más información en ventana
		/// </summary>
		private int fontSize;

		/// <summary>
		/// Indica si el código de barras debe estar centrado horizontalmente en formato 8045
		/// Valor por defecto: false
		/// FORMATO 8045: Centrado importante para ventanas de cítricos con diseño específico
		/// </summary>
		private bool centered;

		// Token: 0x04000058 RID: 88
		private static string[] left_UPCA = new string[]
		{
			"0001101",
			"0011001",
			"0010011",
			"0111101",
			"0100011",
			"0110001",
			"0101111",
			"0111011",
			"0110111",
			"0001011"
		};

		// Token: 0x04000059 RID: 89
		private static string[] right_UPCA = new string[]
		{
			"1110010",
			"1100110",
			"1101100",
			"1000010",
			"1011100",
			"1001110",
			"1010000",
			"1000100",
			"1001000",
			"1110100"
		};

		// ==================== TABLA DE CODIFICACIÓN INTERLEAVED 2 OF 5 FORMATO 8045 ====================
		
		/// <summary>
		/// Tabla de codificación para Interleaved 2 of 5 (ITF) en ventanas formato 8045
		/// Cada dígito (0-9) se codifica con 5 elementos alternando anchos
		/// 'W' = Wide (elemento ancho - 2 o 3 módulos)
		/// 'N' = Narrow (elemento angosto - 1 módulo)
		/// 
		/// CARACTERÍSTICA CLAVE DE ITF EN FORMATO 8045:
		/// - Los dígitos se codifican en PARES
		/// - Un dígito se codifica en las BARRAS
		/// - El siguiente dígito se codifica en los ESPACIOS
		/// - Cada patrón tiene exactamente 2 elementos anchos y 3 angostos
		/// 
		/// USO EN VENTANAS 8045:
		/// - Códigos de cajas de cítricos para exportación
		/// - Identificación de pallets completos
		/// - Números de lote de producción
		/// - Códigos de rastreo de contenedores
		/// 
		/// VENTAJA FORMATO 8045:
		/// Ancho mediano (280px) permite ITF legible sin ocupar mucho espacio en ventana
		/// </summary>
		private static string[] both_2of5 = new string[]
		{
			"NNWWN",  // Dígito 0
			"WNNNW",  // Dígito 1
			"NWNNW",  // Dígito 2
			"WWNNN",  // Dígito 3
			"NNWNW",  // Dígito 4
			"WNWNN",  // Dígito 5
			"NWWNN",  // Dígito 6
			"NNNWW",  // Dígito 7
			"WNNWN",  // Dígito 8
			"NWNWN"   // Dígito 9
		};

		// ==================== TABLAS DE CODIFICACIÓN CODE 128 FORMATO 8045 ====================
		
		/// <summary>
		/// Caracteres comunes a Code 128 subconjuntos A y B - Formato 8045
		/// Incluye 64 caracteres del espacio (' ') al guión bajo ('_')
		/// 
		/// CONTENIDO:
		/// - Espacio y caracteres especiales: ! " # $ % & ' ( ) * + , - . /
		/// - Dígitos: 0 1 2 3 4 5 6 7 8 9
		/// - Más caracteres especiales: : ; < = > ? @
		/// - Letras MAYÚSCULAS: A B C D E F G H I J K L M N O P Q R S T U V W X Y Z
		/// - Caracteres finales: [ \ ] ^ _
		/// 
		/// USO EN VENTANAS FORMATO 8045:
		/// - Nombres de variedades de cítricos: "LIMÓN", "NARANJA", "MANDARINA"
		/// - Códigos de productor: "PROD-123"
		/// - Fechas de cosecha: "2024-03-15"
		/// - Información de exportación: "USA/CAN"
		/// 
		/// ÍNDICES: 0-63
		/// Estos caracteres tienen el mismo índice en Code A y Code B
		/// </summary>
		private static char[] Code128ComboAB = new char[]
		{
			' ',
			'!',
			'"',
			'#',
			'$',
			'%',
			'&',
			'\'',
			'(',
			')',
			'*',
			'+',
			',',
			'-',
			'.',
			'/',
			'0',
			'1',
			'2',
			'3',
			'4',
			'5',
			'6',
			'7',
			'8',
			'9',
			':',
			';',
			'<',
			'=',
			'>',
			'?',
			'@',
			'A',
			'B',
			'C',
			'D',
			'E',
			'F',
			'G',
			'H',
			'I',
			'J',
			'K',
			'L',
			'M',
			'N',
			'O',
			'P',
			'Q',
			'R',
			'S',
			'T',
			'U',
			'V',
			'W',
			'X',
			'Y',
			'Z',
			'[',
			'\\',
			']',
			'^',
			'_'
		};

		/// <summary>
		/// Caracteres ESPECÍFICOS del Code 128 subconjunto B - Formato 8045
		/// Incluye 31 caracteres de letras MINÚSCULAS y símbolos adicionales
		/// 
		/// CONTENIDO:
		/// - Acento grave: `
		/// - Letras minúsculas: a b c d e f g h i j k l m n o p q r s t u v w x y z
		/// - Caracteres especiales finales: { | } ~
		/// 
		/// USO EN VENTANAS 8045:
		/// - Descripciones mixtas: "Limón Meyer orgánico"
		/// - Información de calidad: "extra", "premium"
		/// - Códigos de rastreo: "lote-a2024"
		/// - Nombres de exportador: "Fruticola del Norte"
		/// 
		/// ÍNDICES: 64-94 (sumando 64 a los índices de este array)
		/// Estos caracteres SOLO están disponibles en Code B (no en Code A)
		/// </summary>
		private static char[] Code128B = new char[]
		{
			'`',
			'a',
			'b',
			'c',
			'd',
			'e',
			'f',
			'g',
			'h',
			'i',
			'j',
			'k',
			'l',
			'm',
			'n',
			'o',
			'p',
			'q',
			'r',
			's',
			't',
			'u',
			'v',
			'w',
			'x',
			'y',
			'z',
			'{',
			'|',
			'}',
			'~'
		};

		/// <summary>
		/// Tabla COMPLETA de codificación Code 128 para ventanas formato 8045
		/// Contiene 103 patrones de barras (índices 0-102)
		/// Cada patrón es una cadena de 11 bits ('1' = barra negra, '0' = espacio blanco)
		/// 
		/// ESTRUCTURA:
		/// - Índices 0-94: Caracteres imprimibles (según subconjunto A, B o C)
		/// - Índices 95-102: Caracteres especiales y de control
		/// 
		/// CÓDIGOS ESPECIALES EN FORMATO 8045:
		/// - 99: Cambio a Code C (pares de dígitos) - Para fechas y números de lote
		/// - 100: Cambio a Code B (alfanuméricos) - Para nombres y descripciones
		/// - 101: Cambio a Code A (control) - RARO en ventanas
		/// - 103: Inicio Code A (START A)
		/// - 104: Inicio Code B (START B) - MÁS USADO en ventanas de cítricos
		/// - 105: Inicio Code C (START C) - Para códigos completamente numéricos
		/// 
		/// EJEMPLO VENTANA 8045:
		/// "NARANJA 2024" en etiqueta de caja:
		/// 1. START B (104) - inicia con letras
		/// 2. N-A-R-A-N-J-A (7 caracteres)
		/// 3. espacio
		/// 4. CHANGE C (99) - cambia a números
		/// 5. "20" como un símbolo (Code C)
		/// 6. "24" como un símbolo (Code C)
		/// 
		/// OPTIMIZACIÓN FORMATO 8045:
		/// Ancho mediano (280px) permite codes más largos que balanza (225px)
		/// pero más compactos que estándar (640px)
		/// 
		/// FORMATO DE PATRÓN (11 bits):
		/// Cada patrón tiene exactamente 11 módulos (6 barras/espacios)
		/// Siempre empieza con barra ('1') y alterna barras y espacios
		/// </summary>
		private static string[] Code128Encoding = new string[]
		{
			"11011001100",
			"11001101100",
			"11001100110",
			"10010011000",
			"10010001100",
			"10001001100",
			"10011001000",
			"10011000100",
			"10001100100",
			"11001001000",
			"11001000100",
			"11000100100",
			"10110011100",
			"10011011100",
			"10011001110",
			"10111001100",
			"10011101100",
			"10011100110",
			"11001110010",
			"11001011100",
			"11001001110",
			"11011100100",
			"11001110100",
			"11101101110",
			"11101001100",
			"11100101100",
			"11100100110",
			"11101100100",
			"11100110100",
			"11100110010",
			"11011011000",
			"11011000110",
			"11000110110",
			"10100011000",
			"10001011000",
			"10001000110",
			"10110001000",
			"10001101000",
			"10001100010",
			"11010001000",
			"11000101000",
			"11000100010",
			"10110111000",
			"10110001110",
			"10001101110",
			"10111011000",
			"10111000110",
			"10001110110",
			"11101110110",
			"11010001110",
			"11000101110",
			"11011101000",
			"11011100010",
			"11011101110",
			"11101011000",
			"11101000110",
			"11100010110",
			"11101101000",
			"11101100010",
			"11100011010",
			"11101111010",
			"11001000010",
			"11110001010",
			"10100110000",
			"10100001100",
			"10010110000",
			"10010000110",
			"10000101100",
			"10000100110",
			"10110010000",
			"10110000100",
			"10011010000",
			"10011000010",
			"10000110100",
			"10000110010",
			"11000010010",
			"11001010000",
			"11110111010",
			"11000010100",
			"10001111010",
			"10100111100",
			"10010111100",
			"10010011110",
			"10111100100",
			"10011110100",
			"10011110010",
			"11110100100",
			"11110010100",
			"11110010010",
			"11011011110",
			"11011110110",
			"11110110110",
			"10101111000",
			"10100011110",
			"10001011110",
			"10111101000",
			"10111100010",
			"11110101000",
			"11110100010",
			"10111011110",
			"10111101110",
			"11101011110",
			"11110101110",
			"11010000100",
			"11010010000",
			"11010011100"
		};

		/// <summary>
		/// Código de PARADA para Code 128 en ventanas formato 8045
		/// Este patrón especial marca el FINAL de un código de barras Code 128
		/// 
		/// PATRÓN: "11000111010" (13 módulos)
		/// A diferencia de los códigos normales (11 módulos), el código de parada tiene 13
		/// 
		/// FUNCIÓN EN FORMATO 8045:
		/// - Indica que el código de barras ha terminado
		/// - Permite al lector de ventana detectar el final de la lectura
		/// - Es obligatorio en todos los códigos Code 128 para ventanas
		/// 
		/// ESTRUCTURA COMPLETA FORMATO 8045:
		/// [Quiet Zone] + [Start] + [Datos Ventana] + [Checksum] + [Stop] + [Quiet Zone]
		///     10 mod      11 mod     variable          11 mod      13 mod     10 mod
		/// 
		/// FORMATO 8045 COMPACTO:
		/// Recorte final de 60px (vs 75px en PLU 2022) maximiza espacio vertical
		/// </summary>
		private static string Code128Stop = "11000111010";

		// ==================== ENUMERACIONES PARA CODE 128 EN FORMATO 8045 ====================

		/// <summary>
		/// Códigos de CAMBIO entre subconjuntos Code 128 para ventanas formato 8045
		/// Se usan cuando ya se está codificando y se necesita cambiar de modo
		/// 
		/// USO TÍPICO EN VENTANAS 8045:
		/// "NARANJA NAVEL 2024-03" se codificaría como:
		/// [START B] + [NARANJA NAVEL ] + [CHANGE C] + [20][24] + [CHANGE B] + [-03]
		/// 
		/// DIFERENCIA CON StartModes:
		/// - StartModes: Se usa al INICIO del código de barras en ventana
		/// - ChangeModes: Se usa para CAMBIAR de modo en medio de la codificación
		/// 
		/// VALORES:
		/// - CodeA (101): Cambia a Code A (control) - RARO en ventanas de cítricos
		/// - CodeB (100): Cambia a Code B (alfanuméricos) - FRECUENTE en ventanas
		/// - CodeC (99): Cambia a Code C (números) - MUY FRECUENTE para fechas y lotes
		/// 
		/// OPTIMIZACIÓN FORMATO 8045:
		/// Ancho mediano (280px) permite cambios de modo sin preocuparse tanto
		/// por el espacio como en PLU 2022 (225px)
		/// </summary>
		private enum Code128ChangeModes
		{
			/// <summary>
			/// Cambio a Code A (valor 101)
			/// Permite codificar caracteres de control ASCII 0-31
			/// RARO EN FORMATO 8045: Ventanas de cítricos no usan caracteres de control
			/// </summary>
			CodeA = 101,
			
			/// <summary>
			/// Cambio a Code B (valor 100)
			/// Permite codificar mayúsculas, minúsculas y caracteres especiales
			/// FRECUENTE EN 8045: Para nombres de productos y descripciones
			/// </summary>
			CodeB = 100,
			
			/// <summary>
			/// Cambio a Code C (valor 99)
			/// Permite codificar pares de dígitos (00-99)
			/// MUY FRECUENTE EN 8045: Para fechas de cosecha, lotes y códigos de exportación
			/// VENTAJA: Reduce ancho del código en ventanas con espacio limitado
			/// </summary>
			CodeC = 99
		}

		/// <summary>
		/// Códigos de INICIO para Code 128 en ventanas formato 8045
		/// Se usan al PRINCIPIO del código de barras para indicar qué subconjunto usar
		/// 
		/// VALORES:
		/// - CodeUnset (0): Estado inicial, aún no se ha definido el modo
		/// - CodeA (103): Inicia con Code A (control) - RARO en ventanas
		/// - CodeB (104): Inicia con Code B (alfanuméricos) - MÁS COMÚN en 8045
		/// - CodeC (105): Inicia con Code C (números) - Para códigos completamente numéricos
		/// 
		/// SELECCIÓN AUTOMÁTICA EN FORMATO 8045:
		/// El generador elige automáticamente el mejor modo de inicio:
		/// - "NARANJA" → START B (letras)
		/// - "20240315" (fecha YYYYMMDD) → START C (todo números)
		/// - "LOTE-2024" → START B (empieza con letras, luego CHANGE C)
		/// 
		/// DIFERENCIA CON ChangeModes:
		/// - StartModes: Primer símbolo del código de barras en ventana
		/// - ChangeModes: Símbolos para cambiar de modo durante la codificación
		/// 
		/// FORMATO 8045 - EQUILIBRIO:
		/// Ancho mediano (280px) permite códigos más largos que balanza (225px)
		/// sin desperdiciar espacio como estándar (640px)
		/// </summary>
		private enum Code128StartModes
		{
			/// <summary>
			/// Modo no definido (valor 0)
			/// Estado inicial antes de seleccionar un subconjunto para ventana formato 8045
			/// </summary>
			CodeUnset,
			
			/// <summary>
			/// Modo Code A (valor 103)
			/// Códigos de control ASCII 0-31 y mayúsculas
			/// RARO EN FORMATO 8045: Ventanas de cítricos no usan caracteres de control
			/// </summary>
			CodeA = 103,
			
			/// <summary>
			/// Modo Code B (valor 104)
			/// Mayúsculas, minúsculas y caracteres especiales
			/// MÁS USADO EN 8045: Inicio típico para ventanas con nombres de productos
			/// </summary>
			CodeB,
			
			/// <summary>
			/// Modo Code C (valor 105)
			/// Pares de dígitos (00-99)
			/// FRECUENTE EN 8045: Para códigos completamente numéricos (fechas, lotes)
			/// EFICIENTE: Reduce ancho del código en ventanas con espacio medio (280px)
			/// </summary>
			CodeC
		}

		/// <summary>
		/// Enumeración para rastrear el MODO ACTUAL durante la codificación formato 8045
		/// Se usa internamente en DrawCode128() para saber en qué modo estamos
		/// 
		/// DIFERENCIA CON StartModes y ChangeModes:
		/// - Code128Modes: Variable de estado (¿en qué modo estoy ahora?)
		/// - Code128StartModes: Constantes de inicio (¿cómo empiezo?)
		/// - Code128ChangeModes: Constantes de cambio (¿cómo cambio?)
		/// 
		/// VALORES:
		/// - CodeUnset (0): Aún no se ha definido (estado inicial)
		/// - CodeA (1): Actualmente codificando en modo A (raro en ventanas)
		/// - CodeB (2): Actualmente codificando en modo B (común en ventanas)
		/// - CodeC (3): Actualmente codificando en modo C (frecuente en ventanas)
		/// 
		/// FLUJO TÍPICO EN VENTANA FORMATO 8045:
		/// Ejemplo: "NARANJA VALENCIA 2024-03-15"
		/// 1. Inicia en CodeUnset
		/// 2. Detecta letras → cambia a CodeB (START B)
		/// 3. Codifica "NARANJA VALENCIA "
		/// 4. Detecta números "2024" → cambia a CodeC (CHANGE C)
		/// 5. Codifica "20" y "24" (dos símbolos)
		/// 6. Detecta "-" → cambia a CodeB (CHANGE B)
		/// 7. Codifica "-"
		/// 8. Detecta "03" → cambia a CodeC (CHANGE C)
		/// 9. Codifica "03" y "15" (dos símbolos)
		/// 
		/// FORMATO 8045 - ESPACIO EQUILIBRADO:
		/// Ancho mediano (280px) permite múltiples cambios de modo sin
		/// preocuparse tanto por el espacio como en balanza PLU 2022 (225px)
		/// </summary>
		private enum Code128Modes
		{
			/// <summary>
			/// Modo no definido (valor 0)
			/// Estado inicial del generador formato 8045 antes de comenzar la codificación
			/// </summary>
			CodeUnset,
			
			/// <summary>
			/// Modo Code A (valor 1)
			/// Actualmente codificando caracteres de control
			/// RARO EN FORMATO 8045: Ventanas de cítricos casi nunca usan control
			/// </summary>
			CodeA,
			
			/// <summary>
			/// Modo Code B (valor 2)
			/// Actualmente codificando texto alfanumérico
			/// COMÚN EN FORMATO 8045: Nombres de productos, variedades, descripciones
			/// </summary>
			CodeB,
			
			/// <summary>
			/// Modo Code C (valor 3)
			/// Actualmente codificando pares de dígitos
			/// FRECUENTE EN FORMATO 8045: Fechas (YYYYMMDD), lotes, códigos de exportación
			/// EFICIENTE: Reduce ancho del código en ventanas con espacio mediano (280px)
			/// </summary>
			CodeC
		}
	}
}
