using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication1.Data;
using System.IO;
using System.Runtime.InteropServices;

namespace WindowsFormsApplication1
{
    static class Program
    {
        public static WebSocketClient WebSocket { get; private set; }
        public static DatabaseManager Database { get; private set; }

        [STAThread]
        static void Main()
        {
            try
            {
                // Intento configurar y cargar la DLL nativa de SQLite antes de inicializar la DB
                TryConfigureSQLiteInterop();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                // Inicializar base de datos
                Database = DatabaseManager.Instance;
                Console.WriteLine("Base de datos inicializada.");

                // Inicializar WebSocket
                WebSocket = WebSocketClient.Instance;
                Console.WriteLine("Cliente WebSocket inicializado.");

                // ====== CONFIG BASE ======
                string servidor = Database.GetConfigValue("servidor", "localhost");
                string puerto = Database.GetConfigValue("puerto", "8000");

                // Normaliza: si guardaron 127.0.0.1, usa localhost para evitar líos
                if (servidor == "127.0.0.1") servidor = "localhost";

                string httpBase = $"http://{servidor}:{puerto}";
                string apiRoot = $"{httpBase}/api/";

                // ✅ WebSocket correcto según voiceapp/routing.py
                string wsUrl = $"ws://{servidor}:{puerto}/ws/catalogo/notificacion/";

                // ====== 1) Verificar API HTTP disponible ======
                bool apiOk = false;
                try
                {
                    apiOk = Task.Run(async () =>
                    {
                        return await HttpSyncClient.Instance.VerificarConexionAsync();
                    }).GetAwaiter().GetResult();
                }
                catch
                {
                    apiOk = false;
                }

                if (!apiOk)
                {
                    MessageBox.Show(
                        "No se pudo conectar a la API en:\n\n" + apiRoot +
                        "\n\nVerifica que Django esté corriendo y que el puerto/servidor estén correctos.",
                        "Sin conexión",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }

                Console.WriteLine("API HTTP OK: " + apiRoot);

                // ====== 2) Conectar WebSocket si está habilitado ======
                string autoConectar = Database.GetConfigValue("auto_conectar", "false");
                if (autoConectar == "true")
                {
                    Task.Run(async () =>
                    {
                        try
                        {
                            await WebSocket.ConnectAsync(wsUrl);
                            Console.WriteLine("Conexión WebSocket iniciada: " + wsUrl);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error WebSocket: " + ex.Message);
                        }
                    });
                }

                // ====== 3) Mostrar formulario de sincronización ======
                var syncForm = new frm_Sincronizacion();
                var result = syncForm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    Application.Run(new Principal());
                }
                else
                {
                    MessageBox.Show(
                        "Error en sincronización:\n\n" + syncForm.ErrorMessage,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al iniciar la aplicación:\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                Console.WriteLine("Error al iniciar: " + ex.Message);
            }
        }

        // Attempt to locate and load the native SQLite.Interop.dll matching process architecture
        private static void TryConfigureSQLiteInterop()
        {
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;

                // Quick check: if already loaded, return
                try
                {
                    if (NativeMethods.GetModuleHandle("SQLite.Interop.dll") != IntPtr.Zero) return;
                }
                catch { }

                // Determine process architecture
                bool is64 = IntPtr.Size == 8;
                string arch = is64 ? "x64" : "x86";

                // Candidate locations inside output folder
                string[] localCandidates = new string[] {
                    Path.Combine(baseDir, arch, "SQLite.Interop.dll"),
                    Path.Combine(baseDir, "runtimes", is64 ? "win-x64" : "win-x86", "native", "SQLite.Interop.dll"),
                    Path.Combine(baseDir, "SQLite.Interop.dll")
                };

                foreach (var candidate in localCandidates)
                {
                    try
                    {
                        if (File.Exists(candidate))
                        {
                            IntPtr h = NativeMethods.LoadLibrary(candidate);
                            if (h != IntPtr.Zero) return;
                        }
                    }
                    catch { }
                }

                // Search in NuGet global-packages folder
                try
                {
                    string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                    string nugetPackages = Path.Combine(userProfile, ".nuget", "packages");
                    if (Directory.Exists(nugetPackages))
                    {
                        // Look for System.Data.SQLite.Core package directories
                        var dirs = Directory.GetDirectories(nugetPackages, "system.data.sqlite.core.*", SearchOption.TopDirectoryOnly);
                        foreach (var dir in dirs)
                        {
                            // look in runtimes
                            string candidate = Path.Combine(dir, "runtimes", is64 ? "win-x64" : "win-x86", "native", "SQLite.Interop.dll");
                            if (File.Exists(candidate))
                            {
                                try
                                {
                                    // copy to output folder under arch subfolder
                                    string targetDir = Path.Combine(baseDir, arch);
                                    Directory.CreateDirectory(targetDir);
                                    string target = Path.Combine(targetDir, "SQLite.Interop.dll");
                                    File.Copy(candidate, target, true);
                                    IntPtr h = NativeMethods.LoadLibrary(target);
                                    if (h != IntPtr.Zero) return;
                                }
                                catch { }
                            }
                        }
                    }
                }
                catch { }

                // As a last resort, try to load any found in subfolders of the application directory
                try
                {
                    foreach (var file in Directory.GetFiles(baseDir, "SQLite.Interop.dll", SearchOption.AllDirectories))
                    {
                        try
                        {
                            IntPtr h = NativeMethods.LoadLibrary(file);
                            if (h != IntPtr.Zero) return;
                        }
                        catch { }
                    }
                }
                catch { }
            }
            catch { }
        }

        private static class NativeMethods
        {
            [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
            public static extern IntPtr LoadLibrary(string lpFileName);

            [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
            public static extern IntPtr GetModuleHandle(string lpModuleName);
        }
    }
}
