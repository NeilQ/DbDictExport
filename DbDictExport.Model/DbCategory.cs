using System.Collections.Generic;

namespace DbDictExport.Core
{
    public class DbCategory
    {
        public string Name { get; set; }

        public List<DbColumn> Columns { get; set; } = new List<DbColumn>()
        {
            new DbColumn() {Name = "table1"},
            new DbColumn() {Name = "table2"}
        };
    }
}