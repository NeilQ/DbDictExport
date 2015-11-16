using System;
using System.Linq;
using System.Text;
using DbDictExport.Core.Common;

namespace DbDictExport.Core.Codes
{
    public class BllInterfaceKdCodeFactory : AbstractKdCodeFactory
    {
        public BllInterfaceKdCodeFactory(string entityName, string moduleName, DbTable dbTable)
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
            codes.AppendLine($"using {Constants.KDCODE_NAMESPACE_PREFIX}{ModuleName}.Model");

            // namespace
            codes.AppendLine($"namespace {Constants.KDCODE_NAMESPACE_PREFIX}{ModuleName}.IBLL");
            codes.AppendLine("{"); // namespace

            // class
            indent++;
            codes.AppendLine(GetIndentStr(indent) + $"public interface I{EntityName}Service");
            codes.AppendLine(GetIndentStr(indent) + "{"); // class

            var pkColumns = Table.ColumnList.Where(t => t.PrimaryKey).ToList();
            // methods
            indent++;
            if (pkColumns.Count == 1)
            {
                // get by page
                codes.AppendLine(GetIndentStr(indent) +
                                 $"List<{EntityName}> Get{EntityName}s(out long total, int page, int size, string sort, bool asc);");
            }

            var tmpList = pkColumns.Select(pk => $"{GetCSharpType(pk.DbType)} {pk.Name}").ToList();
            if (pkColumns.Any())
            {
                // get by primary key
                codes.Append(Environment.NewLine);
                codes.Append(GetIndentStr(indent) + $"{EntityName} Get{EntityName}(");

                codes.Append(string.Join(", ", tmpList));
                codes.Append(");");
                codes.Append(Environment.NewLine);
                codes.Append(Environment.NewLine);
            }


            // add
            codes.AppendLine(GetIndentStr(indent) + $"int Add({EntityName} entity);");
            codes.Append(Environment.NewLine);

            // update
            codes.AppendLine(GetIndentStr(indent) + $"bool Update({EntityName} entity);");
            codes.Append(Environment.NewLine);

            // delete
            if (pkColumns.Any())
            {
                codes.Append(Environment.NewLine);
                codes.Append(GetIndentStr(indent) + "bool Delete(");
                codes.Append(string.Join(", ", tmpList));
                codes.Append(");");
                codes.Append(Environment.NewLine);
                codes.Append(Environment.NewLine);
            }


            // bulk delete
            if (pkColumns.Count == 1)
            {
                codes.AppendLine(GetIndentStr(indent) + "bool Delete(IEnumerable<int> idList);");
                codes.Append(Environment.NewLine);
            }

            // exists
            if (pkColumns.Count == 0)
            {
                codes.AppendLine(GetIndentStr(indent) + "bool Exists(int id);");
            }

            indent--;
            codes.AppendLine(GetIndentStr(indent) + "}"); // class

            codes.AppendLine("}"); // namespace

            return codes;
        }
    }
}