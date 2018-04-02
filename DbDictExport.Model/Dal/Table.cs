using System.Collections.Generic;
using System.Linq;

namespace DbDictExport.Core.Dal
{
    public class Table
    {
        private const string V = "default";
        public List<Column> Columns;
        public string Name;
        public string Schema;
        public bool IsView;
        public string CleanName;
        public string ClassName;
        public string SequenceName;
        public bool Ignore;

        public Column PK
        {
            get { return this.Columns.SingleOrDefault(x => x.IsPK); }
        }

        public Column GetColumn(string columnName)
        {
            return Columns.Single(x => string.Compare(x.Name, columnName, true) == 0);
        }

        public Column this[string columnName]
        {
            get { return GetColumn(columnName); }
        }

        public override string ToString()
        {
            return (Schema ?? "default") + "." + Name;
        }
    }

    public class Column
    {
        public string Name;
        public string PropertyName;
        public string PropertyType;
        public bool IsPK;
        public bool IsNullable;
        public bool IsAutoIncrement;
        public bool Ignore;

        public string DbType { get; set; }
        public int? Length { get; set; }
        public string Description { get; set; }
        public string DefaultValue { get; set; }
    }

    public class Tables : List<Table>
    {
        public Tables()
        {
        }

        public Table GetTable(string tableName)
        {
            return this.Single(x => string.Compare(x.Name, tableName, true) == 0);
        }

        public Table this[string tableName] => GetTable(tableName);
    }
}
