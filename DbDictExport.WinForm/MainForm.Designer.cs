namespace DbDictExport.WinForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tvDatabase = new System.Windows.Forms.TreeView();
            this.cmsDatabase = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportDataDictionaryDocumentToExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.newConnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sqlServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mySqlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.postgresqlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imgListCommon = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableTabControl = new MetroFramework.Controls.MetroTabControl();
            this.viewTabPage = new MetroFramework.Controls.MetroTabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.MetroGridDesign = new MetroFramework.Controls.MetroGrid();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.MetroGridResultSet = new MetroFramework.Controls.MetroGrid();
            this.createScriptTabPage = new MetroFramework.Controls.MetroTabPage();
            this.metroScrollBar1 = new MetroFramework.Controls.MetroScrollBar();
            this.cmsDbTable = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.generateKdCodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateJingShangCodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateAcartonsCodesPgToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateFMEACodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.cmsDatabase.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableTabControl.SuspendLayout();
            this.viewTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MetroGridDesign)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MetroGridResultSet)).BeginInit();
            this.cmsDbTable.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.BackColor = System.Drawing.SystemColors.Window;
            this.groupBox1.Controls.Add(this.tvDatabase);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Size = new System.Drawing.Size(295, 595);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "DataBase";
            // 
            // tvDatabase
            // 
            this.tvDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvDatabase.Location = new System.Drawing.Point(2, 18);
            this.tvDatabase.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tvDatabase.Name = "tvDatabase";
            this.tvDatabase.Size = new System.Drawing.Size(291, 575);
            this.tvDatabase.TabIndex = 0;
            // 
            // cmsDatabase
            // 
            this.cmsDatabase.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsDatabase.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshToolStripMenuItem,
            this.exportDataDictionaryDocumentToExcelToolStripMenuItem});
            this.cmsDatabase.Name = "cmsDatabase";
            this.cmsDatabase.Size = new System.Drawing.Size(317, 48);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(316, 22);
            this.refreshToolStripMenuItem.Text = "Refresh";
            // 
            // exportDataDictionaryDocumentToExcelToolStripMenuItem
            // 
            this.exportDataDictionaryDocumentToExcelToolStripMenuItem.Name = "exportDataDictionaryDocumentToExcelToolStripMenuItem";
            this.exportDataDictionaryDocumentToExcelToolStripMenuItem.Size = new System.Drawing.Size(316, 22);
            this.exportDataDictionaryDocumentToExcelToolStripMenuItem.Text = "Export data dictionary document to Excel";
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Window;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newConnectToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(16, 48);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1146, 25);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // newConnectToolStripMenuItem
            // 
            this.newConnectToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sqlServerToolStripMenuItem,
            this.mySqlToolStripMenuItem,
            this.postgresqlToolStripMenuItem});
            this.newConnectToolStripMenuItem.Name = "newConnectToolStripMenuItem";
            this.newConnectToolStripMenuItem.Size = new System.Drawing.Size(97, 21);
            this.newConnectToolStripMenuItem.Text = "New Connect";
            // 
            // sqlServerToolStripMenuItem
            // 
            this.sqlServerToolStripMenuItem.Name = "sqlServerToolStripMenuItem";
            this.sqlServerToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.sqlServerToolStripMenuItem.Text = "Sql Server";
            this.sqlServerToolStripMenuItem.Click += new System.EventHandler(this.sqlServerToolStripMenuItem_Click);
            // 
            // mySqlToolStripMenuItem
            // 
            this.mySqlToolStripMenuItem.Name = "mySqlToolStripMenuItem";
            this.mySqlToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.mySqlToolStripMenuItem.Text = "MySql";
            this.mySqlToolStripMenuItem.Click += new System.EventHandler(this.mySqlToolStripMenuItem_Click);
            // 
            // postgresqlToolStripMenuItem
            // 
            this.postgresqlToolStripMenuItem.Name = "postgresqlToolStripMenuItem";
            this.postgresqlToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.postgresqlToolStripMenuItem.Text = "Postgresql";
            this.postgresqlToolStripMenuItem.Click += new System.EventHandler(this.postgresqlToolStripMenuItem_Click);
            // 
            // imgListCommon
            // 
            this.imgListCommon.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgListCommon.ImageStream")));
            this.imgListCommon.TransparentColor = System.Drawing.Color.Transparent;
            this.imgListCommon.Images.SetKeyName(0, "databases_tree_root.ico");
            this.imgListCommon.Images.SetKeyName(1, "database_tree.ico");
            this.imgListCommon.Images.SetKeyName(2, "datatable_tree.ico");
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.Silver;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(16, 73);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(2, 24, 2, 2);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.AutoScroll = true;
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel1MinSize = 30;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(1146, 595);
            this.splitContainer1.SplitterDistance = 295;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.tableTabControl);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(848, 595);
            this.panel1.TabIndex = 2;
            // 
            // tableTabControl
            // 
            this.tableTabControl.Controls.Add(this.viewTabPage);
            this.tableTabControl.Controls.Add(this.createScriptTabPage);
            this.tableTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableTabControl.Location = new System.Drawing.Point(0, 0);
            this.tableTabControl.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tableTabControl.Name = "tableTabControl";
            this.tableTabControl.SelectedIndex = 0;
            this.tableTabControl.Size = new System.Drawing.Size(848, 595);
            this.tableTabControl.Style = MetroFramework.MetroColorStyle.Orange;
            this.tableTabControl.TabIndex = 1;
            this.tableTabControl.UseSelectable = true;
            // 
            // viewTabPage
            // 
            this.viewTabPage.Controls.Add(this.splitContainer2);
            this.viewTabPage.HorizontalScrollbarBarColor = true;
            this.viewTabPage.HorizontalScrollbarHighlightOnWheel = false;
            this.viewTabPage.HorizontalScrollbarSize = 3;
            this.viewTabPage.Location = new System.Drawing.Point(4, 38);
            this.viewTabPage.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.viewTabPage.Name = "viewTabPage";
            this.viewTabPage.Padding = new System.Windows.Forms.Padding(1);
            this.viewTabPage.Size = new System.Drawing.Size(840, 553);
            this.viewTabPage.TabIndex = 0;
            this.viewTabPage.Text = "View";
            this.viewTabPage.VerticalScrollbarBarColor = true;
            this.viewTabPage.VerticalScrollbarHighlightOnWheel = false;
            this.viewTabPage.VerticalScrollbarSize = 6;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BackColor = System.Drawing.Color.Silver;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(1, 1);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox3);
            this.splitContainer2.Size = new System.Drawing.Size(838, 551);
            this.splitContainer2.SplitterDistance = 330;
            this.splitContainer2.SplitterWidth = 3;
            this.splitContainer2.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.AutoSize = true;
            this.groupBox2.BackColor = System.Drawing.SystemColors.Window;
            this.groupBox2.Controls.Add(this.MetroGridDesign);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Size = new System.Drawing.Size(838, 330);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Design";
            // 
            // MetroGridDesign
            // 
            this.MetroGridDesign.AllowUserToAddRows = false;
            this.MetroGridDesign.AllowUserToDeleteRows = false;
            this.MetroGridDesign.AllowUserToResizeRows = false;
            this.MetroGridDesign.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.MetroGridDesign.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.MetroGridDesign.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.MetroGridDesign.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.MetroGridDesign.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(85)))), ((int)(((byte)(85)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.MetroGridDesign.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.MetroGridDesign.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.MetroGridDesign.DefaultCellStyle = dataGridViewCellStyle2;
            this.MetroGridDesign.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MetroGridDesign.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2;
            this.MetroGridDesign.EnableHeadersVisualStyles = false;
            this.MetroGridDesign.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.MetroGridDesign.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.MetroGridDesign.Location = new System.Drawing.Point(2, 18);
            this.MetroGridDesign.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MetroGridDesign.Name = "MetroGridDesign";
            this.MetroGridDesign.ReadOnly = true;
            this.MetroGridDesign.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(85)))), ((int)(((byte)(85)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.MetroGridDesign.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.MetroGridDesign.RowHeadersVisible = false;
            this.MetroGridDesign.RowHeadersWidth = 20;
            this.MetroGridDesign.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.MetroGridDesign.RowTemplate.Height = 27;
            this.MetroGridDesign.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.MetroGridDesign.Size = new System.Drawing.Size(834, 310);
            this.MetroGridDesign.Style = MetroFramework.MetroColorStyle.Silver;
            this.MetroGridDesign.TabIndex = 1;
            this.MetroGridDesign.UseStyleColors = true;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.SystemColors.Window;
            this.groupBox3.Controls.Add(this.MetroGridResultSet);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox3.Size = new System.Drawing.Size(838, 218);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Result Set";
            // 
            // MetroGridResultSet
            // 
            this.MetroGridResultSet.AllowUserToAddRows = false;
            this.MetroGridResultSet.AllowUserToDeleteRows = false;
            this.MetroGridResultSet.AllowUserToResizeRows = false;
            this.MetroGridResultSet.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.MetroGridResultSet.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.MetroGridResultSet.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.MetroGridResultSet.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.MetroGridResultSet.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(85)))), ((int)(((byte)(85)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.MetroGridResultSet.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.MetroGridResultSet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.MetroGridResultSet.DefaultCellStyle = dataGridViewCellStyle5;
            this.MetroGridResultSet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MetroGridResultSet.EnableHeadersVisualStyles = false;
            this.MetroGridResultSet.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.MetroGridResultSet.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.MetroGridResultSet.Location = new System.Drawing.Point(2, 18);
            this.MetroGridResultSet.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MetroGridResultSet.Name = "MetroGridResultSet";
            this.MetroGridResultSet.ReadOnly = true;
            this.MetroGridResultSet.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(85)))), ((int)(((byte)(85)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.MetroGridResultSet.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.MetroGridResultSet.RowHeadersVisible = false;
            this.MetroGridResultSet.RowHeadersWidth = 20;
            this.MetroGridResultSet.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.MetroGridResultSet.RowTemplate.Height = 27;
            this.MetroGridResultSet.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.MetroGridResultSet.Size = new System.Drawing.Size(834, 198);
            this.MetroGridResultSet.Style = MetroFramework.MetroColorStyle.Silver;
            this.MetroGridResultSet.TabIndex = 0;
            // 
            // createScriptTabPage
            // 
            this.createScriptTabPage.HorizontalScrollbarBarColor = true;
            this.createScriptTabPage.HorizontalScrollbarHighlightOnWheel = false;
            this.createScriptTabPage.HorizontalScrollbarSize = 3;
            this.createScriptTabPage.Location = new System.Drawing.Point(4, 38);
            this.createScriptTabPage.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.createScriptTabPage.Name = "createScriptTabPage";
            this.createScriptTabPage.Size = new System.Drawing.Size(839, 556);
            this.createScriptTabPage.TabIndex = 1;
            this.createScriptTabPage.Text = "Create Scripts";
            this.createScriptTabPage.VerticalScrollbarBarColor = true;
            this.createScriptTabPage.VerticalScrollbarHighlightOnWheel = false;
            this.createScriptTabPage.VerticalScrollbarSize = 6;
            // 
            // metroScrollBar1
            // 
            this.metroScrollBar1.LargeChange = 10;
            this.metroScrollBar1.Location = new System.Drawing.Point(0, 0);
            this.metroScrollBar1.Maximum = 100;
            this.metroScrollBar1.Minimum = 0;
            this.metroScrollBar1.MouseWheelBarPartitions = 10;
            this.metroScrollBar1.Name = "metroScrollBar1";
            this.metroScrollBar1.Orientation = MetroFramework.Controls.MetroScrollOrientation.Horizontal;
            this.metroScrollBar1.ScrollbarSize = 10;
            this.metroScrollBar1.Size = new System.Drawing.Size(200, 10);
            this.metroScrollBar1.TabIndex = 0;
            this.metroScrollBar1.UseSelectable = true;
            // 
            // cmsDbTable
            // 
            this.cmsDbTable.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsDbTable.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.generateKdCodesToolStripMenuItem,
            this.generateJingShangCodesToolStripMenuItem,
            this.generateAcartonsCodesPgToolStripMenuItem,
            this.generateFMEACodesToolStripMenuItem});
            this.cmsDbTable.Name = "cmsDbTable";
            this.cmsDbTable.Size = new System.Drawing.Size(254, 114);
            // 
            // generateKdCodesToolStripMenuItem
            // 
            this.generateKdCodesToolStripMenuItem.Name = "generateKdCodesToolStripMenuItem";
            this.generateKdCodesToolStripMenuItem.Size = new System.Drawing.Size(253, 22);
            this.generateKdCodesToolStripMenuItem.Text = "Generate kd Codes";
            // 
            // generateJingShangCodesToolStripMenuItem
            // 
            this.generateJingShangCodesToolStripMenuItem.Name = "generateJingShangCodesToolStripMenuItem";
            this.generateJingShangCodesToolStripMenuItem.Size = new System.Drawing.Size(253, 22);
            this.generateJingShangCodesToolStripMenuItem.Text = "Generate JingShang Codes";
            // 
            // generateAcartonsCodesPgToolStripMenuItem
            // 
            this.generateAcartonsCodesPgToolStripMenuItem.Name = "generateAcartonsCodesPgToolStripMenuItem";
            this.generateAcartonsCodesPgToolStripMenuItem.Size = new System.Drawing.Size(253, 22);
            this.generateAcartonsCodesPgToolStripMenuItem.Text = "Generate Acartons Codes - Pg";
            // 
            // generateFMEACodesToolStripMenuItem
            // 
            this.generateFMEACodesToolStripMenuItem.Name = "generateFMEACodesToolStripMenuItem";
            this.generateFMEACodesToolStripMenuItem.Size = new System.Drawing.Size(253, 22);
            this.generateFMEACodesToolStripMenuItem.Text = "Generate FMEA Codes";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1178, 684);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(16, 48, 16, 16);
            this.Style = MetroFramework.MetroColorStyle.White;
            this.Text = "DbViewer";
            this.groupBox1.ResumeLayout(false);
            this.cmsDatabase.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tableTabControl.ResumeLayout(false);
            this.viewTabPage.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MetroGridDesign)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MetroGridResultSet)).EndInit();
            this.cmsDbTable.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TreeView tvDatabase;
        private System.Windows.Forms.ContextMenuStrip cmsDatabase;
        private System.Windows.Forms.ToolStripMenuItem exportDataDictionaryDocumentToExcelToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem newConnectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ImageList imgListCommon;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private MetroFramework.Controls.MetroScrollBar metroScrollBar1;
        private System.Windows.Forms.ContextMenuStrip cmsDbTable;
        private System.Windows.Forms.ToolStripMenuItem generateKdCodesToolStripMenuItem;
        private MetroFramework.Controls.MetroTabControl tableTabControl;
        private MetroFramework.Controls.MetroTabPage viewTabPage;
        private MetroFramework.Controls.MetroTabPage createScriptTabPage;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupBox2;
        private MetroFramework.Controls.MetroGrid MetroGridDesign;
        private System.Windows.Forms.GroupBox groupBox3;
        private MetroFramework.Controls.MetroGrid MetroGridResultSet;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem sqlServerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mySqlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateJingShangCodesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem postgresqlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateAcartonsCodesPgToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateFMEACodesToolStripMenuItem;
    }
}