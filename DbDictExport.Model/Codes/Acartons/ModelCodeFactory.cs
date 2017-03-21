using System;
using System.Linq;
using System.Text;
using DbDictExport.Core.Common;
using DbDictExport.Core.Dal;

namespace DbDictExport.Core.Codes.Acartons
{
    public class ModelCodeFactory : AbstractCodeFactory
    {
        public ModelCodeFactory(string entityName, string moduleName, Table dbTable)
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
            codes.AppendLine("using PetaPoco;");
            codes.AppendLine("using System;");
            codes.Append(Environment.NewLine);

            // namespace
            codes.AppendLine($"namespace {Constants.ACARTONS_NAMESAPCE_PREFIX}.Model");
            codes.AppendLine("{"); // namespace

            indent++;
            var pkColumns = Table.Columns.Where(t => t.IsPK).ToList();
            //attributes
            codes.AppendLine(GetIndentStr(indent) + $"[TableName(\"{Table.Name}\")]");
            if (pkColumns.Count == 1)
            {
                codes.AppendLine(GetIndentStr(indent) + $"[PrimaryKey(\"{pkColumns[0].Name}\")]");
            }

            // class
            bool hasBaseFields = Table.Columns.Exists(t => t.Name == "add_user")
                && Table.Columns.Exists(t => t.Name == "add_time")
                && Table.Columns.Exists(t => t.Name == "update_user")
                && Table.Columns.Exists(t => t.Name == "update_time");
            if (hasBaseFields)
            {
                codes.AppendLine(GetIndentStr(indent) + $"public class {EntityName} : BaseField");
            }
            else
            {
                codes.AppendLine(GetIndentStr(indent) + $"public class {EntityName}");
            }
            codes.AppendLine(GetIndentStr(indent) + "{");  // class

            // fields
            indent++;
            foreach (var column in Table.Columns)
            {
                if (hasBaseFields &&
                    (column.Name == "update_time"
                    || column.Name == "update_user"
                    || column.Name == "add_time"
                    || column.Name == "add_user"))
                {
                    continue;
                }
                // annatation
                codes.AppendLine(GetIndentStr(indent) + "/// <summary>");
                codes.AppendLine(GetIndentStr(indent) + $"/// {column.Description}");
                codes.AppendLine(GetIndentStr(indent) + "/// </summary>");

                // field
                codes.AppendLine(GetIndentStr(indent) +
                                 $"[Column(\"{column.Name}\")]");
                codes.AppendLine(GetIndentStr(indent) +
                                 $"public {column.PropertyType} {column.PropertyName} {{ get; set; }}");
                codes.Append(Environment.NewLine);
            }

            indent--;
            codes.AppendLine(GetIndentStr(indent) + "}");

            codes.AppendLine("}"); // namespace

            return codes;
        }
    }
}