using System;
using System.Data;
using System.Text;

namespace DbDictExport.Core.Codes
{
    public abstract class AbstractKdCodeFactory
    {
        public string ModuleName { get; set; }

        public string EntityName { get; set; }

        public DbTable Table { get; set; }

        public abstract StringBuilder GenerateCodes();

        protected string GetCSharpType(string dbtype)
        {
            if (dbtype.ToLower().Contains("char")) return "string";
            if (dbtype.ToLower().Contains("bit")) return "bool";
            if (dbtype.ToLower().Contains("bigint")) return "long";
            if (dbtype.ToLower().Contains("int")) return "int";
            return "";
        }

        protected string GetIndentStr(int indent)
        {
            return new string(' ', indent * 4);
        }
    }

}