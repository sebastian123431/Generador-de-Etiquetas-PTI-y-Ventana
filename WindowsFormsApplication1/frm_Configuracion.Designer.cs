namespace WindowsFormsApplication1
{
	partial class frm_Configuracion
	{
		private System.ComponentModel.IContainer components = null;

		private System.Windows.Forms.TextBox txt_servidor;
		private System.Windows.Forms.TextBox txt_puerto;
		private System.Windows.Forms.Button btn_guardar;
		private System.Windows.Forms.Button btn_probar;
		private System.Windows.Forms.Button btn_cancelar;
		private System.Windows.Forms.Label lbl_estado;
		private System.Windows.Forms.Label lbl_servidor;
		private System.Windows.Forms.Label lbl_puerto;
		private System.Windows.Forms.CheckBox chk_auto_conectar;

		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.txt_servidor = new System.Windows.Forms.TextBox();
			this.txt_puerto = new System.Windows.Forms.TextBox();
			this.btn_guardar = new System.Windows.Forms.Button();
			this.btn_probar = new System.Windows.Forms.Button();
			this.btn_cancelar = new System.Windows.Forms.Button();
			this.lbl_estado = new System.Windows.Forms.Label();
			this.lbl_servidor = new System.Windows.Forms.Label();
			this.lbl_puerto = new System.Windows.Forms.Label();
			this.chk_auto_conectar = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();

			// lbl_servidor
			this.lbl_servidor.AutoSize = true;
			this.lbl_servidor.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.lbl_servidor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
			this.lbl_servidor.Location = new System.Drawing.Point(30, 30);
			this.lbl_servidor.Name = "lbl_servidor";
			this.lbl_servidor.Size = new System.Drawing.Size(150, 23);
			this.lbl_servidor.TabIndex = 0;
			this.lbl_servidor.Text = "Servidor Django:";

			// txt_servidor
			this.txt_servidor.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.txt_servidor.Location = new System.Drawing.Point(30, 60);
			this.txt_servidor.Name = "txt_servidor";
			this.txt_servidor.Size = new System.Drawing.Size(400, 27);
			this.txt_servidor.TabIndex = 1;
			this.txt_servidor.Text = "192.168.3.30";

			// lbl_puerto
			this.lbl_puerto.AutoSize = true;
			this.lbl_puerto.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.lbl_puerto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
			this.lbl_puerto.Location = new System.Drawing.Point(30, 110);
			this.lbl_puerto.Name = "lbl_puerto";
			this.lbl_puerto.Size = new System.Drawing.Size(70, 23);
			this.lbl_puerto.TabIndex = 2;
			this.lbl_puerto.Text = "Puerto:";

			// txt_puerto
			this.txt_puerto.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.txt_puerto.Location = new System.Drawing.Point(30, 140);
			this.txt_puerto.Name = "txt_puerto";
			this.txt_puerto.Size = new System.Drawing.Size(150, 27);
			this.txt_puerto.TabIndex = 3;
			this.txt_puerto.Text = "8001";

			// chk_auto_conectar
			this.chk_auto_conectar.AutoSize = true;
			this.chk_auto_conectar.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.chk_auto_conectar.Location = new System.Drawing.Point(30, 190);
			this.chk_auto_conectar.Name = "chk_auto_conectar";
			this.chk_auto_conectar.Size = new System.Drawing.Size(250, 24);
			this.chk_auto_conectar.TabIndex = 4;
			this.chk_auto_conectar.Text = "Conectar automaticamente al iniciar";
			this.chk_auto_conectar.UseVisualStyleBackColor = true;

			// lbl_estado
			this.lbl_estado.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
			this.lbl_estado.ForeColor = System.Drawing.Color.Gray;
			this.lbl_estado.Location = new System.Drawing.Point(30, 230);
			this.lbl_estado.Name = "lbl_estado";
			this.lbl_estado.Size = new System.Drawing.Size(400, 50);
			this.lbl_estado.TabIndex = 5;
			this.lbl_estado.Text = "No conectado";

			// btn_probar
			this.btn_probar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(165)))), ((int)(((byte)(0)))));
			this.btn_probar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btn_probar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.btn_probar.ForeColor = System.Drawing.Color.White;
			this.btn_probar.Location = new System.Drawing.Point(30, 300);
			this.btn_probar.Name = "btn_probar";
			this.btn_probar.Size = new System.Drawing.Size(120, 40);
			this.btn_probar.TabIndex = 6;
			this.btn_probar.Text = "Probar";
			this.btn_probar.UseVisualStyleBackColor = false;
			this.btn_probar.Click += new System.EventHandler(this.btn_probar_Click);

			// btn_guardar
			this.btn_guardar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
			this.btn_guardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btn_guardar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.btn_guardar.ForeColor = System.Drawing.Color.White;
			this.btn_guardar.Location = new System.Drawing.Point(170, 300);
			this.btn_guardar.Name = "btn_guardar";
			this.btn_guardar.Size = new System.Drawing.Size(120, 40);
			this.btn_guardar.TabIndex = 7;
			this.btn_guardar.Text = "Guardar";
			this.btn_guardar.UseVisualStyleBackColor = false;
			this.btn_guardar.Click += new System.EventHandler(this.btn_guardar_Click);

			// btn_cancelar
			this.btn_cancelar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
			this.btn_cancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btn_cancelar.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.btn_cancelar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
			this.btn_cancelar.Location = new System.Drawing.Point(310, 300);
			this.btn_cancelar.Name = "btn_cancelar";
			this.btn_cancelar.Size = new System.Drawing.Size(120, 40);
			this.btn_cancelar.TabIndex = 8;
			this.btn_cancelar.Text = "Cancelar";
			this.btn_cancelar.UseVisualStyleBackColor = false;
			this.btn_cancelar.Click += new System.EventHandler(this.btn_cancelar_Click);

			// frm_Configuracion
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(460, 370);
			this.Controls.Add(this.btn_cancelar);
			this.Controls.Add(this.btn_guardar);
			this.Controls.Add(this.btn_probar);
			this.Controls.Add(this.lbl_estado);
			this.Controls.Add(this.chk_auto_conectar);
			this.Controls.Add(this.txt_puerto);
			this.Controls.Add(this.lbl_puerto);
			this.Controls.Add(this.txt_servidor);
			this.Controls.Add(this.lbl_servidor);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frm_Configuracion";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Configuracion de Conexion";
			this.ResumeLayout(false);
			this.PerformLayout();
		}
	}
}
