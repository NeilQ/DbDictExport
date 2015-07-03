using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;
using DbDictExport.Dal;
using DbDictExport.Model;
using DbDictExport.Common;
using DbDictExport.WinForm.Service;
using MetroFramework.Forms;



namespace DbDictExport.WinForm
{
    public partial class MainForm : MetroForm
    {

        private SqlConnectionStringBuilder connBuilder;
        /*
         * the database tree node's Name attribute start with string "db_"
         * the table tree node's Name arrtibute start with string "tb_"
         * the column tree node's Name arrtibute start with string "col_"
         * */
        private const string DatabaseTreeNodeNamePrefix = "db_";
        private const string TableTreeNodeNamePrefix = "tb_";
        private const string DabaseTempDbName = "tempdb";
        //private string columnTreeNodeNamePrefix = "col_";
        public SqlConnectionStringBuilder ConnBuilder
        {
            get { return connBuilder; }
            set { this.connBuilder = value; }
        }

        private static TreeNode SelectedTableNode { get; set; }

        public MainForm()
        {
            InitializeComponent();
            this.dgvResultSet.DataError += dgvResultSet_DataError;
            this.tvDatabase.BeforeExpand += tvDatabase_BeforeExpand;
            this.tvDatabase.MouseDown += tvDatabase_MouseDown;
            foreach (ToolStripItem item in this.cmsDatabase.Items)
            {
                item.Click += cmsDatabaseItem_Click;
            }
            LoadLoginForm();
            this.tvDatabase.ImageList = this.imgListCommon;
        }



        void dgvResultSet_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (!dgvResultSet.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.Equals(DBNull.Value))
            {
                e.ThrowException = false;
            }

        }

        #region database TreeView's events
        void tvDatabase_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Right:
                    {
                        var clickPoint = new Point(e.X, e.Y);
                        TreeNode currentNode = tvDatabase.GetNodeAt(clickPoint);
                        if (currentNode != null)
                        {
                            if (currentNode.Name.StartsWith(DatabaseTreeNodeNamePrefix))
                            {
                                currentNode.ContextMenuStrip = this.cmsDatabase;
                            }
                            this.tvDatabase.SelectedNode = currentNode;
                        }
                    }
                    break;
                case MouseButtons.Left:
                    {
                        var clickPoint = new Point(e.X, e.Y);
                        TreeNode currentNode = tvDatabase.GetNodeAt(clickPoint);
                        if (currentNode != null)
                        {
                            if (currentNode.Name.StartsWith(TableTreeNodeNamePrefix))
                            {
                                this.dgvResultSet.DataSource = null;
                                this.dgvResultSet.Columns.Clear();
                                this.dgvTable.DataSource = null;
                                this.dgvTable.Columns.Clear();

                                if (SelectedTableNode != null)
                                {
                                    SelectedTableNode.BackColor = Color.White;
                                }
                                SelectedTableNode = currentNode;
                                SelectedTableNode.BackColor = SystemColors.Highlight;

                                var table = currentNode.Tag as DbTable;
                                table = DataAccess.GetTableByName(this.connBuilder, currentNode.Parent.Text, table.Name);
                                if (table != null)
                                {
                                    this.dgvTable.DataSource = table.ColumnList;
                                    dgvTable.Columns["DbTable"].Visible = false;
                                    dgvTable.Columns["Order"].Visible = false;

                                    if (currentNode.Parent.Text == DabaseTempDbName)
                                    {
                                        var dgvr = new DataGridViewRow();
                                        var cell = new DataGridViewTextBoxCell
                                        {
                                            Value = "The temp table do not support viewing records.",
                                        };
                                        dgvr.Cells.Add(cell);

                                        this.dgvResultSet.Columns.Add("Message", "Message");
                                        this.dgvResultSet.Columns["Message"].Width = 600;
                                        this.dgvResultSet.Rows.Add(dgvr);
                                        return;
                                    }
                                    this.dgvResultSet.DataSource = DataAccess.GetResultSetByDbTable(this.connBuilder, table);
                                    this.dgvResultSet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                                }
                            }
                        }
                    }
                    break;
            }
        }

        void tvDatabase_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Name.StartsWith(DatabaseTreeNodeNamePrefix))
            {
                if (e.Node.Nodes.Count == 1 && String.IsNullOrEmpty(e.Node.Nodes[0].Text))
                {
                    // If has the empty node
                    TreeNode rootNode = e.Node;
                    LoadTableTreeNode(rootNode);
                }
            }
        }
        #endregion

        #region ContextMenuStrip click event

        private void cmsDatabaseItem_Click(object sender, EventArgs e)
        {
            var tripItem = sender as ToolStripItem;
            var currentNode = this.tvDatabase.SelectedNode;
            if (tripItem == null) return;
            switch (tripItem.Text)
            {
                case "Export data dictionary document to Excel":
                    try
                    {
                        LoadingFormService.CreateForm();
                        LoadingFormService.SetFormCaption("Exporting...");
                        List<DbTable> tableList = DataAccess.GetDbTableListWithColumns(this.connBuilder, currentNode.Text);
                        //Workbook workbook = ExcelHelper.GenerateWorkbook(tableList);
                        IExcelHelper helper = new AsposeExcelHelper();

                        //LoadingFormService.CloseFrom();

                        var dia = new SaveFileDialog
                        {
                            Filter = "Excel files(*.xlsx)|*.xlsx|Excel files(*.xls)|*.xls;",
                            FileName = currentNode.Text + " Data Dictionary"
                        };
                        if (dia.ShowDialog() == DialogResult.OK)
                        {
                            helper.GenerateWorkbook(tableList, dia.FileName);
                            //workbook.Save(dia.FileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        LoadingFormService.CloseFrom();
                    }
                    break;
                case "Refresh":
                    LoadTableTreeNode(currentNode);
                    break;
            }
        }

        #endregion

        #region MenuItems click events
        private void newConnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadLoginForm();
        }

        /*
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var abox = new AboutBox();
            abox.ShowDialog();
        }
         * */

        #endregion

        #region Load TreeView nodes
        private void LoadTableTreeNode(TreeNode rootNode)
        {
            //if first expand the node
            //clear the empty node
            this.tvDatabase.Cursor = Cursors.AppStarting;
            rootNode.Nodes.Clear();
            List<DbTable> tableList = DataAccess.GetDbTableNameListWithoutColumns(connBuilder, rootNode.Text);
            foreach (var tableNode in tableList.Select(table => new TreeNode
            {
                Name = TableTreeNodeNamePrefix + table.Name,
                Text = table.Schema + "." + table.Name,
                ToolTipText = table.Schema + "." + table.Name,
                Tag = table,
                ImageIndex = 2,
                SelectedImageIndex = 2
            }))
            {
                rootNode.Nodes.Add(tableNode);
            }
            this.tvDatabase.Cursor = Cursors.Default;

        }

        private void LoadDatabaseTreeNode()
        {
            this.tvDatabase.Nodes.Clear();
            var rootNode = new TreeNode
            {
                Text = this.connBuilder.DataSource + string.Format("({0})", this.connBuilder.UserID),
                ImageIndex = 0,
                SelectedImageIndex = 0
            };
            this.tvDatabase.Nodes.Add(rootNode);
            foreach (string dbName in DataAccess.GetDbNameList(this.ConnBuilder))
            {
                var databaseNode = new TreeNode
                {
                    Text = dbName,
                    ToolTipText = dbName,
                    Name = DatabaseTreeNodeNamePrefix + dbName,
                    ImageIndex = 1,
                    SelectedImageIndex = 1
                };

                /*
                 * The child node will not load with database node when the form loaded,
                 * so here put a empty node which do nothing to every database node
                 * that tell someone there maybe some child nodes.
                 * It will be clear when the specific database node be expended,
                 * and load  real child nodes.
                 * */
                var emptyNode = new TreeNode();
                databaseNode.Nodes.Add(emptyNode);
                rootNode.Nodes.Add(databaseNode);
            }
        }
        #endregion

        private void LoadLoginForm()
        {
            var login = new LoginForm();
            if (login.ShowDialog() == DialogResult.OK)
            {
                this.connBuilder = login.ConnBuilder;
                login.Close();
                LoadDatabaseTreeNode();
            }
        }

    }

}
