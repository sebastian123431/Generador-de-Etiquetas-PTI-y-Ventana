using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace LabelKit_2022
{
	/// <summary>
	/// Generador de códigos de barras Code 128 - Versión 2022
	/// Versión especializada para etiquetas de la temporada 2022
	/// Mantiene compatibilidad con formatos históricos de etiquetas
	/// </summary>
	internal class BarcodeGenerator
	{
		/// <summary>
		/// Constructor por defecto del generador de códigos de barras 2022
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
		/// Indica si el código de barras 2022 debe estar centrado horizontalmente
		/// Cuando es true, el código se centra en el ancho especificado
		/// FORMATO 2022: Mantiene compatibilidad con sistemas legacy de etiquetas PTI
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
		/// Nombre de la fuente para el texto legible en etiquetas formato 2022
		/// Por defecto: "Courier New" (fuente monoespaciada estándar)
		/// TEMPORADA 2022: Usa misma fuente que sistemas anteriores para compatibilidad
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
		/// Ancho de la imagen del código de barras formato 2022 en píxeles
		/// Por defecto: 640 píxeles (TAMAÑO ESTÁNDAR)
		/// FORMATO 2022 - CARACTERÍSTICAS:
		/// - Similar a LabelKit estándar (640px)
		/// - Más grande que PLU 2022 balanza (225px)
		/// - Más grande que PLU 8045 ventana (280px)
		/// MOTIVO: Etiquetas PTI tradicionales con espacio amplio
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
		/// Altura de la imagen del código de barras formato 2022 en píxeles
		/// Por defecto: 130 píxeles (estándar para etiquetas)
		/// NOTA: Altura final recortada será de 60px (misma que PLU 8045)
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
		/// Indica si se debe mostrar el texto legible por humanos en formato 2022
		/// Por defecto: true
		/// TEMPORADA 2022: Mantiene estándares de legibilidad de años anteriores
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
		/// Tamaño de la fuente para el texto legible en formato 2022
		/// Por defecto: 12 puntos (ESTÁNDAR)
		/// COMPARACIÓN COMPLETA DE FORMATOS:
		/// - PLU 2022 (balanza): 30pt (MUY GRANDE - lectura a distancia)
		/// - PLU 8045 (ventana): 12pt (estándar - lectura normal)
		/// - LabelKit 2022 (PTI): 12pt (estándar - lectura normal)
		/// - LabelKit estándar: 12pt (estándar - lectura normal)
		/// TEMPORADA 2022: Mantiene fuente estándar histórica
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
		/// Dibuja un código de barras UPC-A para etiquetas formato 2022
		/// Versión de temporada 2022 que mantiene compatibilidad con sistemas históricos PTI
		/// 
		/// FORMATO 2022 - CARACTERÍSTICAS:
		/// - Ancho: 640px (ESTÁNDAR PTI - igual que LabelKit base)
		/// - Fuente: 12pt (estándar - lectura normal)
		/// - Uso: Etiquetas PTI generales de la temporada 2022
		/// - Compatibilidad: Sistemas legacy y nuevos
		/// 
		/// DIFERENCIAS CON OTROS FORMATOS 2022:
		/// - vs PLU 2022 (balanza): Más ancho (640px vs 225px), fuente estándar (12pt vs 30pt)
		/// - vs PLU 8045 (ventana): Más ancho (640px vs 280px), misma fuente (12pt)
		/// - vs LabelKit estándar: Prácticamente igual (temporada 2022)
		/// 
		/// USO TÍPICO FORMATO 2022:
		/// - Etiquetas PTI tradicionales de productos
		/// - Códigos de rastreo de cajas generales
		/// - Identificación de pallets estándar
		/// - Etiquetas de productos no especializados
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
		/// Calcula el dígito verificador para códigos UPC-A en formato 2022
		/// Usa el algoritmo estándar UPC-A que se mantiene en temporada 2022
		/// 
		/// ALGORITMO UPC-A (UNIVERSAL - MISMO PARA TODOS LOS AÑOS):
		/// 1. Suma dígitos en posiciones IMPARES (1,3,5,7,9,11) y multiplica por 3
		/// 2. Suma dígitos en posiciones PARES (2,4,6,8,10)
		/// 3. Suma ambos resultados
		/// 4. El dígito verificador es lo que falta para el próximo múltiplo de 10
		/// 
		/// FORMATO 2022: Algoritmo idéntico a otros años, compatibilidad total
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
		/// Versión formato 2022 compatible con sistemas legacy
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
		/// Dibuja un código de barras Interleaved 2 of 5 (ITF) para formato 2022
		/// Versión temporada 2022 que mantiene compatibilidad con sistemas PTI históricos
		/// 
		/// CARACTERÍSTICAS FORMATO 2022:
		/// - Ancho: 640px (estándar PTI)
		/// - Fuente: 12pt (lectura normal)
		/// - Uso: Cajas y pallets PTI generales
		/// - Compatibilidad: Total con años anteriores
		/// 
		/// DIFERENCIAS TEMPORADA 2022:
		/// No hay cambios significativos vs años anteriores
		/// Se mantiene formato estándar para compatibilidad
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

		// Token: 0x0600007E RID: 126 RVA: 0x00007904 File Offset: 0x00005B04
		private string CheckDigitInterleaved(string code)
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < code.Length; i += 2)
			{
				num2 += int.Parse(code.Substring(i, 1));
			}
			for (int i = 1; i < code.Length; i += 2)
			{
				num += int.Parse(code.Substring(i, 1));
			}
			return ((10 - (num * 3 + num2) % 10) % 10).ToString().Trim();
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00007988 File Offset: 0x00005B88
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
			Rectangle cropArea = new Rectangle(num4, 0, num5, 60);
			return BarcodeGenerator.cropImage(bitmap, cropArea);
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00007D38 File Offset: 0x00005F38
		private static Image cropImage(Image img, Rectangle cropArea)
		{
			Bitmap bitmap = new Bitmap(img);
			return bitmap.Clone(cropArea, bitmap.PixelFormat);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00007D60 File Offset: 0x00005F60
		private static Image cropImage_PLU(Image img_PLU, Rectangle cropArea_PLU)
		{
			Bitmap bitmap = new Bitmap(img_PLU);
			return bitmap.Clone(cropArea_PLU, bitmap.PixelFormat);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00007D88 File Offset: 0x00005F88
		private int CheckDigitCode128(List<int> codes)
		{
			int num = codes[0];
			for (int i = 1; i < codes.Count; i++)
			{
				num += codes[i] * i;
			}
			return num % 103;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00007DCC File Offset: 0x00005FCC
		private bool IsNumber(char value)
		{
			return '0' == value || '1' == value || '2' == value || '3' == value || '4' == value || '5' == value || '6' == value || '7' == value || '8' == value || '9' == value;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00007E14 File Offset: 0x00006014
		private bool IsEven(int value)
		{
			return (value & 1) == 0;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00007E2C File Offset: 0x0000602C
		private bool IsOdd(int value)
		{
			return (value & 1) == 1;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00007E44 File Offset: 0x00006044
		private int EncodeCodeB(char value)
		{
			for (int i = 0; i < BarcodeGenerator.Code128ComboAB.Length; i++)
			{
				if (BarcodeGenerator.Code128ComboAB[i] == value)
				{
					return i;
				}
			}
			for (int i = 0; i < BarcodeGenerator.Code128B.Length; i++)
			{
				if (BarcodeGenerator.Code128B[i] == value)
				{
					return i + BarcodeGenerator.Code128ComboAB.Length;
				}
			}
			throw new Exception("Invalid Character");
		}

		// Token: 0x0400006D RID: 109
		private int height;

		// Token: 0x0400006E RID: 110
		private bool humanReadable;

		// Token: 0x0400006F RID: 111
		private int width;

		// Token: 0x04000070 RID: 112
		private string fontName;

		// Token: 0x04000071 RID: 113
		private int fontSize;

		// Token: 0x04000072 RID: 114
		private bool centered;

		// Token: 0x04000073 RID: 115
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

		// Token: 0x04000074 RID: 116
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

		// Token: 0x04000075 RID: 117
		private static string[] both_2of5 = new string[]
		{
			"NNWWN",
			"WNNNW",
			"NWNNW",
			"WWNNN",
			"NNWNW",
			"WNWNN",
			"NWWNN",
			"NNNWW",
			"WNNWN",
			"NWNWN"
		};

		// Token: 0x04000076 RID: 118
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

		// Token: 0x04000077 RID: 119
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

		// Token: 0x04000078 RID: 120
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

		// Token: 0x04000079 RID: 121
		private static string Code128Stop = "11000111010";

		// Token: 0x02000013 RID: 19
		private enum Code128ChangeModes
		{
			// Token: 0x0400007B RID: 123
			CodeA = 101,
			// Token: 0x0400007C RID: 124
			CodeB = 100,
			// Token: 0x0400007D RID: 125
			CodeC = 99
		}

		// Token: 0x02000014 RID: 20
		private enum Code128StartModes
		{
			// Token: 0x0400007F RID: 127
			CodeUnset,
			// Token: 0x04000080 RID: 128
			CodeA = 103,
			// Token: 0x04000081 RID: 129
			CodeB,
			// Token: 0x04000082 RID: 130
			CodeC
		}

		// Token: 0x02000015 RID: 21
		private enum Code128Modes
		{
			// Token: 0x04000084 RID: 132
			CodeUnset,
			// Token: 0x04000085 RID: 133
			CodeA,
			// Token: 0x04000086 RID: 134
			CodeB,
			// Token: 0x04000087 RID: 135
			CodeC
		}
	}
}
