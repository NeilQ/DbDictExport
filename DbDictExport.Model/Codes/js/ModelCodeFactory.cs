﻿using System;
using System.Linq;
using System.Text;
using DbDictExport.Core.Common;
using DbDictExport.Core.Dal;

namespace DbDictExport.Core.Codes.js
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

            // namespace
            codes.AppendLine($"namespace {Constants.KDCODE_NAMESPACE_PREFIX}{ModuleName}.Model");
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
            bool hasBaseFields = Table.Columns.Exists(t => t.Name == "AddUser")
                && Table.Columns.Exists(t => t.Name == "AddTime")
                && Table.Columns.Exists(t => t.Name == "UpdateUser")
                && Table.Columns.Exists(t => t.Name == "UpdateTime");
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
                    (column.Name == "AddTime"
                    || column.Name == "UpdateUser"
                    || column.Name == "UpdateTime"
                    || column.Name == "AddUser"))
                {
                    continue;
                }
                // annatation
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

            codes.AppendLine("}"); // namespace

            return codes;
        }
    }
}