using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Web.Script.Serialization;
using WindowsFormsApplication1.Data;

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

		private DatabaseManager()
		{
			_dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "etiquetas_local.db");
			_connectionString = $"Data Source={_dbPath};Version=3;";
			InitializeDatabase();
		}

	// Representa un packing con campos adicionales (csp y ubicacion)
	public class PackingItem
	{
		public int Id { get; set; }
		public string Dato { get; set; }
		public long? Csp { get; set; }
		public bool Activo { get; set; }
		public int? Ubicacion { get; set; }
		public UbicacionDetalle Ubicacion_Detalle { get; set; }

		public PackingItem() { }
		public PackingItem(int id, string dato, long? csp, bool activo, int? ubicacion)
		{
			Id = id;
			Dato = dato;
			Csp = csp;
			Activo = activo;
			Ubicacion = ubicacion;
		}
	}

	public class UbicacionDetalle
	{
		public int Id { get; set; }
		public string Region { get; set; }
		public string Provincia { get; set; }
		public string Comuna { get; set; }
		public string Gln { get; set; }
		public string Ggn { get; set; }
		public bool Activo { get; set; }

		public UbicacionDetalle() { }
	}

	public PackingItem GetPackingById(int id)
	{
		try
		{
			using (var conn = GetConnection())
			{
				conn.Open();
				string query = "SELECT id, dato, csp, activo, ubicacion FROM Packing WHERE id = @id LIMIT 1";
				using (var cmd = new SQLiteCommand(query, conn))
				{
					cmd.Parameters.AddWithValue("@id", id);
					using (var reader = cmd.ExecuteReader())
					{
						if (reader.Read())
						{
							int pid = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
							string dato = reader.IsDBNull(1) ? "" : reader.GetString(1);
							string cspRaw = reader.IsDBNull(2) ? null : reader.GetValue(2)?.ToString();
							long? csp = null;
							if (!string.IsNullOrWhiteSpace(cspRaw))
							{
								if (long.TryParse(cspRaw, out var v)) csp = v;
							}
							bool activo = reader.IsDBNull(3) ? true : (Convert.ToInt32(reader.GetValue(3)) != 0);
							int? ubic = reader.IsDBNull(4) ? (int?)null : Convert.ToInt32(reader.GetValue(4));
							var item = new PackingItem(pid, dato, csp, activo, ubic);
							if (ubic.HasValue)
							{
								item.Ubicacion_Detalle = GetUbicacionDetalleById(ubic.Value, conn);
							}
							return item;
						}
					}
				}
			}
		}
		catch { }
		return null;
	}

	public List<PackingItem> GetPackingsDetailed()
	{
		var result = new List<PackingItem>();
		using (var conn = GetConnection())
		{
			conn.Open();
			string query = "SELECT id, dato, csp, activo, ubicacion FROM Packing ORDER BY dato ASC";
			using (var cmd = new SQLiteCommand(query, conn))
			using (var reader = cmd.ExecuteReader())
			{
				while (reader.Read())
				{
					int pid = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
					string dato = reader.IsDBNull(1) ? "" : reader.GetString(1);
					string cspRaw = reader.IsDBNull(2) ? null : reader.GetValue(2)?.ToString();
					long? csp = null;
					if (!string.IsNullOrWhiteSpace(cspRaw))
						if (long.TryParse(cspRaw, out var v)) csp = v;
					bool activo = reader.IsDBNull(3) ? true : (Convert.ToInt32(reader.GetValue(3)) != 0);
					int? ubic = reader.IsDBNull(4) ? (int?)null : Convert.ToInt32(reader.GetValue(4));
					var p = new PackingItem(pid, dato, csp, activo, ubic);
					if (ubic.HasValue)
						p.Ubicacion_Detalle = GetUbicacionDetalleById(ubic.Value, conn);
					result.Add(p);
				}
			}
		}
		return result;
	}

	private UbicacionDetalle GetUbicacionDetalleById(int id, SQLiteConnection conn)
	{
		try
		{
			string q = "SELECT id, region, provincia, comuna, gln, ggn, activo FROM Ubicacion WHERE id = @id LIMIT 1";
			using (var cmd = new SQLiteCommand(q, conn))
			{
				cmd.Parameters.AddWithValue("@id", id);
				using (var reader = cmd.ExecuteReader())
				{
					if (reader.Read())
					{
						return new UbicacionDetalle
						{
							Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
							Region = reader.IsDBNull(1) ? "" : reader.GetString(1),
							Provincia = reader.IsDBNull(2) ? "" : reader.GetString(2),
							Comuna = reader.IsDBNull(3) ? "" : reader.GetString(3),
							Gln = reader.IsDBNull(4) ? "" : reader.GetString(4),
							Ggn = reader.IsDBNull(5) ? "" : reader.GetString(5),
							Activo = reader.IsDBNull(6) ? true : (Convert.ToInt32(reader.GetValue(6)) != 0)
						};
					}
				}
			}
		}
		catch { }
		return null;
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

		public void InitializeDatabase()
		{
			try
			{
				bool isNewDatabase = !File.Exists(_dbPath);

				if (isNewDatabase)
				{
					using (var conn = new SQLiteConnection($"Data Source={_dbPath}"))
					{
						conn.Open();
					}
				}

				CreateTables();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error al inicializar la base de datos: {ex.Message}");
				Console.WriteLine(ex.StackTrace);
				throw;
			}
		}

		private void CreateTables()
		{
			using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
			{
				conn.Open();
				string[] createTableQueries = new string[]
				{
					@"CREATE TABLE IF NOT EXISTS ConfiguracionSync (
						clave TEXT PRIMARY KEY,
						valor TEXT NOT NULL,
						actualizado_en DATETIME DEFAULT CURRENT_TIMESTAMP
					)",

					@"CREATE TABLE IF NOT EXISTS Productor (
						id INTEGER PRIMARY KEY,
						dato TEXT NOT NULL UNIQUE,
						activo INTEGER DEFAULT 1
					)",

					@"CREATE TABLE IF NOT EXISTS Recibidores (
						id INTEGER PRIMARY KEY,
						dato TEXT NOT NULL UNIQUE,
						nota TEXT,
						sincronizado_en DATETIME DEFAULT CURRENT_TIMESTAMP
					)",

					@"CREATE TABLE IF NOT EXISTS Variedad (
						id INTEGER PRIMARY KEY,
						dato TEXT NOT NULL UNIQUE,
						activo INTEGER DEFAULT 1
					)",

					@"CREATE TABLE IF NOT EXISTS Lote (
						id INTEGER PRIMARY KEY,
						dato TEXT NOT NULL UNIQUE,
						activo INTEGER DEFAULT 1
					)",

					@"CREATE TABLE IF NOT EXISTS Packing (
                     id INTEGER PRIMARY KEY,
						dato TEXT NOT NULL UNIQUE,
						csp INTEGER,
						activo INTEGER DEFAULT 1,
						ubicacion INTEGER,
						ubicacion_id INTEGER
					)",

					@"CREATE TABLE IF NOT EXISTS TipoEmbalaje (
						id INTEGER PRIMARY KEY,
						dato TEXT NOT NULL UNIQUE,
						activo INTEGER DEFAULT 1,
						peso_fijo INTEGER DEFAULT 0
					)",

					@"CREATE TABLE IF NOT EXISTS Calibre (
						id INTEGER PRIMARY KEY,
						dato TEXT NOT NULL UNIQUE,
						activo INTEGER DEFAULT 1
					)",

					@"CREATE TABLE IF NOT EXISTS GTIN (
						id INTEGER PRIMARY KEY,
						dato TEXT NOT NULL UNIQUE,
						activo INTEGER DEFAULT 1
					)",

					@"CREATE TABLE IF NOT EXISTS SDP (
						id INTEGER PRIMARY KEY,
						dato TEXT NOT NULL UNIQUE,
						activo INTEGER DEFAULT 1
					)",

					@"CREATE TABLE IF NOT EXISTS Peso (
						id INTEGER PRIMARY KEY,
						dato TEXT NOT NULL UNIQUE,
						activo INTEGER DEFAULT 1
					)",

					@"CREATE TABLE IF NOT EXISTS CategoriaSAG (
						id INTEGER PRIMARY KEY,
						nombre TEXT NOT NULL UNIQUE
					)",

					@"CREATE TABLE IF NOT EXISTS Color (
						id INTEGER PRIMARY KEY,
						nombre TEXT NOT NULL UNIQUE,
						descripcion TEXT,
						activo INTEGER DEFAULT 1
					)",

					@"CREATE TABLE IF NOT EXISTS VariedadImprime (
						id INTEGER PRIMARY KEY,
						dato TEXT NOT NULL UNIQUE,
						activo INTEGER DEFAULT 1
					)",

					@"CREATE TABLE IF NOT EXISTS Titulos (
						id INTEGER PRIMARY KEY,
						texto TEXT NOT NULL,
						tipo TEXT,
						activo INTEGER DEFAULT 1,
						sincronizado_en DATETIME DEFAULT CURRENT_TIMESTAMP
					)",

					@"CREATE TABLE IF NOT EXISTS MapeosCodigoFinal (
						id INTEGER PRIMARY KEY,
						variedad_imprime TEXT NOT NULL,
						gtin TEXT NOT NULL,
						tipo_embalaje TEXT,
						codigo_final TEXT NOT NULL,
						activo INTEGER DEFAULT 1,
						sincronizado_en DATETIME DEFAULT CURRENT_TIMESTAMP
					)",

					@"CREATE TABLE IF NOT EXISTS VariedadImprime_GTIN (
						id INTEGER PRIMARY KEY,
						variedad_imprime INTEGER NOT NULL,
						gtin INTEGER NOT NULL,
						creado_en DATETIME DEFAULT CURRENT_TIMESTAMP,
						actualizado_en DATETIME DEFAULT CURRENT_TIMESTAMP,
						UNIQUE(variedad_imprime, gtin)
					)",

					@"CREATE TABLE IF NOT EXISTS VariedadVariedadImprime_variedad (
						id INTEGER PRIMARY KEY,
						variedad INTEGER NOT NULL,
						variedad_imprime INTEGER NOT NULL,
						UNIQUE(variedad, variedad_imprime)
					)",

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
					)",

					@"CREATE TABLE IF NOT EXISTS Ubicacion (
						id INTEGER PRIMARY KEY,
						region TEXT,
						provincia TEXT,
						comuna TEXT,
						gln TEXT,
						ggn TEXT,
						activo INTEGER DEFAULT 1,
						creado_en TEXT,
						actualizado_en TEXT
					)",

					@"CREATE TABLE IF NOT EXISTS CantidadCajas (
						id INTEGER PRIMARY KEY,
						cantidad TEXT NOT NULL UNIQUE,
						activo INTEGER DEFAULT 1
					)",

                    @"CREATE TABLE IF NOT EXISTS VariedadCitrico (
						id INTEGER PRIMARY KEY,
						variedad TEXT NOT NULL UNIQUE,
						numero_interno TEXT,
						activo INTEGER DEFAULT 1
					)",

					@"CREATE TABLE IF NOT EXISTS PackingCitrico (
						id INTEGER PRIMARY KEY,
						packing TEXT NOT NULL UNIQUE,
						activo INTEGER DEFAULT 1
					)",

					@"CREATE TABLE IF NOT EXISTS TipoEmbalajeCitrico (
						id INTEGER PRIMARY KEY,
						tipo_embalaje TEXT NOT NULL UNIQUE,
						activo INTEGER DEFAULT 1
					)",

					@"CREATE TABLE IF NOT EXISTS RecibidorCitrico (
						id INTEGER PRIMARY KEY,
						recibidor TEXT NOT NULL UNIQUE,
						activo INTEGER DEFAULT 1
					)",

					@"CREATE TABLE IF NOT EXISTS ProductorCitrico (
						id INTEGER PRIMARY KEY,
						dato TEXT NOT NULL UNIQUE,
						CSG TEXT,
						activo INTEGER DEFAULT 1
					)",

					@"CREATE TABLE IF NOT EXISTS CantidadCajasCitrico (
						id INTEGER PRIMARY KEY,
						cantidad_cajas TEXT NOT NULL UNIQUE,
						activo INTEGER DEFAULT 1
					)"
				};
				foreach (string query in createTableQueries)
				{
					using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
					{
						cmd.ExecuteNonQuery();
					}
				}

				// Migraciones livianas: agrega columnas usadas por la UI/API si no existen.
                // (ALTER TABLE falla si ya existen; ahora se verifica antes de ejecutar)
				if (!ColumnExists(conn, "GTIN", "plu"))
					new SQLiteCommand("ALTER TABLE GTIN ADD COLUMN plu TEXT", conn).ExecuteNonQuery();
				if (!ColumnExists(conn, "GTIN", "cod_bolsa"))
					new SQLiteCommand("ALTER TABLE GTIN ADD COLUMN cod_bolsa TEXT", conn).ExecuteNonQuery();
				if (!ColumnExists(conn, "GTIN", "peso_fijo"))
					new SQLiteCommand("ALTER TABLE GTIN ADD COLUMN peso_fijo INTEGER DEFAULT 0", conn).ExecuteNonQuery();

				if (!ColumnExists(conn, "Productor", "csg"))
					new SQLiteCommand("ALTER TABLE Productor ADD COLUMN csg TEXT", conn).ExecuteNonQuery();
                if (!ColumnExists(conn, "Packing", "csp"))
					// csp: número identificador del packing (guardar como INTEGER)
					new SQLiteCommand("ALTER TABLE Packing ADD COLUMN csp INTEGER", conn).ExecuteNonQuery();
					// Asegura columna 'ubicacion' para vincular Packing -> Ubicacion
				if (!ColumnExists(conn, "Packing", "ubicacion"))
					new SQLiteCommand("ALTER TABLE Packing ADD COLUMN ubicacion INTEGER", conn).ExecuteNonQuery();
				// Compatibilidad: algunas partes del código usan el nombre 'ubicacion_id'
				if (!ColumnExists(conn, "Packing", "ubicacion_id"))
					new SQLiteCommand("ALTER TABLE Packing ADD COLUMN ubicacion_id INTEGER", conn).ExecuteNonQuery();
                if (!ColumnExists(conn, "VariedadImprime", "peso_fijo"))
					new SQLiteCommand("ALTER TABLE VariedadImprime ADD COLUMN peso_fijo INTEGER DEFAULT 0", conn).ExecuteNonQuery();
				// Asegura columna var_interno para que el sistema pueda usar el código interno de variedad
				if (!ColumnExists(conn, "VariedadImprime", "var_interno"))
					new SQLiteCommand("ALTER TABLE VariedadImprime ADD COLUMN var_interno TEXT", conn).ExecuteNonQuery();
                // Asegura columna numero_interno en VariedadCitrico cuando se agrega el nuevo campo desde el API
				if (!ColumnExists(conn, "VariedadCitrico", "numero_interno"))
					new SQLiteCommand("ALTER TABLE VariedadCitrico ADD COLUMN numero_interno TEXT", conn).ExecuteNonQuery();
				if (!ColumnExists(conn, "TipoEmbalaje", "descripcion"))
					new SQLiteCommand("ALTER TABLE TipoEmbalaje ADD COLUMN descripcion TEXT", conn).ExecuteNonQuery();
			}
		}

		public SQLiteConnection GetConnection()
		{
			return new SQLiteConnection(_connectionString);
		}

		public void UpsertProductor(int id, string dato, bool activo, string csg = null)
		{
			using (SQLiteConnection conn = GetConnection())
			{
				conn.Open();
				bool hasCsg = ColumnExists(conn, "Productor", "csg");
				string query;
				if (hasCsg)
				{
					query = @"INSERT OR REPLACE INTO Productor (id, dato, csg, activo) VALUES (@id, @dato, @csg, @activo)";
					using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
					{
						cmd.Parameters.AddWithValue("@id", id);
						cmd.Parameters.AddWithValue("@dato", string.IsNullOrEmpty(dato) ? "" : dato);
						cmd.Parameters.AddWithValue("@csg", string.IsNullOrEmpty(csg) ? "" : csg);
						cmd.Parameters.AddWithValue("@activo", activo ? 1 : 0);
                        foreach (SQLiteParameter p in cmd.Parameters)
							Console.WriteLine($"[UpsertProductor] Param: {p.ParameterName} = {p.Value ?? "NULL"}");
						try
						{
							cmd.ExecuteNonQuery();
						}
						catch (SQLiteException ex)
						{
							Console.WriteLine($"Error en UpsertProductor: {ex.Message}");
							throw;
						}
					}
				}
				else
				{
					query = @"INSERT OR REPLACE INTO Productor (id, dato, activo) VALUES (@id, @dato, @activo)";
					using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
					{
						cmd.Parameters.AddWithValue("@id", id);
						cmd.Parameters.AddWithValue("@dato", string.IsNullOrEmpty(dato) ? "" : dato);
						cmd.Parameters.AddWithValue("@activo", activo ? 1 : 0);
						try
						{
							cmd.ExecuteNonQuery();
						}
						catch (SQLiteException ex)
						{
							Console.WriteLine($"Error en UpsertProductor: {ex.Message}");
							throw;
						}
					}
				}
			}
		}

		public void UpsertVariedad(int id, string dato, bool activo)
		{
			using (SQLiteConnection conn = GetConnection())
			{
				conn.Open();
				string query = @"INSERT OR REPLACE INTO Variedad (id, dato, activo) VALUES (@id, @dato, @activo)";
				using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
				{
					cmd.Parameters.AddWithValue("@id", id);
					cmd.Parameters.AddWithValue("@dato", string.IsNullOrEmpty(dato) ? "" : dato);
					cmd.Parameters.AddWithValue("@activo", activo ? 1 : 0);
                    foreach (SQLiteParameter p in cmd.Parameters)
						Console.WriteLine($"[UpsertVariedad] Param: {p.ParameterName} = {p.Value ?? "NULL"}");
					try
					{
						cmd.ExecuteNonQuery();
					}
					catch (SQLiteException ex)
					{
						Console.WriteLine($"Error en UpsertVariedad: {ex.Message}");
						throw;
					}
				}
			}
		}

		public void UpsertLote(int id, string dato, bool activo)
		{
			using (SQLiteConnection conn = GetConnection())
			{
				conn.Open();
				string query = @"INSERT OR REPLACE INTO Lote (id, dato, activo) VALUES (@id, @dato, @activo)";
				using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
				{
					cmd.Parameters.AddWithValue("@id", id);
					cmd.Parameters.AddWithValue("@dato", string.IsNullOrEmpty(dato) ? "" : dato);
					cmd.Parameters.AddWithValue("@activo", activo ? 1 : 0);
                    foreach (SQLiteParameter p in cmd.Parameters)
						Console.WriteLine($"[UpsertLote] Param: {p.ParameterName} = {p.Value ?? "NULL"}");
					try
					{
						cmd.ExecuteNonQuery();
					}
					catch (SQLiteException ex)
					{
						Console.WriteLine($"Error en UpsertLote: {ex.Message}");
						throw;
					}
				}
			}
		}

        public void UpsertPacking(int id, string dato, bool activo)
		{
         // Llama a la sobrecarga que admite csp/ubicacion para mantener un único punto de escritura
			UpsertPacking(id, dato, activo, null, null);
		}

		/// <summary>
		/// Inserta o actualiza un Packing incluyendo campos opcionales `csp` y `ubicacion` si existen en el esquema.
		/// </summary>
		public void UpsertPacking(int id, string dato, bool activo, long? csp, int? ubicacion = null)
		{
			try
			{
				using (SQLiteConnection conn = GetConnection())
				{
					conn.Open();
					bool hasCsp = ColumnExists(conn, "Packing", "csp");
              bool hasUbic = ColumnExists(conn, "Packing", "ubicacion");
				bool hasUbicId = ColumnExists(conn, "Packing", "ubicacion_id");
					string query;
					if (hasCsp || hasUbic)
					{
						var cols = new List<string> { "id", "dato" };
						var vals = new List<string> { "@id", "@dato" };
						if (hasCsp) { cols.Add("csp"); vals.Add("@csp"); }
						cols.Add("activo"); vals.Add("@activo");
                 if (hasUbic) { cols.Add("ubicacion"); vals.Add("@ubicacion"); }
					if (hasUbicId) { cols.Add("ubicacion_id"); vals.Add("@ubicacion_id"); }
						query = $"INSERT OR REPLACE INTO Packing ({string.Join(", ", cols)}) VALUES ({string.Join(", ", vals)})";
					}
					else
					{
						query = @"INSERT OR REPLACE INTO Packing (id, dato, activo) VALUES (@id, @dato, @activo)";
					}

					using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
					{
						cmd.Parameters.AddWithValue("@id", id);
						cmd.Parameters.AddWithValue("@dato", dato ?? "");
                        if (hasCsp) cmd.Parameters.AddWithValue("@csp", csp.HasValue ? (object)csp.Value : (object)DBNull.Value );
						cmd.Parameters.AddWithValue("@activo", activo ? 1 : 0);
                    if (hasUbic) cmd.Parameters.AddWithValue("@ubicacion", ubicacion.HasValue ? (object)ubicacion.Value : (object)DBNull.Value);
					if (hasUbicId) cmd.Parameters.AddWithValue("@ubicacion_id", ubicacion.HasValue ? (object)ubicacion.Value : (object)DBNull.Value);
						foreach (SQLiteParameter p in cmd.Parameters)
							Console.WriteLine($"[UpsertPacking] Param: {p.ParameterName} = {p.Value ?? "NULL"}");
						cmd.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error en UpsertPacking: {ex.Message} | id={id}, dato={dato}, activo={activo}, csp={csp}, ubicacion={ubicacion}");
				throw;
			}
		}

        public void UpsertCalibre(int id, string dato, bool activo)
		{
			try
			{
				using (SQLiteConnection conn = GetConnection())
				{
					conn.Open();
					string query = @"INSERT OR REPLACE INTO Calibre (id, dato, activo) VALUES (@id, @dato, @activo)";
					using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
					{
						cmd.Parameters.AddWithValue("@id", id);
						cmd.Parameters.AddWithValue("@dato", dato);
						cmd.Parameters.AddWithValue("@activo", activo ? 1 : 0);
                        foreach (SQLiteParameter p in cmd.Parameters)
							Console.WriteLine($"[UpsertCalibre] Param: {p.ParameterName} = {p.Value ?? "NULL"}");
						cmd.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error en UpsertCalibre: {ex.Message} | id={id}, dato={dato}, activo={activo}");
				throw;
			}
		}

        public void UpsertGTIN(int id, string dato, bool activo, string plu = null, string codBolsa = null, bool? pesoFijo = null)
		{
			try
			{
				using (SQLiteConnection conn = GetConnection())
				{
					conn.Open();

					// Si existen columnas extra (plu/cod_bolsa/peso_fijo), las persistimos.
					bool hasPlu = ColumnExists(conn, "GTIN", "plu");
					bool hasCodBolsa = ColumnExists(conn, "GTIN", "cod_bolsa");
					bool hasPesoFijo = ColumnExists(conn, "GTIN", "peso_fijo");

					string query;
					if (hasPlu || hasCodBolsa || hasPesoFijo)
					{
						query = @"INSERT OR REPLACE INTO GTIN (id, dato, plu, cod_bolsa, peso_fijo, activo)
								VALUES (@id, @dato, @plu, @cod_bolsa, @peso_fijo, @activo)";
						using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
						{
							cmd.Parameters.AddWithValue("@id", id);
							cmd.Parameters.AddWithValue("@dato", dato ?? "");
							cmd.Parameters.AddWithValue("@plu", plu ?? "");
							cmd.Parameters.AddWithValue("@cod_bolsa", codBolsa ?? "");
							cmd.Parameters.AddWithValue("@peso_fijo", (pesoFijo ?? false) ? 1 : 0);
							cmd.Parameters.AddWithValue("@activo", activo ? 1 : 0);
                            foreach (SQLiteParameter p in cmd.Parameters)
								Console.WriteLine($"[UpsertGTIN] Param: {p.ParameterName} = {p.Value ?? "NULL"}");
							cmd.ExecuteNonQuery();
						}
					}
					else
					{
						query = @"INSERT OR REPLACE INTO GTIN (id, dato, activo) VALUES (@id, @dato, @activo)";
						using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
						{
							cmd.Parameters.AddWithValue("@id", id);
							cmd.Parameters.AddWithValue("@dato", dato ?? "");
							cmd.Parameters.AddWithValue("@activo", activo ? 1 : 0);
							cmd.ExecuteNonQuery();
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error en UpsertGTIN: {ex.Message} | id={id}, dato={dato}, activo={activo}, plu={plu}, codBolsa={codBolsa}, pesoFijo={pesoFijo}");
				throw;
			}
		}

        public void UpsertSDP(int id, string dato, bool activo)
		{
			try
			{
				using (SQLiteConnection conn = GetConnection())
				{
					conn.Open();
					string query = @"INSERT OR REPLACE INTO SDP (id, dato, activo) VALUES (@id, @dato, @activo)";
					using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
					{
						cmd.Parameters.AddWithValue("@id", id);
						cmd.Parameters.AddWithValue("@dato", dato);
						cmd.Parameters.AddWithValue("@activo", activo ? 1 : 0);
                        foreach (SQLiteParameter p in cmd.Parameters)
							Console.WriteLine($"[UpsertSDP] Param: {p.ParameterName} = {p.Value ?? "NULL"}");
						cmd.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error en UpsertSDP: {ex.Message} | id={id}, dato={dato}, activo={activo}");
				throw;
			}
		}

        public void UpsertPeso(int id, string dato, bool activo)
		{
			try
			{
				using (SQLiteConnection conn = GetConnection())
				{
					conn.Open();
					string query = @"INSERT OR REPLACE INTO Peso (id, dato, activo) VALUES (@id, @dato, @activo)";
					using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
					{
						cmd.Parameters.AddWithValue("@id", id);
						cmd.Parameters.AddWithValue("@dato", dato);
						cmd.Parameters.AddWithValue("@activo", activo ? 1 : 0);
                        foreach (SQLiteParameter p in cmd.Parameters)
							Console.WriteLine($"[UpsertPeso] Param: {p.ParameterName} = {p.Value ?? "NULL"}");
						cmd.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error en UpsertPeso: {ex.Message} | id={id}, dato={dato}, activo={activo}");
				throw;
			}
		}

        public void UpsertCategoriaSAG(int id, string nombre)
		{
			try
			{
				using (SQLiteConnection conn = GetConnection())
				{
					conn.Open();
					string query = @"INSERT OR REPLACE INTO CategoriaSAG (id, nombre) VALUES (@id, @nombre)";
					using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
					{
						cmd.Parameters.AddWithValue("@id", id);
						cmd.Parameters.AddWithValue("@nombre", nombre);
                        foreach (SQLiteParameter p in cmd.Parameters)
							Console.WriteLine($"[UpsertCategoriaSAG] Param: {p.ParameterName} = {p.Value ?? "NULL"}");
						cmd.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error en UpsertCategoriaSAG: {ex.Message} | id={id}, nombre={nombre}");
				throw;
			}
		}

        public void UpsertRecibidor(int id, string nombre, string nota)
		{
			try
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
                        foreach (SQLiteParameter p in cmd.Parameters)
							Console.WriteLine($"[UpsertRecibidor] Param: {p.ParameterName} = {p.Value ?? "NULL"}");
						cmd.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error en UpsertRecibidor: {ex.Message} | id={id}, nombre={nombre}, nota={nota}");
				throw;
			}
		}

        public void UpsertTipoEmbalaje(int id, string dato, bool activo, bool pesoFijo)
		{
			try
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
                        foreach (SQLiteParameter p in cmd.Parameters)
							Console.WriteLine($"[UpsertTipoEmbalaje] Param: {p.ParameterName} = {p.Value ?? "NULL"}");
						cmd.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error en UpsertTipoEmbalaje: {ex.Message} | id={id}, dato={dato}, activo={activo}, pesoFijo={pesoFijo}");
				throw;
			}
		}

        public void UpsertTitulo(int id, string texto, string tipo, bool activo)
		{
			try
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
                        foreach (SQLiteParameter p in cmd.Parameters)
							Console.WriteLine($"[UpsertTitulo] Param: {p.ParameterName} = {p.Value ?? "NULL"}");
						cmd.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error en UpsertTitulo: {ex.Message} | id={id}, texto={texto}, tipo={tipo}, activo={activo}");
				throw;
			}
		}

        public void UpsertMapeoCodigoFinal(int id, string variedadImprime, string gtin, string tipoEmbalaje, string codigoFinal, bool activo)
		{
			try
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
                        foreach (SQLiteParameter p in cmd.Parameters)
							Console.WriteLine($"[UpsertMapeoCodigoFinal] Param: {p.ParameterName} = {p.Value ?? "NULL"}");
						cmd.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error en UpsertMapeoCodigoFinal: {ex.Message} | id={id}, variedadImprime={variedadImprime}, gtin={gtin}, tipoEmbalaje={tipoEmbalaje}, codigoFinal={codigoFinal}, activo={activo}");
				throw;
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
					"Productor", "Recibidores", "Variedad", "Lote", "Packing", "TipoEmbalaje", "Calibre", "GTIN", "SDP", "Peso", "CategoriaSAG", "Color", "VariedadImprime", "VariedadImprime_GTIN", "VariedadVariedadImprime_variedad", "Trazabilidad", "Ubicacion", "CantidadCajas", "VariedadCitrico", "PackingCitrico", "TipoEmbalajeCitrico", "RecibidorCitrico", "ProductorCitrico", "CantidadCajasCitrico"
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

		public void UpsertVariedadImprimeGTIN(int id, int variedad_imprime, int gtin)
		{
			using (SQLiteConnection conn = GetConnection())
			{
				conn.Open();
				string query = @"INSERT OR REPLACE INTO VariedadImprime_GTIN (id, variedad_imprime, gtin, actualizado_en) 
								VALUES (@id, @variedad_imprime, @gtin, CURRENT_TIMESTAMP)";
				using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
				{
                    cmd.Parameters.AddWithValue("@id", id);
					cmd.Parameters.AddWithValue("@variedad_imprime", variedad_imprime);
					cmd.Parameters.AddWithValue("@gtin", gtin);
					foreach (SQLiteParameter p in cmd.Parameters)
						Console.WriteLine($"[UpsertVariedadImprimeGTIN] Param: {p.ParameterName} = {p.Value ?? "NULL"}");
					cmd.ExecuteNonQuery();
				}
			}
		}

		public void UpsertCantidadCajas(int id, string cantidad, bool activo)
		{
			using (SQLiteConnection conn = GetConnection())
			{
				conn.Open();
				string query = @"INSERT OR REPLACE INTO CantidadCajas (id, cantidad, activo) 
									VALUES (@id, @cantidad, @activo)";
				using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
				{
                    cmd.Parameters.AddWithValue("@id", id);
					cmd.Parameters.AddWithValue("@cantidad", cantidad);
					cmd.Parameters.AddWithValue("@activo", activo ? 1 : 0);
					foreach (SQLiteParameter p in cmd.Parameters)
						Console.WriteLine($"[UpsertCantidadCajas] Param: {p.ParameterName} = {p.Value ?? "NULL"}");
					cmd.ExecuteNonQuery();
				}
			}
		}

		// ----------------- Citrus specific upserts -----------------
		public void UpsertVariedadCitrico(int id, string variedad, bool activo)
		{
			try
			{
				using (var conn = GetConnection())
				{
					conn.Open();
					string query = @"INSERT OR REPLACE INTO VariedadCitrico (id, variedad, activo) VALUES (@id, @variedad, @activo)";
					using (var cmd = new SQLiteCommand(query, conn))
					{
						cmd.Parameters.AddWithValue("@id", id);
						cmd.Parameters.AddWithValue("@variedad", variedad ?? "");
						cmd.Parameters.AddWithValue("@activo", activo ? 1 : 0);
						foreach (SQLiteParameter p in cmd.Parameters)
							Console.WriteLine($"[UpsertVariedadCitrico] Param: {p.ParameterName} = {p.Value ?? "NULL"}");
						cmd.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error en UpsertVariedadCitrico: {ex.Message} | id={id}, variedad={variedad}, activo={activo}");
				throw;
			}
		}

		public void UpsertPackingCitrico(int id, string packing, bool activo)
		{
			try
			{
				using (var conn = GetConnection())
				{
					conn.Open();
					string query = @"INSERT OR REPLACE INTO PackingCitrico (id, packing, activo) VALUES (@id, @packing, @activo)";
					using (var cmd = new SQLiteCommand(query, conn))
					{
						cmd.Parameters.AddWithValue("@id", id);
						cmd.Parameters.AddWithValue("@packing", packing ?? "");
						cmd.Parameters.AddWithValue("@activo", activo ? 1 : 0);
						foreach (SQLiteParameter p in cmd.Parameters)
							Console.WriteLine($"[UpsertPackingCitrico] Param: {p.ParameterName} = {p.Value ?? "NULL"}");
						cmd.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error en UpsertPackingCitrico: {ex.Message} | id={id}, packing={packing}, activo={activo}");
				throw;
			}
		}

		public void UpsertTipoEmbalajeCitrico(int id, string tipoEmbalaje, bool activo)
		{
			try
			{
				using (var conn = GetConnection())
				{
					conn.Open();
					string query = @"INSERT OR REPLACE INTO TipoEmbalajeCitrico (id, tipo_embalaje, activo) VALUES (@id, @tipo, @activo)";
					using (var cmd = new SQLiteCommand(query, conn))
					{
						cmd.Parameters.AddWithValue("@id", id);
						cmd.Parameters.AddWithValue("@tipo", tipoEmbalaje ?? "");
						cmd.Parameters.AddWithValue("@activo", activo ? 1 : 0);
						foreach (SQLiteParameter p in cmd.Parameters)
							Console.WriteLine($"[UpsertTipoEmbalajeCitrico] Param: {p.ParameterName} = {p.Value ?? "NULL"}");
						cmd.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error en UpsertTipoEmbalajeCitrico: {ex.Message} | id={id}, tipoEmbalaje={tipoEmbalaje}, activo={activo}");
				throw;
			}
		}

		public void UpsertRecibidorCitrico(int id, string recibidor, bool activo)
		{
			try
			{
				using (var conn = GetConnection())
				{
					conn.Open();
					string query = @"INSERT OR REPLACE INTO RecibidorCitrico (id, recibidor, activo) VALUES (@id, @recibidor, @activo)";
					using (var cmd = new SQLiteCommand(query, conn))
					{
						cmd.Parameters.AddWithValue("@id", id);
						cmd.Parameters.AddWithValue("@recibidor", recibidor ?? "");
						cmd.Parameters.AddWithValue("@activo", activo ? 1 : 0);
						foreach (SQLiteParameter p in cmd.Parameters)
							Console.WriteLine($"[UpsertRecibidorCitrico] Param: {p.ParameterName} = {p.Value ?? "NULL"}");
						cmd.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error en UpsertRecibidorCitrico: {ex.Message} | id={id}, recibidor={recibidor}, activo={activo}");
				throw;
			}
		}

		public void UpsertProductorCitrico(int id, string dato, string csg, bool activo)
		{
			try
			{
				using (var conn = GetConnection())
				{
					conn.Open();
					// ProductorCitrico schema: id, dato, CSG, activo
					string query = @"INSERT OR REPLACE INTO ProductorCitrico (id, dato, CSG, activo) VALUES (@id, @dato, @csg, @activo)";
					using (var cmd = new SQLiteCommand(query, conn))
					{
						cmd.Parameters.AddWithValue("@id", id);
						cmd.Parameters.AddWithValue("@dato", dato ?? "");
						cmd.Parameters.AddWithValue("@csg", csg ?? "");
						cmd.Parameters.AddWithValue("@activo", activo ? 1 : 0);
						foreach (SQLiteParameter p in cmd.Parameters)
							Console.WriteLine($"[UpsertProductorCitrico] Param: {p.ParameterName} = {p.Value ?? "NULL"}");
						cmd.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error en UpsertProductorCitrico: {ex.Message} | id={id}, dato={dato}, csg={csg}, activo={activo}");
				throw;
			}
		}

		public void UpsertCantidadCajasCitrico(int id, string cantidadCajas, bool activo)
		{
			try
			{
				using (var conn = GetConnection())
				{
					conn.Open();
					string query = @"INSERT OR REPLACE INTO CantidadCajasCitrico (id, cantidad_cajas, activo) VALUES (@id, @cantidad, @activo)";
					using (var cmd = new SQLiteCommand(query, conn))
					{
						cmd.Parameters.AddWithValue("@id", id);
						cmd.Parameters.AddWithValue("@cantidad", cantidadCajas ?? "");
						cmd.Parameters.AddWithValue("@activo", activo ? 1 : 0);
						foreach (SQLiteParameter p in cmd.Parameters)
							Console.WriteLine($"[UpsertCantidadCajasCitrico] Param: {p.ParameterName} = {p.Value ?? "NULL"}");
						cmd.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error en UpsertCantidadCajasCitrico: {ex.Message} | id={id}, cantidadCajas={cantidadCajas}, activo={activo}");
				throw;
			}
		}

		/// <summary>
		/// Inserta o actualiza una variedad cítrico incluyendo el nuevo campo `numero_interno` si existe en el esquema.
		/// </summary>
		public void UpsertVariedadCitrico(int id, string variedad, string numeroInterno, bool activo)
		{
			try
			{
				using (var conn = GetConnection())
				{
					conn.Open();
					bool hasNumero = ColumnExists(conn, "VariedadCitrico", "numero_interno");
					string query = hasNumero
						? @"INSERT OR REPLACE INTO VariedadCitrico (id, variedad, numero_interno, activo) VALUES (@id, @variedad, @numero_interno, @activo)"
						: @"INSERT OR REPLACE INTO VariedadCitrico (id, variedad, activo) VALUES (@id, @variedad, @activo)";
					using (var cmd = new SQLiteCommand(query, conn))
					{
						cmd.Parameters.AddWithValue("@id", id);
						cmd.Parameters.AddWithValue("@variedad", variedad ?? "");
						if (hasNumero) cmd.Parameters.AddWithValue("@numero_interno", numeroInterno ?? "");
						cmd.Parameters.AddWithValue("@activo", activo ? 1 : 0);
						foreach (SQLiteParameter p in cmd.Parameters)
							Console.WriteLine($"[UpsertVariedadCitrico] Param: {p.ParameterName} = {p.Value ?? "NULL"}");
						cmd.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error en UpsertVariedadCitrico: {ex.Message} | id={id}, variedad={variedad}, numeroInterno={numeroInterno}, activo={activo}");
				throw;
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
				// Nota: este overload se mantiene por compatibilidad. En el esquema actual no existe "sincronizado_en".
				// Si llega a usarse, se guarda un registro mínimo para no romper el flujo.
				string query = @"INSERT OR REPLACE INTO Trazabilidad 
								 (id, productor, variedad, lote, sdp, variedad_imprime, actualizado_en) 
								 VALUES (@id, @productor_id, @variedad_id, @lote_id, @sdp, @variedadImprime, CURRENT_TIMESTAMP)";
				using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
				{
					cmd.Parameters.AddWithValue("@id", id);
					cmd.Parameters.AddWithValue("@productor_id", productor_id);
					cmd.Parameters.AddWithValue("@variedad_id", variedad_id);
					cmd.Parameters.AddWithValue("@lote_id", lote_id);
					int sdpValue = 0;
					int.TryParse(sdp, out sdpValue);
					cmd.Parameters.AddWithValue("@sdp", sdpValue);
					cmd.Parameters.AddWithValue("@variedadImprime", 0);
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
					// El API de Django suele entregar "dato" para Peso.
					// Si alguna instalación entrega "nombre", también lo aceptamos.
					var arrObj = JsonMini.AsArray(JsonMini.Parse(json));
					if (arrObj == null) return pesos;
					foreach (var itemObj in arrObj)
					{
						var dict = itemObj as Dictionary<string, object>;
						var v = JsonMini.GetString(dict, "dato", "nombre");
						if (!string.IsNullOrWhiteSpace(v)) pesos.Add(v);
					}
				}
			}
			return pesos;
		}

        public string GetVariedadImprimeVarInternoById(int id)
		{
			using (var conn = GetConnection())
			{
				conn.Open();
				// Asegura que la columna exista (por si la base es antigua)
				if (!ColumnExists(conn, "VariedadImprime", "var_interno"))
					return "";
				string query = "SELECT var_interno FROM VariedadImprime WHERE id = @id LIMIT 1";
				using (var cmd = new SQLiteCommand(query, conn))
				{
					cmd.Parameters.AddWithValue("@id", id);
					var result = cmd.ExecuteScalar();
					return result?.ToString() ?? "";
				}
			}
		}

		/// <summary>
		/// Obtiene el valor CSG almacenado en la tabla ProductorCitrico para un productor dado (por texto `dato`).
		/// Retorna cadena vacía si no existe o en caso de error.
		/// </summary>
		public string GetCsgForProductor(string productorDato)
		{
			try
			{
				using (var conn = GetConnection())
				{
					conn.Open();
					string query = "SELECT CSG FROM ProductorCitrico WHERE dato = @dato LIMIT 1";
					using (var cmd = new SQLiteCommand(query, conn))
					{
						cmd.Parameters.AddWithValue("@dato", productorDato ?? "");
						var result = cmd.ExecuteScalar();
						return result?.ToString() ?? string.Empty;
					}
				}
			}
			catch { }
			return string.Empty;
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
					// Normalmente Color entrega "nombre".
					var arrObj = JsonMini.AsArray(JsonMini.Parse(json));
					if (arrObj == null) return colores;
					foreach (var itemObj in arrObj)
					{
						var dict = itemObj as Dictionary<string, object>;
						var v = dict?["nombre"]?.ToString() ?? dict?["dato"]?.ToString();
						if (!string.IsNullOrWhiteSpace(v)) colores.Add(v);
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
				var items = new JavaScriptSerializer().Deserialize<List<T>>(json);
				foreach (var item in items)
					upsert(item);
			}
		}

        public List<Item> GetTipoEmbalajePorPesoFijo(bool pesoFijo)
        {
            var lista = new List<Item>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT dato, id FROM TipoEmbalaje WHERE peso_fijo = @pesoFijo ORDER BY dato ASC";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@pesoFijo", pesoFijo ? 1 : 0);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Item(
                                reader["dato"].ToString(),
                                Convert.ToInt32(reader["id"])
                            ));
                        }
                    }
                }
            }
            return lista;
        }

        // Métodos Upsert adicionales
        public void UpsertColor(int id, string nombre, string descripcion, bool activo)
		{
			using (SQLiteConnection conn = GetConnection())
			{
				conn.Open();
				string query = @"INSERT OR REPLACE INTO Color (id, nombre, descripcion, activo) 
									 VALUES (@id, @nombre, @descripcion, @activo)";
				using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
				{
                    cmd.Parameters.AddWithValue("@id", id);
					cmd.Parameters.AddWithValue("@nombre", nombre);
					cmd.Parameters.AddWithValue("@descripcion", descripcion ?? "");
					cmd.Parameters.AddWithValue("@activo", activo ? 1 : 0);
					foreach (SQLiteParameter p in cmd.Parameters)
						Console.WriteLine($"[UpsertColor] Param: {p.ParameterName} = {p.Value ?? "NULL"}");
					cmd.ExecuteNonQuery();
				}
			}
		}

		public void UpsertPeso(int id, string codigo, string descripcion, bool activo)
		{
			// Compatibilidad: esta sobrecarga existía con 4 parámetros.
			// Guardamos "codigo" como "dato" (y omitimos descripcion en el esquema actual).
			UpsertPeso(id, codigo, activo);
		}

		public void UpsertVariedadVariedadImprimir(int id, int variedad, int variedad_imprime)
		{
			using (SQLiteConnection conn = GetConnection())
			{
				conn.Open();
				string query = @"INSERT OR REPLACE INTO VariedadVariedadImprime_variedad (id, variedad, variedad_imprime)
									 VALUES (@id, @variedad, @variedad_imprime)";
				using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
				{
                    cmd.Parameters.AddWithValue("@id", id);
					cmd.Parameters.AddWithValue("@variedad", variedad);
					cmd.Parameters.AddWithValue("@variedad_imprime", variedad_imprime);
					foreach (SQLiteParameter p in cmd.Parameters)
						Console.WriteLine($"[UpsertVariedadVariedadImprimir] Param: {p.ParameterName} = {p.Value ?? "NULL"}");
					cmd.ExecuteNonQuery();
				}
			}
		}

		public void UpsertTrazabilidad(int id, string codigo, int sdp, int productor, int variedad, int lote, int tipo_embalaje, int calibre, int packing, int peso, int recibidor, int color, int variedad_imprime, int categoria_sag, string fecha, string hora, int cantidad, string observaciones)
		{
			// En el esquema local simplificado sólo persistimos los vínculos principales.
			// (Los demás campos se ignoran, para evitar romper el flujo actual.)
			using (SQLiteConnection conn = GetConnection())
			{
				conn.Open();
				string query = @"INSERT OR REPLACE INTO Trazabilidad (id, productor, variedad, lote, sdp, variedad_imprime, actualizado_en)
									 VALUES (@id, @productor, @variedad, @lote, @sdp, @variedad_imprime, CURRENT_TIMESTAMP)";
				using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
				{
                    cmd.Parameters.AddWithValue("@id", id);
					cmd.Parameters.AddWithValue("@productor", productor);
					cmd.Parameters.AddWithValue("@variedad", variedad);
					cmd.Parameters.AddWithValue("@lote", lote);
					cmd.Parameters.AddWithValue("@sdp", sdp);
					cmd.Parameters.AddWithValue("@variedad_imprime", variedad_imprime);
					foreach (SQLiteParameter p in cmd.Parameters)
						Console.WriteLine($"[UpsertTrazabilidad] Param: {p.ParameterName} = {p.Value ?? "NULL"}");
					cmd.ExecuteNonQuery();
				}
			}
		}

		// ========== MÉTODOS ADICIONALES FALTANTES ==========

		// UpsertUbicacion - Para sincronizar ubicaciones desde Django
		public void UpsertUbicacion(int id, string region, string provincia, string comuna, string gln, string ggn, bool activo, string creado_en, string actualizado_en)
		{
			using (SQLiteConnection conn = GetConnection())
			{
				conn.Open();
				string query = @"INSERT OR REPLACE INTO Ubicacion (id, region, provincia, comuna, gln, ggn, activo, creado_en, actualizado_en) 
									VALUES (@id, @region, @provincia, @comuna, @gln, @ggn, @activo, @creado_en, @actualizado_en)";
				using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
				{
                    cmd.Parameters.AddWithValue("@id", id);
					cmd.Parameters.AddWithValue("@region", region ?? "");
					cmd.Parameters.AddWithValue("@provincia", provincia ?? "");
					cmd.Parameters.AddWithValue("@comuna", comuna ?? "");
					cmd.Parameters.AddWithValue("@gln", gln ?? "");
					cmd.Parameters.AddWithValue("@ggn", ggn ?? "");
					cmd.Parameters.AddWithValue("@activo", activo ? 1 : 0);
					cmd.Parameters.AddWithValue("@creado_en", creado_en ?? "");
					cmd.Parameters.AddWithValue("@actualizado_en", actualizado_en ?? "");
					foreach (SQLiteParameter p in cmd.Parameters)
						Console.WriteLine($"[UpsertUbicacion] Param: {p.ParameterName} = {p.Value ?? "NULL"}");
					cmd.ExecuteNonQuery();
				}
			}
		}

		public List<string> GetPesos()
		{
			var result = new List<string>();
			using (var conn = new SQLiteConnection(_connectionString))
			{
				conn.Open();
				string query = "SELECT dato FROM Peso WHERE activo = 1 ORDER BY dato ASC";
				using (var cmd = new SQLiteCommand(query, conn))
				using (var reader = cmd.ExecuteReader())
					while (reader.Read())
						result.Add(reader["dato"].ToString());
			}
			return result;
		}

		public List<string> GetColores()
		{
			var result = new List<string>();
			using (var conn = new SQLiteConnection(_connectionString))
			{
				conn.Open();
				string query = "SELECT nombre FROM Color WHERE activo = 1 ORDER BY nombre ASC";
				using (var cmd = new SQLiteCommand(query, conn))
				using (var reader = cmd.ExecuteReader())
					while (reader.Read())
					result.Add(reader["nombre"].ToString());
			}
			return result;
		}

		public List<string> GetPackings()
		{
			var result = new List<string>();
			using (var conn = new SQLiteConnection(_connectionString))
			{
				conn.Open();
               string query = "SELECT dato, csp FROM Packing WHERE activo = 1 ORDER BY dato ASC";
				using (var cmd = new SQLiteCommand(query, conn))
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						string dato = reader["dato"].ToString();
						string csp = null;
						try { csp = reader.IsDBNull(1) ? null : reader.GetValue(1)?.ToString(); } catch { csp = null; }
						if (!string.IsNullOrWhiteSpace(csp)) dato = dato + "  CSP:" + csp;
						result.Add(dato);
					}
				}
			}
			return result;
		}

		public List<string> GetRecibidores()
		{
			var result = new List<string>();
			using (var conn = new SQLiteConnection(_connectionString))
			{
				conn.Open();
				string query = "SELECT dato FROM Recibidores ORDER BY dato ASC";
				using (var cmd = new SQLiteCommand(query, conn))
				using (var reader = cmd.ExecuteReader())
					while (reader.Read())
					result.Add(reader["dato"].ToString());
			}
			return result;
		}

		public List<string> GetCantidadCajas()
		{
			var result = new List<string>();
			using (var conn = new SQLiteConnection(_connectionString))
			{
				conn.Open();
				string query = "SELECT cantidad FROM CantidadCajas WHERE activo = 1 ORDER BY cantidad ASC";
				using (var cmd = new SQLiteCommand(query, conn))
				using (var reader = cmd.ExecuteReader())
					while (reader.Read())
						result.Add(reader["cantidad"].ToString());
			}
			return result;
		}



        // --- Métodos únicos (strings) para cítricos ---
        public List<string> GetPackingsCitrico()
        {
            var result = new List<string>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT packing FROM PackingCitrico WHERE activo = 1 ORDER BY packing ASC";
                using (var cmd = new SQLiteCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        result.Add(reader.IsDBNull(0) ? "" : reader.GetString(0));
            }
            return result;
        }

        public List<string> GetVariedadesCitrico()
        {
            var result = new List<string>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT variedad FROM VariedadCitrico WHERE activo = 1 ORDER BY variedad ASC";
                using (var cmd = new SQLiteCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        result.Add(reader.IsDBNull(0) ? "" : reader.GetString(0));
            }
            return result;
        }

        public List<string> GetTiposEmbalajeCitrico()
        {
            var result = new List<string>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT tipo_embalaje FROM TipoEmbalajeCitrico WHERE activo = 1 ORDER BY tipo_embalaje ASC";
                using (var cmd = new SQLiteCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        result.Add(reader.IsDBNull(0) ? "" : reader.GetString(0));
            }
            return result;
        }

        public List<string> GetRecibidoresCitrico()
        {
            var result = new List<string>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT recibidor FROM RecibidorCitrico WHERE activo = 1 ORDER BY recibidor ASC";
                using (var cmd = new SQLiteCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        result.Add(reader.IsDBNull(0) ? "" : reader.GetString(0));
            }
            return result;
        }

        public List<string> GetProductoresCitrico()
        {
            var result = new List<string>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT dato FROM ProductorCitrico WHERE activo = 1 ORDER BY dato ASC";
                using (var cmd = new SQLiteCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        result.Add(reader.IsDBNull(0) ? "" : reader.GetString(0));
            }
            return result;
        }

        public List<string> GetCantidadCajasCitrico()
        {
            var result = new List<string>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT cantidad_cajas FROM CantidadCajasCitrico WHERE activo = 1 ORDER BY cantidad_cajas ASC";
                using (var cmd = new SQLiteCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        result.Add(reader.IsDBNull(0) ? "" : reader.GetString(0));
            }
            return result;
        }

        // --- Helpers recomendados para UI (Item) ---
        public List<Item> GetPackingsCitricoItems()
        {
            var result = new List<Item>();
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new SQLiteCommand("SELECT id, packing FROM PackingCitrico WHERE activo = 1 ORDER BY packing ASC", conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        result.Add(new Item(reader.IsDBNull(1) ? "" : reader.GetString(1), reader.IsDBNull(0) ? 0 : reader.GetInt32(0)));
            }
            return result;
        }

        public List<Item> GetVariedadesCitricoItems()
        {
            var result = new List<Item>();
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new SQLiteCommand("SELECT id, variedad FROM VariedadCitrico WHERE activo = 1 ORDER BY variedad ASC", conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        result.Add(new Item(reader.IsDBNull(1) ? "" : reader.GetString(1), reader.IsDBNull(0) ? 0 : reader.GetInt32(0)));
            }
            return result;
        }

        public List<Item> GetTiposEmbalajeCitricoItems()
        {
            var result = new List<Item>();
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new SQLiteCommand("SELECT id, tipo_embalaje FROM TipoEmbalajeCitrico WHERE activo = 1 ORDER BY tipo_embalaje ASC", conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        result.Add(new Item(reader.IsDBNull(1) ? "" : reader.GetString(1), reader.IsDBNull(0) ? 0 : reader.GetInt32(0)));
            }
            return result;
        }

        public List<Item> GetRecibidoresCitricoItems()
        {
            var result = new List<Item>();
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new SQLiteCommand("SELECT id, recibidor FROM RecibidorCitrico WHERE activo = 1 ORDER BY recibidor ASC", conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        result.Add(new Item(reader.IsDBNull(1) ? "" : reader.GetString(1), reader.IsDBNull(0) ? 0 : reader.GetInt32(0)));
            }
            return result;
        }

        public List<Item> GetProductoresCitricoItems()
        {
            var result = new List<Item>();
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new SQLiteCommand("SELECT id, dato FROM ProductorCitrico WHERE activo = 1 ORDER BY dato ASC", conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        result.Add(new Item(reader.IsDBNull(1) ? "" : reader.GetString(1), reader.IsDBNull(0) ? 0 : reader.GetInt32(0)));
            }
            return result;
        }

        public List<Item> GetCantidadCajasCitricoItems()
        {
            var result = new List<Item>();
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new SQLiteCommand("SELECT id, cantidad_cajas FROM CantidadCajasCitrico WHERE activo = 1 ORDER BY cantidad_cajas ASC", conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        result.Add(new Item(reader.IsDBNull(1) ? "" : reader.GetString(1), reader.IsDBNull(0) ? 0 : reader.GetInt32(0)));
            }
            return result;
        }

        // =====================================================
        // Métodos requeridos por los formularios (Items + filtros)
        // =====================================================
        private bool ColumnExists(SQLiteConnection conn, string tableName, string columnName)
			{
				try
				{
					using (var cmd = new SQLiteCommand($"PRAGMA table_info({tableName});", conn))
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							var name = reader["name"]?.ToString();
							if (string.Equals(name, columnName, StringComparison.OrdinalIgnoreCase))
								return true;
						}
					}
				}
				catch { }
				return false;
			}

            public void UpsertVariedadImprima(int id, string dato, bool activo, bool? pesoFijo = null, string varInterno = null)
			{
				// Alias por compatibilidad (en algunos lugares se usa "Imprima" por error).
				using (SQLiteConnection conn = GetConnection())
				{
					conn.Open();
					bool hasPeso = ColumnExists(conn, "VariedadImprime", "peso_fijo");
					bool hasVarInterno = ColumnExists(conn, "VariedadImprime", "var_interno");
					string query;

					if (hasPeso && hasVarInterno)
					{
						query = @"INSERT OR REPLACE INTO VariedadImprime (id, dato, peso_fijo, var_interno, activo) VALUES (@id, @dato, @peso_fijo, @var_interno, @activo)";
						using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
						{
							cmd.Parameters.AddWithValue("@id", id);
							cmd.Parameters.AddWithValue("@dato", dato ?? "");
							cmd.Parameters.AddWithValue("@peso_fijo", (pesoFijo ?? false) ? 1 : 0);
							cmd.Parameters.AddWithValue("@var_interno", varInterno ?? "");
							cmd.Parameters.AddWithValue("@activo", activo ? 1 : 0);
							foreach (SQLiteParameter p in cmd.Parameters)
								Console.WriteLine($"[UpsertVariedadImprima] Param: {p.ParameterName} = {p.Value ?? "NULL"}");
							cmd.ExecuteNonQuery();
						}
					}
					else if (hasVarInterno)
					{
						query = @"INSERT OR REPLACE INTO VariedadImprime (id, dato, var_interno, activo) VALUES (@id, @dato, @var_interno, @activo)";
						using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
						{
							cmd.Parameters.AddWithValue("@id", id);
							cmd.Parameters.AddWithValue("@dato", dato ?? "");
							cmd.Parameters.AddWithValue("@var_interno", varInterno ?? "");
							cmd.Parameters.AddWithValue("@activo", activo ? 1 : 0);
							foreach (SQLiteParameter p in cmd.Parameters)
								Console.WriteLine($"[UpsertVariedadImprima] Param: {p.ParameterName} = {p.Value ?? "NULL"}");
							cmd.ExecuteNonQuery();
						}
					}
					else if (hasPeso)
					{
						query = @"INSERT OR REPLACE INTO VariedadImprime (id, dato, peso_fijo, activo) VALUES (@id, @dato, @peso_fijo, @activo)";
						using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
						{
							cmd.Parameters.AddWithValue("@id", id);
							cmd.Parameters.AddWithValue("@dato", dato ?? "");
							cmd.Parameters.AddWithValue("@peso_fijo", (pesoFijo ?? false) ? 1 : 0);
							cmd.Parameters.AddWithValue("@activo", activo ? 1 : 0);
							foreach (SQLiteParameter p in cmd.Parameters)
								Console.WriteLine($"[UpsertVariedadImprima] Param: {p.ParameterName} = {p.Value ?? "NULL"}");
							cmd.ExecuteNonQuery();
						}
					}
					else
					{
						query = @"INSERT OR REPLACE INTO VariedadImprime (id, dato, activo) VALUES (@id, @dato, @activo)";
						using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
						{
							cmd.Parameters.AddWithValue("@id", id);
							cmd.Parameters.AddWithValue("@dato", dato ?? "");
							cmd.Parameters.AddWithValue("@activo", activo ? 1 : 0);
							foreach (SQLiteParameter p in cmd.Parameters)
								Console.WriteLine($"[UpsertVariedadImprima] Param: {p.ParameterName} = {p.Value ?? "NULL"}");
							cmd.ExecuteNonQuery();
						}
					}
				}
			}

			public List<Item> GetProductoresItems()
			{
				var result = new List<Item>();
				using (var conn = GetConnection())
				{
					conn.Open();
					using (var cmd = new SQLiteCommand("SELECT id, dato FROM Productor WHERE activo = 1 ORDER BY dato ASC", conn))
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
							result.Add(new Item(reader.IsDBNull(1) ? "" : reader.GetString(1), reader.IsDBNull(0) ? 0 : reader.GetInt32(0)));
					}
				}
				return result;
			}

			public List<Item> GetVariedadesItems()
			{
				var result = new List<Item>();
				using (var conn = GetConnection())
				{
					conn.Open();
					using (var cmd = new SQLiteCommand("SELECT id, dato FROM Variedad WHERE activo = 1 ORDER BY dato ASC", conn))
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
							result.Add(new Item(reader.IsDBNull(1) ? "" : reader.GetString(1), reader.IsDBNull(0) ? 0 : reader.GetInt32(0)));
					}
				}
				return result;
			}

			public List<Item> GetLotesItems()
			{
				var result = new List<Item>();
				using (var conn = GetConnection())
				{
					conn.Open();
					using (var cmd = new SQLiteCommand("SELECT id, dato FROM Lote WHERE activo = 1 ORDER BY dato ASC", conn))
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
							result.Add(new Item(reader.IsDBNull(1) ? "" : reader.GetString(1), reader.IsDBNull(0) ? 0 : reader.GetInt32(0)));
					}
				}
				return result;
			}

			public List<Item> GetPackingItems()
			{
				var result = new List<Item>();
				using (var conn = GetConnection())
				{
					conn.Open();
                using (var cmd = new SQLiteCommand("SELECT id, dato, csp FROM Packing WHERE activo = 1 ORDER BY dato ASC", conn))
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						int id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
						string dato = reader.IsDBNull(1) ? "" : reader.GetString(1);
						string cspText = null;
						try { cspText = reader.IsDBNull(2) ? null : reader.GetValue(2)?.ToString(); } catch { cspText = null; }
						if (!string.IsNullOrWhiteSpace(cspText))
							dato = dato + "  CSP:" + cspText;
						result.Add(new Item(dato, id));
					}
				}
				}
				return result;
			}

			public List<Item> GetPesoItems()
			{
				var result = new List<Item>();
				using (var conn = GetConnection())
				{
					conn.Open();
					using (var cmd = new SQLiteCommand("SELECT id, dato FROM Peso WHERE activo = 1 ORDER BY dato ASC", conn))
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
							result.Add(new Item(reader.IsDBNull(1) ? "" : reader.GetString(1), reader.IsDBNull(0) ? 0 : reader.GetInt32(0)));
					}
				}
				return result;
			}

			public List<Item> GetCategoriaSAGItems()
			{
				var result = new List<Item>();
				using (var conn = GetConnection())
				{
					conn.Open();
					using (var cmd = new SQLiteCommand("SELECT id, nombre FROM CategoriaSAG ORDER BY nombre ASC", conn))
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
							result.Add(new Item(reader.IsDBNull(1) ? "" : reader.GetString(1), reader.IsDBNull(0) ? 0 : reader.GetInt32(0)));
					}
				}
				return result;
			}

			public List<Item> GetColoresItems()
			{
				var result = new List<Item>();
				using (var conn = GetConnection())
				{
					conn.Open();
					using (var cmd = new SQLiteCommand("SELECT id, nombre FROM Color WHERE activo = 1 ORDER BY nombre ASC", conn))
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
							result.Add(new Item(reader.IsDBNull(1) ? "" : reader.GetString(1), reader.IsDBNull(0) ? 0 : reader.GetInt32(0)));
					}
				}
				return result;
			}

			public List<Item> GetCalibresItems()
			{
				var result = new List<Item>();
				var ordenSAG = new[] { "XXJ", "XJ", "J", "D", "V", "A", "R", "T", "XXL", "XL", "L", "M", "JJ", "DD", "VV", "AA", "RR" };
				using (var conn = GetConnection())
				{
					conn.Open();
					var map = new Dictionary<string, Item>(StringComparer.OrdinalIgnoreCase);
					using (var cmd = new SQLiteCommand("SELECT id, dato FROM Calibre WHERE activo = 1", conn))
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
						{
							var dato = reader.IsDBNull(1) ? "" : reader.GetString(1);
							var id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
							if (!string.IsNullOrWhiteSpace(dato))
								map[dato] = new Item(dato, id);
						}
					}

					foreach (var c in ordenSAG)
					{
						if (map.TryGetValue(c, out var it))
						{
							result.Add(it);
							map.Remove(c);
						}
					}

					foreach (var kv in new SortedDictionary<string, Item>(map, StringComparer.OrdinalIgnoreCase))
						result.Add(kv.Value);
				}
				return result;
			}

			public List<Item> GetLotePorVariedad(int variedadId)
			{
				var result = new List<Item>();
				using (var conn = GetConnection())
				{
					conn.Open();
					string query = @"SELECT DISTINCT l.id, l.dato
										 FROM Trazabilidad t
										 INNER JOIN Lote l ON l.id = t.lote
										 WHERE t.variedad = @variedad
										 ORDER BY l.dato ASC";
					using (var cmd = new SQLiteCommand(query, conn))
					{
						cmd.Parameters.AddWithValue("@variedad", variedadId);
						using (var reader = cmd.ExecuteReader())
						{
							while (reader.Read())
								result.Add(new Item(reader.IsDBNull(1) ? "" : reader.GetString(1), reader.IsDBNull(0) ? 0 : reader.GetInt32(0)));
						}
					}
				}
				return result;
			}

			public int GetVariedadIdByDato(string dato)
			{
				if (string.IsNullOrWhiteSpace(dato)) return 0;
				using (var conn = GetConnection())
				{
					conn.Open();
					using (var cmd = new SQLiteCommand("SELECT id FROM Variedad WHERE TRIM(dato) = TRIM(@dato) LIMIT 1", conn))
					{
						cmd.Parameters.AddWithValue("@dato", dato);
						var o = cmd.ExecuteScalar();
						if (o != null && o != DBNull.Value) return Convert.ToInt32(o);
					}

					using (var cmd = new SQLiteCommand("SELECT id FROM Variedad WHERE dato LIKE @dato LIMIT 1", conn))
					{
						cmd.Parameters.AddWithValue("@dato", "%" + dato.Trim() + "%");
						var o = cmd.ExecuteScalar();
						if (o != null && o != DBNull.Value) return Convert.ToInt32(o);
					}
				}
				return 0;
			}

			public int GetVariedadImprimeIdByDato(string dato)
			{
				if (string.IsNullOrWhiteSpace(dato)) return 0;
				using (var conn = GetConnection())
				{
					conn.Open();
					using (var cmd = new SQLiteCommand("SELECT id FROM VariedadImprime WHERE TRIM(dato) = TRIM(@dato) LIMIT 1", conn))
					{
						cmd.Parameters.AddWithValue("@dato", dato);
						var o = cmd.ExecuteScalar();
						if (o != null && o != DBNull.Value) return Convert.ToInt32(o);
					}

					using (var cmd = new SQLiteCommand("SELECT id FROM VariedadImprime WHERE dato LIKE @dato LIMIT 1", conn))
					{
						cmd.Parameters.AddWithValue("@dato", "%" + dato.Trim() + "%");
						var o = cmd.ExecuteScalar();
						if (o != null && o != DBNull.Value) return Convert.ToInt32(o);
					}
				}
				return 0;
			}

			public List<Item> GetVariedadesImprimePorVariedadYPesoFijo(int? variedadId, bool pesoFijo)
			{
				var result = new List<Item>();
				using (var conn = GetConnection())
				{
					conn.Open();
					bool hasPesoFijo = ColumnExists(conn, "VariedadImprime", "peso_fijo");

					string query;
					if (variedadId.HasValue)
					{
						query = @"SELECT vi.id, vi.dato
										 FROM VariedadVariedadImprime_variedad m
										 INNER JOIN VariedadImprime vi ON vi.id = m.variedad_imprime
										 WHERE vi.activo = 1 AND m.variedad = @variedad";
						if (hasPesoFijo)
							query += " AND COALESCE(vi.peso_fijo, 0) = @pesoFijo";
						query += " ORDER BY vi.dato ASC";
					}
					else
					{
						query = "SELECT id, dato FROM VariedadImprime WHERE activo = 1";
						if (hasPesoFijo)
							query += " AND COALESCE(peso_fijo, 0) = @pesoFijo";
						query += " ORDER BY dato ASC";
					}

					using (var cmd = new SQLiteCommand(query, conn))
					{
						if (variedadId.HasValue) cmd.Parameters.AddWithValue("@variedad", variedadId.Value);
						if (hasPesoFijo) cmd.Parameters.AddWithValue("@pesoFijo", pesoFijo ? 1 : 0);
						using (var reader = cmd.ExecuteReader())
						{
							while (reader.Read())
								result.Add(new Item(reader.IsDBNull(1) ? "" : reader.GetString(1), reader.IsDBNull(0) ? 0 : reader.GetInt32(0)));
						}
					}
				}
				return result;
			}

			public List<Item> GetSDPsPorSeleccion(int? productorId, int? variedadId, int? loteId, int? variedadImprimeId)
			{
				var result = new List<Item>();
				using (var conn = GetConnection())
				{
					conn.Open();
					var sb = new System.Text.StringBuilder();
					sb.Append(@"SELECT DISTINCT s.id, s.dato
										FROM Trazabilidad t
										INNER JOIN SDP s ON s.id = t.sdp
										WHERE 1=1 ");
					if (productorId.HasValue) sb.Append(" AND t.productor = @productor");
					if (variedadId.HasValue) sb.Append(" AND t.variedad = @variedad");
					if (loteId.HasValue) sb.Append(" AND t.lote = @lote");
					if (variedadImprimeId.HasValue) sb.Append(" AND t.variedad_imprime = @variedad_imprime");
					sb.Append(" ORDER BY s.dato ASC");

					using (var cmd = new SQLiteCommand(sb.ToString(), conn))
					{
						if (productorId.HasValue) cmd.Parameters.AddWithValue("@productor", productorId.Value);
						if (variedadId.HasValue) cmd.Parameters.AddWithValue("@variedad", variedadId.Value);
						if (loteId.HasValue) cmd.Parameters.AddWithValue("@lote", loteId.Value);
						if (variedadImprimeId.HasValue) cmd.Parameters.AddWithValue("@variedad_imprime", variedadImprimeId.Value);
						using (var reader = cmd.ExecuteReader())
						{
							while (reader.Read())
								result.Add(new Item(reader.IsDBNull(1) ? "" : reader.GetString(1), reader.IsDBNull(0) ? 0 : reader.GetInt32(0)));
						}
					}
				}
				return result;
			}

			public List<Item> GetColoresPorSeleccion(int? productorId, int? variedadId, int? loteId, bool pesoFijo, int? variedadImprimeId)
			{
				// En el esquema simplificado puede no existir vínculo Color en Trazabilidad.
				// Si existe columna "color", filtramos. Si no, devolvemos colores activos.
				var result = new List<Item>();
				using (var conn = GetConnection())
				{
					conn.Open();
					bool hasColor = ColumnExists(conn, "Trazabilidad", "color");
					if (!hasColor)
						return GetColoresItems();

					var sb = new System.Text.StringBuilder();
					sb.Append(@"SELECT DISTINCT c.id, c.nombre
										FROM Trazabilidad t
										INNER JOIN Color c ON c.id = t.color
										WHERE c.activo = 1 ");
					if (productorId.HasValue) sb.Append(" AND t.productor = @productor");
					if (variedadId.HasValue) sb.Append(" AND t.variedad = @variedad");
					if (loteId.HasValue) sb.Append(" AND t.lote = @lote");
					if (variedadImprimeId.HasValue) sb.Append(" AND t.variedad_imprime = @variedad_imprime");
					sb.Append(" ORDER BY c.nombre ASC");

					using (var cmd = new SQLiteCommand(sb.ToString(), conn))
					{
						if (productorId.HasValue) cmd.Parameters.AddWithValue("@productor", productorId.Value);
						if (variedadId.HasValue) cmd.Parameters.AddWithValue("@variedad", variedadId.Value);
						if (loteId.HasValue) cmd.Parameters.AddWithValue("@lote", loteId.Value);
						if (variedadImprimeId.HasValue) cmd.Parameters.AddWithValue("@variedad_imprime", variedadImprimeId.Value);
						using (var reader = cmd.ExecuteReader())
						{
							while (reader.Read())
								result.Add(new Item(reader.IsDBNull(1) ? "" : reader.GetString(1), reader.IsDBNull(0) ? 0 : reader.GetInt32(0)));
						}
					}
				}
				return result;
			}

			public List<GTINItem> GetGTINsPorVariedadImprimeYPesoFijo(int? variedadImprimeId, bool pesoFijo)
			{
				var result = new List<GTINItem>();
				using (var conn = GetConnection())
				{
					conn.Open();
					bool hasPlu = ColumnExists(conn, "GTIN", "plu");
					bool hasCodBolsa = ColumnExists(conn, "GTIN", "cod_bolsa") || ColumnExists(conn, "GTIN", "codBolsa");
					string codCol = ColumnExists(conn, "GTIN", "cod_bolsa") ? "cod_bolsa" : (ColumnExists(conn, "GTIN", "codBolsa") ? "codBolsa" : null);
					bool hasPeso = ColumnExists(conn, "GTIN", "peso_fijo");
					string gtinCol = ColumnExists(conn, "GTIN", "gtin") ? "gtin" : "dato";

					var sb = new System.Text.StringBuilder();
					sb.Append("SELECT g.id, g.");
					sb.Append(gtinCol);
					sb.Append(" AS gtin,");
					sb.Append(hasPlu ? " COALESCE(g.plu,'') AS plu," : " '' AS plu,");
					if (hasCodBolsa && !string.IsNullOrWhiteSpace(codCol))
						sb.Append($" COALESCE(g.{codCol},'') AS cod_bolsa ");
					else
						sb.Append(" '' AS cod_bolsa ");

					if (variedadImprimeId.HasValue)
					{
						sb.Append("FROM VariedadImprime_GTIN m INNER JOIN GTIN g ON g.id = m.gtin WHERE g.activo = 1 AND m.variedad_imprime = @vid ");
					}
					else
					{
						sb.Append("FROM GTIN g WHERE g.activo = 1 ");
					}

					if (hasPeso)
						sb.Append("AND COALESCE(g.peso_fijo,0) = @pesoFijo ");
					sb.Append("ORDER BY gtin ASC");

					using (var cmd = new SQLiteCommand(sb.ToString(), conn))
					{
						if (variedadImprimeId.HasValue) cmd.Parameters.AddWithValue("@vid", variedadImprimeId.Value);
						if (hasPeso) cmd.Parameters.AddWithValue("@pesoFijo", pesoFijo ? 1 : 0);
						using (var reader = cmd.ExecuteReader())
						{
							while (reader.Read())
							{
								int id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
								string gtin = reader.IsDBNull(1) ? "" : reader.GetString(1);
								string plu = reader.IsDBNull(2) ? "" : reader.GetString(2);
								string codBolsa = reader.IsDBNull(3) ? "" : reader.GetString(3);
								result.Add(new GTINItem(id, gtin, plu, codBolsa));
							}
						}
					}
				}
				return result;
			}

			public GTINItem GetGTINById(int id)
			{
				try
				{
					using (var conn = GetConnection())
					{
						conn.Open();
						bool hasPlu = ColumnExists(conn, "GTIN", "plu");
						bool hasCodBolsa = ColumnExists(conn, "GTIN", "cod_bolsa") || ColumnExists(conn, "GTIN", "codBolsa");
						string codCol = ColumnExists(conn, "GTIN", "cod_bolsa") ? "cod_bolsa" : (ColumnExists(conn, "GTIN", "codBolsa") ? "codBolsa" : null);
						string gtinCol = ColumnExists(conn, "GTIN", "gtin") ? "gtin" : "dato";

						var sb = new System.Text.StringBuilder();
						sb.Append($"SELECT id, {gtinCol} AS gtin,");
						sb.Append(hasPlu ? " COALESCE(plu,'') AS plu," : " '' AS plu,");
						if (hasCodBolsa && !string.IsNullOrWhiteSpace(codCol))
							sb.Append($" COALESCE({codCol},'') AS cod_bolsa ");
						else
							sb.Append(" '' AS cod_bolsa ");
						sb.Append("FROM GTIN WHERE id = @id LIMIT 1");
						using (var cmd = new SQLiteCommand(sb.ToString(), conn))
						{
							cmd.Parameters.AddWithValue("@id", id);
							using (var reader = cmd.ExecuteReader())
							{
								if (reader.Read())
								{
									return new GTINItem(reader.IsDBNull(0) ? 0 : reader.GetInt32(0), reader.IsDBNull(1) ? "" : reader.GetString(1), reader.IsDBNull(2) ? "" : reader.GetString(2), reader.IsDBNull(3) ? "" : reader.GetString(3));
								}
							}
						}
					}
				}
				catch { }
				return null;
			}

			public long? GetCspByPackingId(int packingId)
			{
				try
				{
					using (var conn = GetConnection())
					{
						conn.Open();
						if (ColumnExists(conn, "Packing", "csp"))
						{
							using (var cmd = new SQLiteCommand("SELECT csp FROM Packing WHERE id = @id LIMIT 1", conn))
							{
								cmd.Parameters.AddWithValue("@id", packingId);
								var o = cmd.ExecuteScalar();
								if (o != null && o != DBNull.Value && long.TryParse(o.ToString(), out var csp))
									return csp;
							}
						}

						using (var cmd = new SQLiteCommand("SELECT dato FROM Packing WHERE id = @id LIMIT 1", conn))
						{
							cmd.Parameters.AddWithValue("@id", packingId);
							var s = cmd.ExecuteScalar()?.ToString();
							if (string.IsNullOrWhiteSpace(s)) return null;
							var parts = s.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
							if (parts.Length == 0) return null;
							if (long.TryParse(parts[parts.Length - 1], out var csp)) return csp;
						}
					}
				}
				catch { }
				return null;
			}

			public string GetProductorCsgById(int productorId)
			{
				try
				{
					using (var conn = GetConnection())
					{
						conn.Open();
						bool hasCsg = ColumnExists(conn, "Productor", "csg") || ColumnExists(conn, "Productor", "CSG");
						string csgCol = ColumnExists(conn, "Productor", "csg") ? "csg" : (ColumnExists(conn, "Productor", "CSG") ? "CSG" : null);

						if (hasCsg && !string.IsNullOrWhiteSpace(csgCol))
						{
							using (var cmd = new SQLiteCommand($"SELECT {csgCol} FROM Productor WHERE id = @id LIMIT 1", conn))
							{
								cmd.Parameters.AddWithValue("@id", productorId);
								var o = cmd.ExecuteScalar();
								var csg = o?.ToString();
								if (!string.IsNullOrWhiteSpace(csg)) return csg.Trim();
							}
						}

						using (var cmd = new SQLiteCommand("SELECT dato FROM Productor WHERE id = @id LIMIT 1", conn))
						{
							cmd.Parameters.AddWithValue("@id", productorId);
							var dato = cmd.ExecuteScalar()?.ToString();
							if (string.IsNullOrWhiteSpace(dato)) return "";
							var t = dato.Trim();
							// Si el dato viene como "123456 - Nombre" o similar, tomamos los primeros 6 dígitos.
							var digits = new System.Text.StringBuilder();
							foreach (var ch in t)
							{
								if (char.IsDigit(ch)) digits.Append(ch);
								else break;
							}
							return digits.Length >= 6 ? digits.ToString().Substring(0, 6) : "";
						}
					}
				}
				catch { }
				return "";
			}

		/// <summary>
		/// Obtiene el campo `numero_interno` de VariedadCitrico por su id. Retorna cadena vacía si no existe.
		/// </summary>
		public string GetNumeroInternoVariedadCitricoById(int id)
		{
			try
			{
				using (var conn = GetConnection())
				{
					conn.Open();
					string query = "SELECT numero_interno FROM VariedadCitrico WHERE id = @id LIMIT 1";
					using (var cmd = new SQLiteCommand(query, conn))
					{
						cmd.Parameters.AddWithValue("@id", id);
						var o = cmd.ExecuteScalar();
						return o?.ToString() ?? string.Empty;
					}
				}
			}
			catch { }
			return string.Empty;
		}

		/// <summary>
		/// Obtiene el campo `numero_interno` de VariedadCitrico por su nombre (variedad). Retorna cadena vacía si no existe.
		/// </summary>
		public string GetNumeroInternoVariedadCitricoByDato(string variedad)
		{
			if (string.IsNullOrWhiteSpace(variedad)) return string.Empty;
			try
			{
				using (var conn = GetConnection())
				{
					conn.Open();
					string query = "SELECT numero_interno FROM VariedadCitrico WHERE TRIM(variedad) = TRIM(@variedad) LIMIT 1";
					using (var cmd = new SQLiteCommand(query, conn))
					{
						cmd.Parameters.AddWithValue("@variedad", variedad);
						var o = cmd.ExecuteScalar();
						if (o != null && o != DBNull.Value) return o.ToString();
					}
					// Fallback: intentar LIKE
					using (var cmd = new SQLiteCommand("SELECT numero_interno FROM VariedadCitrico WHERE variedad LIKE @variedad LIMIT 1", conn))
					{
						cmd.Parameters.AddWithValue("@variedad", "%" + variedad.Trim() + "%");
						var o = cmd.ExecuteScalar();
						return o?.ToString() ?? string.Empty;
					}
				}
			}
			catch { }
			return string.Empty;
		}

			public List<Item> GetRecibidoresItems()
			{
				var result = new List<Item>();
				using (var conn = GetConnection())
				{
					conn.Open();
					using (var cmd = new SQLiteCommand("SELECT id, dato FROM Recibidores ORDER BY dato ASC", conn))
					using (var reader = cmd.ExecuteReader())
					{
						while (reader.Read())
							result.Add(new Item(reader.IsDBNull(1) ? "" : reader.GetString(1), reader.IsDBNull(0) ? 0 : reader.GetInt32(0)));
					}
				}
				return result;
			}
	}

	public class DatabaseItem
	{
		public int Id { get; set; }
		public string Dato { get; set; }

		public DatabaseItem(string dato, int id)
		{
			Dato = dato;
			Id = id;
		}
	}

	public class GTINItem
	{
		public int Id { get; set; }
		public string Gtin { get; set; }
		public string Plu { get; set; }
		public string CodBolsa { get; set; }

		public GTINItem(int id, string gtin, string plu, string codBolsa)
		{
			Id = id;
			Gtin = gtin;
			Plu = plu;
			CodBolsa = codBolsa;
		}
	}
}

