using System;
using System.Data.SQLite;
using System.Collections.Generic;

class DatabaseInspector
{
    static void Main()
    {
        string dbPath = @"D:\escritorio\vercion mejorada\generador de etiquetas\bin\Debug\etiquetas.db";
        string connectionString = $"Data Source={dbPath};Version=3;";
        
        try
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                Console.WriteLine("✓ Base de datos conectada exitosamente\n");
                
                // Obtener todas las tablas
                string query = "SELECT name FROM sqlite_master WHERE type='table' ORDER BY name;";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        Console.WriteLine("TABLAS DISPONIBLES:");
                        Console.WriteLine("==================");
                        List<string> tables = new List<string>();
                        while (reader.Read())
                        {
                            string tableName = reader["name"].ToString();
                            tables.Add(tableName);
                            Console.WriteLine($"  - {tableName}");
                        }
                        
                        Console.WriteLine("\n\nESTRUCTURA DE TABLAS RELEVANTES:");
                        Console.WriteLine("================================\n");
                        
                        // Inspeccionar tabla Recibidor
                        InspectTable(conn, "Recibidor");
                        InspectTable(conn, "Productor");
                        InspectTable(conn, "Ubicacion");
                        InspectTable(conn, "Variedad");
                        InspectTable(conn, "Lote");
                        InspectTable(conn, "SDP");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Error: {ex.Message}");
        }
    }
    
    static void InspectTable(SQLiteConnection conn, string tableName)
    {
        try
        {
            Console.WriteLine($"\n📋 TABLA: {tableName}");
            Console.WriteLine(new string('-', 60));
            
            // Obtener estructura
            string query = $"PRAGMA table_info({tableName});";
            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("Columnas:");
                    while (reader.Read())
                    {
                        string colName = reader["name"].ToString();
                        string colType = reader["type"].ToString();
                        Console.WriteLine($"  - {colName} ({colType})");
                    }
                }
            }
            
            // Contar registros
            query = $"SELECT COUNT(*) as total FROM {tableName};";
            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                object result = cmd.ExecuteScalar();
                Console.WriteLine($"Total de registros: {result}");
            }
            
            // Mostrar primeros registros
            query = $"SELECT * FROM {tableName} LIMIT 3;";
            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
            {
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        Console.WriteLine("Primeros 3 registros:");
                        int rowNum = 0;
                        while (reader.Read() && rowNum < 3)
                        {
                            Console.Write($"  [{rowNum + 1}] ");
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                object value = reader.GetValue(i);
                                Console.Write($"{reader.GetName(i)}='{value}' | ");
                            }
                            Console.WriteLine();
                            rowNum++;
                        }
                    }
                    else
                    {
                        Console.WriteLine("La tabla está vacía");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  ✗ Error al inspeccionar: {ex.Message}");
        }
    }
}
