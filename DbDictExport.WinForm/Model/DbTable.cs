using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DbDictExport.WinForm.Model
{
    /// <summary>
    /// Class for a database table of sql server
    /// </summary>
    public sealed class DbTable
    {
        private List<DbColumn> columnList;

        /// <summary>
        /// Catalog of table
        /// </summary>
        public string Catalog { get; set; }

        /// <summary>
        /// Schema of table
        /// </summary>
        public string Schema { get; set; }

        /// <summary>
        /// Name of table
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// table type of table
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// columns of table
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
