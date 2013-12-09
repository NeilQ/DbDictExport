using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DbDictExport.WinForm.Model
{
    /// <summary>
    /// Class for table column of sql server database
    /// </summary>
    public class DbColumn
    {
        private DbTable dbTable;
        private string name;
        private int order;
        private bool isIdentity;
        private bool isNullable;
        private string dbType;
        private int? length;
        private bool primaryKey;
        private bool foreignKey;
        private string defaultValue;
        private string description;


        public DbTable DbTable
        {
            get
            {
                if (dbTable == null)
                {
                    dbTable = new DbTable();
                }
                return dbTable;
            }
            set { this.dbTable = value; }
        }

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public int Order
        {
            get { return order; }
            set { order = value; }
        }

        public bool IsIdentity
        {
            get { return isIdentity; }
            set { isIdentity = value; }
        }

        public bool IsNullable
        {
            get { return this.isNullable; }
            set { this.isNullable = value; }
        }

        public string DbType
        {
            get { return this.dbType; }
            set { this.dbType = value; }
        }

        public int? Length
        {
            get { return this.length; }
            set { this.length = value; }
        }

        public bool PrimaryKey
        {
            get { return this.primaryKey; }
            set { this.primaryKey = value; }
        }

        public bool ForeignKey
        {
            get { return this.foreignKey; }
            set { this.foreignKey = value; }
        }

        public string DefaultValue
        {
            get { return defaultValue; }
            set { defaultValue = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }
    }
}
