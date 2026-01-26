namespace WindowsFormsApplication1
{
	/// <summary>
	/// Clase parcial frm_generadorVentana - Parte Designer (generada automáticamente)
	/// Este archivo contiene la inicialización de todos los componentes de la interfaz gráfica
	/// para el generador de etiquetas de VENTANA estándar
	/// Incluye: controles, botones, labels, textboxes, comboboxes y configuración visual
	/// NO MODIFICAR MANUALMENTE - Usar el Designer de Visual Studio
	/// </summary>
	public partial class frm_generadorVentana : global::System.Windows.Forms.Form
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
		/// Inicializa todos los componentes visuales del formulario generador de Ventana
		/// Configura propiedades, eventos y diseño de la interfaz
		/// GENERADO AUTOMÁTICAMENTE - No modificar directamente este código
		/// </summary>
	private void InitializeComponent()
	{
            this.pb_etiqueta = new System.Windows.Forms.PictureBox();
            this.btn_copiar = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbx_sdp = new System.Windows.Forms.ComboBox();
            this.label22 = new System.Windows.Forms.Label();
            this.cmb_variedad_Imprime = new System.Windows.Forms.ComboBox();
            this.chb_pesofijo = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.cmb_lote = new System.Windows.Forms.ComboBox();
            this.cmb_cat1 = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cbx_pallets = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cmb_Recibidor = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmb_calibre = new System.Windows.Forms.ComboBox();
            this.button5 = new System.Windows.Forms.Button();
            this.cmb_productor = new System.Windows.Forms.ComboBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.cmb_titulo2 = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txt_fecha_agricola = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.cmb_packing = new System.Windows.Forms.ComboBox();
            this.txt_linea = new System.Windows.Forms.TextBox();
            this.cmb_tipo_embalaje = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.cmb_variedad = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_guardar = new System.Windows.Forms.Button();
            this.sfd = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.pb_etiqueta)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pb_etiqueta
            // 
            this.pb_etiqueta.BackColor = System.Drawing.Color.White;
            this.pb_etiqueta.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pb_etiqueta.Location = new System.Drawing.Point(7, 96);
            this.pb_etiqueta.Margin = new System.Windows.Forms.Padding(8);
            this.pb_etiqueta.Name = "pb_etiqueta";
            this.pb_etiqueta.Padding = new System.Windows.Forms.Padding(2);
            this.pb_etiqueta.Size = new System.Drawing.Size(1146, 420);
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
            this.btn_copiar.Location = new System.Drawing.Point(797, 223);
            this.btn_copiar.Margin = new System.Windows.Forms.Padding(6);
            this.btn_copiar.Name = "btn_copiar";
            this.btn_copiar.Padding = new System.Windows.Forms.Padding(8);
            this.btn_copiar.Size = new System.Drawing.Size(132, 48);
            this.btn_copiar.TabIndex = 21;
            this.btn_copiar.Text = "📋 Copiar";
            this.btn_copiar.UseVisualStyleBackColor = false;
            this.btn_copiar.Click += new System.EventHandler(this.btn_copiar_Click);
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
            this.label8.Size = new System.Drawing.Size(1175, 84);
            this.label8.TabIndex = 22;
            this.label8.Text = "🍇 Generador de Ventana UVAS";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
            this.label10.Location = new System.Drawing.Point(508, 28);
            this.label10.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(50, 23);
            this.label10.TabIndex = 24;
            this.label10.Text = "Peso:";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Controls.Add(this.cbx_sdp);
            this.groupBox1.Controls.Add(this.label22);
            this.groupBox1.Controls.Add(this.cmb_variedad_Imprime);
            this.groupBox1.Controls.Add(this.chb_pesofijo);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label21);
            this.groupBox1.Controls.Add(this.cmb_lote);
            this.groupBox1.Controls.Add(this.cmb_cat1);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.cbx_pallets);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.cmb_Recibidor);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.cmb_calibre);
            this.groupBox1.Controls.Add(this.button5);
            this.groupBox1.Controls.Add(this.cmb_productor);
            this.groupBox1.Controls.Add(this.label20);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.cmb_titulo2);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.btn_copiar);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.txt_fecha_agricola);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.cmb_packing);
            this.groupBox1.Controls.Add(this.txt_linea);
            this.groupBox1.Controls.Add(this.cmb_tipo_embalaje);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.cmb_variedad);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
            this.groupBox1.Location = new System.Drawing.Point(7, 525);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(15, 10, 15, 15);
            this.groupBox1.Size = new System.Drawing.Size(1147, 283);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "  📋 Datos de Etiqueta  ";
            // 
            // cbx_sdp
            // 
            this.cbx_sdp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbx_sdp.Enabled = false;
            this.cbx_sdp.FormattingEnabled = true;
            this.cbx_sdp.Location = new System.Drawing.Point(122, 213);
            this.cbx_sdp.Margin = new System.Windows.Forms.Padding(4);
            this.cbx_sdp.Name = "cbx_sdp";
            this.cbx_sdp.Size = new System.Drawing.Size(160, 33);
            this.cbx_sdp.TabIndex = 59;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
            this.label22.Location = new System.Drawing.Point(67, 223);
            this.label22.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(46, 23);
            this.label22.TabIndex = 58;
            this.label22.Text = "SdP:";
            // 
            // cmb_variedad_Imprime
            // 
            this.cmb_variedad_Imprime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_variedad_Imprime.FormattingEnabled = true;
            this.cmb_variedad_Imprime.Location = new System.Drawing.Point(177, 171);
            this.cmb_variedad_Imprime.Margin = new System.Windows.Forms.Padding(4);
            this.cmb_variedad_Imprime.Name = "cmb_variedad_Imprime";
            this.cmb_variedad_Imprime.Size = new System.Drawing.Size(281, 33);
            this.cmb_variedad_Imprime.TabIndex = 58;
            this.cmb_variedad_Imprime.SelectedIndexChanged += new System.EventHandler(this.cmb_variedad_Imprime_SelectedIndexChanged);
            // 
            // chb_pesofijo
            // 
            this.chb_pesofijo.AutoSize = true;
            this.chb_pesofijo.Location = new System.Drawing.Point(889, 138);
            this.chb_pesofijo.Margin = new System.Windows.Forms.Padding(4);
            this.chb_pesofijo.Name = "chb_pesofijo";
            this.chb_pesofijo.Size = new System.Drawing.Size(135, 29);
            this.chb_pesofijo.TabIndex = 35;
            this.chb_pesofijo.Text = "Es Peso Fijo";
            this.chb_pesofijo.UseVisualStyleBackColor = true;
            this.chb_pesofijo.CheckedChanged += new System.EventHandler(this.chb_pesofijo_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
            this.label1.Location = new System.Drawing.Point(7, 176);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(160, 23);
            this.label1.TabIndex = 59;
            this.label1.Text = "Variedad Imprime:";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
            this.label21.Location = new System.Drawing.Point(826, 28);
            this.label21.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(134, 23);
            this.label21.TabIndex = 54;
            this.label21.Text = "Cantidad Cajas:";
            // 
            // cmb_lote
            // 
            this.cmb_lote.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_lote.FormattingEnabled = true;
            this.cmb_lote.Location = new System.Drawing.Point(122, 131);
            this.cmb_lote.Margin = new System.Windows.Forms.Padding(4);
            this.cmb_lote.Name = "cmb_lote";
            this.cmb_lote.Size = new System.Drawing.Size(281, 33);
            this.cmb_lote.TabIndex = 57;
            this.cmb_lote.SelectedIndexChanged += new System.EventHandler(this.cmb_lote_SelectedIndexChanged);
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
            this.cmb_cat1.Location = new System.Drawing.Point(594, 96);
            this.cmb_cat1.Margin = new System.Windows.Forms.Padding(5);
            this.cmb_cat1.Name = "cmb_cat1";
            this.cmb_cat1.Size = new System.Drawing.Size(160, 28);
            this.cmb_cat1.TabIndex = 49;
            this.cmb_cat1.SelectedIndexChanged += new System.EventHandler(this.cmb_cat1_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
            this.label9.Location = new System.Drawing.Point(63, 136);
            this.label9.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(50, 23);
            this.label9.TabIndex = 56;
            this.label9.Text = "Lote:";
            // 
            // cbx_pallets
            // 
            this.cbx_pallets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbx_pallets.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbx_pallets.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbx_pallets.FormattingEnabled = true;
            this.cbx_pallets.Items.AddRange(new object[] {
            "75",
            "80",
            "85",
            "90",
            "95",
            "102",
            "105",
            "112",
            "114",
            "115",
            "140",
            "170"});
            this.cbx_pallets.Location = new System.Drawing.Point(979, 23);
            this.cbx_pallets.Margin = new System.Windows.Forms.Padding(5);
            this.cbx_pallets.Name = "cbx_pallets";
            this.cbx_pallets.Size = new System.Drawing.Size(120, 28);
            this.cbx_pallets.TabIndex = 52;
            this.cbx_pallets.SelectedIndexChanged += new System.EventHandler(this.cbx_pallets_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(476, 95);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(109, 25);
            this.label11.TabIndex = 50;
            this.label11.Text = "CAT (SAG):";
            // 
            // cmb_Recibidor
            // 
            this.cmb_Recibidor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Recibidor.FormattingEnabled = true;
            this.cmb_Recibidor.Location = new System.Drawing.Point(122, 23);
            this.cmb_Recibidor.Margin = new System.Windows.Forms.Padding(4);
            this.cmb_Recibidor.Name = "cmb_Recibidor";
            this.cmb_Recibidor.Size = new System.Drawing.Size(341, 33);
            this.cmb_Recibidor.TabIndex = 53;
            this.cmb_Recibidor.SelectedIndexChanged += new System.EventHandler(this.cmb_Recibidor_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
            this.label6.Location = new System.Drawing.Point(20, 27);
            this.label6.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(93, 23);
            this.label6.TabIndex = 52;
            this.label6.Text = "Recibidor:";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // cmb_calibre
            // 
            this.cmb_calibre.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_calibre.FormattingEnabled = true;
            this.cmb_calibre.Location = new System.Drawing.Point(623, 176);
            this.cmb_calibre.Margin = new System.Windows.Forms.Padding(4);
            this.cmb_calibre.Name = "cmb_calibre";
            this.cmb_calibre.Size = new System.Drawing.Size(160, 33);
            this.cmb_calibre.TabIndex = 47;
            this.cmb_calibre.SelectedIndexChanged += new System.EventHandler(this.cmb_calibre_SelectedIndexChanged);
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
            this.button5.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button5.FlatAppearance.BorderSize = 0;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button5.ForeColor = System.Drawing.Color.White;
            this.button5.Location = new System.Drawing.Point(941, 223);
            this.button5.Margin = new System.Windows.Forms.Padding(6);
            this.button5.Name = "button5";
            this.button5.Padding = new System.Windows.Forms.Padding(8);
            this.button5.Size = new System.Drawing.Size(174, 48);
            this.button5.TabIndex = 33;
            this.button5.Text = "⚡ Generar\r\nVentana";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // cmb_productor
            // 
            this.cmb_productor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_productor.FormattingEnabled = true;
            this.cmb_productor.Location = new System.Drawing.Point(122, 57);
            this.cmb_productor.Margin = new System.Windows.Forms.Padding(4);
            this.cmb_productor.Name = "cmb_productor";
            this.cmb_productor.Size = new System.Drawing.Size(286, 33);
            this.cmb_productor.TabIndex = 51;
            this.cmb_productor.SelectedIndexChanged += new System.EventHandler(this.cmb_productor_SelectedIndexChanged);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(479, 179);
            this.label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(136, 25);
            this.label20.TabIndex = 48;
            this.label20.Text = "Calibre (SAG):";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(1049, 99);
            this.label19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(89, 15);
            this.label19.TabIndex = 46;
            this.label19.Text = "2 Alfanumérico";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(1049, 62);
            this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(55, 15);
            this.label18.TabIndex = 45;
            this.label18.Text = "4 Dígitos";
            // 
            // cmb_titulo2
            // 
            this.cmb_titulo2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_titulo2.FormattingEnabled = true;
            this.cmb_titulo2.Items.AddRange(new object[] {
            "18 lb - 8,2 kg",
            "20 lb - 9,1 kg",
            "16 lb - 7,3 kg",
            "4,5 kg",
            "5 kg"});
            this.cmb_titulo2.Location = new System.Drawing.Point(567, 23);
            this.cmb_titulo2.Margin = new System.Windows.Forms.Padding(4);
            this.cmb_titulo2.Name = "cmb_titulo2";
            this.cmb_titulo2.Size = new System.Drawing.Size(236, 33);
            this.cmb_titulo2.TabIndex = 2;
            this.cmb_titulo2.SelectedIndexChanged += new System.EventHandler(this.cmb_titulo2_SelectedIndexChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(493, 63);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(88, 25);
            this.label17.TabIndex = 44;
            this.label17.Text = "Packing:";
            // 
            // txt_fecha_agricola
            // 
            this.txt_fecha_agricola.Location = new System.Drawing.Point(979, 56);
            this.txt_fecha_agricola.Margin = new System.Windows.Forms.Padding(4);
            this.txt_fecha_agricola.MaxLength = 4;
            this.txt_fecha_agricola.Name = "txt_fecha_agricola";
            this.txt_fecha_agricola.Size = new System.Drawing.Size(56, 32);
            this.txt_fecha_agricola.TabIndex = 11;
            this.txt_fecha_agricola.TextChanged += new System.EventHandler(this.txt_fecha_agricola_TextChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(474, 136);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(141, 25);
            this.label16.TabIndex = 41;
            this.label16.Text = "Tipo Embalaje:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(825, 60);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(146, 25);
            this.label15.TabIndex = 39;
            this.label15.Text = "Fecha Agrícola:";
            // 
            // cmb_packing
            // 
            this.cmb_packing.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
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
            this.cmb_packing.Location = new System.Drawing.Point(584, 60);
            this.cmb_packing.Margin = new System.Windows.Forms.Padding(4);
            this.cmb_packing.Name = "cmb_packing";
            this.cmb_packing.Size = new System.Drawing.Size(233, 33);
            this.cmb_packing.TabIndex = 4;
            this.cmb_packing.SelectedIndexChanged += new System.EventHandler(this.cmb_packing_SelectedIndexChanged);
            // 
            // txt_linea
            // 
            this.txt_linea.Location = new System.Drawing.Point(979, 92);
            this.txt_linea.Margin = new System.Windows.Forms.Padding(4);
            this.txt_linea.MaxLength = 2;
            this.txt_linea.Name = "txt_linea";
            this.txt_linea.Size = new System.Drawing.Size(56, 32);
            this.txt_linea.TabIndex = 12;
            this.txt_linea.TextChanged += new System.EventHandler(this.txt_linea_TextChanged);
            // 
            // cmb_tipo_embalaje
            // 
            this.cmb_tipo_embalaje.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_tipo_embalaje.FormattingEnabled = true;
            this.cmb_tipo_embalaje.Location = new System.Drawing.Point(623, 133);
            this.cmb_tipo_embalaje.Margin = new System.Windows.Forms.Padding(4);
            this.cmb_tipo_embalaje.Name = "cmb_tipo_embalaje";
            this.cmb_tipo_embalaje.Size = new System.Drawing.Size(160, 33);
            this.cmb_tipo_embalaje.TabIndex = 7;
            this.cmb_tipo_embalaje.SelectedIndexChanged += new System.EventHandler(this.cmb_tipo_embalaje_SelectedIndexChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(907, 95);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(64, 25);
            this.label14.TabIndex = 37;
            this.label14.Text = "Linea:";
            // 
            // cmb_variedad
            // 
            this.cmb_variedad.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_variedad.FormattingEnabled = true;
            this.cmb_variedad.Location = new System.Drawing.Point(122, 92);
            this.cmb_variedad.Margin = new System.Windows.Forms.Padding(4);
            this.cmb_variedad.Name = "cmb_variedad";
            this.cmb_variedad.Size = new System.Drawing.Size(281, 33);
            this.cmb_variedad.TabIndex = 6;
            this.cmb_variedad.SelectedIndexChanged += new System.EventHandler(this.cmb_variedad_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(35, 101);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 17);
            this.label5.TabIndex = 32;
            this.label5.Text = "Variedad:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
            this.label2.Location = new System.Drawing.Point(17, 62);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 23);
            this.label2.TabIndex = 31;
            this.label2.Text = "Productor:";
            // 
            // btn_guardar
            // 
            this.btn_guardar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.btn_guardar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_guardar.FlatAppearance.BorderSize = 0;
            this.btn_guardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_guardar.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_guardar.ForeColor = System.Drawing.Color.White;
            this.btn_guardar.Location = new System.Drawing.Point(982, 15);
            this.btn_guardar.Margin = new System.Windows.Forms.Padding(6);
            this.btn_guardar.Name = "btn_guardar";
            this.btn_guardar.Padding = new System.Windows.Forms.Padding(8);
            this.btn_guardar.Size = new System.Drawing.Size(140, 60);
            this.btn_guardar.TabIndex = 28;
            this.btn_guardar.Text = "💾 Guardar";
            this.btn_guardar.UseVisualStyleBackColor = false;
            this.btn_guardar.Visible = false;
            this.btn_guardar.Click += new System.EventHandler(this.btn_guardar_Click);
            // 
            // sfd
            // 
            this.sfd.Filter = "JPG files (*.jpg)|*.jpg";
            this.sfd.Title = "Guardar Etiqueta";
            this.sfd.FileOk += new System.ComponentModel.CancelEventHandler(this.sfd_FileOk);
            // 
            // frm_generadorVentana
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(1175, 818);
            this.Controls.Add(this.btn_guardar);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.pb_etiqueta);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "frm_generadorVentana";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FRUTICOLA Y EXPORTADORA ATACAMA - Temporada 2025/2026";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pb_etiqueta)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

		}

		// Token: 0x040000B0 RID: 176
		private global::System.ComponentModel.IContainer components = null;

		// Token: 0x040000B7 RID: 183
		private global::System.Windows.Forms.PictureBox pb_etiqueta;

		// Token: 0x040000B8 RID: 184
		private global::System.Windows.Forms.Button btn_copiar;

		// Token: 0x040000B9 RID: 185
		private global::System.Windows.Forms.Label label8;

		// Token: 0x040000BA RID: 186
		private global::System.Windows.Forms.Label label10;

		// Token: 0x040000BB RID: 187
		private global::System.Windows.Forms.GroupBox groupBox1;

		// Token: 0x040000BC RID: 188
		private global::System.Windows.Forms.Button btn_guardar;

		// Token: 0x040000BE RID: 190
		private global::System.Windows.Forms.SaveFileDialog sfd;

		// Token: 0x040000BF RID: 191
		private global::System.Windows.Forms.ComboBox cmb_titulo2;

		// Token: 0x040000C0 RID: 192
		private global::System.Windows.Forms.Label label5;

		// Token: 0x040000C1 RID: 193
		private global::System.Windows.Forms.Label label2;

		// Token: 0x040000C2 RID: 194
		private global::System.Windows.Forms.ComboBox cmb_variedad;

		// Token: 0x040000C3 RID: 195
		private global::System.Windows.Forms.ComboBox cmb_tipo_embalaje;

		// Token: 0x040000C4 RID: 196
		private global::System.Windows.Forms.Label label16;

		// Token: 0x040000C5 RID: 197
		private global::System.Windows.Forms.TextBox txt_fecha_agricola;

		// Token: 0x040000C6 RID: 198
		private global::System.Windows.Forms.Label label15;

		// Token: 0x040000C7 RID: 199
		private global::System.Windows.Forms.TextBox txt_linea;

		// Token: 0x040000C8 RID: 200
		private global::System.Windows.Forms.Label label14;

		// Token: 0x040000C9 RID: 201
		private global::System.Windows.Forms.Label label17;

		// Token: 0x040000CA RID: 202
		private global::System.Windows.Forms.ComboBox cmb_packing;

		// Token: 0x040000CB RID: 203
		private global::System.Windows.Forms.Label label19;

		// Token: 0x040000CC RID: 204
		private global::System.Windows.Forms.Label label18;

		// Token: 0x040000CD RID: 205
		private global::System.Windows.Forms.ComboBox cmb_calibre;

		// Token: 0x040000CE RID: 206
		private global::System.Windows.Forms.Label label20;

		// Token: 0x040000CF RID: 207
		private global::System.Windows.Forms.ComboBox cmb_cat1;

		// Token: 0x040000D0 RID: 208
		private global::System.Windows.Forms.Label label11;

		// Token: 0x040000D1 RID: 209
		private global::System.Windows.Forms.Button button5;

		// Token: 0x040000D2 RID: 210
		private global::System.Windows.Forms.ComboBox cmb_productor;

		// Token: 0x040000D3 RID: 211
		private global::System.Windows.Forms.CheckBox chb_pesofijo;

		// Token: 0x040000D4 RID: 212
		private global::System.Windows.Forms.ComboBox cbx_pallets;

		// Token: 0x040000D5 RID: 213
		private global::System.Windows.Forms.ComboBox cmb_Recibidor;

		// Token: 0x040000D6 RID: 214
		private global::System.Windows.Forms.Label label21;

		// Token: 0x040000D7 RID: 215
		private global::System.Windows.Forms.Label label6;

		// Token: 0x040000D8 RID: 216
		private global::System.Windows.Forms.ComboBox cmb_variedad_Imprime;

		// Token: 0x040000D9 RID: 217
		private global::System.Windows.Forms.Label label1;

		// Token: 0x040000DA RID: 218
		private global::System.Windows.Forms.ComboBox cmb_lote;

		// Token: 0x040000DB RID: 219
		private global::System.Windows.Forms.Label label9;

		// Token: 0x040000DC RID: 220
		private global::System.Windows.Forms.ComboBox cbx_sdp;

		// Token: 0x040000DD RID: 221
		private global::System.Windows.Forms.Label label22;
	}
}
