using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using WindowsFormsApplication1.Data;

namespace WindowsFormsApplication1
{
    public partial class frm_Sincronizacion : Form
    {
        public string ErrorMessage { get; set; }

        public frm_Sincronizacion()
        {
            InitializeComponent();
        }

        private async void frm_Sincronizacion_Shown(object sender, EventArgs e)
        {
            progressBar.Style = ProgressBarStyle.Continuous;
            lbl_Status.Text = "Probando conexión con la API...";

            var progress = new Progress<(int, string)>(tuple =>
            {
                progressBar.Value = tuple.Item1;
                lbl_Status.Text = tuple.Item2;
            });

            try
            {
                // Primero probar la conexión
                var testResult = await HttpSyncClient.Instance.TestConnectionAsync();
                if (!testResult.success)
                {
                    ErrorMessage = testResult.message;
                    MessageBox.Show($"Error de conexión: {ErrorMessage}", "Depuración");
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                    return;
                }

                lbl_Status.Text = "Conexión exitosa. Iniciando sincronización...";
                progressBar.Value = 10;

                var result = await HttpSyncClient.Instance.SincronizarTodoAsync(progress);
                if (result.success)
                {
                    lbl_Status.Text = "Sincronización completada.";
                    progressBar.Value = 100;
                    await Task.Delay(500); // Pequeña pausa para mostrar completado
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    ErrorMessage = result.message;
                    MessageBox.Show($"Error en sincronización: {ErrorMessage}", "Depuración");
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                MessageBox.Show($"Excepción: {ErrorMessage}", "Depuración");
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }
    }
}