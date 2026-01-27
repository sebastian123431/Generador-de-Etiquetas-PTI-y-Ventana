namespace WindowsFormsApplication1
{
	/// <summary>
	/// Clase parcial frm_generador - Parte Designer (generada automáticamente)
	/// Este archivo contiene la inicialización de todos los componentes de la interfaz gráfica
	/// Incluye: controles, botones, labels, textboxes, comboboxes y configuración visual
	/// NO MODIFICAR MANUALMENTE - Usar el Designer de Visual Studio
	/// </summary>
	public partial class frm_generador : global::System.Windows.Forms.Form
	{
		/// <summary>
		/// Libera los recursos utilizados por el formulario
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
		/// Inicializa todos los componentes visuales del formulario generador PTI
		/// Configura propiedades, eventos y diseño de la interfaz
		/// GENERADO AUTOMÁTICAMENTE - No modificar directamente este código
		/// </summary>
	private void InitializeComponent()
	{
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_date = new System.Windows.Forms.TextBox();
            this.lbl_codigo1 = new System.Windows.Forms.Label();
            this.lbl_codigo2 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.pb_etiqueta = new System.Windows.Forms.PictureBox();
            this.btn_copiar = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chb_pesofijo = new System.Windows.Forms.CheckBox();
            this.cbx_sdp = new System.Windows.Forms.ComboBox();
            this.label22 = new System.Windows.Forms.Label();
            this.cmb_variedad_Imprime = new System.Windows.Forms.ComboBox();
            this.cmb_cat1 = new System.Windows.Forms.ComboBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.cmb_lote = new System.Windows.Forms.ComboBox();
            this.cmb_calibre = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.cmb_productor = new System.Windows.Forms.ComboBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.cmb_packing = new System.Windows.Forms.ComboBox();
            this.txt_fecha_agricola = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txt_linea = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.cmb_tipo_embalaje = new System.Windows.Forms.ComboBox();
            this.cmb_titulo2 = new System.Windows.Forms.ComboBox();
            this.cmb_titulo1 = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.cmb_variedad = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmb_gtin = new System.Windows.Forms.ComboBox();
            this.btn_guardar = new System.Windows.Forms.Button();
            this.sfd = new System.Windows.Forms.SaveFileDialog();
            this.button7 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pb_etiqueta)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
            this.label1.Location = new System.Drawing.Point(785, 27);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "GTIN:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(145, 5);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 15);
            this.label4.TabIndex = 3;
            this.label4.Text = "dd-mm-aaaa";
            this.label4.Visible = false;
            // 
            // txt_date
            // 
            this.txt_date.Location = new System.Drawing.Point(-200, -200);
            this.txt_date.Margin = new System.Windows.Forms.Padding(4);
            this.txt_date.MaxLength = 10;
            this.txt_date.Name = "txt_date";
            this.txt_date.Size = new System.Drawing.Size(1, 1);
            this.txt_date.TabIndex = 6;
            this.txt_date.Visible = false;
            // 
            // lbl_codigo1
            // 
            this.lbl_codigo1.AutoSize = true;
            this.lbl_codigo1.BackColor = System.Drawing.Color.Black;
            this.lbl_codigo1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_codigo1.ForeColor = System.Drawing.SystemColors.Window;
            this.lbl_codigo1.Location = new System.Drawing.Point(873, 23);
            this.lbl_codigo1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_codigo1.Name = "lbl_codigo1";
            this.lbl_codigo1.Size = new System.Drawing.Size(29, 20);
            this.lbl_codigo1.TabIndex = 8;
            this.lbl_codigo1.Text = "22";
            this.lbl_codigo1.Visible = false;
            // 
            // lbl_codigo2
            // 
            this.lbl_codigo2.AutoSize = true;
            this.lbl_codigo2.BackColor = System.Drawing.Color.Black;
            this.lbl_codigo2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_codigo2.ForeColor = System.Drawing.SystemColors.Window;
            this.lbl_codigo2.Location = new System.Drawing.Point(907, 15);
            this.lbl_codigo2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_codigo2.Name = "lbl_codigo2";
            this.lbl_codigo2.Size = new System.Drawing.Size(43, 29);
            this.lbl_codigo2.TabIndex = 11;
            this.lbl_codigo2.Text = "22";
            this.lbl_codigo2.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(148, 20);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 15);
            this.label7.TabIndex = 13;
            this.label7.Text = "(Opcional)";
            this.label7.Visible = false;
            // 
            // pb_etiqueta
            // 
            this.pb_etiqueta.BackColor = System.Drawing.Color.White;
            this.pb_etiqueta.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pb_etiqueta.Location = new System.Drawing.Point(135, 307);
            this.pb_etiqueta.Margin = new System.Windows.Forms.Padding(8);
            this.pb_etiqueta.Name = "pb_etiqueta";
            this.pb_etiqueta.Padding = new System.Windows.Forms.Padding(2);
            this.pb_etiqueta.Size = new System.Drawing.Size(772, 528);
            this.pb_etiqueta.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_etiqueta.TabIndex = 19;
            this.pb_etiqueta.TabStop = false;
            // 
            // btn_copiar
            // 
            this.btn_copiar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
            this.btn_copiar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_copiar.FlatAppearance.BorderSize = 0;
            this.btn_copiar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_copiar.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_copiar.ForeColor = System.Drawing.Color.White;
            this.btn_copiar.Location = new System.Drawing.Point(930, 400);
            this.btn_copiar.Margin = new System.Windows.Forms.Padding(6);
            this.btn_copiar.Name = "btn_copiar";
            this.btn_copiar.Padding = new System.Windows.Forms.Padding(8);
            this.btn_copiar.Size = new System.Drawing.Size(160, 60);
            this.btn_copiar.TabIndex = 21;
            this.btn_copiar.Text = "📋 Copiar";
            this.btn_copiar.UseVisualStyleBackColor = false;
            this.btn_copiar.Click += new System.EventHandler(this.btn_copiar_Click);
            this.btn_copiar.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.btn_copiar.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
            this.label8.Dock = System.Windows.Forms.DockStyle.Top;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(0, 0);
            this.label8.Margin = new System.Windows.Forms.Padding(0);
            this.label8.Name = "label8";
            this.label8.Padding = new System.Windows.Forms.Padding(0, 20, 0, 20);
            this.label8.Size = new System.Drawing.Size(1113, 84);
            this.label8.TabIndex = 22;
            this.label8.Text = "🏷️ Generador de Etiqueta - PTI";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label8.Click += new System.EventHandler(this.label8_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
            this.label9.Location = new System.Drawing.Point(391, 161);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(51, 20);
            this.label9.TabIndex = 23;
            this.label9.Text = "Color:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
            this.label10.Location = new System.Drawing.Point(45, 28);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(46, 20);
            this.label10.TabIndex = 24;
            this.label10.Text = "Peso:";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Controls.Add(this.chb_pesofijo);
            this.groupBox1.Controls.Add(this.cbx_sdp);
            this.groupBox1.Controls.Add(this.label22);
            this.groupBox1.Controls.Add(this.cmb_variedad_Imprime);
            this.groupBox1.Controls.Add(this.cmb_cat1);
            this.groupBox1.Controls.Add(this.label21);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.cmb_lote);
            this.groupBox1.Controls.Add(this.cmb_calibre);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.cmb_productor);
            this.groupBox1.Controls.Add(this.label20);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.cmb_packing);
            this.groupBox1.Controls.Add(this.txt_fecha_agricola);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.txt_linea);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.cmb_tipo_embalaje);
            this.groupBox1.Controls.Add(this.cmb_titulo2);
            this.groupBox1.Controls.Add(this.cmb_titulo1);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.cmb_variedad);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cmb_gtin);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
            this.groupBox1.Location = new System.Drawing.Point(16, 86);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(15, 10, 15, 15);
            this.groupBox1.Size = new System.Drawing.Size(1076, 201);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "  📋 Datos de Etiqueta  ";
            // 
            // chb_pesofijo
            // 
            this.chb_pesofijo.AutoSize = true;
            this.chb_pesofijo.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chb_pesofijo.Location = new System.Drawing.Point(848, 0);
            this.chb_pesofijo.Margin = new System.Windows.Forms.Padding(4);
            this.chb_pesofijo.Name = "chb_pesofijo";
            this.chb_pesofijo.Size = new System.Drawing.Size(101, 23);
            this.chb_pesofijo.TabIndex = 58;
            this.chb_pesofijo.Text = "Es Peso Fijo";
            this.chb_pesofijo.UseVisualStyleBackColor = true;
            this.chb_pesofijo.CheckedChanged += new System.EventHandler(this.chb_pesofijo_CheckedChanged);
            // 
            // cbx_sdp
            // 
            this.cbx_sdp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbx_sdp.Enabled = false;
            this.cbx_sdp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbx_sdp.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbx_sdp.FormattingEnabled = true;
            this.cbx_sdp.Location = new System.Drawing.Point(848, 123);
            this.cbx_sdp.Margin = new System.Windows.Forms.Padding(4);
            this.cbx_sdp.Name = "cbx_sdp";
            this.cbx_sdp.Size = new System.Drawing.Size(160, 28);
            this.cbx_sdp.TabIndex = 57;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
            this.label22.Location = new System.Drawing.Point(800, 128);
            this.label22.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(40, 20);
            this.label22.TabIndex = 56;
            this.label22.Text = "SdP:";
            // 
            // cmb_variedad_Imprime
            // 
            this.cmb_variedad_Imprime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_variedad_Imprime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmb_variedad_Imprime.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmb_variedad_Imprime.FormattingEnabled = true;
            this.cmb_variedad_Imprime.Location = new System.Drawing.Point(444, 123);
            this.cmb_variedad_Imprime.Margin = new System.Windows.Forms.Padding(4);
            this.cmb_variedad_Imprime.Name = "cmb_variedad_Imprime";
            this.cmb_variedad_Imprime.Size = new System.Drawing.Size(281, 28);
            this.cmb_variedad_Imprime.TabIndex = 54;
            this.cmb_variedad_Imprime.SelectedIndexChanged += new System.EventHandler(this.cmb_variedad_Imprime_SelectedIndexChanged);
            // 
            // cmb_cat1
            // 
            this.cmb_cat1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_cat1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmb_cat1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmb_cat1.FormattingEnabled = true;
            this.cmb_cat1.Items.AddRange(new object[] {
            "Normal",
            "Europa"});
            this.cmb_cat1.Location = new System.Drawing.Point(99, 90);
            this.cmb_cat1.Margin = new System.Windows.Forms.Padding(4);
            this.cmb_cat1.Name = "cmb_cat1";
            this.cmb_cat1.Size = new System.Drawing.Size(160, 28);
            this.cmb_cat1.TabIndex = 49;
            this.cmb_cat1.SelectedIndexChanged += new System.EventHandler(this.cmb_cat1_SelectedIndexChanged);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
            this.label21.Location = new System.Drawing.Point(292, 127);
            this.label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(139, 20);
            this.label21.TabIndex = 55;
            this.label21.Text = "Variedad Imprime:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
            this.label11.Location = new System.Drawing.Point(8, 94);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(87, 20);
            this.label11.TabIndex = 50;
            this.label11.Text = "CAT (SAG):";
            // 
            // cmb_lote
            // 
            this.cmb_lote.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_lote.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmb_lote.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmb_lote.FormattingEnabled = true;
            this.cmb_lote.Location = new System.Drawing.Point(444, 90);
            this.cmb_lote.Margin = new System.Windows.Forms.Padding(4);
            this.cmb_lote.Name = "cmb_lote";
            this.cmb_lote.Size = new System.Drawing.Size(281, 28);
            this.cmb_lote.TabIndex = 53;
            this.cmb_lote.SelectedIndexChanged += new System.EventHandler(this.cmb_lote_SelectedIndexChanged);
            // 
            // cmb_calibre
            // 
            this.cmb_calibre.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_calibre.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmb_calibre.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmb_calibre.FormattingEnabled = true;
            this.cmb_calibre.Location = new System.Drawing.Point(848, 90);
            this.cmb_calibre.Margin = new System.Windows.Forms.Padding(4);
            this.cmb_calibre.Name = "cmb_calibre";
            this.cmb_calibre.Size = new System.Drawing.Size(160, 28);
            this.cmb_calibre.TabIndex = 47;
            this.cmb_calibre.SelectedIndexChanged += new System.EventHandler(this.cmb_calibre_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
            this.label6.Location = new System.Drawing.Point(388, 94);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 20);
            this.label6.TabIndex = 51;
            this.label6.Text = "Lote:";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(159, 160);
            this.label19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(89, 15);
            this.label19.TabIndex = 46;
            this.label19.Text = "2 Alfanumérico";
            // 
            // cmb_productor
            // 
            this.cmb_productor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_productor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmb_productor.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmb_productor.FormattingEnabled = true;
            this.cmb_productor.Location = new System.Drawing.Point(444, 23);
            this.cmb_productor.Margin = new System.Windows.Forms.Padding(4);
            this.cmb_productor.Name = "cmb_productor";
            this.cmb_productor.Size = new System.Drawing.Size(281, 28);
            this.cmb_productor.TabIndex = 51;
            this.cmb_productor.SelectedIndexChanged += new System.EventHandler(this.cmb_productor_SelectedIndexChanged);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
            this.label20.Location = new System.Drawing.Point(743, 94);
            this.label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(107, 20);
            this.label20.TabIndex = 48;
            this.label20.Text = "Calibre (SAG):";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(164, 128);
            this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(55, 15);
            this.label18.TabIndex = 45;
            this.label18.Text = "4 Dígitos";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
            this.label17.Location = new System.Drawing.Point(25, 62);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(67, 20);
            this.label17.TabIndex = 44;
            this.label17.Text = "Packing:";
            // 
            // cmb_packing
            // 
            this.cmb_packing.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_packing.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmb_packing.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmb_packing.FormattingEnabled = true;
            this.cmb_packing.Items.AddRange(new object[] {
            "120 P. Viña del Cerro                             107131",
            "122 P. Nantoco                                    107130",
            "123 P. Las Compuertas                             107132",
            "124 P. Las Terrazas                               107133",
            "126 P. Huancara                                   107134",
            "127 P. Maitencillo                                107135",
            "128 P. Huancara Terreno                           176837",
            "129 P. Maitencillo Terreno                        176838",
            "130 P. Santa Bernardita Terreno                   176839",
            "131 P. Santa Adriana Terreno                      176840",
            "132 P. La Compañia Terreno                        176841",
            "133 P. Las Pintadas Terreno                       176842",
            "134 P. Viña del Cerro I Terreno                   176855",
            "135 P. Viña del Cerro II Terreno                  176856",
            "136 P. Buenos Aires Terreno                       176857",
            "137 P. Hornitos Terreno                           176858",
            "138 P. Parcela 15 Terreno                         176861",
            "139 P. Santa Laura Terreno                        176862",
            "140 P. Las Compuertas Terreno                     176863",
            "141 P. El Chile Terreno                           176864",
            "142 P. La Ballena Terreno                         176865",
            "143 P. Las Terrazas Terreno                       176867",
            "144 P. Las Lomas Terreno                          176868",
            "146 P. Los Pimientos Terreno                      3101432",
            "147 P. Diaguitas                                  117471"});
            this.cmb_packing.Location = new System.Drawing.Point(99, 57);
            this.cmb_packing.Margin = new System.Windows.Forms.Padding(4);
            this.cmb_packing.Name = "cmb_packing";
            this.cmb_packing.Size = new System.Drawing.Size(233, 28);
            this.cmb_packing.TabIndex = 4;
            this.cmb_packing.SelectedIndexChanged += new System.EventHandler(this.cmb_packing_SelectedIndexChanged);
            // 
            // txt_fecha_agricola
            // 
            this.txt_fecha_agricola.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_fecha_agricola.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_fecha_agricola.Location = new System.Drawing.Point(99, 123);
            this.txt_fecha_agricola.Margin = new System.Windows.Forms.Padding(4);
            this.txt_fecha_agricola.MaxLength = 4;
            this.txt_fecha_agricola.Name = "txt_fecha_agricola";
            this.txt_fecha_agricola.Size = new System.Drawing.Size(56, 27);
            this.txt_fecha_agricola.TabIndex = 11;
            this.txt_fecha_agricola.TextChanged += new System.EventHandler(this.txt_fecha_agricola_TextChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
            this.label15.Location = new System.Drawing.Point(5, 127);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(91, 20);
            this.label15.TabIndex = 39;
            this.label15.Text = "Fecha Agri.:";
            // 
            // txt_linea
            // 
            this.txt_linea.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_linea.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_linea.Location = new System.Drawing.Point(99, 155);
            this.txt_linea.Margin = new System.Windows.Forms.Padding(4);
            this.txt_linea.MaxLength = 2;
            this.txt_linea.Name = "txt_linea";
            this.txt_linea.Size = new System.Drawing.Size(56, 27);
            this.txt_linea.TabIndex = 12;
            this.txt_linea.TextChanged += new System.EventHandler(this.txt_linea_TextChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
            this.label14.Location = new System.Drawing.Point(43, 159);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(50, 20);
            this.label14.TabIndex = 37;
            this.label14.Text = "Linea:";
            // 
            // cmb_tipo_embalaje
            // 
            this.cmb_tipo_embalaje.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_tipo_embalaje.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmb_tipo_embalaje.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmb_tipo_embalaje.FormattingEnabled = true;
            this.cmb_tipo_embalaje.Location = new System.Drawing.Point(848, 57);
            this.cmb_tipo_embalaje.Margin = new System.Windows.Forms.Padding(4);
            this.cmb_tipo_embalaje.Name = "cmb_tipo_embalaje";
            this.cmb_tipo_embalaje.Size = new System.Drawing.Size(160, 28);
            this.cmb_tipo_embalaje.TabIndex = 7;
            this.cmb_tipo_embalaje.SelectedIndexChanged += new System.EventHandler(this.cmb_tipo_embalaje_SelectedIndexChanged);
            // 
            // cmb_titulo2
            // 
            this.cmb_titulo2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_titulo2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmb_titulo2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmb_titulo2.FormattingEnabled = true;
            this.cmb_titulo2.Items.AddRange(new object[] {
            "18 lb - 8,2 kg",
            "20 lb - 9,1 kg",
            "16 lb - 7,3 kg",
            "4,5 kg",
            "5 kg"});
            this.cmb_titulo2.Location = new System.Drawing.Point(99, 23);
            this.cmb_titulo2.Margin = new System.Windows.Forms.Padding(4);
            this.cmb_titulo2.Name = "cmb_titulo2";
            this.cmb_titulo2.Size = new System.Drawing.Size(236, 28);
            this.cmb_titulo2.TabIndex = 2;
            this.cmb_titulo2.SelectedIndexChanged += new System.EventHandler(this.cmb_titulo2_SelectedIndexChanged);
            // 
            // cmb_titulo1
            // 
            this.cmb_titulo1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_titulo1.Enabled = false;
            this.cmb_titulo1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmb_titulo1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmb_titulo1.FormattingEnabled = true;
            this.cmb_titulo1.Items.AddRange(new object[] {
            "Table Grapes, Black Seedless",
            "Table Grapes, Green Seedless",
            "Table Grapes, Red Seedless",
            "Table Grapes, Red Seeded"});
            this.cmb_titulo1.Location = new System.Drawing.Point(444, 156);
            this.cmb_titulo1.Margin = new System.Windows.Forms.Padding(4);
            this.cmb_titulo1.Name = "cmb_titulo1";
            this.cmb_titulo1.Size = new System.Drawing.Size(281, 28);
            this.cmb_titulo1.TabIndex = 1;
            this.cmb_titulo1.SelectedIndexChanged += new System.EventHandler(this.cmb_titulo1_SelectedIndexChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
            this.label16.Location = new System.Drawing.Point(737, 60);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(111, 20);
            this.label16.TabIndex = 41;
            this.label16.Text = "Tipo Embalaje:";
            // 
            // cmb_variedad
            // 
            this.cmb_variedad.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_variedad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmb_variedad.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmb_variedad.FormattingEnabled = true;
            this.cmb_variedad.Location = new System.Drawing.Point(444, 57);
            this.cmb_variedad.Margin = new System.Windows.Forms.Padding(4);
            this.cmb_variedad.Name = "cmb_variedad";
            this.cmb_variedad.Size = new System.Drawing.Size(281, 28);
            this.cmb_variedad.TabIndex = 6;
            this.cmb_variedad.SelectedIndexChanged += new System.EventHandler(this.cmb_variedad_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
            this.label2.Location = new System.Drawing.Point(348, 27);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 20);
            this.label2.TabIndex = 31;
            this.label2.Text = "Productor:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
            this.label5.Location = new System.Drawing.Point(355, 60);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 20);
            this.label5.TabIndex = 32;
            this.label5.Text = "Variedad:";
            // 
            // cmb_gtin
            // 
            this.cmb_gtin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_gtin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmb_gtin.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmb_gtin.FormattingEnabled = true;
            this.cmb_gtin.Location = new System.Drawing.Point(848, 23);
            this.cmb_gtin.Margin = new System.Windows.Forms.Padding(5);
            this.cmb_gtin.Name = "cmb_gtin";
            this.cmb_gtin.Size = new System.Drawing.Size(199, 31);
            this.cmb_gtin.TabIndex = 5;
            this.cmb_gtin.SelectedIndexChanged += new System.EventHandler(this.cmb_gtin_SelectedIndexChanged);
            // 
            // btn_guardar
            // 
            this.btn_guardar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.btn_guardar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_guardar.FlatAppearance.BorderSize = 0;
            this.btn_guardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_guardar.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_guardar.ForeColor = System.Drawing.Color.White;
            this.btn_guardar.Location = new System.Drawing.Point(930, 478);
            this.btn_guardar.Margin = new System.Windows.Forms.Padding(6);
            this.btn_guardar.Name = "btn_guardar";
            this.btn_guardar.Padding = new System.Windows.Forms.Padding(8);
            this.btn_guardar.Size = new System.Drawing.Size(160, 60);
            this.btn_guardar.TabIndex = 28;
            this.btn_guardar.Text = "💾 Guardar";
            this.btn_guardar.UseVisualStyleBackColor = false;
            this.btn_guardar.Click += new System.EventHandler(this.btn_guardar_Click);
            this.btn_guardar.MouseEnter += new System.EventHandler(this.ButtonSave_MouseEnter);
            this.btn_guardar.MouseLeave += new System.EventHandler(this.ButtonSave_MouseLeave);
            // 
            // sfd
            // 
            this.sfd.Filter = "JPG files (*.jpg)|*.jpg";
            this.sfd.Title = "Guardar Etiqueta";
            // 
            // button7
            // 
            this.button7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
            this.button7.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button7.FlatAppearance.BorderSize = 0;
            this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button7.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button7.ForeColor = System.Drawing.Color.White;
            this.button7.Location = new System.Drawing.Point(930, 307);
            this.button7.Margin = new System.Windows.Forms.Padding(6);
            this.button7.Name = "button7";
            this.button7.Padding = new System.Windows.Forms.Padding(8);
            this.button7.Size = new System.Drawing.Size(160, 75);
            this.button7.TabIndex = 36;
            this.button7.Text = "⚡ Generar PTI\r\n80x65";
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            this.button7.MouseEnter += new System.EventHandler(this.Button_MouseEnter);
            this.button7.MouseLeave += new System.EventHandler(this.Button_MouseLeave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "Pack Date:";
            this.label3.Visible = false;
            // 
            // frm_generador
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(1000, 800);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.btn_guardar);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txt_date);
            this.Controls.Add(this.btn_copiar);
            this.Controls.Add(this.pb_etiqueta);
            this.Controls.Add(this.lbl_codigo2);
            this.Controls.Add(this.lbl_codigo1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "frm_generador";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "  ";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pb_etiqueta)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		/// <summary>
		/// Contenedor de componentes del formulario
		/// Administra la liberación de recursos de todos los controles
		/// </summary>
		private global::System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Label "GTIN:" - Etiqueta descriptiva para el campo de código GTIN
		/// Ubicación: Superior derecha del formulario, junto al ComboBox de GTIN
		/// Fuente: Segoe UI 10pt Bold, Color: Azul oscuro (0,51,102)
		/// </summary>
		private global::System.Windows.Forms.Label label1;

		/// <summary>
		/// Label "dd-mm-aaaa" - Formato de fecha (OCULTO)
		/// Control no visible, usado para referencia de formato de fecha
		/// Fuente: Microsoft Sans Serif 7pt
		/// </summary>
		private global::System.Windows.Forms.Label label4;

		/// <summary>
		/// TextBox txt_date - Campo de fecha opcional (OCULTO)
		/// Posición fuera de pantalla (-200,-200), no visible
		/// MaxLength: 10 caracteres
		/// Uso: Fecha de empaque opcional para Voice Pick Code con fecha
		/// </summary>
		private global::System.Windows.Forms.TextBox txt_date;

		/// <summary>
		/// Label lbl_codigo1 - Primer dígito del Voice Pick Code (OCULTO por defecto)
		/// Muestra los primeros 2 dígitos del código de 4 dígitos
		/// BackColor: Negro, ForeColor: Blanco
		/// Fuente: Microsoft Sans Serif 10pt Bold
		/// Se hace visible después de generar el código
		/// </summary>
		private global::System.Windows.Forms.Label lbl_codigo1;

		/// <summary>
		/// Label lbl_codigo2 - Segundo par de dígitos del Voice Pick Code (OCULTO por defecto)
		/// Muestra los últimos 2 dígitos del código de 4 dígitos
		/// BackColor: Negro, ForeColor: Blanco
		/// Fuente: Microsoft Sans Serif 15pt Bold (más grande que lbl_codigo1)
		/// Se hace visible después de generar el código
		/// </summary>
		private global::System.Windows.Forms.Label lbl_codigo2;

		/// <summary>
		/// Label "(Opcional)" - Texto indicativo (OCULTO)
		/// Control no visible, usado para indicar campos opcionales
		/// Fuente: Microsoft Sans Serif 7pt
		/// </summary>
		private global::System.Windows.Forms.Label label7;

		/// <summary>
		/// PictureBox pb_etiqueta - Vista previa de la etiqueta generada
		/// Tamaño: 772x528 píxeles
		/// BackColor: Blanco, BorderStyle: FixedSingle
		/// SizeMode: StretchImage (estira imagen para ajustar)
		/// Ubicación: Centro-izquierda del formulario
		/// Muestra la imagen de la etiqueta PTI generada
		/// </summary>
		private global::System.Windows.Forms.PictureBox pb_etiqueta;

		/// <summary>
		/// Button btn_copiar - Botón "📋 Copiar"
		/// Tamaño: 160x60 píxeles
		/// BackColor: Azul oscuro (0,51,102)
		/// ForeColor: Blanco
		/// Fuente: Segoe UI 11pt Bold
		/// Acción: Copia la imagen de la etiqueta al portapapeles
		/// Eventos: MouseEnter/MouseLeave para efecto hover (cambio a verde)
		/// </summary>
		private global::System.Windows.Forms.Button btn_copiar;

		/// <summary>
		/// Label label8 - Título principal del formulario
		/// Texto: "🏷️ Generador de Etiqueta - PTI"
		/// Dock: Top (ocupa todo el ancho superior)
		/// Tamaño: 1113x84 píxeles
		/// BackColor: Azul oscuro (0,51,102), ForeColor: Blanco
		/// Fuente: Segoe UI 20pt Bold
		/// TextAlign: MiddleCenter (centrado horizontal y vertical)
		/// </summary>
		private global::System.Windows.Forms.Label label8;

		/// <summary>
		/// Label "Color:" - Etiqueta descriptiva (DENTRO DE GROUPBOX)
		/// Texto: "Color:"
		/// Fuente: Segoe UI 9pt Bold, Color: Azul oscuro
		/// Ubicación: Junto al ComboBox de color/tipo de uva
		/// </summary>
		private global::System.Windows.Forms.Label label9;

		/// <summary>
		/// Label "Peso:" - Etiqueta descriptiva (DENTRO DE GROUPBOX)
		/// Texto: "Peso:"
		/// Fuente: Segoe UI 9pt Bold, Color: Azul oscuro
		/// Ubicación: Junto al ComboBox de peso neto
		/// </summary>
		private global::System.Windows.Forms.Label label10;

		/// <summary>
		/// GroupBox groupBox1 - Contenedor principal de datos de etiqueta
		/// Título: "📋 Datos de Etiqueta"
		/// Tamaño: 1076x201 píxeles
		/// BackColor: Blanco
		/// ForeColor: Azul oscuro (0,51,102)
		/// Fuente: Segoe UI 11pt Bold
		/// Contiene TODOS los controles de entrada de datos (30+ controles)
		/// </summary>
		private global::System.Windows.Forms.GroupBox groupBox1;

		/// <summary>
		/// Button btn_guardar - Botón "💾 Guardar"
		/// Tamaño: 160x60 píxeles
		/// BackColor: Verde (76,175,80)
		/// ForeColor: Blanco
		/// Fuente: Segoe UI 11pt Bold
		/// Acción: Abre diálogo para guardar imagen como archivo JPEG
		/// Eventos: MouseEnter/MouseLeave para efecto hover (verde más oscuro)
		/// </summary>
		private global::System.Windows.Forms.Button btn_guardar;

		/// <summary>
		/// SaveFileDialog sfd - Diálogo de guardar archivo
		/// Filter: "JPG files (*.jpg)|*.jpg"
		/// Title: "Guardar Etiqueta"
		/// Usado por btn_guardar para seleccionar ubicación de guardado
		/// </summary>
		private global::System.Windows.Forms.SaveFileDialog sfd;

		/// <summary>
		/// ComboBox cmb_gtin - Selector de código GTIN del producto
		/// Tamaño: 199x31 píxeles
		/// DropDownStyle: DropDownList (solo selección, no edición)
		/// Fuente: Segoe UI 10pt
		/// Contenido: Códigos GTIN de 14 dígitos según variedad seleccionada
		/// Se llena dinámicamente con Llena_GTIN() según variedad
		/// Dispara evento: cmb_gtin_SelectedIndexChanged
		/// </summary>
		private global::System.Windows.Forms.ComboBox cmb_gtin;

		/// <summary>
		/// ComboBox cmb_titulo2 - Selector de peso neto del embalaje
		/// Tamaño: 236x28 píxeles
		/// DropDownStyle: DropDownList
		/// Fuente: Segoe UI 9pt
		/// Opciones fijas:
		///   - "18 lb - 8,2 kg"
		///   - "20 lb - 9,1 kg"
		///   - "16 lb - 7,3 kg"
		///   - "4,5 kg"
		///   - "5 kg"
		/// Dispara evento: cmb_titulo2_SelectedIndexChanged
		/// </summary>
		private global::System.Windows.Forms.ComboBox cmb_titulo2;

		/// <summary>
		/// Label "Variedad:" - Etiqueta descriptiva
		/// Texto: "Variedad:"
		/// Fuente: Segoe UI 9pt Bold, Color: Azul oscuro
		/// Ubicación: Junto al ComboBox de variedad
		/// </summary>
		private global::System.Windows.Forms.Label label5;

		/// <summary>
		/// Label "Productor:" - Etiqueta descriptiva
		/// Texto: "Productor:"
		/// Fuente: Segoe UI 9pt Bold, Color: Azul oscuro
		/// Ubicación: Junto al ComboBox de productor
		/// </summary>
		private global::System.Windows.Forms.Label label2;

		/// <summary>
		/// ComboBox cmb_variedad - Selector de variedad de uva
		/// Tamaño: 281x28 píxeles
		/// DropDownStyle: DropDownList
		/// Fuente: Segoe UI 9pt
		/// Contenido: Se llena con LlenaVariedad() según productor seleccionado
		/// Ejemplos: "ALLISON", "ARRA 15", "CANDY HEARTS", etc.
		/// Dispara evento: cmb_variedad_SelectedIndexChanged
		///   → Llama a Llena_lotes() y Llena_variedad_imprime()
		/// </summary>
		private global::System.Windows.Forms.ComboBox cmb_variedad;

		/// <summary>
		/// ComboBox cmb_titulo1 - Selector de tipo de uva (DESHABILITADO)
		/// Tamaño: 281x28 píxeles
		/// Enabled: false (se llena automáticamente según variedad)
		/// DropDownStyle: DropDownList
		/// Fuente: Segoe UI 9pt
		/// Opciones:
		///   - "Table Grapes, Black Seedless"
		///   - "Table Grapes, Green Seedless"
		///   - "Table Grapes, Red Seedless"
		///   - "Table Grapes, Red Seeded"
		/// Dispara evento: cmb_titulo1_SelectedIndexChanged
		/// </summary>
		private global::System.Windows.Forms.ComboBox cmb_titulo1;

		/// <summary>
		/// ComboBox cmb_tipo_embalaje - Selector de tipo de embalaje
		/// Tamaño: 160x28 píxeles
		/// DropDownStyle: DropDownList
		/// Fuente: Segoe UI 9pt
		/// Contenido: Se llena con LlenaEmbalaje() según modo (peso fijo/variable)
		/// Peso variable: BP, BSU, BZU, PP, SL (28+ opciones)
		/// Peso fijo: CL15, CL27, CL29, CL38*, PG15 (13 opciones)
		/// Dispara evento: cmb_tipo_embalaje_SelectedIndexChanged
		/// </summary>
		private global::System.Windows.Forms.ComboBox cmb_tipo_embalaje;

		/// <summary>
		/// Label "Tipo Embalaje:" - Etiqueta descriptiva
		/// Texto: "Tipo Embalaje:"
		/// Fuente: Segoe UI 9pt Bold, Color: Azul oscuro
		/// Ubicación: Junto al ComboBox de tipo de embalaje
		/// </summary>
		private global::System.Windows.Forms.Label label16;

		/// <summary>
		/// TextBox txt_fecha_agricola - Campo de fecha agrícola
		/// Tamaño: 56x27 píxeles
		/// BorderStyle: FixedSingle
		/// Fuente: Segoe UI 9pt
		/// MaxLength: 4 caracteres
		/// Formato esperado: MMDD (ej: "0115" = 15 de Nov)
		/// Conversiones:
		///   - Busca_Fecha_PTI() → "MMM DD" (ej: "Nov 15")
		///   - Busca_Fecha_YYMMDD() → "YYMMDD" (ej: "251115")
		/// Dispara evento: txt_fecha_agricola_TextChanged
		/// </summary>
		private global::System.Windows.Forms.TextBox txt_fecha_agricola;

		/// <summary>
		/// Label "Fecha Agri.:" - Etiqueta descriptiva
		/// Texto: "Fecha Agri.:" (Fecha Agrícola)
		/// Fuente: Segoe UI 9pt Bold, Color: Azul oscuro
		/// Ubicación: Junto al TextBox de fecha agrícola
		/// </summary>
		private global::System.Windows.Forms.Label label15;

		/// <summary>
		/// TextBox txt_linea - Campo de número de línea de producción
		/// Tamaño: 56x27 píxeles
		/// BorderStyle: FixedSingle
		/// Fuente: Segoe UI 9pt
		/// MaxLength: 2 caracteres (alfanumérico)
		/// Ejemplo: "L1", "A2", "01"
		/// Se concatena con el lote en la etiqueta
		/// Dispara evento: txt_linea_TextChanged
		/// </summary>
		private global::System.Windows.Forms.TextBox txt_linea;

		/// <summary>
		/// Label "Linea:" - Etiqueta descriptiva
		/// Texto: "Linea:" (Línea de producción)
		/// Fuente: Segoe UI 9pt Bold, Color: Azul oscuro
		/// Ubicación: Junto al TextBox de línea
		/// </summary>
		private global::System.Windows.Forms.Label label14;

		/// <summary>
		/// Label "Packing:" - Etiqueta descriptiva
		/// Texto: "Packing:"
		/// Fuente: Segoe UI 9pt Bold, Color: Azul oscuro
		/// Ubicación: Junto al ComboBox de packing
		/// </summary>
		private global::System.Windows.Forms.Label label17;

		/// <summary>
		/// ComboBox cmb_packing - Selector de planta de empaque (CSP)
		/// Tamaño: 233x28 píxeles
		/// DropDownStyle: DropDownList
		/// Fuente: Segoe UI 9pt
		/// Contenido: 25 opciones fijas de plantas de empaque
		/// Formato: "### Nombre Planta    ######## CSP"
		/// Ejemplos:
		///   - "120 P. Viña del Cerro                             107131"
		///   - "126 P. Huancara                                   107134"
		///   - "146 P. Los Pimientos Terreno                      3101432"
		/// Determina provincia (Elqui/Copiapó) según código 126-147
		/// Dispara evento: cmb_packing_SelectedIndexChanged
		/// </summary>
		private global::System.Windows.Forms.ComboBox cmb_packing;

		/// <summary>
		/// Label "2 Alfanumérico" - Texto de ayuda
		/// Texto: "2 Alfanumérico"
		/// Fuente: Microsoft Sans Serif 7pt
		/// Ubicación: Debajo del TextBox de línea
		/// Indica formato esperado para el campo de línea
		/// </summary>
		private global::System.Windows.Forms.Label label19;

		/// <summary>
		/// Label "4 Dígitos" - Texto de ayuda
		/// Texto: "4 Dígitos"
		/// Fuente: Microsoft Sans Serif 7pt
		/// Ubicación: Debajo del TextBox de fecha agrícola
		/// Indica formato esperado: MMDD (4 dígitos numéricos)
		/// </summary>
		private global::System.Windows.Forms.Label label18;

		/// <summary>
		/// ComboBox cmb_calibre - Selector de calibre de uva (para SAG)
		/// Tamaño: 160x28 píxeles
		/// DropDownStyle: DropDownList
		/// Fuente: Segoe UI 9pt
		/// Contenido: Se llena con LlenaCalibres() según variedad
		/// Calibres estándar: XXJ, XJ, J, D, V, A, R, T, XXL, XL, L, M
		/// Calibres dobles: JJ, DD, VV, AA, RR
		/// Usado en etiquetas SAG para indicar tamaño
		/// Dispara evento: cmb_calibre_SelectedIndexChanged
		/// </summary>
		private global::System.Windows.Forms.ComboBox cmb_calibre;

		/// <summary>
		/// Label "Calibre (SAG):" - Etiqueta descriptiva
		/// Texto: "Calibre (SAG):"
		/// Fuente: Segoe UI 9pt Bold, Color: Azul oscuro
		/// Ubicación: Junto al ComboBox de calibre
		/// SAG = Servicio Agrícola y Ganadero (Chile)
		/// </summary>
		private global::System.Windows.Forms.Label label20;

		/// <summary>
		/// ComboBox cmb_cat1 - Selector de categoría para Europa
		/// Tamaño: 160x28 píxeles
		/// DropDownStyle: DropDownList
		/// Fuente: Segoe UI 9pt
		/// Opciones fijas:
		///   - "Normal" (sin información GLN/GGN)
		///   - "Europa" (incluye GLN y GGN en etiqueta)
		/// Afecta generación de etiquetas SAG
		/// Dispara evento: cmb_cat1_SelectedIndexChanged
		/// </summary>
		private global::System.Windows.Forms.ComboBox cmb_cat1;

		/// <summary>
		/// Label "CAT (SAG):" - Etiqueta descriptiva
		/// Texto: "CAT (SAG):"
		/// Fuente: Segoe UI 9pt Bold, Color: Azul oscuro
		/// Ubicación: Junto al ComboBox de categoría
		/// CAT = Categoría para exportación
		/// </summary>
		private global::System.Windows.Forms.Label label11;

		/// <summary>
		/// ComboBox cmb_productor - Selector de productor (CSG)
		/// Tamaño: 281x28 píxeles
		/// DropDownStyle: DropDownList
		/// Fuente: Segoe UI 9pt
		/// Contenido: Se llena con Llena_Productor() desde Catrastro.xml
		/// Formato: "######  Nombre Productor"
		/// Ejemplos: "106957 HUANCARA", "87197 LA COMPAÑÍA"
		/// Al cambiar, recarga variedades disponibles
		/// Dispara evento: cmb_productor_SelectedIndexChanged
		///   → Llama a LlenaVariedad()
		/// </summary>
		private global::System.Windows.Forms.ComboBox cmb_productor;

		/// <summary>
		/// CheckBox chb_pesofijo - Indicador de peso fijo
		/// Texto: "Es Peso Fijo"
		/// Tamaño: 101x23 píxeles
		/// Fuente: Segoe UI 8.25pt
		/// Ubicación: Esquina superior derecha del GroupBox
		/// Al marcar:
		///   - Cambia opciones de embalaje a CL/PG (clamshells/bolsas)
		///   - Modifica lógica de búsqueda de códigos finales
		/// Dispara evento: chb_pesofijo_CheckedChanged
		///   → Llama a Llena_GTIN() y LlenaEmbalaje()
		/// </summary>
		private global::System.Windows.Forms.CheckBox chb_pesofijo;

		/// <summary>
		/// Button button7 - Botón "⚡ Generar PTI 80x65"
		/// Tamaño: 160x75 píxeles
		/// BackColor: Azul oscuro (0,51,102)
		/// ForeColor: Blanco
		/// Fuente: Segoe UI 11pt Bold
		/// Acción: Genera etiqueta PTI 2022 All-in-One (formato completo)
		/// Tamaño etiqueta: 80mm x 65mm
		/// Incluye: SdP, Voice Pick Code, PLU, Code 128
		/// Dispara evento: button7_Click
		///   → Llama a DibujaEtiquetaPTI2022ALLINONE()
		/// Eventos: MouseEnter/MouseLeave para efecto hover
		/// </summary>
		private global::System.Windows.Forms.Button button7;

		/// <summary>
		/// Label "Lote:" - Etiqueta descriptiva
		/// Texto: "Lote:"
		/// Fuente: Segoe UI 9pt Bold, Color: Azul oscuro
		/// Ubicación: Junto al ComboBox de lote
		/// </summary>
		private global::System.Windows.Forms.Label label6;

		/// <summary>
		/// ComboBox cmb_lote - Selector de número de lote
		/// Tamaño: 281x28 píxeles
		/// DropDownStyle: DropDownList
		/// Fuente: Segoe UI 9pt
		/// Contenido: Se llena con Llena_lotes() según productor y variedad
		/// Filtrado: LINQ con productor AND variedad
		/// Al cambiar, recarga SdP disponibles
		/// Dispara evento: cmb_lote_SelectedIndexChanged
		///   → Llama a Llena_SDP()
		/// </summary>
		private global::System.Windows.Forms.ComboBox cmb_lote;

		/// <summary>
		/// ComboBox cmb_variedad_Imprime - Selector de variedad para impresión PTI
		/// Tamaño: 281x28 píxeles
		/// DropDownStyle: DropDownList
		/// Fuente: Segoe UI 9pt
		/// Contenido: Se llena con Llena_variedad_imprime() según variedad comercial
		/// Formato: "## Nombre Técnico - Marca™"
		/// Ejemplos:
		///   - "11 Arrafifteen - Arra15"
		///   - "35 IFG Nineteen - Candy Hearts™"
		///   - "00 Green Seedless 'Unknown Variety'"
		/// Código (##) se usa en formación del lote PTI
		/// Dispara evento: cmb_variedad_Imprime_SelectedIndexChanged
		///   → Llama a Llena_GTIN() y LlenaCalibres()
		/// </summary>
		private global::System.Windows.Forms.ComboBox cmb_variedad_Imprime;

		/// <summary>
		/// Label "Variedad Imprime:" - Etiqueta descriptiva
		/// Texto: "Variedad Imprime:"
		/// Fuente: Segoe UI 9pt Bold, Color: Azul oscuro
		/// Ubicación: Junto al ComboBox de variedad de impresión
		/// </summary>
		private global::System.Windows.Forms.Label label21;

		/// <summary>
		/// Label "SdP:" - Etiqueta descriptiva
		/// Texto: "SdP:" (Sitio de Producción)
		/// Fuente: Segoe UI 9pt Bold, Color: Azul oscuro
		/// Ubicación: Junto al ComboBox de SdP
		/// </summary>
		private global::System.Windows.Forms.Label label22;

		/// <summary>
		/// ComboBox cbx_sdp - Selector de Sitio de Producción (DESHABILITADO)
		/// Tamaño: 160x28 píxeles
		/// Enabled: false (se llena automáticamente según lote)
		/// DropDownStyle: DropDownList
		/// Fuente: Segoe UI 9pt
		/// Contenido: Se llena con Llena_SDP() según productor, variedad y lote
		/// Filtrado: LINQ con productor AND variedad AND lote
		/// Usado solo en etiquetas PTI 2022 All-in-One
		/// SdP puede ser diferente al CSG (productor)
		/// </summary>
		private global::System.Windows.Forms.ComboBox cbx_sdp;

		/// <summary>
		/// Label "Pack Date:" - Etiqueta descriptiva (OCULTO)
		/// Texto: "Pack Date:"
		/// Fuente: Microsoft Sans Serif 8.25pt Bold
		/// Control no visible, referencia histórica
		/// </summary>
		private System.Windows.Forms.Label label3;
    }
}
