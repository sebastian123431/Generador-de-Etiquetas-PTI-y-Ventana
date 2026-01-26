using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication1.Data;

namespace WindowsFormsApplication1
{
    public class TestSincronizacion : Form
    {
        private Button btnTest;
        private TextBox txtLog;

        public TestSincronizacion()
        {
            this.Text = "Test de Sincronización";
            this.Size = new System.Drawing.Size(600, 400);

            btnTest = new Button();
            btnTest.Text = "Probar Sincronización HTTP";
            btnTest.Location = new System.Drawing.Point(10, 10);
            btnTest.Size = new System.Drawing.Size(200, 30);
            btnTest.Click += BtnTest_Click;
            this.Controls.Add(btnTest);

            txtLog = new TextBox();
            txtLog.Multiline = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Location = new System.Drawing.Point(10, 50);
            txtLog.Size = new System.Drawing.Size(560, 300);
            this.Controls.Add(txtLog);
        }

        private async void BtnTest_Click(object sender, EventArgs e)
        {
            txtLog.Clear();
            Log("Iniciando test...");

            try
            {
                // Test 1: Verificar conexión básica
                Log("\n[Test 1] Verificando conexión a Django...");
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(5);
                    var response = await client.GetAsync("http://localhost:8000/api/");
                    Log($"  ? Django responde: {response.StatusCode}");
                }

                // Test 2: Probar endpoints individuales de la API REST
                Log("\n[Test 2] Probando endpoints individuales de la API REST...");
                string[] endpoints = new string[] {
                    "productores", "variedades", "lotes", "packings", "recibidores", "tipos_embalaje", "calibres", "titulos", "gtins", "variedades_imprime", "sdps", "mapeos_codigo_final"
                };
                string baseUrl = "http://localhost:8000/api/";
                foreach (var endpoint in endpoints)
                {
                    string url = baseUrl;
                    switch (endpoint)
                    {
                        case "productores": url += "productor/"; break;
                        case "variedades": url += "variedad/"; break;
                        case "lotes": url += "lote/"; break;
                        case "packings": url += "packing/"; break;
                        case "recibidores": url += "recibidor/"; break;
                        case "tipos_embalaje": url += "tipo-embalaje/"; break;
                        case "calibres": url += "calibre/"; break;
                        case "titulos": url += "titulo/"; break;
                        case "gtins": url += "gtin/"; break;
                        case "variedades_imprime": url += "variedad-imprime/"; break;
                        case "sdps": url += "sdp/"; break;
                        case "mapeos_codigo_final": url += "mapeo-codigo-final/"; break;
                        default: url += endpoint + "/"; break;
                    }
                    using (HttpClient client = new HttpClient())
                    {
                        client.Timeout = TimeSpan.FromSeconds(10);
                        Log($"  Consultando {url}");
                        var response = await client.GetAsync(url);
                        if (response.IsSuccessStatusCode)
                        {
                            string content = await response.Content.ReadAsStringAsync();
                            try
                            {
                                var array = Newtonsoft.Json.Linq.JArray.Parse(content);
                                Log($"    OK - {array.Count} elementos recibidos");
                            }
                            catch
                            {
                                Log($"    OK - Respuesta recibida pero no es un array JSON");
                            }
                        }
                        else
                        {
                            Log($"    Error HTTP: {response.StatusCode}");
                        }
                    }
                }

                // Test 3: Verificar DatabaseManager
                Log("\n[Test 3] Verificando DatabaseManager...");
                var db = DatabaseManager.Instance;
                Log("  ? DatabaseManager inicializado");

                // Test 4: Probar UpsertProductor
                Log("\n[Test 4] Probando UpsertProductor...");
                db.UpsertProductor(999, "TEST PRODUCTOR", true);
                var productores = db.GetProductores();
                Log($"  ? Productores en DB: {productores.Count}");
                
                if (productores.Contains("TEST PRODUCTOR"))
                {
                    Log("  ? Productor de prueba insertado correctamente");
                }

                // Test 5: Sincronización completa
                Log("\n[Test 5] Ejecutando sincronización completa...");
                var httpSync = HttpSyncClient.Instance;
                var (success, message) = await httpSync.SincronizarTodoAsync();
                
                if (success)
                {
                    Log($"  ? {message}");
                    
                    // Verificar datos en DB
                    int prodCount = db.GetProductores().Count;
                    int varCount = db.GetVariedades().Count;
                    int loteCount = db.GetLotes().Count;
                    
                    Log($"\n[Resultado Final]");
                    Log($"  Productores en DB local: {prodCount}");
                    Log($"  Variedades en DB local: {varCount}");
                    Log($"  Lotes en DB local: {loteCount}");
                }
                else
                {
                    Log($"  ? Error: {message}");
                }

                Log("\n? TEST COMPLETADO");
            }
            catch (Exception ex)
            {
                Log($"\n? ERROR: {ex.Message}");
                Log($"StackTrace: {ex.StackTrace}");
            }
        }

        private void Log(string message)
        {
            txtLog.AppendText(message + Environment.NewLine);
            txtLog.ScrollToCaret();
            Application.DoEvents();
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TestSincronizacion());
        }
    }
}
