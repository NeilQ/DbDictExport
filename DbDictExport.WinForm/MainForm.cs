using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using DbDictExport.WinForm.Data;
using DbDictExport.WinForm.Model;
using DbDictExport.WinForm.Service;
using Aspose.Cells;


namespace DbDictExport.WinForm
{
    public partial class MainForm : Form
    {

        private SqlConnectionStringBuilder connBuilder;
        /*
         * the database tree node's Name attribute start with string "db_"
         * the table tree node's Name arrtibute start with string "tb_"
         * the column tree node's Name arrtibute start with string "col_"
         * */
        private string databaseTreeNodeNamePrefix = "db_";
        private string tableTreeNodeNamePrefix = "tb_";
        //private string columnTreeNodeNamePrefix = "col_";
        public SqlConnectionStringBuilder ConnBuilder
        {
            get { return connBuilder; }
            set { this.connBuilder = value; }
        }


        public MainForm()
        {
            MainForm_Init();
        }

        public MainForm(SqlConnectionStringBuilder connBuilder)
        {
            this.ConnBuilder = connBuilder;
            MainForm_Init();
        }

        public void MainForm_Init()
        {
            InitializeComponent();
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.Load += MainForm_Load;
            this.tvDatabase.BeforeExpand += tvDatabase_BeforeExpand;
            this.tvDatabase.NodeMouseClick += tvDatabase_NodeMouseClick;
            this.tvDatabase.MouseDown += tvDatabase_MouseDown;
            foreach (ToolStripItem item in this.cmsDatabase.Items)
            {
                item.Click += new EventHandler(cmsDatabaseItem_Click);
            }
        }

        void MainForm_Load(object sender, EventArgs e)
        {
            if (this.ConnBuilder != null)
            {
                LoadDatabaseTreeNode();
            }

        }

        #region database TreeView's events
        void tvDatabase_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point clickPoint = new Point(e.X, e.Y);
                TreeNode currentNode = tvDatabase.GetNodeAt(clickPoint);
                if (currentNode != null) //check if you right click a tree node
                {
                    if (currentNode.Name.StartsWith(databaseTreeNodeNamePrefix))
                    {
                        currentNode.ContextMenuStrip = this.cmsDatabase;
                    }
                    this.tvDatabase.SelectedNode = currentNode;
                }
            }
        }

        void tvDatabase_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {

        }

        void tvDatabase_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Name.StartsWith(this.databaseTreeNodeNamePrefix))
            {
                if (e.Node.Nodes.Count == 1 && String.IsNullOrEmpty(e.Node.Nodes[0].Text))
                {
                    //if has the empty node
                    TreeNode rootNode = e.Node;
                    LoadTableTreeNode(rootNode);
                }
            }
        }
        #endregion

        private void cmsDatabaseItem_Click(object sender, EventArgs e)
        {
            ToolStripItem tripItem = sender as ToolStripItem;
            TreeNode currentNode = this.tvDatabase.SelectedNode;
            if (tripItem.Text == "Export data dictionary document to Excel")
            {
                try
                {
                    LoadingFormService.CreateForm();
                    LoadingFormService.SetFormCaption("Exporting...");

                    List<DbTable> tableList = DataAccess.GetDbTableList(this.connBuilder, currentNode.Text);
                    Workbook workbook = GenerateWorkbook(tableList);

                    LoadingFormService.CloseFrom();

                    SaveFileDialog dia = new SaveFileDialog();
                    dia.Filter = "Excel files(*.xlsx)|*.xlsx|Excel files(*.xls)|*.xls;";
                    dia.FileName = currentNode.Text + " Data Dictionary";
                    if (dia.ShowDialog() == DialogResult.OK)
                    {
                        workbook.Save(dia.FileName);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (tripItem.Text == "Refresh")
            {
                LoadTableTreeNode(currentNode);
            }

        }

        #region MenuItems click events
        private void newConnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoginForm login = new LoginForm();
            if (login.ShowDialog() == DialogResult.OK)
            {
                this.connBuilder = login.ConnBuilder;
                login.Close();
                LoadDatabaseTreeNode();
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox abox = new AboutBox();
            abox.ShowDialog();
        }
        #endregion

        #region Load TreeView nodes
        private void LoadTableTreeNode(TreeNode rootNode)
        {

            //if first expand the node
            //clear the empty node
            this.tvDatabase.Cursor = Cursors.AppStarting;
            rootNode.Nodes.Clear();
            List<string> tableNameList = DataAccess.GetDbTableNameList(connBuilder, rootNode.Text);
            foreach (var name in tableNameList)
            {
                TreeNode tableNode = new TreeNode()
                {
                    Name = tableTreeNodeNamePrefix + name,
                    Text = name,
                    ToolTipText = name,
                    //Tag = table
                };
                rootNode.Nodes.Add(tableNode);
            }
            this.tvDatabase.Cursor = Cursors.Default;

        }

        private void LoadDatabaseTreeNode()
        {
            this.tvDatabase.Nodes.Clear();
            TreeNode rootNode = new TreeNode();
            rootNode.Text = this.connBuilder.DataSource + string.Format("({0})", this.connBuilder.UserID);
            this.tvDatabase.Nodes.Add(rootNode);

            foreach (string dbName in DataAccess.GetDbNameList(this.ConnBuilder))
            {
                TreeNode databaseNode = new TreeNode()
                {
                    Text = dbName,
                    ToolTipText = dbName,
                    Name = this.databaseTreeNodeNamePrefix + dbName,
                };

                /*
                 * The child node will not load with database node when the form loaded,
                 * so here put a empty node which do nothing to every database node
                 * that tell someone there maybe some child nodes.
                 * It will be clear when the specific database node be expended,
                 * and load  real child nodes.
                 * */
                TreeNode emptyNode = new TreeNode();
                databaseNode.Nodes.Add(emptyNode);
                rootNode.Nodes.Add(databaseNode);
                //Maybe: give a img to every root node
            }
        }
        #endregion

        private Workbook GenerateWorkbook(List<DbTable> tableList)
        {
            Workbook workbook = new Workbook();
            Worksheet indexSheet = workbook.Worksheets[0];      //default "Sheet1"\
            if (tableList.Count > 0)
            {
                indexSheet.Name = tableList[0].Catalog + "_IndexSheet";

                for (int k = 0; k < tableList.Count; k++)
                {
                    //create a work sheet
                    //the max length of worksheet's name can't be larger than 31
                    string sheetName = tableList[k].Name.Length <= 31 ? tableList[k].Name : tableList[k].Name.Substring(0, 25) + "..." + k;
                    Worksheet sheet = workbook.Worksheets.Add(sheetName);
                    sheet.IsGridlinesVisible = false;
                    sheet.Cells.StandardHeight = 17;

                    int hi = indexSheet.Hyperlinks.Add(k, 0, 1, 1, String.Format("'{0}'!A1", sheetName));
                    indexSheet.Hyperlinks[hi].TextToDisplay = tableList[k].Name;

                    #region cell styles
                    Style titleStyle = workbook.Styles[workbook.Styles.Add()];
                    titleStyle.Font.Name = "Microsoft YaHei";
                    titleStyle.Font.Size = 20;
                    titleStyle.HorizontalAlignment = TextAlignmentType.Left;
                    titleStyle.HorizontalAlignment = TextAlignmentType.Center;

                    Style subtitleStyle = workbook.Styles[workbook.Styles.Add()];
                    subtitleStyle.Font.Name = "Microsoft YaHei";
                    subtitleStyle.Font.Size = 20;
                    subtitleStyle.Font.Color = Color.FromArgb(0, 175, 219);
                    subtitleStyle.HorizontalAlignment = TextAlignmentType.Left;
                    subtitleStyle.VerticalAlignment = TextAlignmentType.Center;

                    Style tableHeadStyle = workbook.Styles[workbook.Styles.Add()];
                    tableHeadStyle.Font.Name = "Microsoft YaHei";
                    tableHeadStyle.Font.Size = 12;
                    tableHeadStyle.Font.IsBold = true;
                    tableHeadStyle.Font.Color = Color.White;
                    tableHeadStyle.ForegroundColor = Color.FromArgb(64, 64, 64);
                    tableHeadStyle.Pattern = BackgroundType.Solid;
                    tableHeadStyle.HorizontalAlignment = TextAlignmentType.Left;
                    tableHeadStyle.VerticalAlignment = TextAlignmentType.Center;

                    Style valueCenterStyle = workbook.Styles[workbook.Styles.Add()];
                    valueCenterStyle.Font.Name = "Mircosoft YaHei";
                    valueCenterStyle.Font.Size = 11;
                    valueCenterStyle.HorizontalAlignment = TextAlignmentType.Center;
                    valueCenterStyle.VerticalAlignment = TextAlignmentType.Center;

                    Style valueLeftStyle = workbook.Styles[workbook.Styles.Add()];
                    valueLeftStyle.Font.Name = "Mircosoft YaHei";
                    valueLeftStyle.Font.Size = 11;
                    valueLeftStyle.HorizontalAlignment = TextAlignmentType.Left;
                    valueLeftStyle.VerticalAlignment = TextAlignmentType.Center;
                    #endregion

                    #region fill data in cells
                    //Table title at row 1
                    //sheet.Cells.Merge(0, 0, 1, 2);
                    //sheet.Cells.Merge(0, 2, 1, 8);
                    //sheet.Cells[0, 0].PutValue("TableName");
                    sheet.Cells[0, 0].PutValue(tableList[k].Name);
                    sheet.Cells[0, 0].SetStyle(subtitleStyle);
                    //sheet.Cells[0, 0].SetStyle(titleStyle);
                    sheet.Cells.SetRowHeight(0, 30);
                    //sheet.Cells[1, 0].PutValue(table.Name);
                    //sheet.Cells[0, 2].SetStyle(subtitleStyle);

                    //fields title at row 2
                    sheet.Cells[1, 0].PutValue("#");
                    sheet.Cells[1, 1].PutValue("Field");
                    sheet.Cells[1, 2].PutValue("Description");
                    sheet.Cells[1, 3].PutValue("Identity");
                    sheet.Cells[1, 4].PutValue("PK");
                    sheet.Cells[1, 5].PutValue("Type");
                    sheet.Cells[1, 6].PutValue("Length");
                    sheet.Cells[1, 7].PutValue("Nullable");
                    sheet.Cells[1, 8].PutValue("DefaultValue");
                    sheet.Cells[1, 9].PutValue("Comment");
                    for (int i = 0; i < 10; i++)
                    {
                        sheet.Cells[1, i].SetStyle(tableHeadStyle);
                    }
                    //fields from row 3
                    foreach (DbColumn column in tableList[k].ColumnList)
                    {
                        int rowNo = column.Order + 1;
                        sheet.Cells[rowNo, 0].PutValue(column.Order);
                        sheet.Cells[rowNo, 1].PutValue(column.Name);
                        sheet.Cells[rowNo, 2].PutValue(column.Description);
                        sheet.Cells[rowNo, 3].PutValue(column.IsIdentity ? "√" : "");
                        sheet.Cells[rowNo, 4].PutValue(column.PrimaryKey ? "√" : "");
                        sheet.Cells[rowNo, 5].PutValue(column.DbType);
                        sheet.Cells[rowNo, 6].PutValue(column.Length);
                        sheet.Cells[rowNo, 7].PutValue(column.IsNullable ? "√" : "");
                        sheet.Cells[rowNo, 8].PutValue(column.DefaultValue);
                        sheet.Cells[rowNo, 9].PutValue("");
                        for (int i = 0; i < 10; i++)
                        {
                            Cell cell = sheet.Cells[rowNo, i];
                            cell.SetStyle(valueLeftStyle);
                            if (rowNo % 2 == 1)
                            {
                                Style style = cell.GetStyle();
                                style.ForegroundColor = Color.AliceBlue;
                                style.Pattern = BackgroundType.Solid;
                                cell.SetStyle(style);
                            }
                        }

                        #region
                        /*
                    foreach (var col in new int[] { 0, 3, 4, 6, 7, 8 })
                    {
                        Cell cell = sheet.Cells[rowNo, col];
                        cell.SetStyle(valueCenterStyle);
                        if (rowNo % 2 == 1)
                        {
                            Style style = cell.GetStyle();
                            style.ForegroundColor = Color.AliceBlue;
                            style.Pattern = BackgroundType.Solid;
                            cell.SetStyle(style);
                        }
                    }
                    foreach (var col in new int[] { 1, 2, 5, 9 })
                    {
                        Cell cell = sheet.Cells[rowNo, col];
                        cell.SetStyle(valueLeftStyle);
                        if (rowNo % 2 == 1)
                        {
                            Style style = cell.GetStyle();
                            style.ForegroundColor = Color.AliceBlue;
                            style.Pattern = BackgroundType.Solid;
                            cell.SetStyle(style);
                        }
                    }
                    */
                        #endregion
                    }
                    #endregion

                    #region adjust column width
                    //sheet.AutoFitColumns();
                    // sheet.AutoFitRows();
                    sheet.Cells.SetColumnWidth(0, 6);      //#
                    sheet.Cells.SetColumnWidth(1, 30);     //field
                    sheet.Cells.SetColumnWidth(2, 20);     //desccription
                    sheet.Cells.SetColumnWidth(3, 10);     //identity
                    sheet.Cells.SetColumnWidth(4, 6);     //PK
                    sheet.Cells.SetColumnWidth(5, 17);     //Type
                    sheet.Cells.SetColumnWidth(6, 13);     //Length
                    sheet.Cells.SetColumnWidth(7, 10);      //Nullable
                    sheet.Cells.SetColumnWidth(8, 18);         //default value
                    sheet.Cells.SetColumnWidth(9, 30);          //comments
                    #endregion
                }
            }

            return workbook;
        }

    }

}
