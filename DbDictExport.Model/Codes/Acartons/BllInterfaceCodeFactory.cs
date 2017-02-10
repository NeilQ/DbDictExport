using System;
using System.Linq;
using System.Text;
using DbDictExport.Core.Common;
using DbDictExport.Core.Dal;

namespace DbDictExport.Core.Codes.Acartons
{
    public class BllInterfaceCodeFactory : AbstractCodeFactory
    {
        public BllInterfaceCodeFactory(string entityName, string moduleName, Table dbTable)
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

            // namespace
            codes.Append(Environment.NewLine);
            codes.AppendLine($"namespace {Constants.ACARTONS_NAMESAPCE_PREFIX}.Bll.Interface");
            codes.AppendLine("{"); // namespace

            // class
            indent++;
            codes.AppendLine(GetIndentStr(indent) + $"public interface I{EntityName}Service");
            codes.AppendLine(GetIndentStr(indent) + "{"); // class

            var pkColumns = Table.Columns.Where(t => t.IsPK).ToList();
            // methods
            indent++;
            if (pkColumns.Count >= 2)
            {
                for (int i = 0; i < pkColumns.Count; i++)
                {
                    // get by each primary key
                    codes.Append(GetIndentStr(indent) + $"List<{EntityName}> GetBy{pkColumns[i].PropertyName}(");
                    codes.Append(pkColumns[i].DbType + " ");
                    codes.Append(Extentions.ToRequiredFormatString(pkColumns[i].Name, Models.NamingRule.Camel));
                    codes.Append(");");
                    codes.AppendLine(Environment.NewLine);
                }
            }

            if (pkColumns.Count == 1)
            {
                // get by page
                codes.AppendLine(GetIndentStr(indent) +
                                 $"List<{EntityName}> GetByPage(out int total, int page, int size);");
                codes.Append(Environment.NewLine);

                codes.AppendLine(GetIndentStr(indent) +
                                 $"List<{EntityName}> GetByPage(out int total, int page, int size, string sort);");
                codes.Append(Environment.NewLine);

                codes.AppendLine(GetIndentStr(indent) +
                                 $"List<{EntityName}> GetByPage(out int total, int page, int size, string sort, object condition);");
                codes.Append(Environment.NewLine);
            }

            // get all
            codes.AppendLine(GetIndentStr(indent) + $"List<{EntityName}> GetAll();");
            codes.Append(Environment.NewLine);

            codes.AppendLine(GetIndentStr(indent) + $"List<{EntityName}> GetAll(object condition);");
            codes.Append(Environment.NewLine);

            codes.AppendLine(GetIndentStr(indent) + $"List<{EntityName}> GetAll(string sort, object condition);");
            codes.Append(Environment.NewLine);

            var tmpList = pkColumns.Select(pk => $"{pk.PropertyType} {ToCamelCase(pk.PropertyName)}").ToList();
            if (pkColumns.Any())
            {
                // get by primary key
                codes.Append(GetIndentStr(indent) + $"{EntityName} GetByPK(");

                codes.Append(string.Join(", ", tmpList));
                codes.Append(");");
                codes.AppendLine(Environment.NewLine);
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
                //codes.Append(Environment.NewLine);
                codes.Append(GetIndentStr(indent) + "bool Delete(");
                codes.Append(string.Join(", ", tmpList));
                codes.Append(");");
                codes.Append(Environment.NewLine);
            }


            // bulk delete
            if (pkColumns.Count == 1)
            {
                codes.AppendLine(GetIndentStr(indent) + "bool Delete(IEnumerable<int> idList);");
                codes.Append(Environment.NewLine);
            }

            // exists
            if (pkColumns.Count == 1)
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