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
            codes.AppendLine("using System;");
            codes.AppendLine("using NPoco;");
            codes.AppendLine("using Acartons.Core.Entities;");
            codes.Append(Environment.NewLine);

            // namespace
            codes.AppendLine($"namespace {ModuleName}");
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
            bool hasDeleteFields = Table.Columns.Exists(t => t.Name == "marked_for_delete")
                                   && Table.Columns.Exists(t => t.Name == "delete_user")
                                   && Table.Columns.Exists(t => t.Name == "delete_time");
            bool hasCreateFields = Table.Columns.Exists(t => t.Name == "add_user")
                                   && Table.Columns.Exists(t => t.Name == "add_time");
            if (hasBaseFields && hasDeleteFields)
            {
                codes.AppendLine(GetIndentStr(indent) + $"public class {EntityName} : FullAuditedEntityBase");
            }
            else if (hasBaseFields)
            {
                codes.AppendLine(GetIndentStr(indent) + $"public class {EntityName} : AuditedEntityBase");
            }
            else if (hasCreateFields)
            {
                codes.AppendLine(GetIndentStr(indent) + $"public class {EntityName} : CreateAuditedEntityBase");
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
                if (hasCreateFields && (column.Name == "add_time" || column.Name == "add_user"))
                {
                    continue;
                }

                if (hasBaseFields &&
                    (column.Name == "update_time"
                    || column.Name == "update_user"
                    || column.Name == "add_time"
                    || column.Name == "add_user"))
                {
                    continue;
                }

                if (hasDeleteFields &&
                    (column.Name == "marked_for_delete"
                     || column.Name == "delete_user"
                     || column.Name == "delete_time"))
                {
                    continue;
                }
                // annotation
                codes.AppendLine(GetIndentStr(indent) + "/// <summary>");
                codes.AppendLine(GetIndentStr(indent) + $"/// {column.Description}");
                codes.AppendLine(GetIndentStr(indent) + "/// </summary>");

                // field
                codes.AppendLine(GetIndentStr(indent) +
                                 $"public {column.PropertyType} {column.PropertyName} {{ get; set; }}");
                codes.Append(Environment.NewLine);
            }

            indent--;
            codes.AppendLine(GetIndentStr(indent) + "}");  // entity class end


            // dto
            codes.Append(Environment.NewLine);
            codes.AppendLine(GetIndentStr(indent) + $"public class {EntityName}Save");
            codes.AppendLine(GetIndentStr(indent) + "{");
            // fields
            indent++;
            foreach (var column in Table.Columns)
            {
                if (column.Name == "update_time"
                     || column.Name == "update_user"
                     || column.Name == "add_time"
                     || column.Name == "add_user"
                     || column.Name == "marked_for_delete"
                    || column.Name == "delete_user"
                    || column.Name == "delete_time")
                {
                    continue;
                }

                // annotation
                codes.AppendLine(GetIndentStr(indent) + "/// <summary>");
                codes.AppendLine(GetIndentStr(indent) + $"/// {column.Description}");
                codes.AppendLine(GetIndentStr(indent) + "/// </summary>");

                // field
                codes.AppendLine(GetIndentStr(indent) +
                                 $"public {column.PropertyType} {column.PropertyName} {{ get; set; }}");
                codes.Append(Environment.NewLine);
            }

            indent--;
            codes.AppendLine(GetIndentStr(indent) + "}");

            codes.Append(Environment.NewLine);
            codes.AppendLine(GetIndentStr(indent) + $"public class {EntityName}Dto: {EntityName}Save");
            codes.AppendLine(GetIndentStr(indent) + "{");
            codes.AppendLine(GetIndentStr(indent) + "}");


            codes.AppendLine("}"); // namespace


            return codes;
        }
    }
}