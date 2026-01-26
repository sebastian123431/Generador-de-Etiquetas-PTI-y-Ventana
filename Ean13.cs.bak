using System;

// Token: 0x0200001A RID: 26
internal class Ean13
{
	// Token: 0x060000A3 RID: 163 RVA: 0x000098B1 File Offset: 0x00007AB1
	public Ean13(string code, string title) : this(code, title, new Ean13Settings())
	{
	}

	// Token: 0x060000A4 RID: 164 RVA: 0x000098C4 File Offset: 0x00007AC4
	public Ean13(string code, string title, Ean13Settings settings)
	{
		this.settings = settings;
		this.code = code;
		this.title = title;
		if (!this.CheckCode(code))
		{
			throw new ArgumentException("Invalid EAN-13 code specified.");
		}
	}

	// Token: 0x060000A5 RID: 165 RVA: 0x00009908 File Offset: 0x00007B08
	private bool CheckCode(string code)
	{
		bool result;
		if (code == null || code.Length != 13)
		{
			result = false;
		}
		else
		{
			for (int i = 0; i < code.Length; i++)
			{
				int num;
				if (!int.TryParse(code[i].ToString(), out num))
				{
					return false;
				}
			}
			char c = (char)(48 + Ean13.CalculateChecksum(code.Substring(0, 12)));
			result = (code[12] == c);
		}
		return result;
	}

	// Token: 0x060000A6 RID: 166 RVA: 0x00009998 File Offset: 0x00007B98
	public static int CalculateChecksum(string code)
	{
		if (code == null || code.Length != 12)
		{
			throw new ArgumentException("Code length should be 12, i.e. excluding the checksum digit");
		}
		int num = 0;
		for (int i = 0; i < 12; i++)
		{
			int num2;
			if (!int.TryParse(code[i].ToString(), out num2))
			{
				throw new ArgumentException("Invalid character encountered in specified code.");
			}
			num += ((i % 2 == 0) ? num2 : (num2 * 3));
		}
		int num3 = 10 - num % 10;
		return num3 % 10;
	}

	// Token: 0x040000A3 RID: 163
	private Ean13Settings settings;

	// Token: 0x040000A4 RID: 164
	private string code;

	// Token: 0x040000A5 RID: 165
	private string title;
}
