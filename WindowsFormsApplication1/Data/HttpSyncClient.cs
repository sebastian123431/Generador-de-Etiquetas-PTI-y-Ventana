using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
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
            string servidor = db.GetConfigValue("servidor", "localhost");
            string puerto = db.GetConfigValue("puerto", "8000");

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

                    // 🔥 ENDPOINTS CORREGIDOS (guiones medios)
                    { "tipos_embalaje", $"{_baseUrl}/api/tipo-embalaje/" },
                    { "variedades_imprime", $"{_baseUrl}/api/variedad-imprime/" },
                    { "categorias_sag", $"{_baseUrl}/api/categoria-sag/" },
                    { "variedades_variedad_imprimir", $"{_baseUrl}/api/variedad-variedadimprime/" },

                    { "calibres", $"{_baseUrl}/api/calibre/" },
                    { "packing", $"{_baseUrl}/api/packing/" },
                    { "sdp", $"{_baseUrl}/api/sdp/" },
                    { "colores", $"{_baseUrl}/api/color/" },
                    { "pesos", $"{_baseUrl}/api/peso/" },
                    { "trazabilidades", $"{_baseUrl}/api/trazabilidad/" }
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
                    var jsonArray = JArray.Parse(content);

                    foreach (var item in jsonArray)
                    {
                        switch (endpoint.Key)
                        {
                            case "productores":
                                db.UpsertProductor(
                                    item["id"].Value<int>(),
                                    item["dato"].ToString(),
                                    true
                                );
                                break;

                            case "variedades_imprime":
                                db.UpsertVariedadImprime(
                                    item["id"].Value<int>(),
                                    item["dato"].ToString(),
                                    true
                                );
                                break;

                            // ⚠️ El resto queda disponible para futuro
                            default:
                                break;
                        }
                    }
                }

                return (true, "Sincronización completada exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error en sincronización: {ex.Message}");
            }
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
    }
}
