using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Data.SQLite;
using System.Collections.Generic;

namespace WindowsFormsApplication1.Data
{
    /// <summary>
    /// Cliente HTTP para sincronización con la API REST de Django
    /// </summary>
    public class HttpSyncClient
    {
        private static HttpSyncClient _instance;
        private readonly string _baseUrl;
        private HttpClient _httpClient;

        private HttpSyncClient()
        {
            var db = DatabaseManager.Instance;
            string servidor = db.GetConfigValue("servidor", "192.168.3.30");
            string puerto = db.GetConfigValue("puerto", "8001");

            _baseUrl = $"http://{servidor}:{puerto}";
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromMinutes(2)
            };
        }

        public static HttpSyncClient Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new HttpSyncClient();
                }
                return _instance;
            }
        }

        /// <summary>
        /// Sincroniza todos los datos desde la API REST
        /// </summary>
        public async Task<(bool success, string message)> SincronizarTodoAsync(
            IProgress<(int, string)> progress = null)
        {
            try
            {
                var db = DatabaseManager.Instance;
                db.ClearAllData();

                var endpoints = new Dictionary<string, string>
                {
                    { "productores", $"{_baseUrl}/api/productor/" },
                    { "variedades", $"{_baseUrl}/api/variedad/" },
                    { "lotes", $"{_baseUrl}/api/lote/" },
                    { "packings", $"{_baseUrl}/api/packing/" },
                    { "recibidores", $"{_baseUrl}/api/recibidor/" },
                    { "tipos_embalaje", $"{_baseUrl}/api/tipo-embalaje/" },
                    { "calibres", $"{_baseUrl}/api/calibre/" },
                    { "gtins", $"{_baseUrl}/api/gtin/" },
                    { "sdps", $"{_baseUrl}/api/sdp/" },
                    { "pesos", $"{_baseUrl}/api/peso/" },
                    { "categorias_sag", $"{_baseUrl}/api/categoria-sag/" },
                    { "colores", $"{_baseUrl}/api/color/" },
                    { "variedades_imprime", $"{_baseUrl}/api/variedad-imprime/" },
                    { "ubicaciones", $"{_baseUrl}/api/ubicacion/" },
                    { "variedades_variedad_imprimir", $"{_baseUrl}/api/variedad-variedadimprime/" },
                    { "trazabilidades", $"{_baseUrl}/api/trazabilidad/" },
                    { "variedad_imprime_gtin", $"{_baseUrl}/api/variedad-imprime-gtin/" },
                    { "cantidad_cajas", $"{_baseUrl}/api/cantidad-cajas/" },
                    { "variedad_citrico", $"{_baseUrl}/api/variedad-citrico/" },
                    { "packing_citrico", $"{_baseUrl}/api/packing-citrico/" },
                    { "tipo_embalaje_citrico", $"{_baseUrl}/api/tipo-embalaje-citrico/" },
                    { "recibidor_citrico", $"{_baseUrl}/api/recibidor-citrico/" },
                    { "productor_citrico", $"{_baseUrl}/api/productor-citrico/" },
                    { "cantidad_cajas_citrico", $"{_baseUrl}/api/cantidad-cajas-citrico/" }
                };

                int total = endpoints.Count;
                int current = 0;

                foreach (var endpoint in endpoints)
                {
                    current++;
                    int percentage = current * 100 / total;
                    progress?.Report((percentage, $"Sincronizando {endpoint.Key}..."));

                    var response = await _httpClient.GetAsync(endpoint.Value);

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception(
                            $"Error HTTP {response.StatusCode} en {endpoint.Value}"
                        );
                    }

                    var content = await response.Content.ReadAsStringAsync();
                    var jsonArray = ParseJsonArraySafe(content);

                    // Si la API devuelve vacío / null / formato inesperado, no se cae: simplemente no sincroniza esa tabla.
                    if (jsonArray == null || jsonArray.Count == 0)
                    {
                        continue;
                    }

                    foreach (var item in jsonArray)
                    {
                        // Si un registro viene incompleto (sin id), se omite para no cortar toda la sincronización.
                        if (item == null || item.Type != JTokenType.Object)
                            continue;

                        var idToken = item["id"];
                        if (idToken == null || idToken.Type == JTokenType.Null)
                            continue;

                        switch (endpoint.Key)
                        {
                            case "productores":
                                db.UpsertProductor(
                                    item["id"].Value<int>(),
                                    item["dato"]?.ToString() ?? item["nombre"]?.ToString() ?? "",
                                    item["activo"]?.Value<bool>() ?? true,
                                    item["csg"]?.ToString() ?? item["CSG"]?.ToString() ?? ""
                                );
                                break;

                            case "variedades":
                                db.UpsertVariedad(
                                    item["id"].Value<int>(),
                                    item["dato"]?.ToString() ?? "",
                                    item["activo"]?.Value<bool>() ?? true
                                );
                                break;

                            case "lotes":
                                db.UpsertLote(
                                    item["id"].Value<int>(),
                                    item["dato"]?.ToString() ?? "",
                                    item["activo"]?.Value<bool>() ?? true
                                );
                                break;

                            case "packings":
                                // Intentamos leer csp y ubicacion si vienen en la API
                                long? csp = null;
                                int? ubicacion = null;
                                try
                                {
                                    var cspToken = item["csp"] ?? item["CSP"];
                                    if (cspToken != null && cspToken.Type != JTokenType.Null)
                                    {
                                        if (cspToken.Type == JTokenType.Integer) csp = cspToken.Value<long>();
                                        else if (long.TryParse(cspToken.ToString(), out var cspv)) csp = cspv;
                                    }
                                }
                                catch { }

                                try
                                {
                                    var ubicToken = item["ubicacion"];
                                    if (ubicToken != null && ubicToken.Type != JTokenType.Null)
                                    {
                                        if (ubicToken.Type == JTokenType.Integer) ubicacion = ubicToken.Value<int>();
                                        else if (int.TryParse(ubicToken.ToString(), out var u)) ubicacion = u;
                                    }
                                }
                                catch { }

                                // Debug: informa lo que vamos a insertar para facilitar diagnóstico
                                try
                                {
                                    Console.WriteLine($"[Sync][Packing] id={item["id"]?.ToString() ?? "?"} dato={item["dato"]?.ToString() ?? ""} parsed_csp={(csp.HasValue? csp.Value.ToString():"NULL")} parsed_ubicacion={(ubicacion.HasValue? ubicacion.Value.ToString():"NULL")}");
                                }
                                catch { }

                                db.UpsertPacking(
                                    item["id"].Value<int>(),
                                    item["dato"]?.ToString() ?? "",
                                    item["activo"]?.Value<bool>() ?? true,
                                    csp,
                                    ubicacion
                                );
                                break;

                            case "recibidores":
                                db.UpsertRecibidor(
                                    item["id"].Value<int>(),
                                    item["dato"]?.ToString() ?? "",
                                    item["nota"]?.ToString() ?? ""
                                );
                                break;

                            case "tipos_embalaje":
                                db.UpsertTipoEmbalaje(
                                    item["id"].Value<int>(),
                                    item["dato"]?.ToString() ?? "",
                                    item["activo"]?.Value<bool>() ?? true,
                                    item["peso_fijo"]?.Value<bool>() ?? false
                                );
                                break;

                            case "calibres":
                                db.UpsertCalibre(
                                    item["id"].Value<int>(),
                                    item["dato"]?.ToString() ?? "",
                                    item["activo"]?.Value<bool>() ?? true
                                );
                                break;

                            case "gtins":
                                {
                                    bool? pesoFijo = null;
                                    var pfToken = item["peso_fijo"];
                                    if (pfToken != null)
                                    {
                                        try
                                        {
                                            if (pfToken.Type == JTokenType.Boolean) pesoFijo = pfToken.Value<bool>();
                                            else if (int.TryParse(pfToken.ToString(), out var pfInt)) pesoFijo = pfInt != 0;
                                        }
                                        catch { pesoFijo = null; }
                                    }

                                    db.UpsertGTIN(
                                        item["id"].Value<int>(),
                                        item["gtin"]?.ToString() ?? item["dato"]?.ToString() ?? "",
                                        item["activo"]?.Value<bool>() ?? true,
                                        item["plu"]?.ToString() ?? "",
                                        item["cod_bolsa"]?.ToString() ?? item["codBolsa"]?.ToString() ?? "",
                                        pesoFijo
                                    );
                                }
                                break;

                            case "sdps":
                                db.UpsertSDP(
                                    item["id"].Value<int>(),
                                    item["dato"]?.ToString() ?? "",
                                    item["activo"]?.Value<bool>() ?? true
                                );
                                break;

                            case "pesos":
                                db.UpsertPeso(
                                    item["id"].Value<int>(),
                                    item["dato"]?.ToString() ?? "",
                                    item["activo"]?.Value<bool>() ?? true
                                );
                                break;

                            case "categorias_sag":
                                db.UpsertCategoriaSAG(
                                    item["id"].Value<int>(),
                                    item["nombre"]?.ToString() ?? ""
                                );
                                break;

                            case "colores":
                                db.UpsertColor(
                                    item["id"].Value<int>(),
                                    item["nombre"]?.ToString() ?? "",
                                    item["descripcion"]?.ToString() ?? "",
                                    item["activo"]?.Value<bool>() ?? true
                                );
                                break;

                            case "variedades_imprime":
                                {
                                    bool? pesoFijo = null;
                                    var pfToken = item["peso_fijo"];
                                    if (pfToken != null)
                                    {
                                        try
                                        {
                                            if (pfToken.Type == JTokenType.Boolean) pesoFijo = pfToken.Value<bool>();
                                            else if (int.TryParse(pfToken.ToString(), out var pfInt)) pesoFijo = pfInt != 0;
                                        }
                                        catch { pesoFijo = null; }
                                    }

                                    db.UpsertVariedadImprima(
                                        item["id"].Value<int>(),
                                        item["dato"]?.ToString() ?? item["nombre"]?.ToString() ?? "",
                                        item["activo"]?.Value<bool>() ?? true,
                                        pesoFijo,
                                        item["var_interno"]?.ToString() ?? ""
                                    );
                                }
                                break;

                            case "ubicaciones":
                                db.UpsertUbicacion(
                                    item["id"].Value<int>(),
                                    item["region"]?.ToString() ?? "",
                                    item["provincia"]?.ToString() ?? "",
                                    item["comuna"]?.ToString() ?? "",
                                    item["gln"]?.ToString() ?? "",
                                    item["ggn"]?.ToString() ?? "",
                                    item["activo"]?.Value<bool>() ?? true,
                                    item["creado_en"]?.ToString() ?? "",
                                    item["actualizado_en"]?.ToString() ?? ""
                                );
                                break;

                            case "variedades_variedad_imprimir":
                                db.UpsertVariedadVariedadImprimir(
                                    item["id"].Value<int>(),
                                    item["variedad"]?.Value<int>() ?? 0,
                                    item["variedad_imprime"]?.Value<int>() ?? 0
                                );
                                break;

                            case "trazabilidades":
                                db.UpsertTrazabilidad(
                                    item["id"].Value<int>(),
                                    item["codigo"]?.ToString() ?? "",
                                    item["sdp"]?.Value<int>() ?? 0,
                                    item["productor"]?.Value<int>() ?? 0,
                                    item["variedad"]?.Value<int>() ?? 0,
                                    item["lote"]?.Value<int>() ?? 0,
                                    item["tipo_embalaje"]?.Value<int>() ?? 0,
                                    item["calibre"]?.Value<int>() ?? 0,
                                    item["packing"]?.Value<int>() ?? 0,
                                    item["peso"]?.Value<int>() ?? 0,
                                    item["recibidor"]?.Value<int>() ?? 0,
                                    item["color"]?.Value<int>() ?? 0,
                                    item["variedad_imprime"]?.Value<int>() ?? 0,
                                    item["categoria_sag"]?.Value<int>() ?? 0,
                                    item["fecha"]?.ToString() ?? "",
                                    item["hora"]?.ToString() ?? "",
                                    item["cantidad"]?.Value<int>() ?? 0,
                                    item["observaciones"]?.ToString() ?? ""
                                );
                                break;

                            case "variedad_imprime_gtin":
                                db.UpsertVariedadImprimeGTIN(
                                    item["id"].Value<int>(),
                                    item["variedad_imprime"]?.Value<int>() ?? 0,
                                    item["gtin"]?.Value<int>() ?? 0
                                );
                                break;

                            case "cantidad_cajas":
                                db.UpsertCantidadCajas(
                                    item["id"].Value<int>(),
                                    item["cantidad"]?.ToString() ?? item["dato"]?.ToString() ?? "",
                                    item["activo"]?.Value<bool>() ?? true
                                );
                                break;

                            case "variedad_citrico":
                                try
                                {
                                    // Usa el upsert centralizado en DatabaseManager para manejar presencia/ausencia
                                    // de la columna `numero_interno` en el esquema local.
                                    var id = item["id"].Value<int>();
                                    var variedad = item["variedad"]?.ToString() ?? item["dato"]?.ToString() ?? "";
                                    // API serializer puede devolver `numero_interno` (snake_case)
                                    var numeroInterno = item["numero_interno"]?.ToString() ?? item["numeroInterno"]?.ToString() ?? "";
                                    var activo = item["activo"]?.Value<bool>() ?? true;
                                    db.UpsertVariedadCitrico(id, variedad, numeroInterno, activo);
                                }
                                catch { }
                                break;

                            case "packing_citrico":
                                try
                                {
                                    using (var conn = db.GetConnection())
                                    {
                                        conn.Open();
                                        using (var cmd = new SQLiteCommand(@"INSERT OR REPLACE INTO PackingCitrico (id, packing, activo) VALUES (@id, @packing, @activo)", conn))
                                        {
                                            cmd.Parameters.AddWithValue("@id", item["id"].Value<int>());
                                            cmd.Parameters.AddWithValue("@packing", item["packing"]?.ToString() ?? item["dato"]?.ToString() ?? "");
                                            cmd.Parameters.AddWithValue("@activo", (item["activo"]?.Value<bool>() ?? true) ? 1 : 0);
                                            cmd.ExecuteNonQuery();
                                        }
                                    }
                                }
                                catch { }
                                break;

                            case "tipo_embalaje_citrico":
                                try
                                {
                                    using (var conn = db.GetConnection())
                                    {
                                        conn.Open();
                                        using (var cmd = new SQLiteCommand(@"INSERT OR REPLACE INTO TipoEmbalajeCitrico (id, tipo_embalaje, activo) VALUES (@id, @tipo, @activo)", conn))
                                        {
                                            cmd.Parameters.AddWithValue("@id", item["id"].Value<int>());
                                            cmd.Parameters.AddWithValue("@tipo", item["tipo_embalaje"]?.ToString() ?? item["dato"]?.ToString() ?? "");
                                            cmd.Parameters.AddWithValue("@activo", (item["activo"]?.Value<bool>() ?? true) ? 1 : 0);
                                            cmd.ExecuteNonQuery();
                                        }
                                    }
                                }
                                catch { }
                                break;

                            case "recibidor_citrico":
                                try
                                {
                                    using (var conn = db.GetConnection())
                                    {
                                        conn.Open();
                                        using (var cmd = new SQLiteCommand(@"INSERT OR REPLACE INTO RecibidorCitrico (id, recibidor, activo) VALUES (@id, @recibidor, @activo)", conn))
                                        {
                                            cmd.Parameters.AddWithValue("@id", item["id"].Value<int>());
                                            cmd.Parameters.AddWithValue("@recibidor", item["recibidor"]?.ToString() ?? item["dato"]?.ToString() ?? "");
                                            cmd.Parameters.AddWithValue("@activo", (item["activo"]?.Value<bool>() ?? true) ? 1 : 0);
                                            cmd.ExecuteNonQuery();
                                        }
                                    }
                                }
                                catch { }
                                break;

                            case "productor_citrico":
                                try
                                {
                                    using (var conn = db.GetConnection())
                                    {
                                        conn.Open();
                                        using (var cmd = new SQLiteCommand(@"INSERT OR REPLACE INTO ProductorCitrico (id, dato, CSG, activo) VALUES (@id, @dato, @csg, @activo)", conn))
                                        {
                                            cmd.Parameters.AddWithValue("@id", item["id"].Value<int>());
                                            cmd.Parameters.AddWithValue("@dato", item["dato"]?.ToString() ?? "");
                                            cmd.Parameters.AddWithValue("@csg", item["CSG"]?.ToString() ?? item["csg"]?.ToString() ?? "");
                                            cmd.Parameters.AddWithValue("@activo", (item["activo"]?.Value<bool>() ?? true) ? 1 : 0);
                                            cmd.ExecuteNonQuery();
                                        }
                                    }
                                }
                                catch { }
                                break;

                            case "cantidad_cajas_citrico":
                                try
                                {
                                    using (var conn = db.GetConnection())
                                    {
                                        conn.Open();
                                        using (var cmd = new SQLiteCommand(@"INSERT OR REPLACE INTO CantidadCajasCitrico (id, cantidad_cajas, activo) VALUES (@id, @cantidad, @activo)", conn))
                                        {
                                            cmd.Parameters.AddWithValue("@id", item["id"].Value<int>());
                                            cmd.Parameters.AddWithValue("@cantidad", item["cantidad_cajas"]?.ToString() ?? item["dato"]?.ToString() ?? "");
                                            cmd.Parameters.AddWithValue("@activo", (item["activo"]?.Value<bool>() ?? true) ? 1 : 0);
                                            cmd.ExecuteNonQuery();
                                        }
                                    }
                                }
                                catch { }
                                break;
                        }
                    }
                }

                progress?.Report((100, "Sincronización completada"));
                return (true, "Sincronización completada exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error en sincronización: {ex.Message}");
            }
        }

        /// <summary>
        /// Parsea de forma segura una respuesta JSON que normalmente es un array.
        /// - Si viene vacía o "null": retorna array vacío.
        /// - Si viene como objeto paginado (ej: { results: [...] }): extrae results.
        /// - Si el formato no es esperado: retorna array vacío.
        /// </summary>
        private JArray ParseJsonArraySafe(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return new JArray();

            content = content.Trim();
            if (string.Equals(content, "null", StringComparison.OrdinalIgnoreCase))
                return new JArray();

            try
            {
                var token = JToken.Parse(content);
                if (token.Type == JTokenType.Array)
                    return (JArray)token;

                if (token.Type == JTokenType.Object)
                {
                    var obj = (JObject)token;
                    var results = obj["results"] ?? obj["data"] ?? obj["items"];
                    if (results != null && results.Type == JTokenType.Array)
                        return (JArray)results;
                }
            }
            catch
            {
                // Ignora el error y retorna vacío
            }

            return new JArray();
        }

        /// <summary>
        /// Verifica si el servidor Django está disponible
        /// </summary>
        public async Task<bool> VerificarConexionAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Prueba la conexión con la API
        /// </summary>
        public async Task<(bool success, string message)> TestConnectionAsync()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(10);
                    var response = await client.GetAsync($"{_baseUrl}/api/");
                    return response.IsSuccessStatusCode
                        ? (true, "Conexión exitosa con la API")
                        : (false, $"Error HTTP: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                return (false, $"Error de conexión: {ex.Message}");
            }
        }

		// Método usado solo para pruebas desde frm_generador (debug)
		public async Task<List<ColorApiItem>> GetColoresDesdeAPI()
		{
			var result = new List<ColorApiItem>();
			try
			{
				var response = await _httpClient.GetAsync($"{_baseUrl}/api/color/");
				response.EnsureSuccessStatusCode();
				var json = await response.Content.ReadAsStringAsync();
				var data = JArray.Parse(json);
				foreach (var item in data)
				{
					result.Add(new ColorApiItem
					{
						Id = item["id"]?.Value<int>() ?? 0,
						Nombre = item["nombre"]?.ToString() ?? item["dato"]?.ToString() ?? "",
						Activo = item["activo"]?.Value<bool>() ?? true
					});
				}
			}
			catch
			{
				// Devuelve vacío si falla
			}
			return result;
		}
    }

	public class ColorApiItem
	{
		public int Id { get; set; }
		public string Nombre { get; set; }
		public bool Activo { get; set; }
	}
}