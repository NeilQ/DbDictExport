using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbDictExport.Core.Common;

namespace DbDictExport.Core.Codes
{
    public class DalKdCodeFactory : AbstractKdCodeFactory
    {
        public DalKdCodeFactory(string entityName, string moduleName, DbTable dbTable)
        {
            EntityName = entityName;
            ModuleName = moduleName;
            Table = dbTable;

            if (string.IsNullOrEmpty(EntityName))
                EntityName = Constants.KDCODE_DEFAULT_ENTITY_NAME;
            if (string.IsNullOrEmpty(ModuleName))
                ModuleName = Constants.KDCODE_DEFAULT_MODULE_NAME;
        }

        public override StringBuilder GenerateCodes()
        {
            if (Table.ColumnList == null || Table.ColumnList.Count == 0) return null;
            var codes = new StringBuilder();
            var indent = 0;
            // using
            codes.AppendLine("using System.Collections.Generic;");
            codes.AppendLine("using System.Data.SqlClient;");
            codes.AppendLine("using System.Text;");
            codes.AppendLine("using KD.Service.Procurement.IDAL;");
            codes.AppendLine("using KD.Service.Procurement.Model;");

            // namespace
            codes.AppendLine($"namespace {Constants.KDCODE_NAMESPACE_PREFIX}{ModuleName}.DAL.SQLServe");
            codes.AppendLine("{");

            // class
            indent++;
            codes.AppendLine(GetIndentStr(indent) +
                             string.Format("public class {0}Manager : BaseManager<{0}>, I{0}Manager", EntityName));
            codes.AppendLine(GetIndentStr(indent) + "{");

            // methods
            // get by primary
            indent++;
            var pkColumns = Table.ColumnList.Where(t => t.PrimaryKey).ToList();
            if (pkColumns.Any())
            {
                codes.Append(Environment.NewLine);
                codes.Append(GetIndentStr(indent) + string.Format("{0} Get{0}(", EntityName));
                var tmpList = pkColumns.Select(pk => $"{GetCSharpType(pk.DbType)} {pk.Name}");
                codes.Append(string.Join(", ", tmpList));
                codes.Append(")\r\n");
                codes.AppendLine(GetIndentStr(indent) + "{");

                // method body
                indent++;
                codes.Append(GetIndentStr(indent) + "var sql = string.Format(\"SELECT TOP 1 * FROM {0} WHERE ");
                //codes.Append("Marks = 1 and ID=@ID");
                var whereStr = new List<string>();
                if (Table.ColumnList.Exists(t => t.Name.ToLower() == "Marks"))
                {
                    whereStr.Add("Marks=1");
                }
                foreach (var pk in pkColumns)
                {
                    whereStr.Add(string.Format("{0} = @{0}", pk.Name));
                }
                codes.Append(string.Join(" AND ", whereStr));
                codes.Append("\", TableName);\r\n");

                var paramsList = pkColumns.Select(t => string.Format("new SqlParameter(\"{0}\", {0});", t.Name));
                codes.AppendLine(GetIndentStr(indent) + $"return GetEntity(sql, {string.Join(", ", paramsList)});");

                indent--;
                codes.AppendLine(GetIndentStr(indent) + "}");
            }

            if (pkColumns.Count < 2)
            {
                codes.AppendLine(Environment.NewLine);
                codes.AppendLine(GetIndentStr(indent) + string.Format("public List<{0}> Get{0}s(out long total, int page, int size, string sort, bool asc)", EntityName));
                codes.AppendLine(GetIndentStr(indent) + "{");
                indent++;
                codes.AppendLine(GetIndentStr(indent) +
                                 "var sql = new StringBuilder(string.Format(\"SELECT * FROM {0} WHERE Marks = 1\", TableName));");
                codes.AppendLine(GetIndentStr(indent) + "if (string.IsNullOrEmpty(sort))");
                codes.AppendLine(GetIndentStr(indent) + "{");
                codes.AppendLine(GetIndentStr(indent + 1) + "sort = \"AddTime\";");
                codes.AppendLine(GetIndentStr(indent) + "}");

                codes.AppendLine(GetIndentStr(indent) + "return GetEntities(page, size, out total, sql.ToString(), sort, asc);");
                indent--;
                codes.AppendLine(GetIndentStr(indent) + "}");
            }

            indent--;
            codes.AppendLine(GetIndentStr(indent) + "}"); // class 
            codes.AppendLine("}"); // namespace

            return codes;
        }
    }
}