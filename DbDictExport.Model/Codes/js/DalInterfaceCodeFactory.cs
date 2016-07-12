using System;
using System.Linq;
using System.Text;
using DbDictExport.Core.Common;
using DbDictExport.Core.Dal;

namespace DbDictExport.Core.Codes.js
{
    public class DalInterfaceCodeFactory : AbstractCodeFactory
    {
        public DalInterfaceCodeFactory(string entityName, string moduleName, Table dbTable)
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
            if (Table.Columns == null) return null;

            var codes = new StringBuilder();
            var indent = 0;
            // using
            codes.AppendLine("using System.Collections.Generic;");
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
            var pkColumns = Table.Columns.Where(t => t.IsPK);
            if (pkColumns.Any())
            {
                codes.Append(Environment.NewLine);
                codes.Append(GetIndentStr(indent) + $"{EntityName} GetByPK(");
                //codes.Append("int ticketId, int costId");
                var tmpList = pkColumns.Select(pk => $"{pk.PropertyType} {ToCamelCase(pk.Name)}");
                codes.Append(string.Join(", ", tmpList));
                codes.Append(");");
                codes.Append(Environment.NewLine);
                codes.Append(Environment.NewLine);
            }

            // get by page
            if (pkColumns.Count() < 2)
            {
                codes.AppendLine(
                string.Format(GetIndentStr(indent) + "List<{0}> GetByPage(out int total, int page, int size, string sort, bool asc, object condition);",
                    EntityName));
            }


            indent--;
            codes.AppendLine(GetIndentStr(indent) + "}"); // class 

            codes.AppendLine("}"); // namespace
            return codes;
        }


    }
}