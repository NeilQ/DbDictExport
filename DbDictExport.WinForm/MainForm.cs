using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DbDictExport.Common;
using DbDictExport.Dal;
using DbDictExport.Model;
using DbDictExport.WinForm.Service;
using MetroFramework.Controls;
using MetroFramework.Forms;

namespace DbDictExport.WinForm
{
    public partial class MainForm : MetroForm
    {

        private SqlConnectionStringBuilder _connBuilder;


        //private string columnTreeNodeNamePrefix = "col_";
        public SqlConnectionStringBuilder ConnBuilder
        {
            get { return _connBuilder; }
            set { _connBuilder = value; }
        }

        private static TreeNode SelectedTableNode { get; set; }

        public MainForm()
        {
            InitializeComponent();
            MetroGridResultSet.DataError += dgvResultSet_DataError;
            tvDatabase.BeforeExpand += tvDatabase_BeforeExpand;
            tvDatabase.MouseDown += tvDatabase_MouseDown;
            foreach (ToolStripItem item in cmsDatabase.Items)
            {
                item.Click += cmsDatabaseItem_Click;
            }
            LoadLoginForm();
            tvDatabase.ImageList = imgListCommon;
        }



        void dgvResultSet_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (!MetroGridResultSet.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.Equals(DBNull.Value))
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
                            if (currentNode.Name.StartsWith(Constants.DATABASE_TREE_NODE_NAME_PREFIX))
                            {
                                currentNode.ContextMenuStrip = cmsDatabase;
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

                                var table = currentNode.Tag as DbTable;
                                SetGridData(currentNode.Parent.Text, table.Name);
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
                case "Export data dictionary document to Excel":
                    try
                    {
                        LoadingFormService.CreateForm();
                        LoadingFormService.SetFormCaption(Constants.EXPORT_CAPTION);
                        List<DbTable> tableList = DataAccess.GetDbTableListWithColumns(_connBuilder, currentNode.Text);
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
                            helper.GenerateWorkbook(tableList, dia.FileName);
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
            tvDatabase.Cursor = Cursors.AppStarting;
            rootNode.Nodes.Clear();
            List<DbTable> tableList = DataAccess.GetDbTableNameListWithoutColumns(_connBuilder, rootNode.Text);
            foreach (var tableNode in tableList.Select(table => new TreeNode
            {
                Name = Constants.TABLE_TREE_NODE_NAME_PREFIX + table.Name,
                Text = String.Format("{0}.{1}", table.Schema, table.Name),
                ToolTipText = String.Format("{0}.{1}", table.Schema, table.Name),
                Tag = table,
                ImageIndex = 2,
                SelectedImageIndex = 2
            }))
            {
                rootNode.Nodes.Add(tableNode);
            }
            tvDatabase.Cursor = Cursors.Default;

        }

        private void LoadDatabaseTreeNode()
        {
            tvDatabase.Nodes.Clear();
            var rootNode = new TreeNode
            {
                Text = _connBuilder.DataSource + string.Format("({0})", _connBuilder.UserID),
                ImageIndex = 0,
                SelectedImageIndex = 0
            };
            tvDatabase.Nodes.Add(rootNode);
            foreach (string dbName in DataAccess.GetDbNameList(ConnBuilder))
            {
                var databaseNode = new TreeNode
                {
                    Text = dbName,
                    ToolTipText = dbName,
                    Name = Constants.DATABASE_TREE_NODE_NAME_PREFIX + dbName,
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
                _connBuilder = login.ConnBuilder;
                ClearGridData();
                login.Close();
                LoadDatabaseTreeNode();
            }
        }

        private void SetGridData(string dbName, string tableName)
        {
            var table = DataAccess.GetTableByName(_connBuilder, dbName, tableName);
            if (table == null) return;

            MetroGridDesign.DataSource = table.ColumnList;

            MetroGridDesign.Columns["DbTable"].Visible = false;
            MetroGridDesign.Columns["Order"].Visible = false;


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
            MetroGridResultSet.DataSource = DataAccess.GetResultSetByDbTable(_connBuilder, table);

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
