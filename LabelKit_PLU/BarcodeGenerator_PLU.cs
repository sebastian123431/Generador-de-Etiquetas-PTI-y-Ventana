using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace LabelKit_PLU
{
	/// <summary>
	/// Generador de códigos de barras PLU (Price Look-Up)
	/// Especializado en generar códigos de barras para productos con peso variable
	/// PLU es usado comúnmente en frutas, verduras y productos vendidos por peso
	/// </summary>
	internal class BarcodeGenerator_PLU
	{
		/// <summary>
		/// Constructor por defecto del generador de códigos PLU
		/// Inicializa valores predeterminados: tamaño 280x130, fuente Courier New 12pt
		/// Tamaño más pequeño que Code128 estándar, apropiado para etiquetas PLU
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

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600001D RID: 29 RVA: 0x000034B4 File Offset: 0x000016B4
		// (set) Token: 0x0600001E RID: 30 RVA: 0x000034CC File Offset: 0x000016CC
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

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600001F RID: 31 RVA: 0x000034D8 File Offset: 0x000016D8
		// (set) Token: 0x06000020 RID: 32 RVA: 0x000034F0 File Offset: 0x000016F0
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

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000021 RID: 33 RVA: 0x000034FC File Offset: 0x000016FC
		// (set) Token: 0x06000022 RID: 34 RVA: 0x00003514 File Offset: 0x00001714
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

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000023 RID: 35 RVA: 0x00003520 File Offset: 0x00001720
		// (set) Token: 0x06000024 RID: 36 RVA: 0x00003538 File Offset: 0x00001738
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

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000025 RID: 37 RVA: 0x00003544 File Offset: 0x00001744
		// (set) Token: 0x06000026 RID: 38 RVA: 0x0000355C File Offset: 0x0000175C
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

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000027 RID: 39 RVA: 0x00003568 File Offset: 0x00001768
		// (set) Token: 0x06000028 RID: 40 RVA: 0x00003580 File Offset: 0x00001780
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

		// Token: 0x06000029 RID: 41 RVA: 0x0000358C File Offset: 0x0000178C
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

		// Token: 0x0600002A RID: 42 RVA: 0x0000392C File Offset: 0x00001B2C
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

		// Token: 0x0600002B RID: 43 RVA: 0x000039B0 File Offset: 0x00001BB0
		public string DrawInterleaved2of5(Graphics g, string code, int x, int y)
		{
			return this.DrawInterleaved2of5(g, code, x, y, false);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000039D0 File Offset: 0x00001BD0
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

		// Token: 0x0600002D RID: 45 RVA: 0x00003CD4 File Offset: 0x00001ED4
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

		// Token: 0x0600002E RID: 46 RVA: 0x00003D58 File Offset: 0x00001F58
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
			Rectangle cropArea = new Rectangle(num4, 0, num5, 100);
			return BarcodeGenerator_PLU.cropImage(bitmap, cropArea);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00004108 File Offset: 0x00002308
		private static Image cropImage(Image img, Rectangle cropArea)
		{
			Bitmap bitmap = new Bitmap(img);
			return bitmap.Clone(cropArea, bitmap.PixelFormat);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00004130 File Offset: 0x00002330
		private static Image cropImage_PLU(Image img_PLU, Rectangle cropArea_PLU)
		{
			Bitmap bitmap = new Bitmap(img_PLU);
			return bitmap.Clone(cropArea_PLU, bitmap.PixelFormat);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00004158 File Offset: 0x00002358
		private int CheckDigitCode128(List<int> codes)
		{
			int num = codes[0];
			for (int i = 1; i < codes.Count; i++)
			{
				num += codes[i] * i;
			}
			return num % 103;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x0000419C File Offset: 0x0000239C
		private bool IsNumber(char value)
		{
			return '0' == value || '1' == value || '2' == value || '3' == value || '4' == value || '5' == value || '6' == value || '7' == value || '8' == value || '9' == value;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000041E4 File Offset: 0x000023E4
		private bool IsEven(int value)
		{
			return (value & 1) == 0;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000041FC File Offset: 0x000023FC
		private bool IsOdd(int value)
		{
			return (value & 1) == 1;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00004214 File Offset: 0x00002414
		private int EncodeCodeB(char value)
		{
			for (int i = 0; i < BarcodeGenerator_PLU.Code128ComboAB.Length; i++)
			{
				if (BarcodeGenerator_PLU.Code128ComboAB[i] == value)
				{
					return i;
				}
			}
			for (int i = 0; i < BarcodeGenerator_PLU.Code128B.Length; i++)
			{
				if (BarcodeGenerator_PLU.Code128B[i] == value)
				{
					return i + BarcodeGenerator_PLU.Code128ComboAB.Length;
				}
			}
			throw new Exception("Invalid Character");
		}

		// Token: 0x0400001C RID: 28
		private int height;

		// Token: 0x0400001D RID: 29
		private bool humanReadable;

		// Token: 0x0400001E RID: 30
		private int width;

		// Token: 0x0400001F RID: 31
		private string fontName;

		// Token: 0x04000020 RID: 32
		private int fontSize;

		// Token: 0x04000021 RID: 33
		private bool centered;

		// Token: 0x04000022 RID: 34
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

		// Token: 0x04000023 RID: 35
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

		// Token: 0x04000024 RID: 36
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

		// Token: 0x04000025 RID: 37
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

		// Token: 0x04000026 RID: 38
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

		// Token: 0x04000027 RID: 39
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

		// Token: 0x04000028 RID: 40
		private static string Code128Stop = "11000111010";

		// Token: 0x02000007 RID: 7
		private enum Code128ChangeModes
		{
			// Token: 0x0400002A RID: 42
			CodeA = 101,
			// Token: 0x0400002B RID: 43
			CodeB = 100,
			// Token: 0x0400002C RID: 44
			CodeC = 99
		}

		// Token: 0x02000008 RID: 8
		private enum Code128StartModes
		{
			// Token: 0x0400002E RID: 46
			CodeUnset,
			// Token: 0x0400002F RID: 47
			CodeA = 103,
			// Token: 0x04000030 RID: 48
			CodeB,
			// Token: 0x04000031 RID: 49
			CodeC
		}

		// Token: 0x02000009 RID: 9
		private enum Code128Modes
		{
			// Token: 0x04000033 RID: 51
			CodeUnset,
			// Token: 0x04000034 RID: 52
			CodeA,
			// Token: 0x04000035 RID: 53
			CodeB,
			// Token: 0x04000036 RID: 54
			CodeC
		}
	}
}
