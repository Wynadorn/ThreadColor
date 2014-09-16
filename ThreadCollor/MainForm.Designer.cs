namespace ThreadCollor
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.button_add = new System.Windows.Forms.Button();
            this.listView_overview = new ThreadCollor.ListViewNF();
            this.columnHeader_filename = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_filepath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_red = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_green = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_blue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_hex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_color = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button_remove = new System.Windows.Forms.Button();
            this.button_start = new System.Windows.Forms.Button();
            this.button_export = new System.Windows.Forms.Button();
            this.button_import = new System.Windows.Forms.Button();
            this.label_log = new System.Windows.Forms.Label();
            this.comboBox_cores = new System.Windows.Forms.ComboBox();
            this.label_threads = new System.Windows.Forms.Label();
            this.label_cores = new System.Windows.Forms.Label();
            this.numericUpDown_threads = new System.Windows.Forms.NumericUpDown();
            this.textBox_filter = new System.Windows.Forms.TextBox();
            this.label_filter = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_threads)).BeginInit();
            this.SuspendLayout();
            // 
            // button_add
            // 
            this.button_add.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_add.Location = new System.Drawing.Point(12, 238);
            this.button_add.Name = "button_add";
            this.button_add.Size = new System.Drawing.Size(75, 23);
            this.button_add.TabIndex = 0;
            this.button_add.Text = "Add files...";
            this.button_add.UseVisualStyleBackColor = true;
            this.button_add.Click += new System.EventHandler(this.button_add_Click);
            // 
            // listView_overview
            // 
            this.listView_overview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView_overview.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader_filename,
            this.columnHeader_filepath,
            this.columnHeader_status,
            this.columnHeader_red,
            this.columnHeader_green,
            this.columnHeader_blue,
            this.columnHeader_hex,
            this.columnHeader_color});
            this.listView_overview.FullRowSelect = true;
            this.listView_overview.GridLines = true;
            this.listView_overview.LabelWrap = false;
            this.listView_overview.Location = new System.Drawing.Point(0, 0);
            this.listView_overview.Name = "listView_overview";
            this.listView_overview.Size = new System.Drawing.Size(753, 232);
            this.listView_overview.TabIndex = 1;
            this.listView_overview.UseCompatibleStateImageBehavior = false;
            this.listView_overview.View = System.Windows.Forms.View.Details;
            this.listView_overview.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(this.listView_overview_ColumnWidthChanged);
            // 
            // columnHeader_filename
            // 
            this.columnHeader_filename.Text = "Filename";
            this.columnHeader_filename.Width = 105;
            // 
            // columnHeader_filepath
            // 
            this.columnHeader_filepath.Text = "File path";
            this.columnHeader_filepath.Width = 304;
            // 
            // columnHeader_status
            // 
            this.columnHeader_status.Text = "Status";
            // 
            // columnHeader_red
            // 
            this.columnHeader_red.Text = "Red";
            // 
            // columnHeader_green
            // 
            this.columnHeader_green.Text = "Green";
            // 
            // columnHeader_blue
            // 
            this.columnHeader_blue.Text = "Blue";
            // 
            // columnHeader_hex
            // 
            this.columnHeader_hex.Text = "Hex";
            // 
            // columnHeader_color
            // 
            this.columnHeader_color.Text = "Color";
            this.columnHeader_color.Width = 40;
            // 
            // button_remove
            // 
            this.button_remove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_remove.Location = new System.Drawing.Point(12, 267);
            this.button_remove.Name = "button_remove";
            this.button_remove.Size = new System.Drawing.Size(75, 23);
            this.button_remove.TabIndex = 2;
            this.button_remove.Text = "Remove";
            this.button_remove.UseVisualStyleBackColor = true;
            this.button_remove.Click += new System.EventHandler(this.button_remove_Click);
            // 
            // button_start
            // 
            this.button_start.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_start.Enabled = false;
            this.button_start.Location = new System.Drawing.Point(248, 267);
            this.button_start.Name = "button_start";
            this.button_start.Size = new System.Drawing.Size(75, 23);
            this.button_start.TabIndex = 3;
            this.button_start.Text = "Start";
            this.button_start.UseVisualStyleBackColor = true;
            this.button_start.Click += new System.EventHandler(this.button_start_Click);
            // 
            // button_export
            // 
            this.button_export.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_export.Enabled = false;
            this.button_export.Location = new System.Drawing.Point(661, 267);
            this.button_export.Name = "button_export";
            this.button_export.Size = new System.Drawing.Size(75, 23);
            this.button_export.TabIndex = 4;
            this.button_export.Text = "Export";
            this.button_export.UseVisualStyleBackColor = true;
            this.button_export.Click += new System.EventHandler(this.button_export_Click);
            // 
            // button_import
            // 
            this.button_import.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_import.Enabled = false;
            this.button_import.Location = new System.Drawing.Point(580, 267);
            this.button_import.Name = "button_import";
            this.button_import.Size = new System.Drawing.Size(75, 23);
            this.button_import.TabIndex = 5;
            this.button_import.Text = "Import";
            this.button_import.UseVisualStyleBackColor = true;
            this.button_import.Click += new System.EventHandler(this.button_import_Click);
            // 
            // label_log
            // 
            this.label_log.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label_log.AutoSize = true;
            this.label_log.Enabled = false;
            this.label_log.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_log.Location = new System.Drawing.Point(580, 248);
            this.label_log.Name = "label_log";
            this.label_log.Size = new System.Drawing.Size(25, 13);
            this.label_log.TabIndex = 6;
            this.label_log.Text = "Log";
            // 
            // comboBox_cores
            // 
            this.comboBox_cores.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBox_cores.FormattingEnabled = true;
            this.comboBox_cores.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.comboBox_cores.Location = new System.Drawing.Point(188, 269);
            this.comboBox_cores.Name = "comboBox_cores";
            this.comboBox_cores.Size = new System.Drawing.Size(50, 21);
            this.comboBox_cores.TabIndex = 7;
            this.comboBox_cores.Text = "1";
            this.comboBox_cores.TextChanged += new System.EventHandler(this.comboBox_cores_TextChanged);
            // 
            // label_threads
            // 
            this.label_threads.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label_threads.AutoSize = true;
            this.label_threads.Location = new System.Drawing.Point(136, 243);
            this.label_threads.Name = "label_threads";
            this.label_threads.Size = new System.Drawing.Size(46, 13);
            this.label_threads.TabIndex = 8;
            this.label_threads.Text = "Threads";
            // 
            // label_cores
            // 
            this.label_cores.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label_cores.AutoSize = true;
            this.label_cores.Location = new System.Drawing.Point(136, 272);
            this.label_cores.Name = "label_cores";
            this.label_cores.Size = new System.Drawing.Size(34, 13);
            this.label_cores.TabIndex = 9;
            this.label_cores.Text = "Cores";
            // 
            // numericUpDown_threads
            // 
            this.numericUpDown_threads.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numericUpDown_threads.Location = new System.Drawing.Point(188, 241);
            this.numericUpDown_threads.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_threads.Name = "numericUpDown_threads";
            this.numericUpDown_threads.Size = new System.Drawing.Size(50, 20);
            this.numericUpDown_threads.TabIndex = 10;
            this.numericUpDown_threads.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numericUpDown_threads.ValueChanged += new System.EventHandler(this.numericUpDown_threads_ValueChanged);
            // 
            // textBox_filter
            // 
            this.textBox_filter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox_filter.Enabled = false;
            this.textBox_filter.Location = new System.Drawing.Point(367, 270);
            this.textBox_filter.Name = "textBox_filter";
            this.textBox_filter.Size = new System.Drawing.Size(157, 20);
            this.textBox_filter.TabIndex = 11;
            this.textBox_filter.TextChanged += new System.EventHandler(this.textBox_filter_TextChanged);
            // 
            // label_filter
            // 
            this.label_filter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label_filter.AutoSize = true;
            this.label_filter.Enabled = false;
            this.label_filter.Location = new System.Drawing.Point(364, 248);
            this.label_filter.Name = "label_filter";
            this.label_filter.Size = new System.Drawing.Size(62, 13);
            this.label_filter.TabIndex = 12;
            this.label_filter.Text = "Filter results";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(753, 300);
            this.Controls.Add(this.label_filter);
            this.Controls.Add(this.textBox_filter);
            this.Controls.Add(this.numericUpDown_threads);
            this.Controls.Add(this.label_cores);
            this.Controls.Add(this.label_threads);
            this.Controls.Add(this.comboBox_cores);
            this.Controls.Add(this.label_log);
            this.Controls.Add(this.button_import);
            this.Controls.Add(this.button_export);
            this.Controls.Add(this.button_start);
            this.Controls.Add(this.button_remove);
            this.Controls.Add(this.listView_overview);
            this.Controls.Add(this.button_add);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(762, 150);
            this.Name = "MainForm";
            this.Text = "Image colors";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_threads)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_add;
        private System.Windows.Forms.ColumnHeader columnHeader_filename;
        private System.Windows.Forms.ColumnHeader columnHeader_filepath;
        private System.Windows.Forms.ColumnHeader columnHeader_red;
        private System.Windows.Forms.ColumnHeader columnHeader_green;
        private System.Windows.Forms.ColumnHeader columnHeader_blue;
        private System.Windows.Forms.ColumnHeader columnHeader_hex;
        private System.Windows.Forms.ColumnHeader columnHeader_status;
        private System.Windows.Forms.Button button_remove;
        private System.Windows.Forms.Button button_start;
        private System.Windows.Forms.Button button_export;
        private System.Windows.Forms.Button button_import;
        private System.Windows.Forms.Label label_log;
        private System.Windows.Forms.ComboBox comboBox_cores;
        private System.Windows.Forms.Label label_threads;
        private System.Windows.Forms.Label label_cores;
        private System.Windows.Forms.NumericUpDown numericUpDown_threads;
        private System.Windows.Forms.ColumnHeader columnHeader_color;
        private System.Windows.Forms.TextBox textBox_filter;
        private System.Windows.Forms.Label label_filter;
        private ListViewNF listView_overview;
    }
}

