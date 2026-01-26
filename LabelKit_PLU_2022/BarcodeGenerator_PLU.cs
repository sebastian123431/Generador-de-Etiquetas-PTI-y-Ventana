using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace LabelKit_PLU_2022
{
	/// <summary>
	/// Generador de códigos de barras PLU (Price Look-Up) - Versión 2022
	/// Especializado en productos con peso variable para la temporada 2022
	/// Genera códigos PLU más pequeños (225x130) con fuente grande (30pt)
	/// Mantiene compatibilidad con formatos históricos de etiquetas
	/// </summary>
	internal class BarcodeGenerator_PLU
	{
		/// <summary>
		/// Constructor por defecto del generador de códigos PLU 2022
		/// Inicializa valores predeterminados: tamaño 225x130, fuente Courier New 30pt
		/// </summary>
		public BarcodeGenerator_PLU()
		{
			this.width = 225;
			this.height = 130;
			this.humanReadable = true;
			this.fontSize = 30;
			this.fontName = "Courier New";
			this.centered = false;
		}

		/// <summary>
		/// Indica si el código de barras PLU debe estar centrado horizontalmente
		/// Cuando es true, el código se centra en el ancho especificado
		/// Útil para etiquetas con peso variable donde se necesita alineación precisa
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
		/// Nombre de la fuente para el texto legible por humanos en etiquetas PLU
		/// Por defecto: "Courier New" (fuente monoespaciada, ideal para códigos)
		/// La fuente grande (30pt) es importante para PLU ya que se leen a distancia
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
		/// Ancho de la imagen del código de barras PLU en píxeles
		/// Por defecto: 225 píxeles (más pequeño que Code 128 estándar)
		/// Optimizado para etiquetas de peso variable donde el espacio es limitado
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
		/// Altura de la imagen del código de barras PLU en píxeles
		/// Por defecto: 130 píxeles (estándar para etiquetas de productos)
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
		/// Indica si se debe mostrar el texto legible por humanos debajo del código PLU
		/// Por defecto: true
		/// IMPORTANTE: En PLU es crítico que el texto sea legible para verificación manual
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
		/// Tamaño de la fuente para el texto legible en etiquetas PLU
		/// Por defecto: 30 puntos (MUY GRANDE comparado con Code 128 estándar que usa 12pt)
		/// RAZÓN: Las etiquetas PLU deben ser legibles desde cierta distancia en el mostrador
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
		/// Dibuja un código de barras UPC-A adaptado para etiquetas PLU (Price Look-Up)
		/// Versión especializada para productos con peso variable en la temporada 2022
		/// 
		/// DIFERENCIAS CON VERSION ESTÁNDAR:
		/// - Usa fuente más grande (30pt vs 12pt) para mejor legibilidad en mostrador
		/// - Ancho reducido (225px vs 640px) para etiquetas de precio pequeñas
		/// - Optimizado para impresión en etiquetas de balanza
		/// 
		/// USO TÍPICO:
		/// - Frutas y verduras vendidas por peso
		/// - Productos de panadería y carnicería
		/// - Cualquier artículo con peso variable
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
		/// Calcula el dígito verificador para códigos UPC-A en etiquetas PLU
		/// Usa el mismo algoritmo estándar pero adaptado para PLU de peso variable
		/// 
		/// ALGORITMO UPC-A:
		/// 1. Suma dígitos en posiciones IMPARES (1,3,5,7,9,11) y multiplica por 3
		/// 2. Suma dígitos en posiciones PARES (2,4,6,8,10)
		/// 3. Suma ambos resultados
		/// 4. El dígito verificador es lo que falta para el próximo múltiplo de 10
		/// 
		/// EJEMPLO PRÁCTICO:
		/// Código: 12345678901
		/// Impares (posiciones 1,3,5,7,9,11): 1+3+5+7+9+1 = 26 * 3 = 78
		/// Pares (posiciones 2,4,6,8,10): 2+4+6+8+0 = 20
		/// Suma total: 78 + 20 = 98
		/// Checksum: (10 - (98 % 10)) % 10 = 2
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
		/// Versión simplificada para etiquetas PLU
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
		/// Dibuja un código de barras Interleaved 2 of 5 (ITF) para etiquetas PLU
		/// Versión 2022 optimizada para productos de peso variable
		/// 
		/// CARACTERÍSTICAS PLU:
		/// - Fuente grande (30pt) para lectura a distancia
		/// - Ancho reducido (225px) para etiquetas pequeñas de balanza
		/// - ITF es ideal para cajas y embalajes secundarios de productos PLU
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
		/// Calcula el dígito verificador para códigos Interleaved 2 of 5 en etiquetas PLU
		/// Similar al algoritmo UPC-A pero con las posiciones invertidas
		/// 
		/// DIFERENCIA CON UPC-A:
		/// - UPC-A: Impares *3 + Pares
		/// - ITF: Pares + Impares *3 (invertido)
		/// 
		/// ALGORITMO:
		/// 1. Suma dígitos en posiciones PARES (2,4,6,8...)
		/// 2. Suma dígitos en posiciones IMPARES (1,3,5,7...) y multiplica por 3
		/// 3. El dígito verificador es lo que falta para el próximo múltiplo de 10
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
		/// Dibuja un código de barras Code 128 optimizado para etiquetas PLU - Versión 2022
		/// 
		/// DIFERENCIAS CLAVE CON VERSIÓN ESTÁNDAR:
		/// - Ancho: 225px (vs 640px) - Optimizado para etiquetas de balanza
		/// - Fuente: 30pt (vs 12pt) - Mayor legibilidad en mostrador
		/// - Altura recorte: 75px (vs 120px) - Etiquetas PLU más compactas
		/// 
		/// APLICACIONES TÍPICAS:
		/// - Etiquetas de productos frescos (frutas, verduras)
		/// - Productos de panadería pesados en el momento
		/// - Carnes y embutidos con peso variable
		/// - Productos a granel del supermercado
		/// 
		/// TEMPORADA 2022:
		/// - Mantiene compatibilidad con sistemas de balanza legacy
		/// - Formato histórico para migración gradual a sistemas nuevos
		/// 
		/// PROCESO (10 PASOS):
		/// 1. Crear bitmap PLU (225x130px)
		/// 2. Analizar texto y elegir subconjunto Code 128 (A/B/C)
		/// 3. Optimizar automáticamente (Code C para números)
		/// 4. Agregar dígito de verificación
		/// 5. Convertir a patrones de barras
		/// 6. Calcular dimensiones (adaptado al ancho reducido)
		/// 7. Ajustar posición si está centrado
		/// 8. Dibujar barras con grosor ajustado
		/// 9. Agregar texto legible GRANDE (30pt)
		/// 10. Recortar a 75px de altura (vs 120px estándar)
		/// </summary>
		/// <param name="g">Contexto gráfico (se reemplaza internamente)</param>
		/// <param name="code">Texto a codificar (cualquier carácter ASCII)</param>
		/// <param name="x">Posición X inicial</param>
		/// <param name="y">Posición Y inicial</param>
		/// <returns>Imagen del código de barras PLU recortada a 75px de altura</returns>
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
			Rectangle cropArea = new Rectangle(num4, 0, num5, 75);
			return BarcodeGenerator_PLU.cropImage(bitmap, cropArea);
		}

		/// <summary>
		/// Recorta una imagen al área especificada para etiquetas PLU
		/// Crea una copia de la imagen original recortada al rectángulo indicado
		/// 
		/// USO EN PLU:
		/// - Elimina espacios blancos innecesarios de la etiqueta
		/// - Optimiza el tamaño del archivo de imagen
		/// - Asegura que la etiqueta tenga exactamente el tamaño deseado
		/// </summary>
		/// <param name="img">Imagen original a recortar</param>
		/// <param name="cropArea">Rectángulo que define el área de recorte</param>
		/// <returns>Nueva imagen recortada en formato PLU</returns>
		private static Image cropImage(Image img, Rectangle cropArea)
		{
			Bitmap bitmap = new Bitmap(img);
			return bitmap.Clone(cropArea, bitmap.PixelFormat);
		}

		/// <summary>
		/// Recorta una imagen PLU al área especificada
		/// Versión especializada con nomenclatura PLU (mismo funcionamiento que cropImage)
		/// 
		/// NOTA: Esta función es idéntica a cropImage() pero con nombres de parámetros
		/// específicos de PLU para claridad en el código legacy de 2022
		/// </summary>
		/// <param name="img_PLU">Imagen PLU original a recortar</param>
		/// <param name="cropArea_PLU">Rectángulo que define el área de recorte PLU</param>
		/// <returns>Nueva imagen PLU recortada</returns>
		private static Image cropImage_PLU(Image img_PLU, Rectangle cropArea_PLU)
		{
			Bitmap bitmap = new Bitmap(img_PLU);
			return bitmap.Clone(cropArea_PLU, bitmap.PixelFormat);
		}

		/// <summary>
		/// Calcula el dígito de verificación para Code 128 en etiquetas PLU
		/// Usa el algoritmo estándar Code 128 con checksum módulo 103
		/// 
		/// ALGORITMO CODE 128:
		/// 1. Toma el primer código (código de inicio: 103, 104 o 105) como base
		/// 2. Para cada código siguiente, lo multiplica por su POSICIÓN y suma
		/// 3. El resultado MÓDULO 103 es el dígito de verificación
		/// 
		/// EJEMPLO:
		/// Códigos: [104, 33, 34, 35]  (START B, 'A', 'B', 'C')
		/// Cálculo: 104 + (33*1) + (34*2) + (35*3) = 104 + 33 + 68 + 105 = 310
		/// Checksum: 310 % 103 = 1
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
		/// Usado para optimización automática a Code C en etiquetas PLU
		/// 
		/// IMPORTANCIA EN PLU:
		/// - Muchos códigos PLU son numéricos (peso, precio, código de producto)
		/// - Code C es más eficiente para números (1 símbolo = 2 dígitos)
		/// - Reduce el ancho del código de barras en etiquetas pequeñas
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
		/// FUNCIONAMIENTO:
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
		/// Codifica un carácter según la tabla Code 128 subset B para etiquetas PLU
		/// Code B es el subconjunto más usado en PLU porque incluye:
		/// - Letras MAYUSCULAS (para nombres de productos)
		/// - Letras minúsculas (para descripciones)
		/// - Dígitos 0-9 (para códigos y precios)
		/// - Caracteres especiales ($, /, kg, etc.)
		/// 
		/// PROCESO DE CODIFICACIÓN:
		/// 1. Busca el carácter en Code128ComboAB (caracteres comunes A y B)
		/// 2. Si no está, busca en Code128B (letras minúsculas exclusivas)
		/// 3. Retorna el índice correspondiente (0-94)
		/// 4. Si no existe, lanza excepción
		/// 
		/// EJEMPLOS:
		/// - 'A' (mayúscula) → índice 33 (está en ComboAB)
		/// - 'a' (minúscula) → índice 65 (64 de ComboAB + 1 de Code128B)
		/// - '$' (especial) → índice 4 (está en ComboAB)
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

		// ==================== MIEMBROS PRIVADOS DE CONFIGURACIÓN PLU ====================
		
		/// <summary>
		/// Altura del código de barras PLU en píxeles
		/// Valor por defecto: 130 píxeles (estándar para etiquetas de productos)
		/// NOTA: La altura final recortada será de 75px para etiquetas PLU compactas
		/// </summary>
		private int height;

		/// <summary>
		/// Indica si se debe mostrar el texto legible por humanos debajo del código PLU
		/// Valor por defecto: true
		/// CRUCIAL EN PLU: El texto grande (30pt) permite verificación visual rápida
		/// </summary>
		private bool humanReadable;

		/// <summary>
		/// Ancho del código de barras PLU en píxeles
		/// Valor por defecto: 225 píxeles
		/// CARACTERÍSTICA PLU: Mucho más pequeño que Code 128 estándar (640px)
		/// MOTIVO: Etiquetas de balanza tienen espacio limitado
		/// </summary>
		private int width;

		/// <summary>
		/// Nombre de la fuente utilizada para el texto legible en etiquetas PLU
		/// Valor por defecto: "Courier New" (fuente monoespaciada)
		/// </summary>
		private string fontName;

		/// <summary>
		/// Tamaño en puntos de la fuente para el texto legible en PLU
		/// Valor por defecto: 30 puntos
		/// DIFERENCIA CLAVE: 2.5x más grande que Code 128 estándar (12pt)
		/// MOTIVO: Legibilidad a distancia en mostradores de supermercado
		/// </summary>
		private int fontSize;

		/// <summary>
		/// Indica si el código de barras PLU debe estar centrado horizontalmente
		/// Valor por defecto: false
		/// </summary>
		private bool centered;

		// Token: 0x0400008E RID: 142
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

		// Token: 0x0400008F RID: 143
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

		// ==================== TABLA DE CODIFICACIÓN INTERLEAVED 2 OF 5 PARA PLU ====================
		
		/// <summary>
		/// Tabla de codificación para Interleaved 2 of 5 (ITF) en etiquetas PLU 2022
		/// Cada dígito (0-9) se codifica con 5 elementos alternando anchos
		/// 'W' = Wide (elemento ancho - 2 o 3 módulos)
		/// 'N' = Narrow (elemento angosto - 1 módulo)
		/// 
		/// CARACTERÍSTICA CLAVE DE ITF EN PLU:
		/// - Los dígitos se codifican en PARES
		/// - Un dígito se codifica en las BARRAS
		/// - El siguiente dígito se codifica en los ESPACIOS
		/// - Cada patrón tiene exactamente 2 elementos anchos y 3 angostos
		/// 
		/// USO EN PLU 2022:
		/// - Cajas de productos frescos
		/// - Embalajes secundarios de productos a granel
		/// - Códigos de logística interna
		/// 
		/// EJEMPLO PRÁCTICO:
		/// Para codificar el par "45" en una etiqueta PLU:
		/// - '4' = "NNWNW" se codifica en barras
		/// - '5' = "WNWNN" se codifica en espacios
		/// Resultado: [barra N][espacio W][barra W][espacio N][barra N][espacio W][barra W][espacio N][barra N]
		/// </summary>
		private static string[] both_2of5 = new string[]
		{
			"NNWWN",  // Dígito 0: 2 anchos en posiciones 3-4
			"WNNNW",  // Dígito 1: 2 anchos en posiciones 1 y 5
			"NWNNW",  // Dígito 2: 2 anchos en posiciones 2 y 5
			"WWNNN",  // Dígito 3: 2 anchos en posiciones 1-2
			"NNWNW",  // Dígito 4: 2 anchos en posiciones 3 y 5
			"WNWNN",  // Dígito 5: 2 anchos en posiciones 1 y 3
			"NWWNN",  // Dígito 6: 2 anchos en posiciones 2-3
			"NNNWW",  // Dígito 7: 2 anchos en posiciones 4-5
			"WNNWN",  // Dígito 8: 2 anchos en posiciones 1 y 4
			"NWNWN"   // Dígito 9: 2 anchos en posiciones 2 y 4
		};

		// ==================== TABLAS DE CODIFICACIÓN CODE 128 PARA ETIQUETAS PLU ====================
		
		/// <summary>
		/// Caracteres comunes a Code 128 subconjuntos A y B - Versión PLU 2022
		/// Incluye 64 caracteres del espacio (' ') al guión bajo ('_')
		/// 
		/// CONTENIDO:
		/// - Espacio y caracteres especiales: ! " # $ % & ' ( ) * + , - . /
		/// - Dígitos: 0 1 2 3 4 5 6 7 8 9 (IMPORTANTE EN PLU: precios, pesos, códigos)
		/// - Más caracteres especiales: : ; < = > ? @
		/// - Letras MAYÚSCULAS: A B C D E F G H I J K L M N O P Q R S T U V W X Y Z
		/// - Caracteres finales: [ \ ] ^ _
		/// 
		/// USO EN PLU 2022:
		/// - Nombres de productos en mayúsculas ("MANZANA ROJA")
		/// - Códigos de producto alfanuméricos ("A123")
		/// - Precios con símbolos ("$12.50/kg")
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
		/// Caracteres ESPECÍFICOS del Code 128 subconjunto B - Versión PLU 2022
		/// Incluye 31 caracteres de letras MINÚSCULAS y símbolos adicionales
		/// 
		/// CONTENIDO:
		/// - Acento grave: `
		/// - Letras minúsculas: a b c d e f g h i j k l m n o p q r s t u v w x y z
		/// - Caracteres especiales finales: { | } ~
		/// 
		/// USO EN PLU 2022:
		/// - Descripciones de productos en minúsculas ("orgánico", "fresco")
		/// - Nombres mixtos ("Manzana Gala")
		/// - URLs o códigos de rastreo ("lote-a123")
		/// 
		/// ÍNDICES: 64-94 (sumando 64 a los índices de este array)
		/// Estos caracteres SOLO están disponibles en Code B (no en Code A)
		/// Code A tiene caracteres de control en estas posiciones
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
		/// Tabla COMPLETA de codificación Code 128 para etiquetas PLU 2022
		/// Contiene 103 patrones de barras (índices 0-102)
		/// Cada patrón es una cadena de 11 bits ('1' = barra negra, '0' = espacio blanco)
		/// 
		/// ESTRUCTURA:
		/// - Índices 0-94: Caracteres imprimibles (según subconjunto A, B o C)
		/// - Índices 95-102: Caracteres especiales y de control
		/// 
		/// CÓDIGOS ESPECIALES EN PLU:
		/// - 99: Cambio a Code C (pares de dígitos) - FRECUENTE EN PLU para precios
		/// - 100: Cambio a Code B (caracteres alfanuméricos) - MÁS USADO EN PLU
		/// - 101: Cambio a Code A (caracteres de control) - RARO EN PLU
		/// - 103: Inicio Code A (START A)
		/// - 104: Inicio Code B (START B) - INICIO TÍPICO EN PLU
		/// - 105: Inicio Code C (START C) - INICIO PARA CÓDIGOS NUMÉRICOS
		/// 
		/// EJEMPLO DE USO EN PLU:
		/// Para codificar "PERA 450" en etiqueta PLU:
		/// 1. START B (104) - inicia con letras
		/// 2. 'P' = índice 48 → Code128Encoding[48]
		/// 3. 'E' = índice 37 → Code128Encoding[37]
		/// 4. 'R' = índice 50 → Code128Encoding[50]
		/// 5. 'A' = índice 33 → Code128Encoding[33]
		/// 6. ' ' = índice 0 → Code128Encoding[0]
		/// 7. CHANGE C (99) - cambia a números
		/// 8. '45' = índice 45 → Code128Encoding[45] (dos dígitos en uno)
		/// 9. '0X' = necesita CODE B de nuevo si no es par
		/// 
		/// OPTIMIZACIÓN PLU:
		/// El generador detecta automáticamente cuándo cambiar a Code C
		/// para reducir el ancho del código en etiquetas pequeñas de balanza
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
		/// Código de PARADA para Code 128 en etiquetas PLU 2022
		/// Este patrón especial marca el FINAL de un código de barras Code 128
		/// 
		/// PATRÓN: "11000111010" (13 módulos)
		/// A diferencia de los códigos normales (11 módulos), el código de parada tiene 13
		/// 
		/// FUNCIÓN EN PLU:
		/// - Indica que el código de barras ha terminado
		/// - Permite al lector de balanza detectar el final de la lectura
		/// - Es obligatorio en todos los códigos Code 128 para PLU
		/// 
		/// ESTRUCTURA COMPLETA DE CÓDIGO PLU:
		/// [Quiet Zone] + [Start] + [Datos PLU] + [Checksum] + [Stop] + [Quiet Zone]
		///     10 mod      11 mod    variable      11 mod      13 mod     10 mod
		/// 
		/// NOTA PLU 2022: El quiet zone es más pequeño que el estándar debido
		/// al tamaño reducido de las etiquetas de balanza (225px vs 640px)
		/// </summary>
		private static string Code128Stop = "11000111010";

		// ==================== ENUMERACIONES PARA CODE 128 EN PLU ====================

		/// <summary>
		/// Códigos de CAMBIO entre subconjuntos Code 128 para etiquetas PLU
		/// Se usan cuando ya se está codificando y se necesita cambiar de modo
		/// 
		/// USO TÍPICO EN PLU:
		/// "MANZANA 2.50" se codificaría como:
		/// [START B] + [M][A][N][Z][A][N][A][ ] + [CHANGE C] + [25][0_padded]
		/// 
		/// DIFERENCIA CON StartModes:
		/// - StartModes: Se usa al INICIO del código de barras PLU
		/// - ChangeModes: Se usa para CAMBIAR de modo en medio de la codificación
		/// 
		/// VALORES:
		/// - CodeA (101): Cambia a Code A (caracteres de control) - RARO EN PLU
		/// - CodeB (100): Cambia a Code B (alfanuméricos) - FRECUENTE EN PLU
		/// - CodeC (99): Cambia a Code C (pares de dígitos) - MUY FRECUENTE EN PLU
		/// 
		/// OPTIMIZACIÓN PLU 2022:
		/// El generador cambia automáticamente a Code C cuando detecta:
		/// - Precios ("12.50" → "1250")
		/// - Pesos ("0.750" → "0750")
		/// - Códigos de producto numéricos
		/// Esto reduce el ancho del código en etiquetas pequeñas
		/// </summary>
		private enum Code128ChangeModes
		{
			/// <summary>
			/// Cambio a Code A (valor 101)
			/// Permite codificar caracteres de control ASCII 0-31
			/// RARO EN PLU: Casi nunca se usan caracteres de control en etiquetas de productos
			/// </summary>
			CodeA = 101,
			
			/// <summary>
			/// Cambio a Code B (valor 100)
			/// Permite codificar mayúsculas, minúsculas y caracteres especiales
			/// FRECUENTE EN PLU: Para nombres de productos y descripciones
			/// </summary>
			CodeB = 100,
			
			/// <summary>
			/// Cambio a Code C (valor 99)
			/// Permite codificar pares de dígitos (00-99)
			/// MUY FRECUENTE EN PLU: Para precios, pesos y códigos numéricos
			/// VENTAJA PLU: Reduce el ancho del código en etiquetas pequeñas de balanza
			/// </summary>
			CodeC = 99
		}

		/// <summary>
		/// Códigos de INICIO para Code 128 en etiquetas PLU 2022
		/// Se usan al PRINCIPIO del código de barras para indicar qué subconjunto usar
		/// 
		/// VALORES:
		/// - CodeUnset (0): Estado inicial, aún no se ha definido el modo
		/// - CodeA (103): Inicia con Code A (caracteres de control) - RARO EN PLU
		/// - CodeB (104): Inicia con Code B (alfanuméricos) - MÁS COMÚN EN PLU
		/// - CodeC (105): Inicia con Code C (números) - FRECUENTE EN PLU NUMÉRICO
		/// 
		/// SELECCIÓN AUTOMÁTICA EN PLU:
		/// El generador elige automáticamente el mejor modo de inicio:
		/// - "4011" (PLU de banana) → START C (todo números)
		/// - "MANZANA" → START B (letras)
		/// - "PERA 450" → START B (empieza con letras, luego CHANGE C)
		/// 
		/// DIFERENCIA CON ChangeModes:
		/// - StartModes: Primer símbolo del código de barras PLU
		/// - ChangeModes: Símbolos para cambiar de modo durante la codificación
		/// 
		/// OPTIMIZACIÓN PLU 2022:
		/// - Analiza los primeros caracteres del texto
		/// - Si empieza con 2+ dígitos → START C (más eficiente)
		/// - Si empieza con letras → START B (más común)
		/// - Esto reduce el ancho final del código en etiquetas de balanza
		/// </summary>
		private enum Code128StartModes
		{
			/// <summary>
			/// Modo no definido (valor 0)
			/// Estado inicial antes de seleccionar un subconjunto para etiqueta PLU
			/// </summary>
			CodeUnset,
			
			/// <summary>
			/// Modo Code A (valor 103)
			/// Códigos de control ASCII 0-31 y mayúsculas
			/// RARO EN PLU: Casi nunca se inicia con este modo en etiquetas de productos
			/// </summary>
			CodeA = 103,
			
			/// <summary>
			/// Modo Code B (valor 104)
			/// Mayúsculas, minúsculas y caracteres especiales
			/// MÁS USADO EN PLU: Inicio típico para nombres de productos ("MANZANA", "Pera", etc.)
			/// </summary>
			CodeB,
			
			/// <summary>
			/// Modo Code C (valor 105)
			/// Pares de dígitos (00-99)
			/// FRECUENTE EN PLU: Para códigos numéricos puros ("4011", "4050", etc.)
			/// MÁS EFICIENTE: Reduce el ancho del código en etiquetas de balanza
			/// </summary>
			CodeC
		}

		/// <summary>
		/// Enumeración para rastrear el MODO ACTUAL durante la codificación de PLU
		/// Se usa internamente en DrawCode128() para saber en qué modo estamos
		/// 
		/// DIFERENCIA CON StartModes y ChangeModes:
		/// - Code128Modes: Variable de estado (¿en qué modo estoy ahora?)
		/// - Code128StartModes: Constantes de inicio (¿cómo empiezo?)
		/// - Code128ChangeModes: Constantes de cambio (¿cómo cambio?)
		/// 
		/// VALORES:
		/// - CodeUnset (0): Aún no se ha definido (estado inicial)
		/// - CodeA (1): Actualmente codificando en modo A (raro en PLU)
		/// - CodeB (2): Actualmente codificando en modo B (común en PLU)
		/// - CodeC (3): Actualmente codificando en modo C (frecuente en PLU)
		/// 
		/// FLUJO TÍPICO EN ETIQUETA PLU:
		/// Ejemplo: "PERA $2.50/kg"
		/// 1. Inicia en CodeUnset
		/// 2. Detecta letras → cambia a CodeB (START B)
		/// 3. Codifica "PERA $"
		/// 4. Detecta números "2.50" → cambia a CodeC (CHANGE C)
		/// 5. Codifica "25" y "0X" (relleno si es impar)
		/// 6. Detecta "/kg" → cambia a CodeB (CHANGE B)
		/// 7. Codifica "/kg"
		/// 
		/// OPTIMIZACIÓN PLU 2022:
		/// El generador minimiza los cambios de modo para reducir el ancho
		/// del código en etiquetas pequeñas de balanza (225px)
		/// </summary>
		private enum Code128Modes
		{
			/// <summary>
			/// Modo no definido (valor 0)
			/// Estado inicial del generador PLU antes de comenzar la codificación
			/// </summary>
			CodeUnset,
			
			/// <summary>
			/// Modo Code A (valor 1)
			/// Actualmente codificando caracteres de control
			/// RARO EN PLU: Casi nunca se usa en etiquetas de productos
			/// </summary>
			CodeA,
			
			/// <summary>
			/// Modo Code B (valor 2)
			/// Actualmente codificando texto alfanumérico
			/// COMÚN EN PLU: Nombres de productos, descripciones, símbolos ($, /, kg)
			/// </summary>
			CodeB,
			
			/// <summary>
			/// Modo Code C (valor 3)
			/// Actualmente codificando pares de dígitos
			/// FRECUENTE EN PLU: Precios, pesos, códigos de producto numéricos
			/// EFICIENTE: Reduce el ancho del código en etiquetas de balanza pequeñas
			/// </summary>
			CodeC
		}
	}
}
