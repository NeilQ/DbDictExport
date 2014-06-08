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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tvDatabase = new System.Windows.Forms.TreeView();
            this.cmsDatabase = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportDataDictionaryDocumentToExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.newConnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvTable = new System.Windows.Forms.DataGridView();
            this.imgListCommon = new System.Windows.Forms.ImageList(this.components);
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dgvResultSet = new System.Windows.Forms.DataGridView();
            this.groupBox1.SuspendLayout();
            this.cmsDatabase.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTable)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResultSet)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tvDatabase);
            this.groupBox1.Location = new System.Drawing.Point(12, 31);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(329, 784);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "DataBase";
            // 
            // tvDatabase
            // 
            this.tvDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvDatabase.Location = new System.Drawing.Point(3, 23);
            this.tvDatabase.Name = "tvDatabase";
            this.tvDatabase.Size = new System.Drawing.Size(323, 758);
            this.tvDatabase.TabIndex = 0;
            // 
            // cmsDatabase
            // 
            this.cmsDatabase.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshToolStripMenuItem,
            this.exportDataDictionaryDocumentToExcelToolStripMenuItem});
            this.cmsDatabase.Name = "cmsDatabase";
            this.cmsDatabase.Size = new System.Drawing.Size(353, 52);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(352, 24);
            this.refreshToolStripMenuItem.Text = "Refresh";
            // 
            // exportDataDictionaryDocumentToExcelToolStripMenuItem
            // 
            this.exportDataDictionaryDocumentToExcelToolStripMenuItem.Name = "exportDataDictionaryDocumentToExcelToolStripMenuItem";
            this.exportDataDictionaryDocumentToExcelToolStripMenuItem.Size = new System.Drawing.Size(352, 24);
            this.exportDataDictionaryDocumentToExcelToolStripMenuItem.Text = "Export data dictionary document to Excel";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newConnectToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1316, 28);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // newConnectToolStripMenuItem
            // 
            this.newConnectToolStripMenuItem.Name = "newConnectToolStripMenuItem";
            this.newConnectToolStripMenuItem.Size = new System.Drawing.Size(109, 24);
            this.newConnectToolStripMenuItem.Text = "New Connect";
            this.newConnectToolStripMenuItem.Click += new System.EventHandler(this.newConnectToolStripMenuItem_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgvTable);
            this.groupBox2.Location = new System.Drawing.Point(347, 31);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(969, 441);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Design";
            // 
            // dgvTable
            // 
            this.dgvTable.AllowUserToAddRows = false;
            this.dgvTable.AllowUserToDeleteRows = false;
            this.dgvTable.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTable.Location = new System.Drawing.Point(3, 23);
            this.dgvTable.Name = "dgvTable";
            this.dgvTable.ReadOnly = true;
            this.dgvTable.RowTemplate.Height = 27;
            this.dgvTable.Size = new System.Drawing.Size(963, 415);
            this.dgvTable.TabIndex = 0;
            // 
            // imgListCommon
            // 
            this.imgListCommon.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgListCommon.ImageStream")));
            this.imgListCommon.TransparentColor = System.Drawing.Color.Transparent;
            this.imgListCommon.Images.SetKeyName(0, "databases_tree_root.ico");
            this.imgListCommon.Images.SetKeyName(1, "database_tree.ico");
            this.imgListCommon.Images.SetKeyName(2, "datatable_tree.ico");
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dgvResultSet);
            this.groupBox3.Location = new System.Drawing.Point(347, 475);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(966, 337);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Result Set";
            // 
            // dgvResultSet
            // 
            this.dgvResultSet.AllowUserToAddRows = false;
            this.dgvResultSet.AllowUserToDeleteRows = false;
            this.dgvResultSet.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvResultSet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResultSet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvResultSet.Location = new System.Drawing.Point(3, 23);
            this.dgvResultSet.Name = "dgvResultSet";
            this.dgvResultSet.ReadOnly = true;
            this.dgvResultSet.RowTemplate.Height = 27;
            this.dgvResultSet.Size = new System.Drawing.Size(960, 311);
            this.dgvResultSet.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1316, 821);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DbView";
            this.groupBox1.ResumeLayout(false);
            this.cmsDatabase.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTable)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvResultSet)).EndInit();
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
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dgvTable;
        private System.Windows.Forms.ImageList imgListCommon;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dgvResultSet;
    }
}