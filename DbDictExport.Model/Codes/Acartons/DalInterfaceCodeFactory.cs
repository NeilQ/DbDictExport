using System;
using System.Linq;
using System.Text;
using DbDictExport.Core.Common;
using DbDictExport.Core.Dal;

namespace DbDictExport.Core.Codes.Acartons
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
            codes.AppendLine($"using {Constants.ACARTONS_NAMESAPCE_PREFIX}.Model;");
            codes.AppendLine(Environment.NewLine);
            codes.AppendLine($"namespace {Constants.ACARTONS_NAMESAPCE_PREFIX}.Dal.Interface");
            codes.AppendLine("{"); // namesapce

            indent++;
            // class
            codes.AppendLine(string.Format("{1}public interface I{0}Repo : IRepoBase<{0}>", EntityName,
                GetIndentStr(indent)));
            codes.AppendLine(GetIndentStr(indent) + "{"); //class

            indent++;
            // methods
            // get by primary
            var pkColumns = Table.Columns.Where(t => t.IsPK).ToList();
            if (pkColumns.Count > 1)
            {
                codes.Append(GetIndentStr(indent) + $"{EntityName} GetByPk(");
                var tmpList = pkColumns.Select(pk => $"{pk.PropertyType} {ToCamelCase(pk.Name)}");
                codes.Append(string.Join(", ", tmpList));
                codes.Append(");");
                codes.AppendLine(Environment.NewLine);
            }


            if (pkColumns.Count >= 2)
            {
                for (int i = 0; i < pkColumns.Count; i++)
                {
                    // get by every primary key
                    codes.Append(GetIndentStr(indent) + $"List<{EntityName}> GetBy{pkColumns[i].PropertyName}(");
                    codes.Append(pkColumns[i].PropertyType + " ");
                    codes.Append(ToCamelCase(pkColumns[i].PropertyName));
                    codes.AppendLine(");");
                    codes.Append(Environment.NewLine);
                }
            }

            // get by page
            if (pkColumns.Count() < 2)
            {
                codes.AppendLine(
                string.Format(GetIndentStr(indent) + "List<{0}> GetByPage(out int total, int page, int size, string sort, object condition);",
                    EntityName));
                codes.Append(Environment.NewLine);
            }

            // get all
            codes.AppendLine(
               string.Format(GetIndentStr(indent) + "List<{0}> GetAll(string sort, object condition);",
                   EntityName));
            codes.Append(Environment.NewLine);


            indent--;
            codes.AppendLine(GetIndentStr(indent) + "}"); // class 

            codes.AppendLine("}"); // namespace
            return codes;
        }


    }
}