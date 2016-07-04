

namespace DbDictExport.Core
{
    /// <summary>
    /// DbColumn Class for data table's column. 
    /// </summary>
    public sealed class DbColumn
    {
        private DbTable dbTable;


        /// <summary>
        /// Gets or sets the DbTable.
        /// </summary>
        public DbTable DbTable
        {
            get { return dbTable ?? (dbTable = new DbTable()); }
            set { this.dbTable = value; }
        }

        /// <summary>
        /// Gets or sets the name of the column.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicates whether the column is primary.
        /// </summary>
        public bool PrimaryKey { get; set; }

        /// <summary>
        /// Gets or sets the sql db type of the column.
        /// </summary>
        public string DbType { get; set; }

        /// <summary>
        /// Gets or sets a value indicates whether the column is nullable.
        /// </summary>
        public bool IsNullable { get; set; }

        /// <summary>
        /// Gets or sets the length of the column.
        /// </summary>
        public int? Length { get; set; }

        /// <summary>
        /// Gets or sets a value indicates whether the column is a foreign key.
        /// </summary>
        public bool ForeignKey { get; set; }

        /// <summary>
        /// Gets or sets the description of the column.
        /// </summary>
        public string Description { get; set; }

        
        /// <summary>
        /// Gets or sets a value indicates whether the column is a identity.
        /// </summary>
        public bool IsIdentity { get; set; }

        /// <summary>
        /// Gets or sets the defaule value of the column.
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets the order of the column. 
        /// </summary>
        public int Order { get; set; }
    }
}
