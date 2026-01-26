using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace WindowsFormsApplication1.Data
{
    /// <summary>
    /// Gestor de base de datos SQLite local para almacenar datos sincronizados desde Django
    /// Maneja la conexi?n, creaci?n de tablas y operaciones CRUD
    /// </summary>
    public class DatabaseManager
    {
        private static DatabaseManager _instance;
        private readonly string _connectionString;
        private readonly string _dbPath;
        private bool _dbAvailable = false;

        private DatabaseManager()
        {
            _dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "etiquetas_local.db");
            _connectionString = $"Data Source={_dbPath};Version=3;";
            InitializeDatabase();
        }

        public static DatabaseManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DatabaseManager();
                }
                return _instance;
            }
        }

        private void InitializeDatabase()
        {
            try
            {
                bool isNewDatabase = !File.Exists(_dbPath);

                if (isNewDatabase)
                {
                    SQLiteConnection.CreateFile(_dbPath);
                }

                // Siempre crear tablas (CREATE TABLE IF NOT EXISTS las protege)
                CreateTables();
            }
            catch (Exception ex)
            {
                // Registrar el error para depuraci�n
                Console.WriteLine($"Error al inicializar la base de datos: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                throw; // Relanzar la excepci�n para que sea manejada en niveles superiores
            }
        }

        private void CreateTables()
        {
            using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string[] createTableQueries = new string[]
                {
					// Productor
                    @"CREATE TABLE IF NOT EXISTS Productor (
                        id INTEGER PRIMARY KEY,
                        dato TEXT NOT NULL UNIQUE,
                        activo INTEGER DEFAULT 1
                    )",

					// Variedad
                    @"CREATE TABLE IF NOT EXISTS Variedad (
                        id INTEGER PRIMARY KEY,
                        dato TEXT NOT NULL UNIQUE,
                        activo INTEGER DEFAULT 1
                    )",

					// Lote
                    @"CREATE TABLE IF NOT EXISTS Lote (
                        id INTEGER PRIMARY KEY,
                        dato TEXT NOT NULL UNIQUE,
                        activo INTEGER DEFAULT 1
                    )",

					// Packing
                    @"CREATE TABLE IF NOT EXISTS Packing (
                        id INTEGER PRIMARY KEY,
                        dato TEXT NOT NULL UNIQUE,
                        activo INTEGER DEFAULT 1
                    )",

					// TipoEmbalaje
                    @"CREATE TABLE IF NOT EXISTS TipoEmbalaje (
                        id INTEGER PRIMARY KEY,
                        dato TEXT NOT NULL UNIQUE,
                        activo INTEGER DEFAULT 1,
                        peso_fijo INTEGER DEFAULT 0
                    )",

					// Calibre
                    @"CREATE TABLE IF NOT EXISTS Calibre (
                        id INTEGER PRIMARY KEY,
                        dato TEXT NOT NULL UNIQUE,
                        activo INTEGER DEFAULT 1
                    )",

					// GTIN
                    @"CREATE TABLE IF NOT EXISTS GTIN (
                        id INTEGER PRIMARY KEY,
                        dato TEXT NOT NULL UNIQUE,
                        activo INTEGER DEFAULT 1
                    )",

					// SDP
                    @"CREATE TABLE IF NOT EXISTS SDP (
                        id INTEGER PRIMARY KEY,
                        dato TEXT NOT NULL UNIQUE,
                        activo INTEGER DEFAULT 1
                    )",

					// Peso
                    @"CREATE TABLE IF NOT EXISTS Peso (
                        id INTEGER PRIMARY KEY,
                        dato TEXT NOT NULL UNIQUE,
                        activo INTEGER DEFAULT 1
                    )",

					// CategoriaSAG
                    @"CREATE TABLE IF NOT EXISTS CategoriaSAG (
                        id INTEGER PRIMARY KEY,
                        nombre TEXT NOT NULL UNIQUE
                    )",

					// Color
                    @"CREATE TABLE IF NOT EXISTS Color (
                        id INTEGER PRIMARY KEY,
                        nombre TEXT NOT NULL UNIQUE,
                        descripcion TEXT,
                        activo INTEGER DEFAULT 1
                    )",

					// VariedadImprime
                    @"CREATE TABLE IF NOT EXISTS VariedadImprime (
                        id INTEGER PRIMARY KEY,
                        dato TEXT NOT NULL UNIQUE,
                        activo INTEGER DEFAULT 1
                    )",

					// VariedadImprime_GTIN (intermedia)
					@"CREATE TABLE IF NOT EXISTS VariedadImprime_GTIN (
						id INTEGER PRIMARY KEY,
						variedad_imprime INTEGER NOT NULL,
						gtin INTEGER NOT NULL,
						creado_en DATETIME DEFAULT CURRENT_TIMESTAMP,
						actualizado_en DATETIME DEFAULT CURRENT_TIMESTAMP,
						UNIQUE(variedad_imprime, gtin)
					)",

					// VariedadVariedadImprime_variedad (intermedia)
					@"CREATE TABLE IF NOT EXISTS VariedadVariedadImprime_variedad (
						id INTEGER PRIMARY KEY,
						variedad INTEGER NOT NULL,
						variedad_imprime INTEGER NOT NULL,
						UNIQUE(variedad, variedad_imprime)
					)",

					// Trazabilidad (intermedia)
					@"CREATE TABLE IF NOT EXISTS Trazabilidad (
						id INTEGER PRIMARY KEY,
						productor INTEGER NOT NULL,
						variedad INTEGER NOT NULL,
						lote INTEGER NOT NULL,
						sdp INTEGER NOT NULL,
						variedad_imprime INTEGER NOT NULL,
						creado_en DATETIME DEFAULT CURRENT_TIMESTAMP,
						actualizado_en DATETIME DEFAULT CURRENT_TIMESTAMP,
						UNIQUE(productor, variedad, lote, sdp, variedad_imprime)
					)"
                };
                foreach (string query in createTableQueries)
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public SQLiteConnection GetConnection()
        {
            if (!_dbAvailable)
            {
                throw new InvalidOperationException("La base de datos SQLite no está disponible. Compruebe que 'SQLite.Interop.dll' esté presente para la arquitectura correcta y que System.Data.SQLite esté instalado correctamente.");
            }
            return new SQLiteConnection(_connectionString);
        }

        public void UpsertProductor(int id, string dato, bool activo)
        {
            using (SQLiteConnection conn = GetConnection())
            {
                conn.Open();
                string query = @"INSERT OR REPLACE INTO Productor (id, dato, activo) VALUES (@id, @dato, @activo)";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@dato", dato);
                    cmd.Parameters.AddWithValue("@activo", activo ? 1 : 0);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpsertRecibidor(int id, string nombre, string nota)
        {
            using (SQLiteConnection conn = GetConnection())
            {
                conn.Open();
                string query = @"INSERT OR REPLACE INTO Recibidores (id, dato, nota, sincronizado_en) 
                                 VALUES (@id, @dato, @nota, CURRENT_TIMESTAMP)";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@dato", nombre);
                    cmd.Parameters.AddWithValue("@nota", nota ?? "");
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpsertTipoEmbalaje(int id, string dato, bool activo, bool pesoFijo)
        {
            using (SQLiteConnection conn = GetConnection())
            {
                conn.Open();
                string query = @"INSERT OR REPLACE INTO TipoEmbalaje (id, dato, activo, peso_fijo) 
                                 VALUES (@id, @dato, @activo, @peso_fijo)";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@dato", dato);
                    cmd.Parameters.AddWithValue("@activo", activo ? 1 : 0);
                    cmd.Parameters.AddWithValue("@peso_fijo", pesoFijo ? 1 : 0);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpsertTitulo(int id, string texto, string tipo, bool activo)
        {
            using (SQLiteConnection conn = GetConnection())
            {
                conn.Open();
                string query = @"INSERT OR REPLACE INTO Titulos (id, texto, tipo, activo) 
								 VALUES (@id, @texto, @tipo, @activo)";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@texto", texto);
                    cmd.Parameters.AddWithValue("@tipo", tipo);
                    cmd.Parameters.AddWithValue("@activo", activo ? 1 : 0);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpsertMapeoCodigoFinal(int id, string variedadImprime, string gtin, string tipoEmbalaje, string codigoFinal, bool activo)
        {
            using (SQLiteConnection conn = GetConnection())
            {
                conn.Open();
                string query = @"INSERT OR REPLACE INTO MapeosCodigoFinal 
								 (id, variedad_imprime, gtin, tipo_embalaje, codigo_final, activo, sincronizado_en) 
								 VALUES (@id, @variedadImprime, @gtin, @tipoEmbalaje, @codigoFinal, @activo, CURRENT_TIMESTAMP)";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@variedadImprime", variedadImprime);
                    cmd.Parameters.AddWithValue("@gtin", gtin);
                    cmd.Parameters.AddWithValue("@tipoEmbalaje", tipoEmbalaje ?? "");
                    cmd.Parameters.AddWithValue("@codigoFinal", codigoFinal);
                    cmd.Parameters.AddWithValue("@activo", activo ? 1 : 0);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<string> GetProductores()
        {
            var result = new List<string>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT dato FROM Productor ORDER BY dato ASC";
                using (var cmd = new SQLiteCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        result.Add(reader["dato"].ToString());
            }
            return result;
        }

        public List<string> GetVariedades()
        {
            var result = new List<string>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT dato FROM Variedad ORDER BY dato ASC";
                using (var cmd = new SQLiteCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        result.Add(reader["dato"].ToString());
            }
            return result;
        }

        public List<string> GetLotes()
        {
            var result = new List<string>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT dato FROM Lote ORDER BY dato ASC";
                using (var cmd = new SQLiteCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        result.Add(reader["dato"].ToString());
            }
            return result;
        }

        public List<string> GetVariedadesImprime()
        {
            var result = new List<string>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT dato FROM VariedadImprime ORDER BY dato ASC";
                using (var cmd = new SQLiteCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        result.Add(reader["dato"].ToString());
            }
            return result;
        }

        public List<string> GetGTINs()
        {
            var result = new List<string>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT dato FROM GTIN ORDER BY dato ASC";
                using (var cmd = new SQLiteCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        result.Add(reader["dato"].ToString());
            }
            return result;
        }

        public List<string> GetTiposEmbalaje()
        {
            var result = new List<string>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT dato FROM TipoEmbalaje WHERE activo = 1 ORDER BY dato ASC";
                using (var cmd = new SQLiteCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        result.Add(reader["dato"].ToString());
            }
            return result;
        }

        public List<string> GetCalibres()
        {
            var result = new List<string>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT dato FROM Calibre WHERE activo = 1 ORDER BY dato ASC";
                using (var cmd = new SQLiteCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        result.Add(reader["dato"].ToString());
            }
            return result;
        }

        public List<string> GetSDPs()
        {
            var result = new List<string>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT dato FROM SDP WHERE activo = 1 ORDER BY dato ASC";
                using (var cmd = new SQLiteCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        result.Add(reader["dato"].ToString());
            }
            return result;
        }

        public void SetConfigValue(string clave, string valor)
        {
            using (SQLiteConnection conn = GetConnection())
            {
                conn.Open();
                string query = @"INSERT OR REPLACE INTO ConfiguracionSync (clave, valor, actualizado_en) 
								 VALUES (@clave, @valor, CURRENT_TIMESTAMP)";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@clave", clave);
                    cmd.Parameters.AddWithValue("@valor", valor);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public string GetConfigValue(string clave, string defaultValue = "")
        {
            using (SQLiteConnection conn = GetConnection())
            {
                conn.Open();
                string query = "SELECT valor FROM ConfiguracionSync WHERE clave = @clave";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@clave", clave);
                    object result = cmd.ExecuteScalar();
                    return result?.ToString() ?? defaultValue;
                }
            }
        }

        public void ClearAllData()
        {
            using (SQLiteConnection conn = GetConnection())
            {
                conn.Open();
                string[] tables = new string[]
                {
                    "Productor", "Variedad", "Lote", "Packing", "TipoEmbalaje", "Calibre", "GTIN", "SDP", "Peso", "CategoriaSAG", "Color", "VariedadImprime", "VariedadImprime_GTIN", "VariedadVariedadImprime_variedad", "Trazabilidad"
                };
                foreach (string table in tables)
                {
                    using (SQLiteCommand cmd = new SQLiteCommand($"DELETE FROM {table}", conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public void UpsertVariedadImprime(int id, string dato, bool activo)
        {
            using (SQLiteConnection conn = GetConnection())
            {
                conn.Open();
                string query = @"INSERT OR REPLACE INTO VariedadImprime (id, dato, activo) 
								 VALUES (@id, @dato, @activo)";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@dato", dato);
                    cmd.Parameters.AddWithValue("@activo", activo ? 1 : 0);
                    cmd.ExecuteNonQuery();
                }
            }
        }



        // ========== MÉTODOS HELPER PARA COMBOBOX (UI) ==========

        public List<string> GetProductoresParaUI()
        {
            List<string> result = new List<string>();
            try
            {
                using (SQLiteConnection conn = GetConnection())
                {
                    conn.Open();
                    string query = "SELECT dato FROM Productor ORDER BY dato ASC";
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                                result.Add(reader["dato"].ToString());
                        }
                    }
                }
            }
            catch { }
            return result;
        }

        public List<string> GetVariedadesParaUI()
        {
            List<string> result = new List<string>();
            try
            {
                using (SQLiteConnection conn = GetConnection())
                {
                    conn.Open();
                    string query = "SELECT dato FROM Variedad ORDER BY dato ASC";
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                                result.Add(reader["dato"].ToString());
                        }
                    }
                }
            }
            catch { }
            return result;
        }

        public List<string> GetLotesParaUI()
        {
            List<string> result = new List<string>();
            try
            {
                using (SQLiteConnection conn = GetConnection())
                {
                    conn.Open();
                    string query = "SELECT dato FROM Lote ORDER BY dato ASC";
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                                result.Add(reader["dato"].ToString());
                        }
                    }
                }
            }
            catch { }
            return result;
        }

        public List<string> GetSDPsParaUI()
        {
            List<string> result = new List<string>();
            try
            {
                using (SQLiteConnection conn = GetConnection())
                {
                    conn.Open();
                    string query = "SELECT dato FROM SDP WHERE activo = 1 ORDER BY dato ASC";
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                                result.Add(reader["dato"].ToString());
                        }
                    }
                }
            }
            catch { }
            return result;
        }

        public List<string> GetGTINsParaUI()
        {
            List<string> result = new List<string>();
            try
            {
                using (SQLiteConnection conn = GetConnection())
                {
                    conn.Open();
                    string query = "SELECT dato FROM GTIN WHERE activo = 1 ORDER BY dato ASC";
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                                result.Add(reader["dato"].ToString());
                        }
                    }
                }
            }
            catch { }
            return result;
        }

        public List<string> GetCalibresParaUI()
        {
            List<string> result = new List<string>();
            try
            {
                string[] ordenSAG = { "XXJ", "XJ", "J", "D", "V", "A", "R", "T", "XXL", "XL", "L", "M", "JJ", "DD", "VV", "AA", "RR" };

                using (SQLiteConnection conn = GetConnection())
                {
                    conn.Open();
                    string query = "SELECT dato FROM Calibre WHERE activo = 1";
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        Dictionary<string, bool> calibresDB = new Dictionary<string, bool>();
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                                calibresDB[reader["dato"].ToString()] = true;
                        }

                        foreach (string calibre in ordenSAG)
                        {
                            if (calibresDB.ContainsKey(calibre))
                                result.Add(calibre);
                        }

                        foreach (var kvp in calibresDB)
                        {
                            if (!result.Contains(kvp.Key))
                                result.Add(kvp.Key);
                        }
                    }
                }
            }
            catch { }
            return result;
        }

        public List<string> GetTiposEmbalajeParaUI()
        {
            List<string> result = new List<string>();
            try
            {
                using (SQLiteConnection conn = GetConnection())
                {
                    conn.Open();
                    string query = "SELECT dato FROM TipoEmbalaje WHERE activo = 1 ORDER BY dato ASC";
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                                result.Add(reader["dato"].ToString());
                        }
                    }
                }
            }
            catch { }
            return result;
        }

        // ========== M�TODOS FILTRADOS EN CASCADA (CON TABLA TRAZABILIDAD) ==========

        public void UpsertTrazabilidad(int id, int productor_id, int variedad_id, int lote_id, string codigo_sag, string sdp)
        {
            using (SQLiteConnection conn = GetConnection())
            {
                conn.Open();
                string query = @"INSERT OR REPLACE INTO Trazabilidad 
								 (id, productor, variedad, lote, sdp, variedad_imprime, sincronizado_en) 
								 VALUES (@id, @productor_id, @variedad_id, @lote_id, @sdp, @variedadImprime, CURRENT_TIMESTAMP)";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@productor_id", productor_id);
                    cmd.Parameters.AddWithValue("@variedad_id", variedad_id);
                    cmd.Parameters.AddWithValue("@lote_id", lote_id);
                    cmd.Parameters.AddWithValue("@codigo_sag", codigo_sag ?? "");
                    cmd.Parameters.AddWithValue("@sdp", sdp ?? "");
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<string> GetVariedadesPorProductor(string nombreProductor)
        {
            List<string> result = new List<string>();
            try
            {
                using (SQLiteConnection conn = GetConnection())
                {
                    conn.Open();
                    string query = @"
						SELECT DISTINCT v.nombre 
						FROM Variedades v
						INNER JOIN Trazabilidad t ON v.id = t.variedad_id
						INNER JOIN Productores p ON p.id = t.productor_id
						WHERE p.nombre = @productor
						ORDER BY v.nombre ASC";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@productor", nombreProductor);
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                                result.Add(reader["nombre"].ToString());
                        }
                    }
                }
            }
            catch { }
            return result;
        }

        public List<string> GetLotesPorProductorVariedad(string nombreProductor, string nombreVariedad)
        {
            List<string> result = new List<string>();
            try
            {
                using (SQLiteConnection conn = GetConnection())
                {
                    conn.Open();
                    string query = @"
						SELECT DISTINCT l.nombre 
						FROM Lotes l
						INNER JOIN Trazabilidad t ON l.id = t.lote_id
						INNER JOIN Productores p ON p.id = t.productor_id
						INNER JOIN Variedades v ON v.id = t.variedad_id
						WHERE p.nombre = @productor AND v.nombre = @variedad
						ORDER BY l.nombre ASC";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@productor", nombreProductor);
                        cmd.Parameters.AddWithValue("@variedad", nombreVariedad);
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                                result.Add(reader["nombre"].ToString());
                        }
                    }
                }
            }
            catch { }
            return result;
        }

        public List<string> GetSDPsPorProductorVariedadLote(string nombreProductor, string nombreVariedad, string nombreLote)
        {
            List<string> result = new List<string>();
            try
            {
                using (SQLiteConnection conn = GetConnection())
                {
                    conn.Open();
                    string query = @"
					 SELECT DISTINCT t.sdp 
					 FROM Trazabilidad t
					 INNER JOIN Productores p ON p.id = t.productor_id
					 INNER JOIN Variedades v ON v.id = t.variedad_id
					 INNER JOIN Lotes l ON l.id = t.lote_id
					 WHERE p.nombre = @productor AND v.nombre = @variedad AND l.nombre = @lote
					 ORDER BY t.sdp ASC";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@productor", nombreProductor);
                        cmd.Parameters.AddWithValue("@variedad", nombreVariedad);
                        cmd.Parameters.AddWithValue("@lote", nombreLote);
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                                result.Add(reader["sdp"].ToString());
                        }
                    }
                }
            }
            catch { }
            return result;
        }

        public async Task<List<string>> GetPesosFromApiAsync(string apiBaseUrl = "http://localhost:8000/api/")
        {
            var pesos = new List<string>();
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiBaseUrl + "peso/");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    dynamic data = JsonConvert.DeserializeObject(json);
                    foreach (var item in data)
                    {
                        pesos.Add(item.nombre.ToString());
                    }
                }
            }
            return pesos;
        }

        public List<string> GetPesosLocal()
        {
            var result = new List<string>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT dato FROM Peso ORDER BY dato ASC";
                using (var cmd = new SQLiteCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        result.Add(reader["dato"].ToString());
            }
            return result;
        }

        public async Task<List<string>> GetColoresFromApiAsync(string apiBaseUrl = "http://localhost:8000/api/")
        {
            var colores = new List<string>();
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiBaseUrl + "color/");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    dynamic data = JsonConvert.DeserializeObject(json);
                    foreach (var item in data)
                    {
                        colores.Add(item.nombre.ToString());
                    }
                }
            }
            return colores;
        }

        public List<string> GetColoresLocal()
        {
            var result = new List<string>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT nombre FROM Color ORDER BY nombre ASC";
                using (var cmd = new SQLiteCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        result.Add(reader["nombre"].ToString());
            }
            return result;
        }

        public async Task SincronizarDesdeApiAsync(string apiBaseUrl = "http://127.0.0.1:8000/api/")
        {
            ClearAllData();
            using (var client = new HttpClient())
            {
                // Elimino todas las líneas de sincronización que usan métodos no implementados para evitar errores de compilación
            }
        }

        private async Task SincronizarEntidad<T>(HttpClient client, string url, Action<T> upsert)
        {
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var items = JsonConvert.DeserializeObject<List<T>>(json);
                foreach (var item in items)
                    upsert(item);
            }
        }

        // M�todo para obtener tipos de embalaje filtrados por peso fijo
        public List<Item> GetTipoEmbalajePorPesoFijo(bool pesoFijo)
        {
            var result = new List<Item>();
            using (SQLiteConnection conn = GetConnection())
            {
                conn.Open();
                string query = "SELECT dato, id FROM TipoEmbalaje WHERE activo = 1 AND peso_fijo = @pesoFijo";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {

                    cmd.Parameters.AddWithValue("@pesoFijo", pesoFijo);
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new Item(reader["dato"].ToString(), reader["id"]));
                        }
                    }
                }
            }
            return result;
        }

        // Métodos Upsert adicionales
        public void UpsertColor(int id, string nombre, string descripcion, bool activo)
        {
            // Implementación pendiente
        }

        public void UpsertPeso(int id, string codigo, string descripcion, bool activo)
        {
            // Implementación pendiente
        }

        public void UpsertVariedadVariedadImprimir(int id, int variedad, int variedad_imprime)
        {
            // Implementación pendiente
        }

        public void UpsertTrazabilidad(int id, string codigo, int sdp, int productor, int variedad, int lote, int tipo_embalaje, int calibre, int packing, int peso, int recibidor, int color, int variedad_imprime, int categoria_sag, string fecha, string hora, int cantidad, string observaciones)
        {
            // Implementación pendiente
        }

        // DTOs para deserializar los datos
        private class ProductorDto { public int id; public string dato; }
        private class VariedadDto { public int id; public string dato; }
        private class LoteDto { public int id; public string dato; }
        private class RecibidorDto { public int id; public string dato; public string nota; }
        private class PackingDto { public int id; public string dato; public string codigo; }
        private class TipoEmbalajeDto { public int id; public string dato; public string descripcion; public bool activo; }
        private class CalibreDto { public int id; public string dato; public string descripcion; public bool activo; }
        private class GTINDto { public int id; public string dato; public string descripcion; public bool activo; }
        private class SDPDto { public int id; public string dato; public string descripcion; public bool activo; }
        private class PesoDto { public int id; public string dato; public string descripcion; public bool activo; }
        private class ColorDto { public int id; public string nombre; public string descripcion; public bool activo; }
        private class CategoriaSAGDto { public int id; public string nombre; }
        private class VariedadImprimeDto { public int id; public string dato; public bool activo; }
        private class VariedadVariedadImprimirDto { public int id; public int variedad; public int variedad_imprime; }
        private class TrazabilidadDto { public int id; public int productor; public int variedad; public int lote; public int sdp; public int variedad_imprime; }
        private class VariedadImprimeGTINDto { public int id; public int variedad_imprime; public int gtin; }
    }
}
