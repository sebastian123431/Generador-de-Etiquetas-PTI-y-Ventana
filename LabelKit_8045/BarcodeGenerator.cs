using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace LabelKit_8045
{
	/// <summary>
	/// Generador de códigos de barras Code 128 - Formato 8045
	/// Versión especializada para etiquetas de ventana con formato 8045
	/// Usado principalmente en etiquetas de cítricos y productos especiales
	/// </summary>
	internal class BarcodeGenerator
	{
		/// <summary>
		/// Constructor por defecto del generador de códigos de barras 8045
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
		/// Indica si el código de barras LabelKit 8045 debe estar centrado horizontalmente
		/// Cuando es true, el código se centra en el ancho especificado
		/// LABELKIT 8045: Formato estándar PTI con recorte optimizado de 70px
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
		/// Nombre de la fuente para el texto legible en LabelKit formato 8045
		/// Por defecto: "Courier New" (fuente monoespaciada estándar)
		/// FORMATO 8045: Usa especificaciones PTI estándar
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
		/// Ancho de la imagen del código de barras LabelKit 8045 en píxeles
		/// Por defecto: 640 píxeles (TAMAÑO ESTÁNDAR PTI)
		/// FORMATO 8045 - CARACTERÍSTICAS:
		/// - Igual a LabelKit estándar (640px)
		/// - Igual a LabelKit 2022 (640px)
		/// - Más grande que PLU 8045 ventana (280px)
		/// - Más grande que PLU 2022 balanza (225px)
		/// DIFERENCIA CLAVE: Recorte de 70px (vs 120px estándar o 60px PLU)
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
		/// Altura de la imagen del código de barras LabelKit 8045 en píxeles
		/// Por defecto: 130 píxeles (estándar para etiquetas)
		/// NOTA: Altura final recortada será de 70px (INTERMEDIO)
		/// COMPARATIVA: 60px (PLU 8045) < 70px (LabelKit 8045) < 75px (PLU 2022) < 120px (estándar)
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
		/// Indica si se debe mostrar el texto legible por humanos en LabelKit 8045
		/// Por defecto: true
		/// FORMATO 8045: Mantiene estándares de legibilidad PTI
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
		/// Tamaño de la fuente para el texto legible en LabelKit formato 8045
		/// Por defecto: 12 puntos (ESTÁNDAR PTI)
		/// FORMATO 8045: Usa mismo tamaño que LabelKit estándar y 2022
		/// DIFERENCIA: Solo el recorte es distinto (70px vs 120px estándar)
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
		/// Dibuja un código de barras UPC-A para LabelKit formato 8045
		/// Versión estándar PTI con optimización de recorte a 70px
		/// 
		/// LABELKIT 8045 - CARACTERÍSTICAS:
		/// - Ancho: 640px (ESTÁNDAR PTI - igual que base y 2022)
		/// - Fuente: 12pt (estándar - lectura normal)
		/// - Recorte: 70px (INTERMEDIO entre PLU 60px y estándar 120px)
		/// - Uso: Etiquetas PTI generales con formato 8045
		/// 
		/// VENTAJA FORMATO 8045:
		/// Recorte de 70px optimiza espacio vertical manteniendo legibilidad
		/// Ideal para etiquetas con múltiples líneas de información
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

		// Token: 0x06000045 RID: 69 RVA: 0x00004D3C File Offset: 0x00002F3C
		private string CheckDigitUPCA(string code)
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < code.Length; i += 2)
			{
				num += int.Parse(code.Substring(i, 1));
			}
			for (int i = 1; i < code.Length; i += 2)
			{
				num2 += int.Parse(code.Substring(i, 1));
			}
			return ((10 - (num * 3 + num2) % 10) % 10).ToString().Trim();
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00004DC0 File Offset: 0x00002FC0
		public string DrawInterleaved2of5(Graphics g, string code, int x, int y)
		{
			return this.DrawInterleaved2of5(g, code, x, y, false);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00004DE0 File Offset: 0x00002FE0
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

		// Token: 0x06000048 RID: 72 RVA: 0x000050E4 File Offset: 0x000032E4
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

		// Token: 0x06000049 RID: 73 RVA: 0x00005168 File Offset: 0x00003368
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
			Rectangle cropArea = new Rectangle(num4, 0, num5, 70);
			return BarcodeGenerator.cropImage(bitmap, cropArea);
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00005518 File Offset: 0x00003718
		private static Image cropImage(Image img, Rectangle cropArea)
		{
			Bitmap bitmap = new Bitmap(img);
			return bitmap.Clone(cropArea, bitmap.PixelFormat);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00005540 File Offset: 0x00003740
		private static Image cropImage_PLU(Image img_PLU, Rectangle cropArea_PLU)
		{
			Bitmap bitmap = new Bitmap(img_PLU);
			return bitmap.Clone(cropArea_PLU, bitmap.PixelFormat);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00005568 File Offset: 0x00003768
		private int CheckDigitCode128(List<int> codes)
		{
			int num = codes[0];
			for (int i = 1; i < codes.Count; i++)
			{
				num += codes[i] * i;
			}
			return num % 103;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x000055AC File Offset: 0x000037AC
		private bool IsNumber(char value)
		{
			return '0' == value || '1' == value || '2' == value || '3' == value || '4' == value || '5' == value || '6' == value || '7' == value || '8' == value || '9' == value;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x000055F4 File Offset: 0x000037F4
		private bool IsEven(int value)
		{
			return (value & 1) == 0;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x0000560C File Offset: 0x0000380C
		private bool IsOdd(int value)
		{
			return (value & 1) == 1;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00005624 File Offset: 0x00003824
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

		// Token: 0x04000037 RID: 55
		private int height;

		// Token: 0x04000038 RID: 56
		private bool humanReadable;

		// Token: 0x04000039 RID: 57
		private int width;

		// Token: 0x0400003A RID: 58
		private string fontName;

		// Token: 0x0400003B RID: 59
		private int fontSize;

		// Token: 0x0400003C RID: 60
		private bool centered;

		// Token: 0x0400003D RID: 61
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

		// Token: 0x0400003E RID: 62
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

		// Token: 0x0400003F RID: 63
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

		// Token: 0x04000040 RID: 64
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

		// Token: 0x04000041 RID: 65
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

		// Token: 0x04000042 RID: 66
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

		// Token: 0x04000043 RID: 67
		private static string Code128Stop = "11000111010";

		// Token: 0x0200000B RID: 11
		private enum Code128ChangeModes
		{
			// Token: 0x04000045 RID: 69
			CodeA = 101,
			// Token: 0x04000046 RID: 70
			CodeB = 100,
			// Token: 0x04000047 RID: 71
			CodeC = 99
		}

		// Token: 0x0200000C RID: 12
		private enum Code128StartModes
		{
			// Token: 0x04000049 RID: 73
			CodeUnset,
			// Token: 0x0400004A RID: 74
			CodeA = 103,
			// Token: 0x0400004B RID: 75
			CodeB,
			// Token: 0x0400004C RID: 76
			CodeC
		}

		// Token: 0x0200000D RID: 13
		private enum Code128Modes
		{
			// Token: 0x0400004E RID: 78
			CodeUnset,
			// Token: 0x0400004F RID: 79
			CodeA,
			// Token: 0x04000050 RID: 80
			CodeB,
			// Token: 0x04000051 RID: 81
			CodeC
		}
	}
}
