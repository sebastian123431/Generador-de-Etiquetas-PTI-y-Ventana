using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication1.Data;

namespace WindowsFormsApplication1
{
    /// <summary>
    /// Formulario principal de la aplicaci�n
    /// Act�a como men� principal para seleccionar el tipo de generador de etiquetas a usar
    /// Incluye indicador de conexi�n WebSocket en tiempo real
    /// </summary>
    public partial class Principal : Form
    {
        // Controles para el indicador de conexi�n
        private Panel panelConexion;
        private Label lblEstadoConexion;
        private Label lblUltimoDato;
        private ProgressBar progressBarSync;
        private Button btnConfiguracion;
        private Timer timerConexion;
        private Timer timerProgressBar;
        private int progressValue = 0;

        /// <summary>
        /// Constructor del formulario principal
        /// Inicializa los componentes del formulario y configura el indicador de conexi�n
        /// </summary>
        public Principal()
        {
            this.InitializeComponent();
            ConfigurarIndicadorConexion();
        }

        /// <summary>
        /// Configura el panel de indicador de estado de conexi�n WebSocket
        /// Incluye label de estado, progress bar, �ltimo dato y bot�n de configuraci�n
        /// </summary>
        private void ConfigurarIndicadorConexion()
        {
            // Panel contenedor en la parte superior derecha
            panelConexion = new Panel();
            panelConexion.Size = new Size(350, 110);
            panelConexion.Location = new Point(this.ClientSize.Width - 360, 10);
            panelConexion.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            panelConexion.BackColor = Color.FromArgb(240, 240, 240);
            panelConexion.BorderStyle = BorderStyle.FixedSingle;

            // Bot�n de configuraci�n (?)
            btnConfiguracion = new Button();
            btnConfiguracion.Size = new Size(35, 35);
            btnConfiguracion.Location = new Point(305, 10);
            btnConfiguracion.Text = "?";
            btnConfiguracion.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btnConfiguracion.BackColor = Color.FromArgb(0, 51, 102);
            btnConfiguracion.ForeColor = Color.White;
            btnConfiguracion.FlatStyle = FlatStyle.Flat;
            btnConfiguracion.FlatAppearance.BorderSize = 0;
            btnConfiguracion.Cursor = Cursors.Hand;
            btnConfiguracion.Click += BtnConfiguracion_Click;
            panelConexion.Controls.Add(btnConfiguracion);

            // Label de estado de conexi�n
            lblEstadoConexion = new Label();
            lblEstadoConexion.AutoSize = false;
            lblEstadoConexion.Size = new Size(200, 25);
            lblEstadoConexion.Location = new Point(10, 10);
            lblEstadoConexion.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblEstadoConexion.Text = "? Desconectado";
            lblEstadoConexion.ForeColor = Color.Red;
            panelConexion.Controls.Add(lblEstadoConexion);

            // Progress Bar de sincronizaci�n
            progressBarSync = new ProgressBar();
            progressBarSync.Size = new Size(280, 20);
            progressBarSync.Location = new Point(10, 40);
            progressBarSync.Style = ProgressBarStyle.Continuous;
            progressBarSync.Value = 0;
            progressBarSync.Visible = false;
            panelConexion.Controls.Add(progressBarSync);

            // Label de �ltimo dato recibido
            lblUltimoDato = new Label();
            lblUltimoDato.AutoSize = false;
            lblUltimoDato.Size = new Size(280, 40);
            lblUltimoDato.Location = new Point(10, 65);
            lblUltimoDato.Font = new Font("Segoe UI", 8F, FontStyle.Italic);
            lblUltimoDato.ForeColor = Color.Gray;
            lblUltimoDato.Text = "Sin actividad reciente";
            panelConexion.Controls.Add(lblUltimoDato);

            // Agregar el panel al formulario (sobre el header)
            this.Controls.Add(panelConexion);
            panelConexion.BringToFront();

            // Timer para actualizar el estado de conexi�n cada segundo
            timerConexion = new Timer();
            timerConexion.Interval = 1000;
            timerConexion.Tick += TimerConexion_Tick;
            timerConexion.Start();

            // Timer para animar el progress bar durante sincronizaci�n
            timerProgressBar = new Timer();
            timerProgressBar.Interval = 50;
            timerProgressBar.Tick += TimerProgressBar_Tick;

            // Suscribirse a eventos del WebSocket
            if (WebSocketClient.Instance != null)
            {
                WebSocketClient.Instance.ConnectionStatusChanged += WebSocket_ConnectionStatusChanged;
                WebSocketClient.Instance.MessageReceived += WebSocket_MessageReceived;
                WebSocketClient.Instance.ErrorOccurred += WebSocket_ErrorOccurred;
            }

            // Actualizar estado inicial
            ActualizarEstadoConexion();
        }

        /// <summary>
        /// Timer tick para animar el progress bar durante sincronizaci�n
        /// </summary>
        private void TimerProgressBar_Tick(object sender, EventArgs e)
        {
            if (progressBarSync.Value < 100)
            {
                progressBarSync.Value += 5;
            }
            else
            {
                progressBarSync.Value = 0;
            }
        }

        /// <summary>
        /// Actualiza el indicador visual de conexi�n
        /// </summary>
        private void ActualizarEstadoConexion()
        {
            if (lblEstadoConexion == null) return;

            if (this.InvokeRequired)
            {
                this.Invoke(new Action(ActualizarEstadoConexion));
                return;
            }

            bool conectado = WebSocketClient.Instance != null && WebSocketClient.Instance.IsConnected;

            if (conectado)
            {
                lblEstadoConexion.Text = "? Conectado";
                lblEstadoConexion.ForeColor = Color.FromArgb(76, 175, 80); // Verde
                panelConexion.BackColor = Color.FromArgb(230, 255, 230); // Fondo verde claro
            }
            else
            {
                lblEstadoConexion.Text = "? Desconectado";
                lblEstadoConexion.ForeColor = Color.Red;
                panelConexion.BackColor = Color.FromArgb(255, 230, 230); // Fondo rojo claro
                progressBarSync.Visible = false;
                timerProgressBar.Stop();
            }
        }

        /// <summary>
        /// Timer tick - actualiza peri�dicamente el estado de conexi�n
        /// </summary>
        private void TimerConexion_Tick(object sender, EventArgs e)
        {
            ActualizarEstadoConexion();
        }

        /// <summary>
        /// Evento cuando cambia el estado de conexi�n del WebSocket
        /// </summary>
        private void WebSocket_ConnectionStatusChanged(object sender, bool conectado)
        {
            ActualizarEstadoConexion();

            if (conectado)
            {
                // Mostrar progress bar durante sincronizaci�n inicial
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        progressBarSync.Visible = true;
                        progressBarSync.Value = 0;
                        timerProgressBar.Start();
                        lblUltimoDato.Text = "Sincronizando datos iniciales...";
                    }));
                }
                else
                {
                    progressBarSync.Visible = true;
                    progressBarSync.Value = 0;
                    timerProgressBar.Start();
                    lblUltimoDato.Text = "Sincronizando datos iniciales...";
                }
            }
        }

        /// <summary>
        /// Evento cuando se recibe un mensaje del WebSocket
        /// Muestra el �ltimo dato recibido y anima el progress bar
        /// </summary>
        private void WebSocket_MessageReceived(object sender, string mensaje)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => WebSocket_MessageReceived(sender, mensaje)));
                return;
            }

            // Mostrar animaci�n de progress bar brevemente
            progressBarSync.Visible = true;
            progressBarSync.Value = 100;

            // Actualizar label con el mensaje recibido
            if (mensaje.Contains("Sincronizaci�n inicial completada"))
            {
                lblUltimoDato.Text = "? Sincronizaci�n completada";
                lblUltimoDato.ForeColor = Color.FromArgb(76, 175, 80);

                // Ocultar progress bar despu�s de 2 segundos
                Timer tempTimer = new Timer();
                tempTimer.Interval = 2000;
                tempTimer.Tick += (s, args) =>
                {
                    progressBarSync.Visible = false;
                    timerProgressBar.Stop();
                    tempTimer.Stop();
                    tempTimer.Dispose();
                };
                tempTimer.Start();
            }
            else
            {
                // Mostrar mensaje abreviado
                string mensajeCorto = mensaje.Length > 40 ? mensaje.Substring(0, 40) + "..." : mensaje;
                lblUltimoDato.Text = $"Recibido: {mensajeCorto}";
                lblUltimoDato.ForeColor = Color.FromArgb(0, 51, 102);

                // Animar progress bar brevemente
                progressBarSync.Value = 0;
                timerProgressBar.Start();

                Timer hideTimer = new Timer();
                hideTimer.Interval = 1500;
                hideTimer.Tick += (s, args) =>
                {
                    timerProgressBar.Stop();
                    progressBarSync.Visible = false;
                    hideTimer.Stop();
                    hideTimer.Dispose();
                };
                hideTimer.Start();
            }

            // Agregar timestamp
            lblUltimoDato.Text += $"\n{DateTime.Now:HH:mm:ss}";
        }

        /// <summary>
        /// Evento cuando ocurre un error en el WebSocket
        /// </summary>
        private void WebSocket_ErrorOccurred(object sender, string error)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => WebSocket_ErrorOccurred(sender, error)));
                return;
            }

            lblUltimoDato.Text = $"? Error: {(error.Length > 30 ? error.Substring(0, 30) + "..." : error)}";
            lblUltimoDato.ForeColor = Color.Red;
            progressBarSync.Visible = false;
            timerProgressBar.Stop();
        }

        /// <summary>
        /// Evento clic del bot�n de configuraci�n
        /// Abre el formulario de configuraci�n de conexi�n
        /// </summary>
        private void BtnConfiguracion_Click(object sender, EventArgs e)
        {
            frm_Configuracion formConfig = new frm_Configuracion();
            formConfig.ShowDialog(this);
        }

        /// <summary>
        /// Evento clic del men� PTI
        /// Abre el formulario para generar etiquetas tipo PTI (Producci�n Tienda Industrial)
        /// </summary>
        /// <param name="sender">Objeto que dispar� el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private async void pTIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var syncForm = new frm_Sincronizacion())
            {
                var result = syncForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    frm_generador frm_generador = new frm_generador();
                    frm_generador.Show();
                }
                else
                {
                    MessageBox.Show("Error al sincronizar datos:\n" + syncForm.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Evento clic del men� VENTANA
        /// Abre el formulario para generar etiquetas tipo VENTANA (etiquetas de ventana est�ndar)
        /// </summary>
        /// <param name="sender">Objeto que dispar� el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void vENTANAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_generadorVentana frm_generadorVentana = new frm_generadorVentana();
            frm_generadorVentana.Show();
        }

        /// <summary>
        /// Evento de carga del formulario principal
        /// Se ejecuta cuando el formulario se carga por primera vez
        /// </summary>
        /// <param name="sender">Objeto que dispar� el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void Principal_Load(object sender, EventArgs e)
        {
            ActualizarEstadoConexion();
            // Depuración: mensaje antes de mostrar la ventana de sincronización
            // MessageBox.Show("Mostrando ventana de sincronización", "Depuración");
            using (var syncForm = new frm_Sincronizacion())
            {
                var result = syncForm.ShowDialog();
                if (result != DialogResult.OK)
                {
                    MessageBox.Show("Error al sincronizar datos:\n" + syncForm.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Evento cuando el mouse entra en el �rea de un bot�n
        /// Cambia el color de fondo del bot�n a verde para indicar hover
        /// </summary>
        /// <param name="sender">Bot�n que dispar� el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void Button_MouseEnter(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                // Cambia a color verde claro cuando el mouse est� sobre el bot�n
                btn.BackColor = Color.FromArgb(76, 175, 80);
            }
        }

        /// <summary>
        /// Evento cuando el mouse sale del �rea de un bot�n
        /// Restaura el color de fondo del bot�n al color original azul oscuro
        /// </summary>
        /// <param name="sender">Bot�n que dispar� el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void Button_MouseLeave(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                // Restaura al color azul oscuro original
                btn.BackColor = Color.FromArgb(0, 51, 102);
            }
        }

        /// <summary>
        /// Evento clic del men� VENTANA CITRICOS
        /// Abre el formulario para generar etiquetas especiales de ventana para productos c�tricos
        /// </summary>
        /// <param name="sender">Objeto que dispar� el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void vENTANACITRICOSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_generadorVentana_Citricos frm_generadorVentana_Citricos = new frm_generadorVentana_Citricos();
            frm_generadorVentana_Citricos.Show();
        }

        /// <summary>
        /// Limpia los recursos cuando el formulario se cierra
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (timerConexion != null)
            {
                timerConexion.Stop();
                timerConexion.Dispose();
            }

            if (timerProgressBar != null)
            {
                timerProgressBar.Stop();
                timerProgressBar.Dispose();
            }

            if (WebSocketClient.Instance != null)
            {
                WebSocketClient.Instance.ConnectionStatusChanged -= WebSocket_ConnectionStatusChanged;
                WebSocketClient.Instance.MessageReceived -= WebSocket_MessageReceived;
                WebSocketClient.Instance.ErrorOccurred -= WebSocket_ErrorOccurred;
            }

            base.OnFormClosing(e);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            SincronizarDatosApi();
        }

        private async void SincronizarDatosApi()
        {
            // Mostrar ProgressBar en modo indeterminado (Marquee)
            progressBarSync.Style = ProgressBarStyle.Marquee;
            progressBarSync.MarqueeAnimationSpeed = 30;
            progressBarSync.Visible = true;

            labelInstrucciones.Text = "Sincronizando datos con la API...";
            labelInstrucciones.ForeColor = Color.Orange;
            Console.WriteLine("Iniciando sincronizaci�n con la API...");

            var resultado = await HttpSyncClient.Instance.SincronizarTodoAsync();

            // Ocultar ProgressBar y restaurar estilo
            progressBarSync.Visible = false;
            progressBarSync.Style = ProgressBarStyle.Continuous;
            progressBarSync.MarqueeAnimationSpeed = 0;

            if (resultado.success)
            {
                labelInstrucciones.Text = "? Sincronizaci�n exitosa";
                labelInstrucciones.ForeColor = Color.Green;
                Console.WriteLine("Sincronizaci�n exitosa.");
            }
            else
            {
                labelInstrucciones.Text = "? Error en sincronizaci�n";
                labelInstrucciones.ForeColor = Color.Red;
                MessageBox.Show($"Error al sincronizar datos: {resultado.message}", "Sincronizaci�n", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Console.WriteLine("Error en sincronizaci�n: " + resultado.message);
            }
        }
    }
}
