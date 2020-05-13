using System;
using System.Linq;
using System.Text;
using DbDictExport.Core.Common;
using DbDictExport.Core.Dal;

namespace DbDictExport.Core.Codes.FMEA
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
            codes.AppendLine("using System.Collections.Generic;");
            codes.AppendLine("using System.ComponentModel.DataAnnotations;");
            codes.AppendLine("using FMEA.Core.Services.Dto;");
            codes.Append(Environment.NewLine);

            // namespace
            codes.AppendLine($"namespace {ModuleName}.{ Inflector.MakePlural(EntityName)}");
            codes.AppendLine("{"); // namespace

            indent++;
            var pkColumns = Table.Columns.Where(t => t.IsPK).ToList();

            // dto
            codes.Append(Environment.NewLine);
            codes.AppendLine(GetIndentStr(indent) + $"public class {EntityName}SaveInput: EntityDto");
            codes.AppendLine(GetIndentStr(indent) + "{");
            // fields
            indent++;
            foreach (var column in Table.Columns)
            {
                if (column.Name == "Id"
                    || column.Name == "UpdateTime"
                    || column.Name == "UpdateUserId"
                    || column.Name == "CreateTime"
                    || column.Name == "CreateUserId"
                    || column.Name == "IsDeleted"
                    || column.Name == "DeleteUserId"
                    || column.Name == "DeleteTime")
                {
                    continue;
                }

                // annotation
                if (!string.IsNullOrEmpty(column.Description))
                {
                    codes.AppendLine(GetIndentStr(indent) + "/// <summary>");
                    codes.AppendLine(GetIndentStr(indent) + $"/// {column.Description}");
                    codes.AppendLine(GetIndentStr(indent) + "/// </summary>");
                }

                // field
                codes.AppendLine(GetIndentStr(indent) +
                                 $"public {column.PropertyType} {column.PropertyName} {{ get; set; }}");
                codes.Append(Environment.NewLine);
            }

            indent--;
            codes.AppendLine(GetIndentStr(indent) + "}");

            codes.Append(Environment.NewLine);
            codes.AppendLine(GetIndentStr(indent) + $"public class {EntityName}Dto: {EntityName}SaveInput");
            codes.AppendLine(GetIndentStr(indent) + "{");
            codes.AppendLine(GetIndentStr(indent) + "}");

            codes.Append(Environment.NewLine);
            codes.AppendLine(GetIndentStr(indent) + $"public class {EntityName}Query: PageRequest");
            codes.AppendLine(GetIndentStr(indent) + "{");
            codes.AppendLine(GetIndentStr(indent) + "}");


            codes.AppendLine("}"); // namespace


            return codes;
        }
    }
}