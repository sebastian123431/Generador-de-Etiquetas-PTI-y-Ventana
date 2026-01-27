using System;
using System.Collections.Generic;
using System.IO;
using System.Data.SQLite;

namespace WindowsFormsApplication1.Data
{
    public class DatabaseManager
    {
        private static DatabaseManager _instance;
        private readonly string _connectionString;
        private bool _dbAvailable = false;

        private DatabaseManager()
        {
            _connectionString = "Data Source=etiquetas.db;Version=3;";
            _dbAvailable = true;
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

        public SQLiteConnection GetConnection()
        {
            if (!_dbAvailable)
            {
                throw new InvalidOperationException("La base de datos no está disponible.");
            }
            return new SQLiteConnection(_connectionString);
        }

        public void InitializeDatabase()
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string[] createTables = {
                    @"CREATE TABLE IF NOT EXISTS TipoEmbalaje (
                        id INTEGER PRIMARY KEY,
                        dato TEXT,
                        descripcion TEXT,
                        activo INTEGER,
                        peso_fijo INTEGER
                    )",
                    @"CREATE TABLE IF NOT EXISTS Productor (
                        id INTEGER PRIMARY KEY,
                        dato TEXT,
                        activo INTEGER
                    )",
                    @"CREATE TABLE IF NOT EXISTS Variedad (
                        id INTEGER PRIMARY KEY,
                        dato TEXT,
                        activo INTEGER
                    )",
                    @"CREATE TABLE IF NOT EXISTS Lote (
                        id INTEGER PRIMARY KEY,
                        dato TEXT,
                        activo INTEGER
                    )",
                    @"CREATE TABLE IF NOT EXISTS VariedadImprime (
                        id INTEGER PRIMARY KEY,
                        dato TEXT,
                        activo INTEGER
                    )",
                    @"CREATE TABLE IF NOT EXISTS GTIN (
                        id INTEGER PRIMARY KEY,
                        dato TEXT,
                        descripcion TEXT,
                        activo INTEGER
                    )",
                    @"CREATE TABLE IF NOT EXISTS SDP (
                        id INTEGER PRIMARY KEY,
                        dato TEXT,
                        descripcion TEXT,
                        activo INTEGER
                    )",
                    @"CREATE TABLE IF NOT EXISTS ConfiguracionSync (
                        Clave TEXT PRIMARY KEY,
                        Valor TEXT
                    )"
                };
                foreach (var sql in createTables)
                {
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        // Métodos SQLite para CRUD y helpers
        public void UpsertTipoEmbalaje(int id, string dato, bool activo, bool pesoFijo)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = @"INSERT OR REPLACE INTO TipoEmbalaje (id, dato, activo, peso_fijo) VALUES (@id, @dato, @activo, @peso_fijo)";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@dato", dato);
                    cmd.Parameters.AddWithValue("@activo", activo ? 1 : 0);
                    cmd.Parameters.AddWithValue("@peso_fijo", pesoFijo ? 1 : 0);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Item> GetTipoEmbalajePorPesoFijo(bool pesoFijo)
        {
            var result = new List<Item>();
            using (var conn = GetConnection())
            {
                conn.Open();
                // Asegurar que la tabla existe
                string createTable = @"CREATE TABLE IF NOT EXISTS TipoEmbalaje (
                    id INTEGER PRIMARY KEY,
                    dato TEXT,
                    descripcion TEXT,
                    activo INTEGER,
                    peso_fijo INTEGER
                )";
                using (var cmdCreate = new SQLiteCommand(createTable, conn))
                {
                    cmdCreate.ExecuteNonQuery();
                }
                string query = "SELECT id, dato FROM TipoEmbalaje WHERE activo = 1 AND peso_fijo = @peso_fijo";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@peso_fijo", pesoFijo ? 1 : 0);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new Item(reader.GetString(1), reader.GetInt32(0)));
                        }
                    }
                }
            }
            return result;
        }

        // Métodos SQLite para Productor
        public void UpsertProductor(int id, string dato, bool activo)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = @"INSERT OR REPLACE INTO Productor (id, dato, activo) VALUES (@id, @dato, @activo)";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@dato", dato);
                    cmd.Parameters.AddWithValue("@activo", activo ? 1 : 0);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<string> GetProductores()
        {
            var result = new List<string>();
            using (var conn = GetConnection())
            {
                conn.Open();
                // Asegurar que la tabla existe
                string createTable = @"CREATE TABLE IF NOT EXISTS Productor (
                    id INTEGER PRIMARY KEY,
                    dato TEXT,
                    activo INTEGER
                )";
                using (var cmdCreate = new SQLiteCommand(createTable, conn))
                {
                    cmdCreate.ExecuteNonQuery();
                }
                string query = "SELECT dato FROM Productor WHERE activo = 1";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(reader.GetString(0));
                        }
                    }
                }
            }
            return result;
        }

        // Métodos SQLite para Variedad
        public void UpsertVariedad(int id, string dato, bool activo)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = @"INSERT OR REPLACE INTO Variedad (id, dato, activo) VALUES (@id, @data, @active)";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@data", dato);
                    cmd.Parameters.AddWithValue("@active", activo ? 1 : 0);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<string> GetVariedades()
        {
            var result = new List<string>();
            using (var conn = GetConnection())
            {
                conn.Open();
                // Asegurar que la tabla existe
                string createTable = @"CREATE TABLE IF NOT EXISTS Variedad (
                    id INTEGER PRIMARY KEY,
                    dato TEXT,
                    activo INTEGER
                )";
                using (var cmdCreate = new SQLiteCommand(createTable, conn))
                {
                    cmdCreate.ExecuteNonQuery();
                }
                string query = "SELECT dato FROM Variedad WHERE activo = 1";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(reader.GetString(0));
                        }
                    }
                }
            }
            return result;
        }

        // Métodos SQLite para Lote
        public void UpsertLote(int id, string dato, bool activo)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = @"INSERT OR REPLACE INTO Lote (id, dato, activo) VALUES (@id, @dato, @activo)";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@dato", dato);
                    cmd.Parameters.AddWithValue("@activo", activo ? 1 : 0);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<string> GetLotes()
        {
            var result = new List<string>();
            using (var conn = GetConnection())
            {
                conn.Open();
                // Asegurar que la tabla existe
                string createTable = @"CREATE TABLE IF NOT EXISTS Lote (
                    id INTEGER PRIMARY KEY,
                    dato TEXT,
                    activo INTEGER
                )";
                using (var cmdCreate = new SQLiteCommand(createTable, conn))
                {
                    cmdCreate.ExecuteNonQuery();
                }
                string query = "SELECT dato FROM Lote WHERE activo = 1";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(reader.GetString(0));
                        }
                    }
                }
            }
            return result;
        }

        // Métodos SQLite para VariedadImprime
        public void UpsertVariedadImprime(int id, string dato, bool activo)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = @"INSERT OR REPLACE INTO VariedadImprime (id, dato, activo) VALUES (@id, @dato, @activo)";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@dato", dato);
                    cmd.Parameters.AddWithValue("@activo", activo ? 1 : 0);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<string> GetVariedadesImprime()
        {
            var result = new List<string>();
            using (var conn = GetConnection())
            {
                conn.Open();
                // Asegurar que la tabla existe
                string createTable = @"CREATE TABLE IF NOT EXISTS VariedadImprime (
                    id INTEGER PRIMARY KEY,
                    dato TEXT,
                    activo INTEGER
                )";
                using (var cmdCreate = new SQLiteCommand(createTable, conn))
                {
                    cmdCreate.ExecuteNonQuery();
                }
                string query = "SELECT dato FROM VariedadImprime WHERE activo = 1";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(reader.GetString(0));
                        }
                    }
                }
            }
            return result;
        }

        // Métodos SQLite para GTIN
        public void UpsertGTIN(int id, string dato, string descripcion, bool activo)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = @"INSERT OR REPLACE INTO GTIN (id, dato, descripcion, activo) VALUES (@id, @dato, @descripcion, @activo)";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@dato", dato);
                    cmd.Parameters.AddWithValue("@descripcion", descripcion);
                    cmd.Parameters.AddWithValue("@activo", activo ? 1 : 0);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<string> GetGTINs()
        {
            var result = new List<string>();
            using (var conn = GetConnection())
            {
                conn.Open();
                // Asegurar que la tabla existe
                string createTable = @"CREATE TABLE IF NOT EXISTS GTIN (
                    id INTEGER PRIMARY KEY,
                    dato TEXT,
                    descripcion TEXT,
                    activo INTEGER
                )";
                using (var cmdCreate = new SQLiteCommand(createTable, conn))
                {
                    cmdCreate.ExecuteNonQuery();
                }
                string query = "SELECT dato FROM GTIN WHERE activo = 1";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(reader.GetString(0));
                        }
                    }
                }
            }
            return result;
        }

        // Métodos SQLite para SDP
        public void UpsertSDP(int id, string dato, string descripcion, bool activo)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = @"INSERT OR REPLACE INTO SDP (id, dato, descripcion, activo) VALUES (@id, @dato, @descripcion, @activo)";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@dato", dato);
                    cmd.Parameters.AddWithValue("@descripcion", descripcion);
                    cmd.Parameters.AddWithValue("@activo", activo ? 1 : 0);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<string> GetSDPs()
        {
            var result = new List<string>();
            using (var conn = GetConnection())
            {
                conn.Open();
                // Asegurar que la tabla existe
                string createTable = @"CREATE TABLE IF NOT EXISTS SDP (
                    id INTEGER PRIMARY KEY,
                    dato TEXT,
                    descripcion TEXT,
                    activo INTEGER
                )";
                using (var cmdCreate = new SQLiteCommand(createTable, conn))
                {
                    cmdCreate.ExecuteNonQuery();
                }
                string query = "SELECT dato FROM SDP WHERE activo = 1";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(reader.GetString(0));
                        }
                    }
                }
            }
            return result;
        }

        // Métodos de configuración con SQLite
        public void SetConfigValue(string clave, string valor)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                // Asegurar que la tabla existe
                string createTable = @"CREATE TABLE IF NOT EXISTS ConfiguracionSync (
                    Clave TEXT PRIMARY KEY,
                    Valor TEXT
                )";
                using (var cmdCreate = new SQLiteCommand(createTable, conn))
                {
                    cmdCreate.ExecuteNonQuery();
                }
                string query = @"INSERT OR REPLACE INTO ConfiguracionSync (Clave, Valor) VALUES (@clave, @valor)";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@clave", clave);
                    cmd.Parameters.AddWithValue("@valor", valor);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public string GetConfigValue(string clave, string defaultValue = "")
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                // Asegurar que la tabla existe
                string createTable = @"CREATE TABLE IF NOT EXISTS ConfiguracionSync (
                    Clave TEXT PRIMARY KEY,
                    Valor TEXT
                )";
                using (var cmdCreate = new SQLiteCommand(createTable, conn))
                {
                    cmdCreate.ExecuteNonQuery();
                }
                string query = "SELECT Valor FROM ConfiguracionSync WHERE Clave = @clave";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@clave", clave);
                    var result = cmd.ExecuteScalar();
                    return result?.ToString() ?? defaultValue;
                }
            }
        }

        public void ClearAllData()
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string[] tables = { "Productor", "Variedad", "Lote", "Packing", "TipoEmbalaje", "Calibre", "GTIN", "SDP", "Peso", "CategoriaSAG", "Color", "VariedadImprime", "VariedadImprime_GTIN", "VariedadVariedadImprime_variedad", "Trazabilidad", "ConfiguracionSync" };
                foreach (var table in tables)
                {
                    using (var cmd = new SQLiteCommand($"DROP TABLE IF EXISTS {table}", conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public class TipoEmbalajeDto
        {
            public int id { get; set; }
            public string dato { get; set; }
            public string descripcion { get; set; }
            public bool activo { get; set; }
            public bool peso_fijo { get; set; }
        }

        public class Item
        {
            public string Dato { get; set; }
            public int Id { get; set; }
            public Item(string dato, int id)
            {
                Dato = dato;
                Id = id;
            }
            public override string ToString() => Dato;
        }

        public class ProductorDto
        {
            public int id { get; set; }
            public string dato { get; set; }
            public bool activo { get; set; }
        }

        public class VariedadDto
        {
            public int id { get; set; }
            public string dato { get; set; }
            public bool activo { get; set; }
        }

        public class LoteDto
        {
            public int id { get; set; }
            public string dato { get; set; }
            public bool activo { get; set; }
        }

        public class VariedadImprimeDto
        {
            public int id { get; set; }
            public string dato { get; set; }
            public bool activo { get; set; }
        }

        public class GTINDto
        {
            public int id { get; set; }
            public string dato { get; set; }
            public string descripcion { get; set; }
            public bool activo { get; set; }
        }

        public class SDPDto
        {
            public int id { get; set; }
            public string dato { get; set; }
            public string descripcion { get; set; }
            public bool activo { get; set; }
        }

        public class ConfiguracionSync
        {
            public string Clave { get; set; }
            public string Valor { get; set; }
        }
    }
}
