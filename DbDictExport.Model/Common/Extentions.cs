using System;
using System.Collections.Generic;
using System.Data;

namespace DbDictExport.Core.Common
{
    public static class Extentions
    {
        /// <summary>
        /// To the data table.
        /// </summary>
        /// <param name="columnList">The column list.</param>
        /// <returns>DataTable.</returns>
        public static DataTable ToDataTable(this List<DbColumn> columnList)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("Name");
            dataTable.Columns.Add("PrimaryKey", Type.GetType("System.Boolean"));
            dataTable.Columns.Add("DbType");
            dataTable.Columns.Add("IsNullable", Type.GetType("System.Boolean"));
            dataTable.Columns.Add("Length");
            dataTable.Columns.Add("ForeignKey", Type.GetType("System.Boolean"));
            dataTable.Columns.Add("Description");
            dataTable.Columns.Add("IsIdentity", Type.GetType("System.Boolean"));
            dataTable.Columns.Add("DefaultValue");

            if (columnList == null)
            {
                return dataTable;
            }

            for (var i = 0; i < columnList.Count; i++)
            {
                var row = dataTable.NewRow();
                row["Name"] = columnList[i].Name;
                row["PrimaryKey"] = columnList[i].PrimaryKey;
                row["DbType"] = columnList[i].DbType;
                row["IsNullable"] = columnList[i].IsNullable;
                row["Length"] = columnList[i].Length;
                row["ForeignKey"] = columnList[i].ForeignKey;
                row["Description"] = columnList[i].Description;
                row["IsIdentity"] = columnList[i].IsIdentity;
                row["DefaultValue"] = columnList[i].DefaultValue;

                dataTable.Rows.Add(row);
            }
            return dataTable;
        }
    }
}
