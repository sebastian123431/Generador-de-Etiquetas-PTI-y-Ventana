namespace WindowsFormsApplication1
{
	/// <summary>
	/// Clase parcial Principal - Parte Designer (generada automáticamente)
	/// Este archivo contiene la inicialización de todos los componentes de la interfaz gráfica
	/// del formulario principal (menú de selección)
	/// Incluye: paneles, botones de navegación, labels y configuración visual
	/// NO MODIFICAR MANUALMENTE - Usar el Designer de Visual Studio
	/// </summary>
	public partial class Principal : global::System.Windows.Forms.Form
	{
		/// <summary>
		/// Libera los recursos utilizados por el formulario principal
		/// Limpia los componentes cuando el formulario se cierra
		/// </summary>
		/// <param name="disposing">true para liberar recursos administrados y no administrados; false solo no administrados</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		/// <summary>
		/// Método requerido para compatibilidad con el Diseñador de Windows Forms
		/// Inicializa todos los componentes visuales del formulario principal
		/// Configura el menú con botones para PTI, Ventana Uvas y Ventana Cítricos
		/// GENERADO AUTOMÁTICAMENTE - No modificar directamente este código
		/// </summary>
        private void InitializeComponent()
        {
            this.panelHeader = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.labelSubtitle = new System.Windows.Forms.Label();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnPTI = new System.Windows.Forms.Button();
            this.btnVentanaUvas = new System.Windows.Forms.Button();
            this.btnVentanaCitricos = new System.Windows.Forms.Button();
            this.labelInstrucciones = new System.Windows.Forms.Label();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.labelVersion = new System.Windows.Forms.Label();
            this.panelHeader.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.panelFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
            this.panelHeader.Controls.Add(this.labelTitle);
            this.panelHeader.Controls.Add(this.labelSubtitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1307, 120);
            this.panelHeader.TabIndex = 0;
            // 
            // labelTitle
            // 
            this.labelTitle.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelTitle.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.ForeColor = System.Drawing.Color.White;
            this.labelTitle.Location = new System.Drawing.Point(0, 20);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(1307, 46);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "ATACAMA - Sistema de Etiquetado";
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSubtitle
            // 
            this.labelSubtitle.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelSubtitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80))))); 
            this.labelSubtitle.Location = new System.Drawing.Point(0, 70);
            this.labelSubtitle.Name = "labelSubtitle";
            this.labelSubtitle.Size = new System.Drawing.Size(1307, 32);
            this.labelSubtitle.TabIndex = 1;
            this.labelSubtitle.Text = "Generador de Etiquetas para Cajas y Pallets";
            this.labelSubtitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelMain
            // 
            this.panelMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panelMain.Controls.Add(this.panelButtons);
            this.panelMain.Controls.Add(this.labelInstrucciones);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 120);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1307, 730);
            this.panelMain.TabIndex = 1;
            // 
            // panelButtons
            // 
            this.panelButtons.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelButtons.Controls.Add(this.btnPTI);
            this.panelButtons.Controls.Add(this.btnVentanaUvas);
            this.panelButtons.Controls.Add(this.btnVentanaCitricos);
            this.panelButtons.Location = new System.Drawing.Point(304, 200);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(700, 300);
            this.panelButtons.TabIndex = 1;
            // 
            // btnPTI
            // 
            this.btnPTI.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
            this.btnPTI.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPTI.FlatAppearance.BorderSize = 0;
            this.btnPTI.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPTI.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPTI.ForeColor = System.Drawing.Color.White;
            this.btnPTI.Location = new System.Drawing.Point(150, 20);
            this.btnPTI.Name = "btnPTI";
            this.btnPTI.Size = new System.Drawing.Size(400, 70);
            this.btnPTI.TabIndex = 0;
            this.btnPTI.Text = "PTI";
            this.btnPTI.UseVisualStyleBackColor = false;
            this.btnPTI.Click += new System.EventHandler(this.pTIToolStripMenuItem_Click);
            this.btnPTI.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.btnPTI.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // btnVentanaUvas
            // 
            this.btnVentanaUvas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
            this.btnVentanaUvas.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnVentanaUvas.FlatAppearance.BorderSize = 0;
            this.btnVentanaUvas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVentanaUvas.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVentanaUvas.ForeColor = System.Drawing.Color.White;
            this.btnVentanaUvas.Location = new System.Drawing.Point(150, 110);
            this.btnVentanaUvas.Name = "btnVentanaUvas";
            this.btnVentanaUvas.Size = new System.Drawing.Size(400, 70);
            this.btnVentanaUvas.TabIndex = 1;
            this.btnVentanaUvas.Text = "VENTANA UVAS";
            this.btnVentanaUvas.UseVisualStyleBackColor = false;
            this.btnVentanaUvas.Click += new System.EventHandler(this.vENTANAToolStripMenuItem_Click);
            this.btnVentanaUvas.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.btnVentanaUvas.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // btnVentanaCitricos
            // 
            this.btnVentanaCitricos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
            this.btnVentanaCitricos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnVentanaCitricos.FlatAppearance.BorderSize = 0;
            this.btnVentanaCitricos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVentanaCitricos.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVentanaCitricos.ForeColor = System.Drawing.Color.White;
            this.btnVentanaCitricos.Location = new System.Drawing.Point(150, 200);
            this.btnVentanaCitricos.Name = "btnVentanaCitricos";
            this.btnVentanaCitricos.Size = new System.Drawing.Size(400, 70);
            this.btnVentanaCitricos.TabIndex = 2;
            this.btnVentanaCitricos.Text = "VENTANA CÍTRICOS";
            this.btnVentanaCitricos.UseVisualStyleBackColor = false;
            this.btnVentanaCitricos.Click += new System.EventHandler(this.vENTANACITRICOSToolStripMenuItem_Click);
            this.btnVentanaCitricos.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.btnVentanaCitricos.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // labelInstrucciones
            // 
            this.labelInstrucciones.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelInstrucciones.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInstrucciones.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
            this.labelInstrucciones.Location = new System.Drawing.Point(0, 120);
            this.labelInstrucciones.Name = "labelInstrucciones";
            this.labelInstrucciones.Size = new System.Drawing.Size(1307, 32);
            this.labelInstrucciones.TabIndex = 0;
            this.labelInstrucciones.Text = "Seleccione una opción para continuar";
            this.labelInstrucciones.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelFooter
            // 
            this.panelFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.panelFooter.Controls.Add(this.labelVersion);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(0, 850);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Size = new System.Drawing.Size(1307, 50);
            this.panelFooter.TabIndex = 2;
            // 
            // labelVersion
            // 
            this.labelVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelVersion.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVersion.ForeColor = System.Drawing.Color.White;
            this.labelVersion.Location = new System.Drawing.Point(0, 0);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(1307, 50);
            this.labelVersion.TabIndex = 0;
            this.labelVersion.Text = "Version 07/01/2026 V1.0 - FEAL - Temp. 2025/2026 - MRC";
            this.labelVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Principal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1307, 900);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelFooter);
            this.Controls.Add(this.panelHeader);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Principal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ATACAMA - Sistema de Etiquetado";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Principal_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelMain.ResumeLayout(false);
            this.panelButtons.ResumeLayout(false);
            this.panelFooter.ResumeLayout(false);
            this.ResumeLayout(false);

		}

        // Token: 0x0400014C RID: 332
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x0400014D RID: 333
		private global::System.Windows.Forms.Panel panelHeader;

		// Token: 0x0400014E RID: 334
		private global::System.Windows.Forms.Panel panelMain;

		// Token: 0x0400014F RID: 335
		private global::System.Windows.Forms.Panel panelFooter;

		// Token: 0x04000150 RID: 336
		private global::System.Windows.Forms.Panel panelButtons;

		// Token: 0x04000151 RID: 337
		private global::System.Windows.Forms.Label labelTitle;

		// Token: 0x04000152 RID: 338
		private global::System.Windows.Forms.Label labelSubtitle;

		// Token: 0x04000153 RID: 339
		private global::System.Windows.Forms.Label labelInstrucciones;

		// Token: 0x04000154 RID: 340
		private global::System.Windows.Forms.Label labelVersion;

		// Token: 0x04000155 RID: 341
		private global::System.Windows.Forms.Button btnPTI;

		// Token: 0x04000156 RID: 342
		private global::System.Windows.Forms.Button btnVentanaUvas;

		// Token: 0x04000157 RID: 343
		private global::System.Windows.Forms.Button btnVentanaCitricos;
	}
}
