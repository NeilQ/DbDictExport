using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DbDictExport.Core;
using DbDictExport.Core.Common;
using DbDictExport.Core.Dal;
using DbDictExport.WinForm.CodeForms;
using DbDictExport.WinForm.LoginForms;
using DbDictExport.WinForm.Service;
using MetroFramework.Forms;
using MySql.Data.MySqlClient;
using Npgsql;

namespace DbDictExport.WinForm
{
    public partial class MainForm : MetroForm
    {


        //private string columnTreeNodeNamePrefix = "col_";
        public SqlConnectionStringBuilder SqlServerConnectionStringBuilder { get; set; }

        public MySqlConnectionStringBuilder MySqlConnectionStringBuilder { get; set; }

        public NpgsqlConnectionStringBuilder PostgresConnectionStringBuilder { get; set; }

        private static TreeNode SelectedTableNode { get; set; }

        public MainForm()
        {
            InitializeComponent();
            MetroGridResultSet.DataError += dgvResultSet_DataError;
            MetroGridDesign.SelectionMode = DataGridViewSelectionMode.CellSelect;
            MetroGridResultSet.SelectionMode = DataGridViewSelectionMode.CellSelect;
            tvDatabase.BeforeExpand += tvDatabase_BeforeExpand;
            tvDatabase.MouseDown += tvDatabase_MouseDown;
            foreach (ToolStripItem item in cmsDatabase.Items)
            {
                item.Click += cmsDatabaseItem_Click;
            }
            foreach (ToolStripItem item in cmsDbTable.Items)
            {
                item.Click += cmsDbTableItem_Click;
            }
            tvDatabase.ImageList = imgListCommon;

            generateKdCodesToolStripMenuItem.Text = Constants.CONTEXT_MENU_TABLE_GENERATE_KD_CODES;
            generateJingShangCodesToolStripMenuItem.Text = Constants.CONTEXT_MENU_TABLE_GENERATE_JINGSHANG_CODES;
            generateAcartonsCodesPgToolStripMenuItem.Text = Constants.CONTEXT_MENU_TABLE_GENERATE_ACARTONS_CODES;
            generateFMEACodesToolStripMenuItem.Text = Constants.CONTEXT_MENU_TABLE_GENERATE_FMEA_CODES;
        }

        void dgvResultSet_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (!MetroGridResultSet.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.Equals(DBNull.Value))
            {
                e.ThrowException = false;
            }

        }

        private string GetServerHost()
        {
            switch (Global.DataBaseType)
            {
                case DataBaseType.SqlServer:
                    return SqlServerConnectionStringBuilder.DataSource + $"({SqlServerConnectionStringBuilder.UserID})";
                case DataBaseType.Mysql:
                    return MySqlConnectionStringBuilder.Server +
                           $"({MySqlConnectionStringBuilder.UserID})";
                case DataBaseType.Postgresql:
                    return PostgresConnectionStringBuilder.Host +
                           $":{PostgresConnectionStringBuilder.Port} - {PostgresConnectionStringBuilder.Username}";
                default:
                    return string.Empty;
            }
        }

        private string GetConnectionString()
        {
            switch (Global.DataBaseType)
            {
                case DataBaseType.SqlServer:
                    return SqlServerConnectionStringBuilder.ConnectionString;
                case DataBaseType.Mysql:
                    return MySqlConnectionStringBuilder.ConnectionString;
                case DataBaseType.Postgresql:
                    return PostgresConnectionStringBuilder.ConnectionString;
                default:
                    return string.Empty;
            }
        }

        private void SetDataBaseName(string database)
        {
            switch (Global.DataBaseType)
            {
                case DataBaseType.SqlServer:
                    SqlServerConnectionStringBuilder.InitialCatalog = database;
                    break;
                case DataBaseType.Mysql:
                    MySqlConnectionStringBuilder.Database = database;
                    break;
                case DataBaseType.Postgresql:
                    PostgresConnectionStringBuilder.Database = database;
                    break;
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
                            if (currentNode.Name.StartsWith(Constants.DATABASE_TREE_NODE_NAME_PREFIX))
                            {
                                currentNode.ContextMenuStrip = cmsDatabase;
                            }
                            else if (currentNode.Name.StartsWith(Constants.TABLE_TREE_NODE_NAME_PREFIX))
                            {
                                currentNode.ContextMenuStrip = cmsDbTable;
                            }
                            tvDatabase.SelectedNode = currentNode;
                        }
                    }
                    break;
                case MouseButtons.Left:
                    {
                        var clickPoint = new Point(e.X, e.Y);
                        TreeNode currentNode = tvDatabase.GetNodeAt(clickPoint);
                        if (currentNode != null)
                        {
                            if (currentNode.Name.StartsWith(Constants.TABLE_TREE_NODE_NAME_PREFIX))
                            {
                                ClearGridData();

                                if (SelectedTableNode != null)
                                {
                                    SelectedTableNode.BackColor = Color.White;
                                }
                                SelectedTableNode = currentNode;
                                SelectedTableNode.BackColor = SystemColors.Highlight;

                                var table = currentNode.Tag as Table;
                                SetGridData(table);
                            }
                        }
                    }
                    break;
            }
        }

        void tvDatabase_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Name.StartsWith(Constants.DATABASE_TREE_NODE_NAME_PREFIX))
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
            var currentNode = tvDatabase.SelectedNode;
            if (tripItem == null) return;
            switch (tripItem.Text)
            {
                case Constants.CONTEXT_MENU_DATABASE_EXPORT_DICTIONARY:
                    try
                    {
                        LoadingFormService.CreateForm();
                        LoadingFormService.SetFormCaption(Constants.EXPORT_CAPTION);
                        // List<DbTable> tableList = DataAccess.GetDbTableListWithColumns(SqlServerConnectionStringBuilder, currentNode.Text);
                        SetDataBaseName(currentNode.Text);
                        var tables = Poco.LoadTables(GetConnectionString(), Global.ProviderName).OrderBy(t => t.DisplayName).ToList();
                        //Workbook workbook = ExcelHelper.GenerateWorkbook(tableList);
                        IExcelHelper helper = new AsposeExcelHelper();

                        //LoadingFormService.CloseFrom();

                        var dia = new SaveFileDialog
                        {
                            Filter = Constants.SAVE_FILE_DIALOG_FILTER,
                            FileName = currentNode.Text + " Data Dictionary"
                        };
                        if (dia.ShowDialog() == DialogResult.OK)
                        {
                            helper.GenerateWorkbook(tables, dia.FileName);
                            //workbook.Save(dia.FileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, Constants.ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        LoadingFormService.CloseFrom();
                    }
                    break;
                case Constants.CONTEXT_MENU_DATABASE_REFRESH:
                    LoadTableTreeNode(currentNode);
                    break;
            }
        }

        private void cmsDbTableItem_Click(object sender, EventArgs eventArgs)
        {
            var tripItem = sender as ToolStripItem;
            var currentNode = tvDatabase.SelectedNode;
            if (tripItem == null) return;
            Table currTable;
            switch (tripItem.Text)
            {
                case Constants.CONTEXT_MENU_TABLE_GENERATE_KD_CODES:
                    var table = currentNode.Tag as DbTable;
                    if (table == null) break;
                    table.ColumnList = DataAccess.GetDbColumnList(SqlServerConnectionStringBuilder, table.Name);
                    var form = new KdCodeForm(table);
                    form.Show();
                    break;
                case Constants.CONTEXT_MENU_TABLE_GENERATE_JINGSHANG_CODES:
                    currTable = currentNode.Tag as Table;
                    if (currTable == null) break;
                    var jsForm = new JSCodeForm(currTable);
                    jsForm.Show();
                    break;
                case Constants.CONTEXT_MENU_TABLE_GENERATE_ACARTONS_CODES:
                    currTable = currentNode.Tag as Table;
                    if (currTable == null) break;
                    var acartonsForm = new AcartonsCodeForm(currTable);
                    acartonsForm.Show();
                    break;
                case Constants.CONTEXT_MENU_TABLE_GENERATE_FMEA_CODES:
                    currTable = currentNode.Tag as Table;
                    if (currTable == null) break;
                    var feamForm = new FmeaCodeForm(currTable);
                    feamForm.Show();
                    break;
            }
        }
        #endregion

        #region MenuItems click events

        private void sqlServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadSqlServerLoginForm();
        }

        private void mySqlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadMysqlLoginForm();
        }

        private void postgresqlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadPostgresLoginForm();
        }
        #endregion

        #region Load TreeView nodes
        private void LoadTableTreeNode(TreeNode rootNode)
        {
            //if first expand the node
            //clear the empty node
            tvDatabase.Cursor = Cursors.AppStarting;
            rootNode.Nodes.Clear();

            SetDataBaseName(rootNode.Text);
            var tables = Poco.LoadTables(GetConnectionString(), Global.ProviderName).OrderBy(t => t.DisplayName);
            foreach (var table in tables)
            {
                var treeNode = new TreeNode
                {
                    Name = Constants.TABLE_TREE_NODE_NAME_PREFIX + table.Name,
                    Text = table.DisplayName,
                    ToolTipText = table.DisplayName,
                    Tag = table,
                    ImageIndex = Constants.TREENODE_DATATABLE_IMAGE_INDEX,
                    SelectedImageIndex = Constants.TREENODE_DATATABLE_IMAGE_INDEX
                };
                rootNode.Nodes.Add(treeNode);
            }

            tvDatabase.Cursor = Cursors.Default;
        }

        private void LoadDatabaseTreeNode()
        {
            tvDatabase.Nodes.Clear();
            var rootNode = new TreeNode
            {
                Text = GetServerHost(),
                ImageIndex = Constants.TREENODE_ROOT_IMAGE_INDEX,
                SelectedImageIndex = Constants.TREENODE_ROOT_IMAGE_INDEX
            };
            tvDatabase.Nodes.Add(rootNode);
            foreach (string dbName in Poco.LoadDatabases(GetConnectionString(), Global.ProviderName))
            {
                var databaseNode = new TreeNode
                {
                    Text = dbName,
                    ToolTipText = dbName,
                    Name = Constants.DATABASE_TREE_NODE_NAME_PREFIX + dbName,
                    ImageIndex = Constants.TREENODE_DATABASE_IMAGE_INDEX,
                    SelectedImageIndex = Constants.TREENODE_DATABASE_IMAGE_INDEX
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

        private void LoadSqlServerLoginForm()
        {
            var login = new LoginForm();
            if (login.ShowDialog() == DialogResult.OK)
            {
                SqlServerConnectionStringBuilder = login.ConnBuilder;
                ClearGridData();
                login.Close();
                LoadDatabaseTreeNode();
            }
        }

        private void LoadMysqlLoginForm()
        {
            var login = new MysqlLoginForm();
            if (login.ShowDialog() == DialogResult.OK)
            {
                MySqlConnectionStringBuilder = login.ConnBuilder;
                ClearGridData();
                login.Close();
                LoadDatabaseTreeNode();
            }
        }

        private void LoadPostgresLoginForm()
        {
            var login = new PostgresLoginForm();
            if (login.ShowDialog() == DialogResult.OK)
            {
                PostgresConnectionStringBuilder = login.ConnBuilder;
                ClearGridData();
                login.Close();
                LoadDatabaseTreeNode();
            }
        }

        private void SetGridData(string dbName, string tableName)
        {
            var table = DataAccess.GetTableByName(SqlServerConnectionStringBuilder, dbName, tableName);
            if (table == null) return;
            MetroGridDesign.DataSource = table.ColumnList.ToDataTable();

            if (dbName == Constants.DATABASE_TEMP_DB_NAME)
            {
                var dgvr = new DataGridViewRow();
                var cell = new DataGridViewTextBoxCell
                {
                    Value = "The temp table do not support viewing records."
                };
                dgvr.Cells.Add(cell);

                MetroGridResultSet.Columns.Add("Message", "Message");
                MetroGridResultSet.Columns["Message"].Width = 600;
                MetroGridResultSet.Rows.Add(dgvr);
                return;
            }
            MetroGridResultSet.DataSource = DataAccess.GetResultSetByDbTable(SqlServerConnectionStringBuilder, table);

            MetroGridDesign.ClearSelection();
            MetroGridResultSet.ClearSelection();
        }

        private void SetGridData(Table table)
        {
            //var table = DataAccess.GetTableByName(_connBuilder, dbName, tableName);
            if (table == null) return;
            MetroGridDesign.DataSource = table.Columns.ToDataTable();

            if (table.Name == Constants.DATABASE_TEMP_DB_NAME)
            {
                var dgvr = new DataGridViewRow();
                var cell = new DataGridViewTextBoxCell
                {
                    Value = "The temp table do not support viewing records."
                };
                dgvr.Cells.Add(cell);

                MetroGridResultSet.Columns.Add("Message", "Message");
                MetroGridResultSet.Columns["Message"].Width = 600;
                MetroGridResultSet.Rows.Add(dgvr);
                return;
            }
            //MetroGridResultSet.DataSource = DataAccess.GetResultSetByDbTable(_connBuilder, table);

            MetroGridDesign.ClearSelection();
            MetroGridResultSet.ClearSelection();
        }

        private void ClearGridData()
        {
            MetroGridResultSet.DataSource = null;
            MetroGridResultSet.Columns.Clear();

            MetroGridDesign.DataSource = null;
            MetroGridDesign.Columns.Clear();
        }


    }

}
