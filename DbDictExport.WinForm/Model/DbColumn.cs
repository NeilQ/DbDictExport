

namespace DbDictExport.WinForm.Model
{
    /// <summary>
    /// Class for table column of sql server database
    /// </summary>
    public class DbColumn
    {
        private DbTable dbTable;


        public DbTable DbTable
        {
            get { return dbTable ?? (dbTable = new DbTable()); }
            set { this.dbTable = value; }
        }

        public string Name { get; set; }

        public bool PrimaryKey { get; set; }

        public string DbType { get; set; }

        public bool IsNullable { get; set; }

        public int? Length { get; set; }

        public bool ForeignKey { get; set; }

        public string Description { get; set; }

        public bool IsIdentity { get; set; }

        public string DefaultValue { get; set; }



        public int Order { get; set; }
    }
}
