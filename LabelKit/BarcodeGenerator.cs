using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace LabelKit
{
	/// <summary>
	/// Generador de códigos de barras tipo Code 128
	/// Permite crear imágenes de códigos de barras con diferentes configuraciones
	/// Code 128 es un estándar de alta densidad que puede codificar todos los caracteres ASCII
	/// </summary>
	internal class BarcodeGenerator
	{
		/// <summary>
		/// Constructor por defecto del generador de códigos de barras
		/// Inicializa valores predeterminados: tamaño 640x130, fuente Courier New 12pt
		/// </summary>
		public BarcodeGenerator()
		{
			this.width = 640;
			this.height = 130;
			this.humanReadable = true;
			this.fontSize = 12;
			this.fontName = "Courier New";
			this.centered = false;
		}

		/// <summary>
		/// Indica si el código de barras debe estar centrado
		/// Cuando es true, el código se centra en el ancho especificado
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
		/// Nombre de la fuente para el texto legible por humanos
		/// Por defecto: "Courier New"
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
		/// Ancho de la imagen del código de barras en píxeles
		/// Por defecto: 640 píxeles
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
		/// Altura de la imagen del código de barras en píxeles
		/// Por defecto: 130 píxeles
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
		/// Indica si se debe mostrar el texto legible por humanos debajo del código
		/// Por defecto: true
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
		/// Tamaño de la fuente para el texto legible por humanos
		/// Por defecto: 12 puntos
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
		/// Dibuja un código de barras UPC-A (Universal Product Code tipo A)
		/// UPC-A es el código de barras estándar de productos en Estados Unidos y Canadá
		/// Formato: 12 dígitos (11 datos + 1 dígito verificador)
		/// </summary>
		/// <param name="g">Contexto gráfico donde se dibujará el código</param>
		/// <param name="code">Código de 11 dígitos (el 12º se calcula automáticamente)</param>
		/// <param name="x">Posición X donde iniciar el dibujo</param>
		/// <param name="y">Posición Y donde iniciar el dibujo</param>
		/// <returns>Cadena vacía si es exitoso, mensaje de error si falla</returns>
		public string DrawUPCA(Graphics g, string code, int x, int y)
		{
			// Limpia espacios del código
			code = code.Trim();
			
			// Valida que el código contenga solo números
			try
			{
				long.Parse(code);
			}
			catch
			{
				return "Code is not valid for a UPCA barcode: " + code;
			}
			
			// Rellena con ceros a la izquierda si es necesario (mínimo 11 dígitos)
			while (code.Length < 11)
			{
				code = "0" + code;
			}
			
			// Trunca a 11 dígitos
			code = code.Substring(0, 11);
			
			// Agrega el dígito verificador (12º dígito)
			code = code.Trim() + this.CheckDigitUPCA(code);
			string text = "101";
			for (int i = 0; i < 6; i++)
			{
				int num = int.Parse(code.Substring(i, 1));
				text += BarcodeGenerator.left_UPCA[num];
			}
			text += "01010";
			for (int i = 6; i < 12; i++)
			{
				int num = int.Parse(code.Substring(i, 1));
				text += BarcodeGenerator.right_UPCA[num];
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
		/// Calcula el dígito verificador para códigos UPC-A
		/// Algoritmo: 
		/// 1. Suma dígitos en posiciones impares (1,3,5,7,9,11) y multiplica por 3
		/// 2. Suma dígitos en posiciones pares (2,4,6,8,10)
		/// 3. Suma ambos resultados
		/// 4. El dígito verificador es lo que falta para el próximo múltiplo de 10
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
		/// Sobrecarga simplificada que llama al método principal con checksum=false
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
		/// Dibuja un código de barras Interleaved 2 of 5 (ITF)
		/// ITF es usado comúnmente en logística y cajas de productos
		/// Codifica pares de dígitos: uno en barras, otro en espacios
		/// El código debe tener longitud par (se agrega '0' al inicio si es necesario)
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
				string text2 = BarcodeGenerator.both_2of5[num];
				i++;
				int num2 = int.Parse(code.Substring(i, 1));
				string text3 = BarcodeGenerator.both_2of5[num2];
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
		/// Calcula el dígito verificador para códigos Interleaved 2 of 5
		/// Similar al algoritmo UPC-A pero con las posiciones invertidas
		/// Algoritmo:
		/// 1. Suma dígitos en posiciones pares (2,4,6,8...)
		/// 2. Suma dígitos en posiciones impares (1,3,5,7...) y multiplica por 3
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

		// Token: 0x06000013 RID: 19 RVA: 0x00002944 File Offset: 0x00000B44
		public Image DrawCode128(Graphics g, string code, int x, int y)
		{
			Bitmap bitmap = new Bitmap(this.width, this.height, PixelFormat.Format32bppArgb);
			g = Graphics.FromImage(bitmap);
			List<int> list = new List<int>();
			BarcodeGenerator.Code128Modes code128Modes = BarcodeGenerator.Code128Modes.CodeUnset;
			for (int i = 0; i < code.Length; i++)
			{
				if (this.IsNumber(code[i]) && i + 1 < code.Length && this.IsNumber(code[i + 1]))
				{
					if (code128Modes != BarcodeGenerator.Code128Modes.CodeC)
					{
						if (code128Modes == BarcodeGenerator.Code128Modes.CodeUnset)
						{
							list.Add(105);
						}
						else
						{
							list.Add(99);
						}
						code128Modes = BarcodeGenerator.Code128Modes.CodeC;
					}
					list.Add(int.Parse(code.Substring(i, 2)));
					i++;
				}
				else
				{
					if (code128Modes != BarcodeGenerator.Code128Modes.CodeB)
					{
						if (code128Modes == BarcodeGenerator.Code128Modes.CodeUnset)
						{
							list.Add(104);
						}
						else
						{
							list.Add(100);
						}
						code128Modes = BarcodeGenerator.Code128Modes.CodeB;
					}
					list.Add(this.EncodeCodeB(code[i]));
				}
			}
			list.Add(this.CheckDigitCode128(list));
			string text = "";
			for (int i = 0; i < list.Count; i++)
			{
				text += BarcodeGenerator.Code128Encoding[list[i]];
			}
			text += BarcodeGenerator.Code128Stop;
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
			Rectangle cropArea = new Rectangle(num4, 0, num5, 120);
			return BarcodeGenerator.cropImage(bitmap, cropArea);
		}

		/// <summary>
		/// Recorta una imagen al área especificada
		/// Crea una copia de la imagen original recortada al rectángulo indicado
		/// </summary>
		/// <param name="img">Imagen original a recortar</param>
		/// <param name="cropArea">Rectángulo que define el área de recorte</param>
		/// <returns>Nueva imagen recortada</returns>
		private static Image cropImage(Image img, Rectangle cropArea)
		{
			Bitmap bitmap = new Bitmap(img);
			return bitmap.Clone(cropArea, bitmap.PixelFormat);
		}

		/// <summary>
		/// Recorta una imagen PLU al área especificada
		/// Versión especializada para códigos PLU (mismo funcionamiento que cropImage)
		/// </summary>
		/// <param name="img_PLU">Imagen PLU original a recortar</param>
		/// <param name="cropArea_PLU">Rectángulo que define el área de recorte</param>
		/// <returns>Nueva imagen recortada</returns>
		private static Image cropImage_PLU(Image img_PLU, Rectangle cropArea_PLU)
		{
			Bitmap bitmap = new Bitmap(img_PLU);
			return bitmap.Clone(cropArea_PLU, bitmap.PixelFormat);
		}

		/// <summary>
		/// Calcula el dígito de verificación para Code 128
		/// Algoritmo:
		/// 1. Toma el primer código (inicio) como base
		/// 2. Para cada código siguiente, multiplica por su posición y suma
		/// 3. El resultado módulo 103 es el dígito de verificación
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
		/// Verifica si un carácter es un dígito (0-9)
		/// </summary>
		/// <param name="value">Carácter a verificar</param>
		/// <returns>True si es dígito, False en caso contrario</returns>
		private bool IsNumber(char value)
		{
			return '0' == value || '1' == value || '2' == value || '3' == value || '4' == value || '5' == value || '6' == value || '7' == value || '8' == value || '9' == value;
		}

		/// <summary>
		/// Verifica si un número es par
		/// Usa operación bit a bit (AND con 1) para verificar paridad
		/// </summary>
		/// <param name="value">Número a verificar</param>
		/// <returns>True si es par, False si es impar</returns>
		private bool IsEven(int value)
		{
			return (value & 1) == 0;
		}

		/// <summary>
		/// Verifica si un número es impar
		/// Usa operación bit a bit (AND con 1) para verificar paridad
		/// </summary>
		/// <param name="value">Número a verificar</param>
		/// <returns>True si es impar, False si es par</returns>
		private bool IsOdd(int value)
		{
			return (value & 1) == 1;
		}

		/// <summary>
		/// Codifica un carácter según la tabla Code 128 subset B
		/// Code B incluye todos los caracteres ASCII imprimibles:
		/// - Espacios y caracteres especiales
		/// - Dígitos 0-9
		/// - Letras mayúsculas A-Z
		/// - Letras minúsculas a-z
		/// </summary>
		/// <param name="value">Carácter a codificar</param>
		/// <returns>Índice del carácter en la tabla de codificación</returns>
		/// <exception cref="Exception">Se lanza si el carácter no es válido</exception>
		private int EncodeCodeB(char value)
		{
			// Primero busca en la tabla ComboAB (caracteres comunes a Code A y B)
			for (int i = 0; i < BarcodeGenerator.Code128ComboAB.Length; i++)
			{
				if (BarcodeGenerator.Code128ComboAB[i] == value)
				{
					return i;
				}
			}
			// Si no está en ComboAB, busca en la tabla específica de Code B (minúsculas)
			for (int i = 0; i < BarcodeGenerator.Code128B.Length; i++)
			{
				if (BarcodeGenerator.Code128B[i] == value)
				{
					// Retorna el índice ajustado sumando la longitud de ComboAB
					return i + BarcodeGenerator.Code128ComboAB.Length;
				}
			}
			// Si el carácter no se encuentra en ninguna tabla, lanza excepción
			throw new Exception("Invalid Character");
		}

		// ==================== MIEMBROS PRIVADOS DE CONFIGURACIÓN ====================
		
		/// <summary>
		/// Altura del código de barras en píxeles
		/// Valor por defecto: 130 píxeles
		/// </summary>
		private int height;

		/// <summary>
		/// Indica si se debe mostrar el texto legible por humanos debajo del código
		/// Valor por defecto: true
		/// </summary>
		private bool humanReadable;

		/// <summary>
		/// Ancho del código de barras en píxeles
		/// Valor por defecto: 640 píxeles
		/// </summary>
		private int width;

		/// <summary>
		/// Nombre de la fuente utilizada para el texto legible
		/// Valor por defecto: "Courier New"
		/// </summary>
		private string fontName;

		/// <summary>
		/// Tamaño en puntos de la fuente para el texto legible
		/// Valor por defecto: 12 puntos
		/// </summary>
		private int fontSize;

		/// <summary>
		/// Indica si el código de barras debe estar centrado horizontalmente
		/// Valor por defecto: false
		/// </summary>
		private bool centered;

		// Token: 0x04000007 RID: 7
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

		// Token: 0x04000008 RID: 8
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

		// ==================== TABLAS DE CODIFICACIÓN INTERLEAVED 2 OF 5 ====================
		
		/// <summary>
		/// Tabla de codificación para Interleaved 2 of 5 (ITF)
		/// Cada dígito (0-9) se codifica con 5 elementos alternando anchos
		/// 'W' = Wide (elemento ancho - 2 o 3 módulos)
		/// 'N' = Narrow (elemento angosto - 1 módulo)
		/// 
		/// CARACTERÍSTICA CLAVE DE ITF:
		/// - Los dígitos se codifican en PARES
		/// - Un dígito se codifica en las BARRAS
		/// - El siguiente dígito se codifica en los ESPACIOS
		/// - Cada patrón tiene exactamente 2 elementos anchos y 3 angostos
		/// 
		/// EJEMPLO: Para codificar "45":
		/// - '4' = "NNWNW" se codifica en barras
		/// - '5' = "WNWNN" se codifica en espacios
		/// Resultado: barra angosta, espacio ancho, barra ancho, espacio angosto, barra angosto, espacio ancho, barra angosto, espacio angosto...
		/// </summary>
		private static string[] both_2of5 = new string[]
		{
			"NNWWN",  // Dígito 0: Angosto-Angosto-Ancho-Ancho-Angosto
			"WNNNW",  // Dígito 1: Ancho-Angosto-Angosto-Angosto-Ancho
			"NWNNW",  // Dígito 2: Angosto-Ancho-Angosto-Angosto-Ancho
			"WWNNN",  // Dígito 3: Ancho-Ancho-Angosto-Angosto-Angosto
			"NNWNW",  // Dígito 4: Angosto-Angosto-Ancho-Angosto-Ancho
			"WNWNN",  // Dígito 5: Ancho-Angosto-Ancho-Angosto-Angosto
			"NWWNN",  // Dígito 6: Angosto-Ancho-Ancho-Angosto-Angosto
			"NNNWW",  // Dígito 7: Angosto-Angosto-Angosto-Ancho-Ancho
			"WNNWN",  // Dígito 8: Ancho-Angosto-Angosto-Ancho-Angosto
			"NWNWN"   // Dígito 9: Angosto-Ancho-Angosto-Ancho-Angosto
		};

		// ==================== TABLAS DE CODIFICACIÓN CODE 128 ====================
		
		/// <summary>
		/// Caracteres comunes a Code 128 subconjuntos A y B
		/// Incluye 64 caracteres del espacio (' ') al guión bajo ('_')
		/// 
		/// CONTENIDO:
		/// - Espacio y caracteres especiales: ! " # $ % & ' ( ) * + , - . /
		/// - Dígitos: 0 1 2 3 4 5 6 7 8 9
		/// - Más caracteres especiales: : ; < = > ? @
		/// - Letras MAYÚSCULAS: A B C D E F G H I J K L M N O P Q R S T U V W X Y Z
		/// - Caracteres finales: [ \ ] ^ _
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
		/// Caracteres ESPECÍFICOS del Code 128 subconjunto B
		/// Incluye 31 caracteres de letras MINÚSCULAS y símbolos adicionales
		/// 
		/// CONTENIDO:
		/// - Acento grave: `
		/// - Letras minúsculas: a b c d e f g h i j k l m n o p q r s t u v w x y z
		/// - Caracteres especiales finales: { | } ~
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
		/// Tabla COMPLETA de codificación Code 128
		/// Contiene 103 patrones de barras (índices 0-102)
		/// Cada patrón es una cadena de 11 bits ('1' = barra negra, '0' = espacio blanco)
		/// 
		/// ESTRUCTURA:
		/// - Índices 0-94: Caracteres imprimibles (según subconjunto A, B o C)
		/// - Índices 95-102: Caracteres especiales y de control
		/// 
		/// CÓDIGOS ESPECIALES:
		/// - 99: Cambio a Code C (pares de dígitos)
		/// - 100: Cambio a Code B (caracteres alfanuméricos)
		/// - 101: Cambio a Code A (caracteres de control)
		/// - 103: Inicio Code A (START A)
		/// - 104: Inicio Code B (START B)
		/// - 105: Inicio Code C (START C)
		/// 
		/// EJEMPLO DE USO:
		/// Para codificar 'A' en Code B:
		/// 1. 'A' tiene índice 33 en Code128ComboAB
		/// 2. Se busca Code128Encoding[33] = "10100011000"
		/// 3. Este patrón se dibuja como: barra-espacio-barra-espacios-barras-espacios
		/// 
		/// FORMATO DE PATRÓN (11 bits):
		/// Cada patrón tiene exactamente 11 módulos (6 barras/espacios)
		/// Siempre empieza con barra ('1') y termina con espacio o barra
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
		/// Código de PARADA para Code 128
		/// Este patrón especial marca el FINAL de un código de barras Code 128
		/// 
		/// PATRÓN: "11000111010" (13 módulos)
		/// A diferencia de los códigos normales (11 módulos), el código de parada tiene 13
		/// 
		/// FUNCIÓN:
		/// - Indica que el código de barras ha terminado
		/// - Permite al lector detectar el final de la lectura
		/// - Es obligatorio en todos los códigos Code 128
		/// 
		/// ESTRUCTURA DEL CÓDIGO COMPLETO:
		/// [Quiet Zone] + [Start] + [Datos] + [Checksum] + [Stop] + [Quiet Zone]
		///     10 mod      11 mod    11 mod    11 mod      13 mod     10 mod
		/// </summary>
		private static string Code128Stop = "11000111010";

		// ==================== ENUMERACIONES PARA CODE 128 ====================

		/// <summary>
		/// Códigos de CAMBIO entre subconjuntos Code 128
		/// Se usan cuando ya se está codificando y se necesita cambiar de modo
		/// 
		/// DIFERENCIA CON StartModes:
		/// - StartModes: Se usa al INICIO del código de barras
		/// - ChangeModes: Se usa para CAMBIAR de modo en medio del código
		/// 
		/// VALORES:
		/// - CodeA (101): Cambia a Code A (caracteres de control y mayúsculas)
		/// - CodeB (100): Cambia a Code B (mayúsculas, minúsculas y especiales)
		/// - CodeC (99): Cambia a Code C (pares de dígitos para números)
		/// 
		/// EJEMPLO DE USO:
		/// "ABC123" se codificaría como:
		/// [START B] + [A] + [B] + [C] + [CHANGE C] + [12] + [3_padded]
		/// </summary>
		private enum Code128ChangeModes
		{
			/// <summary>
			/// Cambio a Code A (valor 101)
			/// Permite codificar caracteres de control ASCII 0-31
			/// </summary>
			CodeA = 101,
			
			/// <summary>
			/// Cambio a Code B (valor 100)
			/// Permite codificar mayúsculas, minúsculas y caracteres especiales
			/// </summary>
			CodeB = 100,
			
			/// <summary>
			/// Cambio a Code C (valor 99)
			/// Permite codificar pares de dígitos (00-99)
			/// Más eficiente para números
			/// </summary>
			CodeC = 99
		}

		/// <summary>
		/// Códigos de INICIO para Code 128
		/// Se usan al PRINCIPIO del código de barras para indicar qué subconjunto usar
		/// 
		/// VALORES:
		/// - CodeUnset (0): Estado inicial, aún no se ha definido el modo
		/// - CodeA (103): Inicia con Code A (caracteres de control)
		/// - CodeB (104): Inicia con Code B (alfanuméricos) - MÁS COMÚN
		/// - CodeC (105): Inicia con Code C (números) - MÁS EFICIENTE para dígitos
		/// 
		/// DIFERENCIA CON ChangeModes:
		/// - StartModes: Primer símbolo del código de barras
		/// - ChangeModes: Símbolos para cambiar de modo durante la codificación
		/// 
		/// SELECCIÓN AUTOMÁTICA:
		/// El generador elige automáticamente el mejor modo de inicio:
		/// - Si empieza con 2+ dígitos → START C
		/// - Si empieza con letras → START B (más común)
		/// - Si empieza con control → START A (raro)
		/// </summary>
		private enum Code128StartModes
		{
			/// <summary>
			/// Modo no definido (valor 0)
			/// Estado inicial antes de seleccionar un subconjunto
			/// </summary>
			CodeUnset,
			
			/// <summary>
			/// Modo Code A (valor 103)
			/// Códigos de control ASCII 0-31 y mayúsculas
			/// </summary>
			CodeA = 103,
			
			/// <summary>
			/// Modo Code B (valor 104)
			/// Mayúsculas, minúsculas y caracteres especiales
			/// El más usado para texto general
			/// </summary>
			CodeB,
			
			/// <summary>
			/// Modo Code C (valor 105)
			/// Pares de dígitos (00-99)
			/// El más eficiente para números
			/// </summary>
			CodeC
		}

		/// <summary>
		/// Enumeración para rastrear el MODO ACTUAL durante la codificación
		/// Se usa internamente en DrawCode128() para saber en qué modo estamos
		/// 
		/// DIFERENCIA CON StartModes y ChangeModes:
		/// - Code128Modes: Variable de estado (¿en qué modo estoy ahora?)
		/// - Code128StartModes: Constantes de inicio (¿cómo empiezo?)
		/// - Code128ChangeModes: Constantes de cambio (¿cómo cambio?)
		/// 
		/// VALORES:
		/// - CodeUnset (0): Aún no se ha definido (estado inicial)
		/// - CodeA (1): Actualmente codificando en modo A
		/// - CodeB (2): Actualmente codificando en modo B
		/// - CodeC (3): Actualmente codificando en modo C
		/// 
		/// FLUJO TÍPICO:
		/// 1. Inicia en CodeUnset
		/// 2. Se detecta el mejor modo → cambia a CodeB o CodeC
		/// 3. Durante la codificación puede cambiar según los caracteres
		/// 4. Ejemplo: CodeB → CodeC (para números) → CodeB (para texto)
		/// </summary>
		private enum Code128Modes
		{
			/// <summary>
			/// Modo no definido (valor 0)
			/// Estado inicial del generador
			/// </summary>
			CodeUnset,
			
			/// <summary>
			/// Modo Code A (valor 1)
			/// Actualmente codificando caracteres de control
			/// </summary>
			CodeA,
			
			/// <summary>
			/// Modo Code B (valor 2)
			/// Actualmente codificando texto alfanumérico
			/// </summary>
			CodeB,
			
			/// <summary>
			/// Modo Code C (valor 3)
			/// Actualmente codificando pares de dígitos
			/// </summary>
			CodeC
		}
	}
}
