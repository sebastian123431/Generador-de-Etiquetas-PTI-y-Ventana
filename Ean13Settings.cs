using System;

/// <summary>
/// Clase de configuración para códigos de barras EAN-13
/// Define parámetros visuales como márgenes, altura y ancho de barras
/// Estos valores se usan al generar códigos de barras EAN-13
/// </summary>
public class Ean13Settings
{
	/// <summary>
	/// Altura del código de barras en píxeles
	/// Valor por defecto: 120 píxeles
	/// </summary>
	private int BarCodeHeight = 120;

	/// <summary>
	/// Margen izquierdo del código de barras en píxeles
	/// Valor por defecto: 10 píxeles
	/// </summary>
	private int LeftMargin = 10;

	/// <summary>
	/// Margen derecho del código de barras en píxeles
	/// Valor por defecto: 10 píxeles
	/// </summary>
	private int RightMargin = 10;

	/// <summary>
	/// Margen superior del código de barras en píxeles
	/// Valor por defecto: 10 píxeles
	/// </summary>
	private int TopMargin = 10;

	/// <summary>
	/// Margen inferior del código de barras en píxeles
	/// Valor por defecto: 10 píxeles
	/// </summary>
	private int BottomMargin = 10;

	/// <summary>
	/// Ancho de cada barra del código de barras en píxeles
	/// Valor por defecto: 2 píxeles
	/// </summary>
	private int BarWidth = 2;
}
