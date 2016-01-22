using System;
using System.Linq;
using System.Text;
using DbDictExport.Core.Common;

namespace DbDictExport.Core.Codes
{
    public class DalInterfaceKdCodeFactory : AbstractKdCodeFactory
    {
        public DalInterfaceKdCodeFactory(string entityName, string moduleName, DbTable dbTable)
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
            codes.AppendLine("using JS.Service.Common.Utility;");
            codes.AppendLine($"using {Constants.KDCODE_NAMESPACE_PREFIX}{ModuleName}.Model;");
            codes.AppendLine(Environment.NewLine);
            codes.AppendLine($"namespace {Constants.KDCODE_NAMESPACE_PREFIX}{ModuleName}.IDAL");
            codes.AppendLine("{"); // namesapce

            indent++;
            // class
            codes.AppendLine(string.Format("{1}public interface I{0}Manager : IBaseManager<{0}>", EntityName,
                GetIndentStr(indent)));
            codes.AppendLine(GetIndentStr(indent) + "{"); //class

            indent++;
            // methods
            // get by primary
            var pkColumns = Table.ColumnList.Where(t => t.PrimaryKey);
            if (pkColumns.Any())
            {
                codes.Append(Environment.NewLine);
                codes.Append(GetIndentStr(indent) + string.Format("{0} Get{0}(", EntityName));
                //codes.Append("int ticketId, int costId");
                var tmpList = pkColumns.Select(pk => $"{MapCSharpType(pk.DbType)} {ToCamelCase(pk.Name)}");
                codes.Append(string.Join(", ", tmpList));
                codes.Append(");");
                codes.Append(Environment.NewLine);
                codes.Append(Environment.NewLine);
            }

            // get by page
            if (pkColumns.Count() < 2)
            {
                codes.AppendLine(
                string.Format(GetIndentStr(indent) + "List<{0}> Get{0}s(PageFilter filter);",
                    EntityName));
            }

            if (pkColumns.Count() < 2)
            {
                codes.Append(Environment.NewLine);
                codes.AppendLine(
                string.Format(GetIndentStr(indent) + "List<{0}> Get{0}s(out int total, int page, int size, string sort, bool asc, PageFilter filter);",
                    EntityName));
            }

            indent--;
            codes.AppendLine(GetIndentStr(indent) + "}"); // class 

            codes.AppendLine("}"); // namespace
            return codes;
        }


    }
}