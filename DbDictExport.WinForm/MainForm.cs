using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using DbDictExport.Dal;
using DbDictExport.Model;
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
        private const string DatabaseTreeNodeNamePrefix = "db_";
        private const string TableTreeNodeNamePrefix = "tb_";
        //private string columnTreeNodeNamePrefix = "col_";
        public SqlConnectionStringBuilder ConnBuilder
        {
            get { return connBuilder; }
            set { this.connBuilder = value; }
        }


        public MainForm()
        {
            InitializeComponent();
            this.tvDatabase.BeforeExpand += tvDatabase_BeforeExpand;
            this.tvDatabase.MouseDown += tvDatabase_MouseDown;
            foreach (ToolStripItem item in this.cmsDatabase.Items)
            {
                item.Click += cmsDatabaseItem_Click;
            }
            LoadLoginForm();
            this.tvDatabase.ImageList = this.imgListCommon;
        }

        #region database TreeView's events
        void tvDatabase_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right)     // Right click.
            {
                Point clickPoint = new Point(e.X, e.Y);
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
            else if (e.Button == MouseButtons.Left)     // Left click.
            {
                Point clickPoint = new Point(e.X, e.Y);
                TreeNode currrentNode = tvDatabase.GetNodeAt(clickPoint);
                if (currrentNode != null)
                {
                    if (currrentNode.Name.StartsWith(TableTreeNodeNamePrefix))
                    {
                        var table = currrentNode.Tag as DbTable;
                        table = DataAccess.GetTableByName(this.connBuilder, currrentNode.Parent.Text, table.Name);
                        if (table != null)
                        {
                            this.dgvTable.DataSource = table.ColumnList;
                            dgvTable.Columns["DbTable"].Visible = false;
                            dgvTable.Columns["Order"].Visible = false;
                        }
                    }
                }
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
            ToolStripItem tripItem = sender as ToolStripItem;
            TreeNode currentNode = this.tvDatabase.SelectedNode;
            if (tripItem.Text == "Export data dictionary document to Excel")
            {
                try
                {
                    LoadingFormService.CreateForm();
                    LoadingFormService.SetFormCaption("Exporting...");

                    List<DbTable> tableList = DataAccess.GetDbTableListWithColumns(this.connBuilder, currentNode.Text);
                    Workbook workbook = GenerateWorkbook(tableList);

                    //LoadingFormService.CloseFrom();

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
                finally
                {
                    LoadingFormService.CloseFrom();
                }
            }
            else if (tripItem.Text == "Refresh")
            {
                LoadTableTreeNode(currentNode);
            }


        }

        #endregion

        #region MenuItems click events
        private void newConnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadLoginForm();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var abox = new AboutBox();
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
            List<DbTable> tableList = DataAccess.GetDbTableNameListWithoutColumns(connBuilder, rootNode.Text);
            foreach (var table in tableList)
            {
                TreeNode tableNode = new TreeNode()
                {
                    Name = TableTreeNodeNamePrefix + table.Name,
                    Text = table.Schema + "." + table.Name,
                    ToolTipText = table.Schema + "." + table.Name,
                    Tag = table,
                    ImageIndex = 2,
                    SelectedImageIndex = 2
                };
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
                var databaseNode = new TreeNode()
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
                //Maybe: give a img to every root node
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

        private Workbook GenerateWorkbook(List<DbTable> tableList)
        {
            #region

            Workbook workbook = new Workbook();
            Worksheet indexSheet = workbook.Worksheets[0];      //default "Sheet1"\
            if (tableList.Count > 0)
            {
                indexSheet.Name = tableList[0].Catalog + "_IndexSheet";

                for (int k = 0; k < tableList.Count; k++)
                {
                    // Create a work sheet
                    // The max length of worksheet's name can't be larger than 31
                    string sheetName = tableList[k].Name.Length <= 31 ? tableList[k].Name : tableList[k].Name.Substring(0, 25) + "..." + k;
                    Worksheet sheet = workbook.Worksheets.Add(sheetName);
                    sheet.IsGridlinesVisible = false;
                    sheet.Cells.StandardHeight = 20;

                    int hi = indexSheet.Hyperlinks.Add(k, 0, 1, 1, String.Format("'{0}'!A1", sheetName));
                    indexSheet.Hyperlinks[hi].TextToDisplay = tableList[k].Name;
                    indexSheet.Cells.StandardHeight = 20;
                    indexSheet.IsGridlinesVisible = false;

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
                    // Table title at row 1
                    sheet.Cells[0, 0].PutValue(tableList[k].Name);
                    sheet.Cells[0, 0].SetStyle(subtitleStyle);
                    sheet.Cells.SetRowHeight(0, 30);

                    // Fields title at row 2
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
                    // Fields from row 3
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
                    }
                    #endregion

                    #region adjust column width
                    sheet.Cells.SetColumnWidth(0, 6);      //#
                    sheet.Cells.SetColumnWidth(1, 30);     //field
                    sheet.Cells.SetColumnWidth(2, 20);     //desccription
                    sheet.Cells.SetColumnWidth(3, 10);     //identity
                    sheet.Cells.SetColumnWidth(4, 6);      //PK
                    sheet.Cells.SetColumnWidth(5, 17);     //Type
                    sheet.Cells.SetColumnWidth(6, 13);     //Length
                    sheet.Cells.SetColumnWidth(7, 10);     //Nullable
                    sheet.Cells.SetColumnWidth(8, 18);     //default value
                    sheet.Cells.SetColumnWidth(9, 30);     //comments
                    #endregion

                }
            }

            return workbook;

            #endregion

            #region
            /*
            HSSFWorkbook workbook = new HSSFWorkbook();
            if (tableList !=null && tableList.Count > 0)
            {
                HSSFSheet indexSheet = (workbook.CreateSheet(tableList[0].Catalog + "_IndexSheet")) as HSSFSheet;
                for (int k = 0; k < tableList.Count; k++)
                {
                    //create a work sheet
                    //the max length of sheetname can't be larger than 31
                    string sheetName = tableList[k].Name.Length <= 31 ? tableList[k].Name : tableList[k].Name.Substring(0, 25) + "..." + k;
                    HSSFSheet sheet = (workbook.CreateSheet(sheetName)) as HSSFSheet;
                    sheet.DisplayGridlines = false;
                    sheet.DefaultRowHeightInPoints = 17;

                    //add links to indexSheet for every table sheet
                    ICell linkCell = indexSheet.CreateRow(0).CreateCell(0);
                    HSSFHyperlink link = new HSSFHyperlink(HyperlinkType.Document);
                    link.Address = String.Format("'{0}'!A1", sheetName);
                    link.Label = tableList[k].Name;
                    linkCell.SetCellValue(tableList[k].Name);
                    linkCell.Hyperlink = link;

                    #region cell styles
                    HSSFCellStyle titleStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                    HSSFFont titleFont = (HSSFFont)workbook.CreateFont();
                    titleFont.FontName = "Microsoft YaHei";
                    titleFont.FontHeightInPoints = 20;
                    titleStyle.SetFont(titleFont);
                    titleStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                    titleStyle.VerticalAlignment = VerticalAlignment.Center;

                    HSSFCellStyle subtitleStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                    HSSFFont subtitleFont = (HSSFFont)workbook.CreateFont();
                    subtitleFont.FontName = "Microsoft YaHei";
                    subtitleFont.FontHeightInPoints = 20;
                    //subtitleFont.Color = new HSSFPalette(new NPOI.HSSF.Record.PaletteRecord()).AddColor((byte)0,(byte)175,(byte)219).GetIndex() ;
                    subtitleFont.Color = HSSFColor.SkyBlue.Index;
                    subtitleStyle.SetFont(titleFont);
                    subtitleStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                    subtitleStyle.VerticalAlignment = VerticalAlignment.Center;

                    HSSFCellStyle tableHeadStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                    HSSFFont tableHeadFont = (HSSFFont)workbook.CreateFont();
                    tableHeadFont.FontName = "Microsoft YaHei";
                    tableHeadFont.FontHeightInPoints = 12;
                    tableHeadFont.Boldweight = 700;
                    tableHeadFont.Color = HSSFColor.White.Index;
                    tableHeadStyle.SetFont(titleFont);
                    tableHeadStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                    tableHeadStyle.VerticalAlignment = VerticalAlignment.Center;

                    HSSFCellStyle valueStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                    HSSFFont valueFont = (HSSFFont)workbook.CreateFont();
                    valueFont.FontName = "Microsoft YaHei";
                    valueFont.FontHeightInPoints = 11;
                    valueStyle.SetFont(titleFont);
                    valueStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;
                    valueStyle.VerticalAlignment = VerticalAlignment.Center;

                    #endregion

                    #region fill data in cells
                    //title--tablename
                    ICell titleCell = sheet.CreateRow(k).CreateCell(0);
                    titleCell.SetCellValue(tableList[k].Name);
                    titleCell.CellStyle = subtitleStyle;

                    //fields title at row 2
                    IRow headRow = sheet.CreateRow(1);
                    headRow.CreateCell(0).SetCellValue("#");
                    headRow.CreateCell(1).SetCellValue("Field");
                    headRow.CreateCell(2).SetCellValue("Description");
                    headRow.CreateCell(3).SetCellValue("Identity");
                    headRow.CreateCell(4).SetCellValue("PK");
                    headRow.CreateCell(5).SetCellValue("Type");
                    headRow.CreateCell(6).SetCellValue("Length");
                    headRow.CreateCell(7).SetCellValue("Nullable");
                    headRow.CreateCell(8).SetCellValue("DefaultValue");
                    headRow.CreateCell(9).SetCellValue("Comment");
                    for (int i = 0; i < 10; i++)
                    {
                        HSSFCell cell = (HSSFCell)sheet.GetRow(1).GetCell(i);
                        cell.CellStyle = tableHeadStyle;
                    }
                    //fields from row 3
                    foreach (DbColumn column in tableList[k].ColumnList)
                    {
                        int rowNo = column.Order + 1;
                        IRow valueRow = sheet.CreateRow(rowNo);
                        valueRow.CreateCell(0).SetCellValue(column.Order);
                        valueRow.CreateCell(1).SetCellValue(column.Name);
                        valueRow.CreateCell(2).SetCellValue(column.Description);
                        valueRow.CreateCell(3).SetCellValue(column.IsIdentity ? "√" : "");
                        valueRow.CreateCell(4).SetCellValue(column.PrimaryKey ? "√" : "");
                        valueRow.CreateCell(5).SetCellValue(column.DbType);
                        valueRow.CreateCell(6).SetCellValue(column.Length.ToString());
                        valueRow.CreateCell(7).SetCellValue(column.IsNullable ? "√" : "");
                        valueRow.CreateCell(8).SetCellValue(column.DefaultValue);
                        valueRow.CreateCell(9).SetCellValue("");
                        for (int i = 0; i < 10; i++)
                        {
                            ICell valueCell = sheet.GetRow(rowNo).GetCell(i);
                            valueCell.CellStyle = valueStyle;
                            if (rowNo % 2 == 1)
                            {
                                HSSFCellStyle style = (HSSFCellStyle)valueCell.CellStyle;
                                style.FillForegroundColor = HSSFColor.PaleBlue.Index;
                                style.FillPattern = FillPattern.SolidForeground;
                                valueCell.CellStyle = style;
                            }
                        }
                    }
                    #endregion

                    #region adjust column width

                    sheet.SetColumnWidth(0, 6*256);         //#
                    sheet.SetColumnWidth(1, 30*256);        //Field
                    sheet.SetColumnWidth(2, 20*256);        //Description
                    sheet.SetColumnWidth(3, 10*256);        //Identity
                    sheet.SetColumnWidth(4, 6*256);         //PK
                    sheet.SetColumnWidth(5, 17*256);         //Type   
                    sheet.SetColumnWidth(6, 13*256);        //Length
                    sheet.SetColumnWidth(7, 10*256);        //Nullable
                    sheet.SetColumnWidth(8, 18*256);        //Default value
                    sheet.SetColumnWidth(9, 30*256);        //Comments

                    #endregion
                }
            }
            return workbook;
             * */
            #endregion

        }
    }

}
