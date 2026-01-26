namespace WindowsFormsApplication1
{
	/// <summary>
	/// Clase parcial frm_generadorVentana_Citricos - Parte Designer (generada automáticamente)
	/// Formulario especializado para generar etiquetas de VENTANA para CÍTRICOS
	/// Este archivo contiene la inicialización de todos los componentes de la interfaz gráfica
	/// 
	/// PROPÓSITO:
	/// - Generar etiquetas de ventana para cajas de cítricos (naranjas, mandarinas, limones)
	/// - Formato simplificado para ventanas de cajas de exportación
	/// - Incluye información de recibidor, productor, calibre y cantidad de cajas
	/// 
	/// DIFERENCIAS CON OTROS FORMULARIOS:
	/// - Más simple que frm_generador (no usa GTIN, PLU ni Voice Pick Code)
	/// - Específico para cítricos (no uvas)
	/// - Enfocado en ventanas de cajas (no etiquetas PTI completas)
	/// - Incluye campo "Recibidor" (comprador final: SUN PACIFIC, HALO, etc.)
	/// - Incluye "Cantidad Cajas" (65, 70, 72 cajas por pallet)
	/// 
	/// NO MODIFICAR MANUALMENTE - Usar el Designer de Visual Studio
	/// </summary>
	public partial class frm_generadorVentana_Citricos : global::System.Windows.Forms.Form
	{
		/// <summary>
		/// Libera los recursos utilizados por el formulario de ventana de cítricos
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
		/// Inicializa todos los componentes visuales del formulario de ventana de cítricos
		/// Configura propiedades, eventos y diseño de la interfaz
		/// GENERADO AUTOMÁTICAMENTE - No modificar directamente este código
		/// </summary>
	private void InitializeComponent()
	{
            this.txt_date = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.pb_etiqueta = new System.Windows.Forms.PictureBox();
            this.btn_copiar = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chb_pesofijo = new System.Windows.Forms.CheckBox();
            this.label21 = new System.Windows.Forms.Label();
            this.cbx_pallets = new System.Windows.Forms.ComboBox();
            this.cmb_Recibidor = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.cmb_productor = new System.Windows.Forms.ComboBox();
            this.cmb_cat1 = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cmb_calibre = new System.Windows.Forms.ComboBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.cmb_packing = new System.Windows.Forms.ComboBox();
            this.cmb_tipo_embalaje = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txt_fecha_agricola = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txt_linea = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.cmb_variedad = new System.Windows.Forms.ComboBox();
            this.cmb_titulo2 = new System.Windows.Forms.ComboBox();
            this.txt_lote = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.btn_guardar = new System.Windows.Forms.Button();
            this.sfd = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.pb_etiqueta)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txt_date
            // 
            this.txt_date.Location = new System.Drawing.Point(7, 15);
            this.txt_date.Margin = new System.Windows.Forms.Padding(4);
            this.txt_date.MaxLength = 10;
            this.txt_date.Name = "txt_date";
            this.txt_date.Size = new System.Drawing.Size(132, 22);
            this.txt_date.TabIndex = 6;
            this.txt_date.Visible = false;
            this.txt_date.Leave += new System.EventHandler(this.txt_date_Leave);
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
            this.btn_copiar.Location = new System.Drawing.Point(811, 222);
            this.btn_copiar.Margin = new System.Windows.Forms.Padding(6);
            this.btn_copiar.Name = "btn_copiar";
            this.btn_copiar.Padding = new System.Windows.Forms.Padding(8);
            this.btn_copiar.Size = new System.Drawing.Size(136, 55);
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
            this.label8.Size = new System.Drawing.Size(1176, 84);
            this.label8.TabIndex = 22;
            this.label8.Text = "🍊 Generador de Ventana CÍTRICOS";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(57, 213);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(58, 25);
            this.label10.TabIndex = 24;
            this.label10.Text = "Peso:";
            this.label10.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Controls.Add(this.btn_guardar);
            this.groupBox1.Controls.Add(this.chb_pesofijo);
            this.groupBox1.Controls.Add(this.label21);
            this.groupBox1.Controls.Add(this.cbx_pallets);
            this.groupBox1.Controls.Add(this.cmb_Recibidor);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.button5);
            this.groupBox1.Controls.Add(this.cmb_productor);
            this.groupBox1.Controls.Add(this.cmb_cat1);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.cmb_calibre);
            this.groupBox1.Controls.Add(this.label20);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.cmb_packing);
            this.groupBox1.Controls.Add(this.btn_copiar);
            this.groupBox1.Controls.Add(this.cmb_tipo_embalaje);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.txt_fecha_agricola);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.txt_linea);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.cmb_variedad);
            this.groupBox1.Controls.Add(this.cmb_titulo2);
            this.groupBox1.Controls.Add(this.txt_lote);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
            this.groupBox1.Location = new System.Drawing.Point(7, 524);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(15, 10, 15, 15);
            this.groupBox1.Size = new System.Drawing.Size(1147, 295);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "  📋 Datos de Etiqueta Cítricos  ";
            // 
            // chb_pesofijo
            // 
            this.chb_pesofijo.AutoSize = true;
            this.chb_pesofijo.Location = new System.Drawing.Point(475, 236);
            this.chb_pesofijo.Margin = new System.Windows.Forms.Padding(4);
            this.chb_pesofijo.Name = "chb_pesofijo";
            this.chb_pesofijo.Size = new System.Drawing.Size(135, 29);
            this.chb_pesofijo.TabIndex = 35;
            this.chb_pesofijo.Text = "Es Peso Fijo";
            this.chb_pesofijo.UseVisualStyleBackColor = true;
            this.chb_pesofijo.Visible = false;
            this.chb_pesofijo.CheckedChanged += new System.EventHandler(this.chb_pesofijo_CheckedChanged);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(368, 139);
            this.label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(147, 25);
            this.label21.TabIndex = 54;
            this.label21.Text = "Cantidad Cajas:";
            // 
            // cbx_pallets
            // 
            this.cbx_pallets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbx_pallets.FormattingEnabled = true;
            this.cbx_pallets.Items.AddRange(new object[] {
            "65",
            "70",
            "72"});
            this.cbx_pallets.Location = new System.Drawing.Point(516, 136);
            this.cbx_pallets.Margin = new System.Windows.Forms.Padding(4);
            this.cbx_pallets.Name = "cbx_pallets";
            this.cbx_pallets.Size = new System.Drawing.Size(160, 33);
            this.cbx_pallets.TabIndex = 52;
            // 
            // cmb_Recibidor
            // 
            this.cmb_Recibidor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Recibidor.FormattingEnabled = true;
            this.cmb_Recibidor.Items.AddRange(new object[] {
            "",
            "SUN PACIFIC",
            "HALO",
            "KOPKE",
            "PACIFIC TRELLIS",
            "PROCITRUS",
            "SIERRA PRODUCE",
            "WONDERFULL"});
            this.cmb_Recibidor.Location = new System.Drawing.Point(516, 54);
            this.cmb_Recibidor.Margin = new System.Windows.Forms.Padding(4);
            this.cmb_Recibidor.Name = "cmb_Recibidor";
            this.cmb_Recibidor.Size = new System.Drawing.Size(312, 33);
            this.cmb_Recibidor.TabIndex = 53;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
            this.label6.Location = new System.Drawing.Point(422, 59);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(93, 23);
            this.label6.TabIndex = 52;
            this.label6.Text = "Recibidor:";
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(102)))));
            this.button5.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button5.FlatAppearance.BorderSize = 0;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button5.ForeColor = System.Drawing.Color.White;
            this.button5.Location = new System.Drawing.Point(950, 222);
            this.button5.Margin = new System.Windows.Forms.Padding(6);
            this.button5.Name = "button5";
            this.button5.Padding = new System.Windows.Forms.Padding(8);
            this.button5.Size = new System.Drawing.Size(176, 55);
            this.button5.TabIndex = 33;
            this.button5.Text = "⚡ Generar\r\nVentana";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // cmb_productor
            // 
            this.cmb_productor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_productor.FormattingEnabled = true;
            this.cmb_productor.Items.AddRange(new object[] {
            "119072   El Tambito",
            "106957   Huancara",
            "166251   La Estancia",
            "151976   El Guanaco"});
            this.cmb_productor.Location = new System.Drawing.Point(516, 95);
            this.cmb_productor.Margin = new System.Windows.Forms.Padding(4);
            this.cmb_productor.Name = "cmb_productor";
            this.cmb_productor.Size = new System.Drawing.Size(231, 33);
            this.cmb_productor.TabIndex = 51;
            this.cmb_productor.SelectedIndexChanged += new System.EventHandler(this.cmb_productor_SelectedIndexChanged);
            // 
            // cmb_cat1
            // 
            this.cmb_cat1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_cat1.FormattingEnabled = true;
            this.cmb_cat1.Items.AddRange(new object[] {
            "Normal",
            "Europa"});
            this.cmb_cat1.Location = new System.Drawing.Point(121, 251);
            this.cmb_cat1.Margin = new System.Windows.Forms.Padding(4);
            this.cmb_cat1.Name = "cmb_cat1";
            this.cmb_cat1.Size = new System.Drawing.Size(160, 33);
            this.cmb_cat1.TabIndex = 49;
            this.cmb_cat1.Visible = false;
            this.cmb_cat1.SelectedIndexChanged += new System.EventHandler(this.cmb_cat1_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 254);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(109, 25);
            this.label11.TabIndex = 50;
            this.label11.Text = "CAT (SAG):";
            this.label11.Visible = false;
            // 
            // cmb_calibre
            // 
            this.cmb_calibre.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_calibre.FormattingEnabled = true;
            this.cmb_calibre.Location = new System.Drawing.Point(141, 172);
            this.cmb_calibre.Margin = new System.Windows.Forms.Padding(4);
            this.cmb_calibre.Name = "cmb_calibre";
            this.cmb_calibre.Size = new System.Drawing.Size(160, 33);
            this.cmb_calibre.TabIndex = 47;
            this.cmb_calibre.SelectedIndexChanged += new System.EventHandler(this.cmb_calibre_SelectedIndexChanged);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(6, 175);
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
            this.label19.Location = new System.Drawing.Point(1023, 128);
            this.label19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(89, 15);
            this.label19.TabIndex = 46;
            this.label19.Text = "2 Alfanumérico";
            this.label19.Visible = false;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(1029, 93);
            this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(83, 15);
            this.label18.TabIndex = 45;
            this.label18.Text = "DD-MM-AAAA";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(27, 93);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(88, 25);
            this.label17.TabIndex = 44;
            this.label17.Text = "Packing:";
            // 
            // cmb_packing
            // 
            this.cmb_packing.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_packing.FormattingEnabled = true;
            this.cmb_packing.Items.AddRange(new object[] {
            "RIO BLANCO COQUIMBO                               121224",
            "COMPAÑIA FRIGORIFICA DEL NORTE SPA                155359"});
            this.cmb_packing.Location = new System.Drawing.Point(116, 90);
            this.cmb_packing.Margin = new System.Windows.Forms.Padding(4);
            this.cmb_packing.Name = "cmb_packing";
            this.cmb_packing.Size = new System.Drawing.Size(281, 33);
            this.cmb_packing.TabIndex = 4;
            this.cmb_packing.SelectedIndexChanged += new System.EventHandler(this.cmb_packing_SelectedIndexChanged);
            // 
            // cmb_tipo_embalaje
            // 
            this.cmb_tipo_embalaje.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_tipo_embalaje.FormattingEnabled = true;
            this.cmb_tipo_embalaje.Location = new System.Drawing.Point(141, 131);
            this.cmb_tipo_embalaje.Margin = new System.Windows.Forms.Padding(4);
            this.cmb_tipo_embalaje.Name = "cmb_tipo_embalaje";
            this.cmb_tipo_embalaje.Size = new System.Drawing.Size(160, 33);
            this.cmb_tipo_embalaje.TabIndex = 7;
            this.cmb_tipo_embalaje.SelectedIndexChanged += new System.EventHandler(this.cmb_tipo_embalaje_SelectedIndexChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(1, 134);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(141, 25);
            this.label16.TabIndex = 41;
            this.label16.Text = "Tipo Embalaje:";
            this.label16.Click += new System.EventHandler(this.label16_Click);
            // 
            // txt_fecha_agricola
            // 
            this.txt_fecha_agricola.Location = new System.Drawing.Point(920, 86);
            this.txt_fecha_agricola.Margin = new System.Windows.Forms.Padding(4);
            this.txt_fecha_agricola.MaxLength = 10;
            this.txt_fecha_agricola.Name = "txt_fecha_agricola";
            this.txt_fecha_agricola.Size = new System.Drawing.Size(101, 32);
            this.txt_fecha_agricola.TabIndex = 11;
            this.txt_fecha_agricola.TextChanged += new System.EventHandler(this.txt_fecha_agricola_TextChanged);
            this.txt_fecha_agricola.Leave += new System.EventHandler(this.txt_fecha_agricola_Leave);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(775, 90);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(146, 25);
            this.label15.TabIndex = 39;
            this.label15.Text = "Fecha Agrícola:";
            // 
            // txt_linea
            // 
            this.txt_linea.Location = new System.Drawing.Point(920, 126);
            this.txt_linea.Margin = new System.Windows.Forms.Padding(4);
            this.txt_linea.MaxLength = 2;
            this.txt_linea.Name = "txt_linea";
            this.txt_linea.Size = new System.Drawing.Size(56, 32);
            this.txt_linea.TabIndex = 12;
            this.txt_linea.Visible = false;
            this.txt_linea.TextChanged += new System.EventHandler(this.txt_linea_TextChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(848, 131);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(64, 25);
            this.label14.TabIndex = 37;
            this.label14.Text = "Linea:";
            this.label14.Visible = false;
            // 
            // cmb_variedad
            // 
            this.cmb_variedad.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_variedad.FormattingEnabled = true;
            this.cmb_variedad.Location = new System.Drawing.Point(116, 49);
            this.cmb_variedad.Margin = new System.Windows.Forms.Padding(4);
            this.cmb_variedad.Name = "cmb_variedad";
            this.cmb_variedad.Size = new System.Drawing.Size(281, 33);
            this.cmb_variedad.TabIndex = 6;
            this.cmb_variedad.SelectedIndexChanged += new System.EventHandler(this.cmb_variedad_SelectedIndexChanged);
            // 
            // cmb_titulo2
            // 
            this.cmb_titulo2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_titulo2.FormattingEnabled = true;
            this.cmb_titulo2.Items.AddRange(new object[] {
            "15,0 kg",
            "15,5 kg"});
            this.cmb_titulo2.Location = new System.Drawing.Point(121, 210);
            this.cmb_titulo2.Margin = new System.Windows.Forms.Padding(4);
            this.cmb_titulo2.Name = "cmb_titulo2";
            this.cmb_titulo2.Size = new System.Drawing.Size(236, 33);
            this.cmb_titulo2.TabIndex = 2;
            this.cmb_titulo2.Visible = false;
            this.cmb_titulo2.SelectedIndexChanged += new System.EventHandler(this.cmb_titulo2_SelectedIndexChanged);
            // 
            // txt_lote
            // 
            this.txt_lote.Location = new System.Drawing.Point(892, 43);
            this.txt_lote.Margin = new System.Windows.Forms.Padding(4);
            this.txt_lote.MaxLength = 5;
            this.txt_lote.Name = "txt_lote";
            this.txt_lote.Size = new System.Drawing.Size(55, 32);
            this.txt_lote.TabIndex = 10;
            this.txt_lote.TextChanged += new System.EventHandler(this.txt_lote_TextChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(839, 54);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(45, 17);
            this.label12.TabIndex = 33;
            this.label12.Text = "Lote:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(30, 54);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 17);
            this.label5.TabIndex = 32;
            this.label5.Text = "Variedad:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(431, 104);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 17);
            this.label2.TabIndex = 31;
            this.label2.Text = "Productor:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(955, 53);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(109, 15);
            this.label13.TabIndex = 36;
            this.label13.Text = "4 ó 5 Alfanumérico";
            // 
            // btn_guardar
            // 
            this.btn_guardar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.btn_guardar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_guardar.FlatAppearance.BorderSize = 0;
            this.btn_guardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_guardar.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_guardar.ForeColor = System.Drawing.Color.White;
            this.btn_guardar.Location = new System.Drawing.Point(660, 222);
            this.btn_guardar.Margin = new System.Windows.Forms.Padding(6);
            this.btn_guardar.Name = "btn_guardar";
            this.btn_guardar.Padding = new System.Windows.Forms.Padding(8);
            this.btn_guardar.Size = new System.Drawing.Size(139, 55);
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
            // 
            // frm_generadorVentana_Citricos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(1176, 828);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txt_date);
            this.Controls.Add(this.pb_etiqueta);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "frm_generadorVentana_Citricos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FRUTICOLA Y EXPORTADORA ATACAMA - Temporada 2025/2026";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pb_etiqueta)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		/// <summary>
		/// Contenedor de componentes del formulario de ventana de cítricos
		/// Administra la liberación de recursos de todos los controles
		/// </summary>
		private global::System.ComponentModel.IContainer components = null;

		/// <summary>
		/// TextBox txt_date - Campo de fecha opcional (OCULTO)
		/// Tamaño: 132x22 píxeles
		/// MaxLength: 10 caracteres
		/// Formato: dd-mm-yyyy
		/// Visible: false (no se usa en ventanas de cítricos)
		/// Ubicación: (7,15) - Esquina superior izquierda
		/// Evento: Leave (txt_date_Leave)
		/// </summary>
		private global::System.Windows.Forms.TextBox txt_date;

		/// <summary>
		/// Label "(Opcional)" - Texto indicativo (OCULTO)
		/// Texto: "(Opcional)"
		/// Fuente: Microsoft Sans Serif 7pt
		/// Visible: false (no se usa en ventanas de cítricos)
		/// Ubicación: (148,20) - Junto a txt_date
		/// </summary>
		private global::System.Windows.Forms.Label label7;

		/// <summary>
		/// PictureBox pb_etiqueta - Vista previa de la etiqueta de ventana de cítricos
		/// Tamaño: 1146x420 píxeles (MÁS ANCHO que formulario de uvas)
		/// BackColor: Blanco
		/// BorderStyle: FixedSingle (borde simple)
		/// SizeMode: StretchImage (estira imagen para ajustar)
		/// Ubicación: (7,96) - Debajo del título, ocupa casi todo el ancho
		/// Muestra la imagen de la ventana de cítricos generada
		/// DIFERENCIA: Más ancho (1146px) vs uvas (772px) para ventanas amplias
		/// </summary>
		private global::System.Windows.Forms.PictureBox pb_etiqueta;

		/// <summary>
		/// Button btn_copiar - Botón "📋 Copiar"
		/// Tamaño: 136x55 píxeles
		/// BackColor: Azul oscuro (0,51,102)
		/// ForeColor: Blanco
		/// Fuente: Segoe UI 11pt Bold
		/// Ubicación: (811,222) - Dentro del GroupBox, zona de botones
		/// Acción: Copia la imagen de la ventana al portapapeles
		/// Evento: Click (btn_copiar_Click)
		/// </summary>
		private global::System.Windows.Forms.Button btn_copiar;

		/// <summary>
		/// Label label8 - Título principal del formulario
		/// Texto: "🍊 Generador de Ventana CÍTRICOS"
		/// Dock: Top (ocupa todo el ancho superior)
		/// Tamaño: 1176x84 píxeles
		/// BackColor: Azul oscuro (0,51,102)
		/// ForeColor: Blanco
		/// Fuente: Segoe UI 20pt Bold
		/// TextAlign: MiddleCenter (centrado)
		/// DIFERENCIA: Icono 🍊 (naranja) vs 🏷️ (etiqueta) en formulario de uvas
		/// </summary>
		private global::System.Windows.Forms.Label label8;

		/// <summary>
		/// Label "Peso:" - Etiqueta descriptiva (OCULTO)
		/// Texto: "Peso:"
		/// Ubicación: (57,213)
		/// Visible: false (no se usa peso en ventanas de cítricos)
		/// </summary>
		private global::System.Windows.Forms.Label label10;

		/// <summary>
		/// GroupBox groupBox1 - Contenedor principal de datos de cítricos
		/// Título: "📋 Datos de Etiqueta Cítricos"
		/// Tamaño: 1147x295 píxeles
		/// BackColor: Blanco
		/// ForeColor: Azul oscuro (0,51,102)
		/// Fuente: Segoe UI 11pt Bold
		/// Ubicación: (7,524) - Debajo de la vista previa
		/// Contiene TODOS los controles de entrada de datos para cítricos
		/// </summary>
		private global::System.Windows.Forms.GroupBox groupBox1;

		/// <summary>
		/// Button btn_guardar - Botón "💾 Guardar" (OCULTO por defecto)
		/// Tamaño: 139x55 píxeles
		/// BackColor: Verde (76,175,80)
		/// ForeColor: Blanco
		/// Fuente: Segoe UI 11pt Bold
		/// Ubicación: (660,222) - Dentro del GroupBox, zona de botones
		/// Visible: false (se puede mostrar si es necesario)
		/// Acción: Abre diálogo para guardar imagen como archivo JPEG
		/// Evento: Click (btn_guardar_Click)
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
		/// ComboBox cmb_titulo2 - Selector de peso neto (OCULTO)
		/// Tamaño: 236x33 píxeles
		/// DropDownStyle: DropDownList
		/// Opciones fijas:
		///   - "15,0 kg"
		///   - "15,5 kg"
		/// Ubicación: (121,210)
		/// Visible: false (no se usa en ventanas de cítricos actuales)
		/// Evento: SelectedIndexChanged (cmb_titulo2_SelectedIndexChanged)
		/// </summary>
		private global::System.Windows.Forms.ComboBox cmb_titulo2;

		/// <summary>
		/// Label "Lote:" - Etiqueta descriptiva
		/// Texto: "Lote:"
		/// Fuente: Microsoft Sans Serif 8.25pt Bold
		/// Ubicación: (839,54) - Junto al TextBox de lote
		/// </summary>
		private global::System.Windows.Forms.Label label12;

		/// <summary>
		/// Label "Variedad:" - Etiqueta descriptiva
		/// Texto: "Variedad:"
		/// Fuente: Microsoft Sans Serif 8.25pt Bold
		/// Ubicación: (30,54) - Junto al ComboBox de variedad
		/// </summary>
		private global::System.Windows.Forms.Label label5;

		/// <summary>
		/// Label "Productor:" - Etiqueta descriptiva
		/// Texto: "Productor:"
		/// Fuente: Microsoft Sans Serif 8.25pt Bold
		/// Ubicación: (431,104) - Junto al ComboBox de productor
		/// Evento: Click (label2_Click)
		/// </summary>
		private global::System.Windows.Forms.Label label2;

		/// <summary>
		/// Label "4 ó 5 Alfanumérico" - Texto de ayuda para lote
		/// Texto: "4 ó 5 Alfanumérico"
		/// Fuente: Microsoft Sans Serif 7pt
		/// Ubicación: (955,53) - Debajo del TextBox de lote
		/// Indica formato esperado para el campo de lote de cítricos
		/// DIFERENCIA: Acepta 4 ó 5 caracteres (uvas acepta variable)
		/// </summary>
		private global::System.Windows.Forms.Label label13;

		/// <summary>
		/// TextBox txt_lote - Campo de número de lote de cítricos
		/// Tamaño: 55x32 píxeles
		/// MaxLength: 5 caracteres (4 ó 5 alfanuméricos)
		/// Ubicación: (892,43) - Superior derecha
		/// Formato: Alfanumérico (ej: "L001", "C2025")
		/// Se concatena con otros datos en la etiqueta
		/// Evento: TextChanged (txt_lote_TextChanged)
		/// </summary>
		private global::System.Windows.Forms.TextBox txt_lote;

		/// <summary>
		/// ComboBox cmb_variedad - Selector de variedad de cítrico
		/// Tamaño: 281x33 píxeles
		/// DropDownStyle: DropDownList
		/// Fuente: Segoe UI 11pt Bold
		/// Ubicación: (116,49) - Primera fila de datos
		/// Contenido: Variedades de cítricos (naranjas, mandarinas, limones)
		/// Ejemplos: "NAVEL", "CLEMENTINA", "MANDARINA", "LIMÓN"
		/// Se llena en tiempo de ejecución según disponibilidad
		/// Evento: SelectedIndexChanged (cmb_variedad_SelectedIndexChanged)
		/// </summary>
		private global::System.Windows.Forms.ComboBox cmb_variedad;

		/// <summary>
		/// ComboBox cmb_tipo_embalaje - Selector de tipo de embalaje para cítricos
		/// Tamaño: 160x33 píxeles
		/// DropDownStyle: DropDownList
		/// Fuente: Segoe UI 11pt Bold
		/// Ubicación: (141,131) - Segunda columna de datos
		/// Contenido: Tipos de embalaje específicos para cítricos
		/// Ejemplos: "Caja Carton", "Bins", "Sacos"
		/// Se llena dinámicamente según el tipo de cítrico
		/// Evento: SelectedIndexChanged (cmb_tipo_embalaje_SelectedIndexChanged)
		/// </summary>
		private global::System.Windows.Forms.ComboBox cmb_tipo_embalaje;

		/// <summary>
		/// Label "Tipo Embalaje:" - Etiqueta descriptiva
		/// Texto: "Tipo Embalaje:"
		/// Fuente: Segoe UI 11pt Bold
		/// Ubicación: (1,134) - Junto al ComboBox de tipo embalaje
		/// Evento: Click (label16_Click)
		/// </summary>
		private global::System.Windows.Forms.Label label16;

		/// <summary>
		/// TextBox txt_fecha_agricola - Campo de fecha agrícola para cítricos
		/// Tamaño: 101x32 píxeles
		/// MaxLength: 10 caracteres
		/// Formato: DD-MM-YYYY (fecha completa para cítricos)
		/// Ubicación: (920,86) - Superior derecha
		/// DIFERENCIA vs uvas: Acepta fecha completa (10 chars) vs solo MMDD (4 chars)
		/// Se valida al salir del campo (evento Leave)
		/// Eventos: TextChanged, Leave (txt_fecha_agricola_TextChanged, txt_fecha_agricola_Leave)
		/// </summary>
		private global::System.Windows.Forms.TextBox txt_fecha_agricola;

		/// <summary>
		/// Label "Fecha Agrícola:" - Etiqueta descriptiva
		/// Texto: "Fecha Agrícola:"
		/// Fuente: Segoe UI 11pt Bold
		/// Ubicación: (775,90) - Junto al TextBox de fecha agrícola
		/// </summary>
		private global::System.Windows.Forms.Label label15;

		/// <summary>
		/// TextBox txt_linea - Campo de número de línea (OCULTO)
		/// Tamaño: 56x32 píxeles
		/// MaxLength: 2 caracteres (alfanumérico)
		/// Ubicación: (920,126)
		/// Visible: false (no se usa en ventanas de cítricos actuales)
		/// Evento: TextChanged (txt_linea_TextChanged)
		/// </summary>
		private global::System.Windows.Forms.TextBox txt_linea;

		/// <summary>
		/// Label "Linea:" - Etiqueta descriptiva (OCULTO)
		/// Texto: "Linea:"
		/// Ubicación: (848,131)
		/// Visible: false (no se usa en ventanas de cítricos actuales)
		/// </summary>
		private global::System.Windows.Forms.Label label14;

		/// <summary>
		/// Label "Packing:" - Etiqueta descriptiva
		/// Texto: "Packing:"
		/// Fuente: Segoe UI 11pt Bold
		/// Ubicación: (27,93) - Junto al ComboBox de packing
		/// </summary>
		private global::System.Windows.Forms.Label label17;

		/// <summary>
		/// ComboBox cmb_packing - Selector de planta de empaque (CSP) para cítricos
		/// Tamaño: 281x33 píxeles
		/// DropDownStyle: DropDownList
		/// Fuente: Segoe UI 11pt Bold
		/// Ubicación: (116,90) - Primera columna de datos
		/// Opciones fijas (2 plantas especializadas en cítricos):
		///   - "RIO BLANCO COQUIMBO                               121224"
		///   - "COMPAÑIA FRIGORIFICA DEL NORTE SPA                155359"
		/// Formato: "Nombre Planta    CSP"
		/// Determina ubicación geográfica del empaque
		/// Evento: SelectedIndexChanged (cmb_packing_SelectedIndexChanged)
		/// </summary>
		private global::System.Windows.Forms.ComboBox cmb_packing;

		/// <summary>
		/// Label "2 Alfanumérico" - Texto de ayuda (OCULTO)
		/// Texto: "2 Alfanumérico"
		/// Fuente: Microsoft Sans Serif 7pt
		/// Ubicación: (1023,128)
		/// Visible: false (ayuda para campo línea que no se usa)
		/// </summary>
		private global::System.Windows.Forms.Label label19;

		/// <summary>
		/// Label "DD-MM-AAAA" - Texto de ayuda para fecha
		/// Texto: "DD-MM-AAAA"
		/// Fuente: Microsoft Sans Serif 7pt
		/// Ubicación: (1029,93) - Debajo del TextBox de fecha agrícola
		/// Indica formato de fecha esperado para cítricos
		/// DIFERENCIA: Formato completo DD-MM-AAAA vs MMDD en uvas
		/// </summary>
		private global::System.Windows.Forms.Label label18;

		/// <summary>
		/// ComboBox cmb_calibre - Selector de calibre de cítrico
		/// Tamaño: 160x33 píxeles
		/// DropDownStyle: DropDownList
		/// Fuente: Segoe UI 11pt Bold
		/// Ubicación: (141,172) - Segunda columna de datos
		/// Contenido: Calibres específicos para cítricos
		/// Ejemplos calibres naranjas: "36", "40", "48", "56", "64", "72", "88"
		/// Ejemplos calibres mandarinas: "1", "1X", "2X", "3X"
		/// Se llena dinámicamente según variedad de cítrico seleccionada
		/// Evento: SelectedIndexChanged (cmb_calibre_SelectedIndexChanged)
		/// </summary>
		private global::System.Windows.Forms.ComboBox cmb_calibre;

		/// <summary>
		/// Label "Calibre (SAG):" - Etiqueta descriptiva
		/// Texto: "Calibre (SAG):"
		/// Fuente: Segoe UI 11pt Bold
		/// Ubicación: (6,175) - Junto al ComboBox de calibre
		/// SAG = Servicio Agrícola y Ganadero (Chile)
		/// </summary>
		private global::System.Windows.Forms.Label label20;

		/// <summary>
		/// ComboBox cmb_cat1 - Selector de categoría (OCULTO)
		/// Tamaño: 160x33 píxeles
		/// DropDownStyle: DropDownList
		/// Opciones:
		///   - "Normal"
		///   - "Europa"
		/// Ubicación: (121,251)
		/// Visible: false (no se usa en ventanas de cítricos actuales)
		/// Evento: SelectedIndexChanged (cmb_cat1_SelectedIndexChanged)
		/// </summary>
		private global::System.Windows.Forms.ComboBox cmb_cat1;

		/// <summary>
		/// Label "CAT (SAG):" - Etiqueta descriptiva (OCULTO)
		/// Texto: "CAT (SAG):"
		/// Ubicación: (6,254)
		/// Visible: false (no se usa en ventanas de cítricos actuales)
		/// </summary>
		private global::System.Windows.Forms.Label label11;

		/// <summary>
		/// Button button5 - Botón "⚡ Generar Ventana"
		/// Tamaño: 176x55 píxeles
		/// BackColor: Azul oscuro (0,51,102)
		/// ForeColor: Blanco
		/// Fuente: Segoe UI 11pt Bold
		/// Ubicación: (950,222) - Dentro del GroupBox, botón principal
		/// Texto: "⚡ Generar\r\nVentana" (2 líneas)
		/// Acción: Genera la imagen de ventana para cítricos
		/// Evento: Click (button5_Click)
		///   → Llama al método de generación de ventana específico para cítricos
		/// </summary>
		private global::System.Windows.Forms.Button button5;

		/// <summary>
		/// ComboBox cmb_productor - Selector de productor de cítricos (CSG)
		/// Tamaño: 231x33 píxeles
		/// DropDownStyle: DropDownList
		/// Fuente: Segoe UI 11pt Bold
		/// Ubicación: (516,95) - Tercera columna de datos
		/// Opciones fijas (4 productores especializados en cítricos):
		///   - "119072   El Tambito"
		///   - "106957   Huancara"
		///   - "166251   La Estancia"
		///   - "151976   El Guanaco"
		/// Formato: "######   Nombre Productor"
		/// Evento: SelectedIndexChanged (cmb_productor_SelectedIndexChanged)
		/// DIFERENCIA: Lista fija de productores de cítricos vs carga dinámica en uvas
		/// </summary>
		private global::System.Windows.Forms.ComboBox cmb_productor;

		/// <summary>
		/// CheckBox chb_pesofijo - Indicador de peso fijo (OCULTO)
		/// Texto: "Es Peso Fijo"
		/// Ubicación: (475,236)
		/// Visible: false (no se usa en ventanas de cítricos)
		/// Evento: CheckedChanged (chb_pesofijo_CheckedChanged)
		/// </summary>
		private global::System.Windows.Forms.CheckBox chb_pesofijo;

		/// <summary>
		/// ComboBox cbx_pallets - Selector de cantidad de cajas por pallet
		/// Tamaño: 160x33 píxeles
		/// DropDownStyle: DropDownList
		/// Fuente: Segoe UI 11pt Bold
		/// Ubicación: (516,136) - Tercera columna de datos
		/// Opciones fijas (3 cantidades estándar para cítricos):
		///   - "65" (65 cajas por pallet)
		///   - "70" (70 cajas por pallet)
		///   - "72" (72 cajas por pallet)
		/// CAMPO CLAVE: Específico para ventanas de cítricos
		/// Indica la cantidad total de cajas en el pallet
		/// DIFERENCIA: No existe en formulario de uvas
		/// </summary>
		private global::System.Windows.Forms.ComboBox cbx_pallets;

		/// <summary>
		/// ComboBox cmb_Recibidor - Selector de recibidor/comprador final
		/// Tamaño: 312x33 píxeles
		/// DropDownStyle: DropDownList
		/// Fuente: Segoe UI 11pt Bold
		/// Ubicación: (516,54) - Tercera columna de datos, primera fila
		/// Opciones fijas (8 recibidores/compradores):
		///   - "" (vacío - sin recibidor específico)
		///   - "SUN PACIFIC"
		///   - "HALO"
		///   - "KOPKE"
		///   - "PACIFIC TRELLIS"
		///   - "PROCITRUS"
		///   - "SIERRA PRODUCE"
		///   - "WONDERFULL"
		/// CAMPO CLAVE: Específico para ventanas de cítricos
		/// Indica el comprador final o distribuidor del producto
		/// DIFERENCIA: No existe en formulario de uvas (campo exclusivo de cítricos)
		/// </summary>
		private global::System.Windows.Forms.ComboBox cmb_Recibidor;

		/// <summary>
		/// Label "Cantidad Cajas:" - Etiqueta descriptiva
		/// Texto: "Cantidad Cajas:"
		/// Fuente: Segoe UI 11pt Bold
		/// Ubicación: (368,139) - Junto al ComboBox de pallets
		/// Indica la cantidad total de cajas en el pallet (65, 70 ó 72)
		/// </summary>
		private global::System.Windows.Forms.Label label21;

		/// <summary>
		/// Label "Recibidor:" - Etiqueta descriptiva
		/// Texto: "Recibidor:"
		/// Fuente: Segoe UI 10pt Bold
		/// Ubicación: (422,59) - Junto al ComboBox de recibidor
		/// Indica el comprador final o distribuidor del producto
		/// </summary>
		private global::System.Windows.Forms.Label label6;
	}
}
