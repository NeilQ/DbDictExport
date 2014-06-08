using System.Collections.Generic;

namespace DbDictExport.WinForm.Model
{
    /// <summary>
    /// DbTable Class for a database table. 
    /// </summary>
    public sealed class DbTable
    {
        private List<DbColumn> columnList;

        /// <summary>
        /// Gets or sets the catalog of data table.
        /// </summary>
        public string Catalog { get; set; }

        /// <summary>
        /// Gets or sets the schema of data table.
        /// </summary>
        public string Schema { get; set; }

        /// <summary>
        /// Gets or sets the name of data table.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the table type of data table.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the columns list of table.
        /// </summary>
        public List<DbColumn> ColumnList
        {
            get 
            {
                if (this.columnList == null)
                {
                    columnList = new List<DbColumn>();
                }
                return columnList;
            }
            set { this.columnList = value; }
        }
    }
}
