using System;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApplication1.Data;

namespace WindowsFormsApplication1
{
	/// <summary>
	/// Formulario de configuración de conexión WebSocket con Django
	/// </summary>
	public partial class frm_Configuracion : Form
	{
		private TextBox txt_servidor;
		private TextBox txt_puerto;
		private Button btn_guardar;
		private Button btn_probar;
		private Button btn_cancelar;
		private Label lbl_estado;
		private Label lbl_servidor;
		private Label lbl_puerto;
		private CheckBox chk_auto_conectar;

		public frm_Configuracion()
		{
			InitializeComponent();
			CargarConfiguracion();
		}

		private void InitializeComponent()
		{
			this.txt_servidor = new TextBox();
			this.txt_puerto = new TextBox();
			this.btn_guardar = new Button();
			this.btn_probar = new Button();
			this.btn_cancelar = new Button();
			this.lbl_estado = new Label();
			this.lbl_servidor = new Label();
			this.lbl_puerto = new Label();
			this.chk_auto_conectar = new CheckBox();
			this.SuspendLayout();

			// lbl_servidor
			this.lbl_servidor.AutoSize = true;
			this.lbl_servidor.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
			this.lbl_servidor.ForeColor = Color.FromArgb(0, 51, 102);
			this.lbl_servidor.Location = new Point(30, 30);
			this.lbl_servidor.Name = "lbl_servidor";
			this.lbl_servidor.Size = new Size(150, 23);
			this.lbl_servidor.TabIndex = 0;
			this.lbl_servidor.Text = "Servidor Django:";

			// txt_servidor
			this.txt_servidor.Font = new Font("Segoe UI", 10F);
			this.txt_servidor.Location = new Point(30, 60);
			this.txt_servidor.Name = "txt_servidor";
			this.txt_servidor.Size = new Size(400, 30);
			this.txt_servidor.TabIndex = 1;
			this.txt_servidor.Text = "localhost";

			// lbl_puerto
			this.lbl_puerto.AutoSize = true;
			this.lbl_puerto.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
			this.lbl_puerto.ForeColor = Color.FromArgb(0, 51, 102);
			this.lbl_puerto.Location = new Point(30, 110);
			this.lbl_puerto.Name = "lbl_puerto";
			this.lbl_puerto.Size = new Size(70, 23);
			this.lbl_puerto.TabIndex = 2;
			this.lbl_puerto.Text = "Puerto:";

			// txt_puerto
			this.txt_puerto.Font = new Font("Segoe UI", 10F);
			this.txt_puerto.Location = new Point(30, 140);
			this.txt_puerto.Name = "txt_puerto";
			this.txt_puerto.Size = new Size(150, 30);
			this.txt_puerto.TabIndex = 3;
			this.txt_puerto.Text = "8000";

			// chk_auto_conectar
			this.chk_auto_conectar.AutoSize = true;
			this.chk_auto_conectar.Font = new Font("Segoe UI", 9F);
			this.chk_auto_conectar.Location = new Point(30, 190);
			this.chk_auto_conectar.Name = "chk_auto_conectar";
			this.chk_auto_conectar.Size = new Size(250, 24);
			this.chk_auto_conectar.TabIndex = 4;
			this.chk_auto_conectar.Text = "Conectar automáticamente al iniciar";
			this.chk_auto_conectar.UseVisualStyleBackColor = true;

			// lbl_estado
			this.lbl_estado.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
			this.lbl_estado.ForeColor = Color.Gray;
			this.lbl_estado.Location = new Point(30, 230);
			this.lbl_estado.Name = "lbl_estado";
			this.lbl_estado.Size = new Size(400, 50);
			this.lbl_estado.TabIndex = 5;
			this.lbl_estado.Text = "No conectado";

			// btn_probar
			this.btn_probar.BackColor = Color.FromArgb(255, 165, 0);
			this.btn_probar.FlatStyle = FlatStyle.Flat;
			this.btn_probar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
			this.btn_probar.ForeColor = Color.White;
			this.btn_probar.Location = new Point(30, 300);
			this.btn_probar.Name = "btn_probar";
			this.btn_probar.Size = new Size(120, 40);
			this.btn_probar.TabIndex = 6;
			this.btn_probar.Text = "Probar";
			this.btn_probar.UseVisualStyleBackColor = false;
			this.btn_probar.Click += new EventHandler(this.btn_probar_Click);

			// btn_guardar
			this.btn_guardar.BackColor = Color.FromArgb(76, 175, 80);
			this.btn_guardar.FlatStyle = FlatStyle.Flat;
			this.btn_guardar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
			this.btn_guardar.ForeColor = Color.White;
			this.btn_guardar.Location = new Point(170, 300);
			this.btn_guardar.Name = "btn_guardar";
			this.btn_guardar.Size = new Size(120, 40);
			this.btn_guardar.TabIndex = 7;
			this.btn_guardar.Text = "Guardar";
			this.btn_guardar.UseVisualStyleBackColor = false;
			this.btn_guardar.Click += new EventHandler(this.btn_guardar_Click);

			// btn_cancelar
			this.btn_cancelar.BackColor = Color.FromArgb(200, 200, 200);
			this.btn_cancelar.FlatStyle = FlatStyle.Flat;
			this.btn_cancelar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
			this.btn_cancelar.ForeColor = Color.FromArgb(60, 60, 60);
			this.btn_cancelar.Location = new Point(310, 300);
			this.btn_cancelar.Name = "btn_cancelar";
			this.btn_cancelar.Size = new Size(120, 40);
			this.btn_cancelar.TabIndex = 8;
			this.btn_cancelar.Text = "Cancelar";
			this.btn_cancelar.UseVisualStyleBackColor = false;
			this.btn_cancelar.Click += new EventHandler(this.btn_cancelar_Click);

			// frm_Configuracion
			this.AutoScaleDimensions = new SizeF(8F, 16F);
			this.AutoScaleMode = AutoScaleMode.Font;
			this.BackColor = Color.White;
			this.ClientSize = new Size(460, 370);
			this.Controls.Add(this.btn_cancelar);
			this.Controls.Add(this.btn_guardar);
			this.Controls.Add(this.btn_probar);
			this.Controls.Add(this.lbl_estado);
			this.Controls.Add(this.chk_auto_conectar);
			this.Controls.Add(this.txt_puerto);
			this.Controls.Add(this.lbl_puerto);
			this.Controls.Add(this.txt_servidor);
			this.Controls.Add(this.lbl_servidor);
			this.FormBorderStyle = FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frm_Configuracion";
			this.StartPosition = FormStartPosition.CenterParent;
			this.Text = "Configuración de Conexión";
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		private void CargarConfiguracion()
		{
			DatabaseManager db = DatabaseManager.Instance;
			txt_servidor.Text = db.GetConfigValue("servidor", "localhost");
			txt_puerto.Text = db.GetConfigValue("puerto", "8000");
			chk_auto_conectar.Checked = db.GetConfigValue("auto_conectar", "false") == "true";
		}

		private async void btn_probar_Click(object sender, EventArgs e)
		{
			btn_probar.Enabled = false;
			lbl_estado.Text = "Probando conexión...";
			lbl_estado.ForeColor = Color.Blue;

			try
			{
				string url = $"ws://{txt_servidor.Text.Trim()}:{txt_puerto.Text.Trim()}/ws/catalogo/notificacion/";
				bool connected = await WebSocketClient.Instance.ConnectAsync(url);

				if (connected)
				{
					lbl_estado.Text = "? Conexión exitosa";
					lbl_estado.ForeColor = Color.Green;
					MessageBox.Show("Conexión establecida correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				else
				{
					lbl_estado.Text = "? Error de conexión";
					lbl_estado.ForeColor = Color.Red;
					MessageBox.Show("No se pudo conectar al servidor", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			catch (Exception ex)
			{
				lbl_estado.Text = "? Error: " + ex.Message;
				lbl_estado.ForeColor = Color.Red;
				MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				btn_probar.Enabled = true;
			}
		}

		private void btn_guardar_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(txt_servidor.Text))
			{
				MessageBox.Show("Por favor ingrese el servidor", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txt_servidor.Focus();
				return;
			}

			if (string.IsNullOrWhiteSpace(txt_puerto.Text))
			{
				MessageBox.Show("Por favor ingrese el puerto", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txt_puerto.Focus();
				return;
			}

			DatabaseManager db = DatabaseManager.Instance;
			db.SetConfigValue("servidor", txt_servidor.Text.Trim());
			db.SetConfigValue("puerto", txt_puerto.Text.Trim());
			db.SetConfigValue("auto_conectar", chk_auto_conectar.Checked ? "true" : "false");

			MessageBox.Show("Configuración guardada correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void btn_cancelar_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
	}
}
