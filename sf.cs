using System;
using System.Reflection;
using System.Text.RegularExpressions;

/// <summary>
/// Clase de funciones auxiliares (String Functions)
/// Proporciona métodos de utilidad para conversión de tipos, validación y manipulación de datos
/// </summary>
public class sf
{
	/// <summary>
	/// Prepara un objeto inicializando sus propiedades con valores predeterminados según su tipo
	/// Recorre todas las propiedades del objeto y asigna valores por defecto si están nulas
	/// Valores: string="", bool=false, int=0, long=0, double=0, float=0.0, DateTime=DateTime.MinValue
	/// </summary>
	/// <param name="objeto">Objeto a inicializar</param>
	public void preparaObjeto(object objeto)
	{
		try
		{
			// Recorre todas las propiedades del objeto usando reflexión
			foreach (PropertyInfo propertyInfo in objeto.GetType().GetProperties())
			{
				if (propertyInfo.CanWrite)
				{
					string text = propertyInfo.PropertyType.ToString();
					switch (text)
					{
					case "System.string":
						if (propertyInfo.GetValue(this, null) == null)
						{
							propertyInfo.SetValue(this, "", null);
						}
						break;
					case "System.bool":
						if (propertyInfo.GetValue(this, null) == null)
						{
							propertyInfo.SetValue(this, false, null);
						}
						break;
					case "System.int32":
						if (propertyInfo.GetValue(this, null) == null)
						{
							propertyInfo.SetValue(this, 0, null);
						}
						break;
					case "System.int64":
						if (propertyInfo.GetValue(this, null) == null)
						{
							propertyInfo.SetValue(this, 0, null);
						}
						break;
					case "System.double":
						if (propertyInfo.GetValue(this, null) == null)
						{
							propertyInfo.SetValue(this, 0, null);
						}
						break;
					case "System.single":
						if (propertyInfo.GetValue(this, null) == null)
						{
							propertyInfo.SetValue(this, 0.0, null);
						}
						break;
					case "System.DateTime":
						if (propertyInfo.GetValue(this, null) == null)
						{
							propertyInfo.SetValue(this, DateTime.MinValue, null);
						}
						break;
					}
				}
			}
		}
		catch (Exception ex)
		{
			// Silenciosamente ignora errores de inicialización
		}
	}

	// Token: 0x060000A9 RID: 169 RVA: 0x00009CC0 File Offset: 0x00007EC0
	public static float aFloat(string valor)
	{
		float result;
		try
		{
			if (valor == "")
			{
				result = 0f;
			}
			else
			{
				valor = valor.Replace(".", ",");
				float num;
				float.TryParse(valor, out num);
				result = num;
			}
		}
		catch (Exception ex)
		{
			result = 0f;
		}
		return result;
	}

	// Token: 0x060000AA RID: 170 RVA: 0x00009D28 File Offset: 0x00007F28
	public static float aFloat(int valor)
	{
		float result;
		try
		{
			if (valor == 0)
			{
				result = 0f;
			}
			else
			{
				result = Convert.ToSingle(valor);
			}
		}
		catch (Exception ex)
		{
			result = 0f;
		}
		return result;
	}

	// Token: 0x060000AB RID: 171 RVA: 0x00009D70 File Offset: 0x00007F70
	public static float aFloat(float valor)
	{
		float result;
		if (valor == -3.4028235E+38f)
		{
			result = 0f;
		}
		else
		{
			result = valor;
		}
		return result;
	}

	// Token: 0x060000AC RID: 172 RVA: 0x00009D9C File Offset: 0x00007F9C
	public static int Entero(string valor)
	{
		int result = 0;
		int.TryParse(valor, out result);
		return result;
	}

	// Token: 0x060000AD RID: 173 RVA: 0x00009DBC File Offset: 0x00007FBC
	public static int Entero(double valor)
	{
		return Convert.ToInt32(valor);
	}

	// Token: 0x060000AE RID: 174 RVA: 0x00009DD4 File Offset: 0x00007FD4
	public static int Entero(object valor)
	{
		return Convert.ToInt32(valor);
	}

	// Token: 0x060000AF RID: 175 RVA: 0x00009DEC File Offset: 0x00007FEC
	public static int Entero(DBNull valor)
	{
		return 0;
	}

	// Token: 0x060000B0 RID: 176 RVA: 0x00009E00 File Offset: 0x00008000
	public static int Entero(bool valor)
	{
		int result;
		try
		{
			if (valor)
			{
				result = 1;
			}
			else
			{
				result = 0;
			}
		}
		catch (Exception ex)
		{
			result = 0;
		}
		return result;
	}

	// Token: 0x060000B1 RID: 177 RVA: 0x00009E38 File Offset: 0x00008038
	public static double Doble(double valor)
	{
		return Convert.ToDouble(valor);
	}

	// Token: 0x060000B2 RID: 178 RVA: 0x00009E50 File Offset: 0x00008050
	public static double Doble(string valor)
	{
		double result;
		double.TryParse(valor, out result);
		return result;
	}

	// Token: 0x060000B3 RID: 179 RVA: 0x00009E6C File Offset: 0x0000806C
	public static double Doble(bool valor)
	{
		return Convert.ToDouble(valor);
	}

	// Token: 0x060000B4 RID: 180 RVA: 0x00009E84 File Offset: 0x00008084
	public static double Doble(object valor)
	{
		return Convert.ToDouble(valor);
	}

	// Token: 0x060000B5 RID: 181 RVA: 0x00009E9C File Offset: 0x0000809C
	public static DateTime fecha(string valor)
	{
		DateTime result;
		DateTime.TryParse(valor, out result);
		return result;
	}

	// Token: 0x060000B6 RID: 182 RVA: 0x00009EB8 File Offset: 0x000080B8
	public static DateTime fecha(DateTime valor)
	{
		return Convert.ToDateTime(valor);
	}

	// Token: 0x060000B7 RID: 183 RVA: 0x00009ED0 File Offset: 0x000080D0
	public static string fechaIso(string dte)
	{
		string result;
		if (sf.esFecha(dte))
		{
			int num = sf.Entero(sf.Left(dte, 2));
			int num2 = sf.Entero(sf.Right(sf.Left(dte, 5), 2));
			int num3 = sf.Entero(sf.Right(dte, 4));
			result = string.Concat(new object[]
			{
				num3,
				"-",
				num2,
				"-",
				num
			});
		}
		else
		{
			result = null;
		}
		return result;
	}

	// Token: 0x060000B8 RID: 184 RVA: 0x00009F64 File Offset: 0x00008164
	public static string fechaSql(string dte)
	{
		string result;
		if (sf.esFecha(dte))
		{
			int num = sf.Entero(sf.Left(dte, 2));
			int num2 = sf.Entero(sf.Right(sf.Left(dte, 5), 2));
			int num3 = sf.Entero(sf.Right(dte, 4));
			result = string.Concat(new object[]
			{
				num3,
				"-",
				num2,
				"-",
				num
			});
		}
		else
		{
			result = null;
		}
		return result;
	}

	// Token: 0x060000B9 RID: 185 RVA: 0x00009FF8 File Offset: 0x000081F8
	public static string fechaSql(DateTime dte)
	{
		string result;
		if (sf.esFecha(dte))
		{
			result = string.Concat(new object[]
			{
				dte.Year,
				"-",
				dte.Month,
				"-",
				dte.Day
			});
		}
		else
		{
			result = null;
		}
		return result;
	}

	// Token: 0x060000BA RID: 186 RVA: 0x0000A068 File Offset: 0x00008268
	public static string fechaIso(DateTime dte)
	{
		string result;
		if (sf.esFecha(dte))
		{
			result = string.Concat(new object[]
			{
				dte.Year,
				"-",
				dte.Month,
				"-",
				dte.Day
			});
		}
		else
		{
			result = null;
		}
		return result;
	}

	// Token: 0x060000BB RID: 187 RVA: 0x0000A0D8 File Offset: 0x000082D8
	public static bool esFecha(DateTime fecha)
	{
		bool result;
		try
		{
			string text = fecha.ToShortDateString();
			if (text != null)
			{
				if (text == "01/01/0001")
				{
					return false;
				}
				if (text == "01/01/1900")
				{
					return false;
				}
			}
			DateTime dateTime = default(DateTime);
			dateTime = DateTime.Parse(fecha.ToString());
			result = true;
		}
		catch (Exception ex)
		{
			result = false;
		}
		return result;
	}

	// Token: 0x060000BC RID: 188 RVA: 0x0000A150 File Offset: 0x00008350
	public static bool esFecha(string fecha)
	{
		bool result;
		try
		{
			if (fecha != null)
			{
				if (fecha == "01/01/0001")
				{
					return false;
				}
				if (fecha == "01/01/1900")
				{
					return false;
				}
			}
			DateTime dateTime = default(DateTime);
			dateTime = DateTime.Parse(fecha.ToString());
			result = true;
		}
		catch (Exception ex)
		{
			result = false;
		}
		return result;
	}

	// Token: 0x060000BD RID: 189 RVA: 0x0000A1B8 File Offset: 0x000083B8
	public static string quitarhora(string fecha)
	{
		try
		{
			fecha = sf.Left(fecha, 10);
		}
		catch (Exception ex)
		{
			return "";
		}
		return fecha;
	}

	// Token: 0x060000BE RID: 190 RVA: 0x0000A1F8 File Offset: 0x000083F8
	public static string Left(string param, int length)
	{
		string result;
		if (param != "")
		{
			string text = param.Substring(0, length);
			result = text;
		}
		else
		{
			result = "";
		}
		return result;
	}

	// Token: 0x060000BF RID: 191 RVA: 0x0000A230 File Offset: 0x00008430
	public static string Right(string param, int length)
	{
		return param.Substring(param.Length - length, length);
	}

	// Token: 0x060000C0 RID: 192 RVA: 0x0000A254 File Offset: 0x00008454
	public static string SafeSql(string inputSQL)
	{
		string text = inputSQL.Replace("'", "''");
		text = text.Replace("[", "[[]");
		return text.Replace("_", "[_]");
	}

	// Token: 0x060000C1 RID: 193 RVA: 0x0000A29C File Offset: 0x0000849C
	public static string moneda2(string valor)
	{
		int num = 0;
		bool flag = false;
		valor = valor.Replace(".", ",");
		string result;
		if (valor[0] != ',')
		{
			while (valor.Length > num)
			{
				if (valor[num] == ',')
				{
					if (flag)
					{
						valor = sf.Left(valor, num);
					}
					else
					{
						flag = true;
					}
				}
				num++;
			}
			result = valor;
		}
		else
		{
			result = "0";
		}
		return result;
	}

	// Token: 0x060000C2 RID: 194 RVA: 0x0000A320 File Offset: 0x00008520
	public static string moneda(string valor)
	{
		int num = 0;
		bool flag = false;
		valor = valor.Replace(",", ".");
		string result;
		if (valor[0] != '.')
		{
			while (valor.Length > num)
			{
				if (valor[num] == '.')
				{
					if (flag)
					{
						valor = sf.Left(valor, num);
					}
					else
					{
						flag = true;
					}
				}
				num++;
			}
			result = valor;
		}
		else
		{
			result = "0";
		}
		return result;
	}

	// Token: 0x060000C3 RID: 195 RVA: 0x0000A3A4 File Offset: 0x000085A4
	public static string Cadena(double valor)
	{
		return Convert.ToString(valor);
	}

	// Token: 0x060000C4 RID: 196 RVA: 0x0000A3BC File Offset: 0x000085BC
	public static string Cadena(object valor)
	{
		return Convert.ToString(valor);
	}

	// Token: 0x060000C5 RID: 197 RVA: 0x0000A3D4 File Offset: 0x000085D4
	public static string Cadena(string valor)
	{
		string result;
		if (Convert.ToString(valor) == null)
		{
			result = "";
		}
		else
		{
			result = valor;
		}
		return result;
	}

	// Token: 0x060000C6 RID: 198 RVA: 0x0000A400 File Offset: 0x00008600
	public static string Cadena(bool valor)
	{
		return Convert.ToString(valor);
	}

	// Token: 0x060000C7 RID: 199 RVA: 0x0000A418 File Offset: 0x00008618
	public static string Cadena(DBNull valor)
	{
		string result;
		try
		{
			result = "";
		}
		catch (Exception ex)
		{
			result = "";
		}
		return result;
	}

	// Token: 0x060000C8 RID: 200 RVA: 0x0000A44C File Offset: 0x0000864C
	public static string CadenaRecortada(string valor, int longitud)
	{
		string result;
		try
		{
			valor = sf.html2text(valor);
			if (longitud > valor.Length)
			{
				longitud = valor.Length;
			}
			result = valor.Substring(0, longitud) + " ...";
		}
		catch (Exception ex)
		{
			result = "";
		}
		return result;
	}

	// Token: 0x060000C9 RID: 201 RVA: 0x0000A4B0 File Offset: 0x000086B0
	public static string CadenaRecortada(DBNull valor, int longitud)
	{
		string result;
		try
		{
			result = "";
		}
		catch (Exception ex)
		{
			result = "";
		}
		return result;
	}

	// Token: 0x060000CA RID: 202 RVA: 0x0000A4E4 File Offset: 0x000086E4
	public static string html2text(string Data)
	{
		return Regex.Replace(Data, "<(.|\\n)*?>", string.Empty);
	}

	// Token: 0x060000CB RID: 203 RVA: 0x0000A508 File Offset: 0x00008708
	public static bool Bool(string valor)
	{
		bool result = false;
		bool.TryParse(valor, out result);
		return result;
	}

	// Token: 0x060000CC RID: 204 RVA: 0x0000A528 File Offset: 0x00008728
	public static bool Bool(int valor)
	{
		bool result;
		try
		{
			result = Convert.ToBoolean(valor);
		}
		catch (Exception ex)
		{
			result = false;
		}
		return result;
	}

	// Token: 0x060000CD RID: 205 RVA: 0x0000A558 File Offset: 0x00008758
	public static bool Bool(DBNull valor)
	{
		bool result;
		try
		{
			result = false;
		}
		catch (Exception ex)
		{
			result = false;
		}
		return result;
	}

	// Token: 0x060000CE RID: 206 RVA: 0x0000A584 File Offset: 0x00008784
	public static bool Bool(bool valor)
	{
		return valor;
	}

	// Token: 0x060000CF RID: 207 RVA: 0x0000A598 File Offset: 0x00008798
	public static bool Bool(object valor)
	{
		bool result;
		try
		{
			result = Convert.ToBoolean(valor);
		}
		catch (Exception ex)
		{
			result = false;
		}
		return result;
	}
}
