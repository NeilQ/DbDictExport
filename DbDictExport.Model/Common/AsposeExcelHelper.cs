using System;
using System.Collections.Generic;
using System.Drawing;
using Aspose.Cells;
using DbDictExport.Core.Dal;

namespace DbDictExport.Core.Common
{
    public sealed class AsposeExcelHelper : IExcelHelper
    {
        public void GenerateWorkbook(IList<Table> tableList, string fileName)
        {
            #region

            var workbook = new Workbook();
            Worksheet indexSheet = workbook.Worksheets[0];      //default "Sheet1"\
            if (tableList.Count > 0)
            {
                indexSheet.Name = tableList[0].Schema + "_IndexSheet";

                for (var k = 0; k < tableList.Count; k++)
                {
                    // Create a work sheet
                    // The max length of worksheet's name can't be larger than 31
                    string sheetName = tableList[k].Name.Length <= 31 ? tableList[k].Name : tableList[k].Name.Substring(0, 25) + "..." + k;

                    Worksheet sheet = workbook.Worksheets.Add(sheetName);
                    sheet.IsGridlinesVisible = false;
                    sheet.Cells.StandardHeight = 20;

                    int hi = indexSheet.Hyperlinks.Add(k, 0, 1, 1, $"'{sheetName}'!A1");
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
                    for (var i = 0; i < 10; i++)
                    {
                        sheet.Cells[1, i].SetStyle(tableHeadStyle);
                    }
                    // Fields from row 3
                    for (var i = 0; i < tableList[k].Columns.Count; i++)
                    {
                        var column = tableList[k].Columns[i];
                        var rowNo = i + 2;
                        sheet.Cells[rowNo, 0].PutValue(i + 1);
                        sheet.Cells[rowNo, 1].PutValue(column.Name);
                        sheet.Cells[rowNo, 2].PutValue(column.Description);
                        sheet.Cells[rowNo, 3].PutValue(column.IsAutoIncrement ? "√" : "");
                        sheet.Cells[rowNo, 4].PutValue(column.IsPK ? "√" : "");
                        sheet.Cells[rowNo, 5].PutValue(column.DbType);
                        sheet.Cells[rowNo, 6].PutValue(column.Length);
                        sheet.Cells[rowNo, 7].PutValue(column.IsNullable ? "√" : "");
                        sheet.Cells[rowNo, 8].PutValue(column.DefaultValue);
                        sheet.Cells[rowNo, 9].PutValue("");
                        for (var index = 0; index < 10; index++)
                        {
                            Cell cell = sheet.Cells[rowNo, index];
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
                    //foreach (var column in tableList[k].Columns)
                    //{
                    //    var rowNo = column. + 1;
                    //    sheet.Cells[rowNo, 0].PutValue(column.Order);
                    //    sheet.Cells[rowNo, 1].PutValue(column.Name);
                    //    sheet.Cells[rowNo, 2].PutValue(column.Description);
                    //    sheet.Cells[rowNo, 3].PutValue(column.IsIdentity ? "√" : "");
                    //    sheet.Cells[rowNo, 4].PutValue(column.PrimaryKey ? "√" : "");
                    //    sheet.Cells[rowNo, 5].PutValue(column.DbType);
                    //    sheet.Cells[rowNo, 6].PutValue(column.Length);
                    //    sheet.Cells[rowNo, 7].PutValue(column.IsNullable ? "√" : "");
                    //    sheet.Cells[rowNo, 8].PutValue(column.DefaultValue);
                    //    sheet.Cells[rowNo, 9].PutValue("");
                    //    for (var i = 0; i < 10; i++)
                    //    {
                    //        Cell cell = sheet.Cells[rowNo, i];
                    //        cell.SetStyle(valueLeftStyle);
                    //        if (rowNo % 2 == 1)
                    //        {
                    //            Style style = cell.GetStyle();
                    //            style.ForegroundColor = Color.AliceBlue;
                    //            style.Pattern = BackgroundType.Solid;
                    //            cell.SetStyle(style);
                    //        }
                    //    }
                    //}
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

            workbook.Save(fileName);

            #endregion

        }
    }
}
