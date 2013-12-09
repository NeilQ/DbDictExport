using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DbDictExport.WinForm.Model
{
    /// <summary>
    /// Class for a database table of sql server
    /// </summary>
    public class DbTable
    {
        private string catalog;
        private string schema;
        private string name;
        private string type;
        private List<DbColumn> columnList;

        /// <summary>
        /// Catalog of table
        /// </summary>
        public string Catalog
        {
            get { return catalog; }
            set { catalog = value; }
        }
        
        /// <summary>
        /// Schema of table
        /// </summary>
        public string Schema
        {
            get { return schema; }
            set { schema = value; }
        }

        /// <summary>
        /// Name of table
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// table type of table
        /// </summary>
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

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
