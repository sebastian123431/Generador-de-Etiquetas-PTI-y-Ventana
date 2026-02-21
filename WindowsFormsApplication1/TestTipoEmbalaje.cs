using System;
using System.Diagnostics;
using WindowsFormsApplication1.Data;

namespace WindowsFormsApplication1
{
    /// <summary>
    /// Programa de test para verificar estado de TipoEmbalaje
    /// Ejecución: Agregar un botón en frm_generador que llame a TestTipoEmbalaje.Run()
    /// </summary>
    public static class TestTipoEmbalaje
    {
        public static void Run()
        {
            Debug.WriteLine("\n╔════════════════════════════════════════╗");
            Debug.WriteLine("║   TEST TIPO EMBALAJE - INICIO          ║");
            Debug.WriteLine("╚════════════════════════════════════════╝\n");

            try
            {
                var db = DatabaseManager.Instance;

                // Test 1: Diagnóstico general
                Debug.WriteLine("📋 TEST 1: DIAGNÓSTICO GENERAL");
                var diag = db.DiagnosticTipoEmbalaje();

                Debug.WriteLine($"\n📊 RESULTADO:");
                Debug.WriteLine($"   • Total registros: {diag.totalRegistros}");
                Debug.WriteLine($"   • Registros activos: {diag.activosTotal}");
                Debug.WriteLine($"   • Tipo embalaje VARIABLE (peso_fijo=0): {diag.pesoFijo0}");
                Debug.WriteLine($"   • Tipo embalaje FIJO (peso_fijo=1): {diag.pesoFijo1}");

                // Test 2: Filtro peso_fijo = 0
                Debug.WriteLine($"\n🔍 TEST 2: OBTENER TIPO EMBALAJE VARIABLE (peso_fijo=0)");
                var variable = db.GetTipoEmbalajePorPesoFijo(false);
                Debug.WriteLine($"   Items retornados: {variable.Count}");
                foreach (var item in variable)
                {
                    Debug.WriteLine($"     ✓ ID={item.Id}, Dato={item.Dato}");
                }

                // Test 3: Filtro peso_fijo = 1
                Debug.WriteLine($"\n🔍 TEST 3: OBTENER TIPO EMBALAJE FIJO (peso_fijo=1)");
                var fijo = db.GetTipoEmbalajePorPesoFijo(true);
                Debug.WriteLine($"   Items retornados: {fijo.Count}");
                foreach (var item in fijo)
                {
                    Debug.WriteLine($"     ✓ ID={item.Id}, Dato={item.Dato}");
                }

                Debug.WriteLine("\n✅ TEST COMPLETADO EXITOSAMENTE");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"\n❌ ERROR: {ex.Message}");
                Debug.WriteLine($"📍 Stack Trace: {ex.StackTrace}");
            }

            Debug.WriteLine("\n╔════════════════════════════════════════╗");
            Debug.WriteLine("║   TEST TIPO EMBALAJE - FIN             ║");
            Debug.WriteLine("╚════════════════════════════════════════╝\n");
        }
    }
}
