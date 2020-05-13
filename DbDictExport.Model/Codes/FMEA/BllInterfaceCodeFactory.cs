using System;
using System.Text;
using DbDictExport.Core.Common;
using DbDictExport.Core.Dal;

namespace DbDictExport.Core.Codes.FMEA
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
            codes.AppendLine("using FMEA.Core.Services;");
            codes.AppendLine("using FMEA.Core.Services.Dto;");

            // namespace
            codes.Append(Environment.NewLine);
            codes.AppendLine($"namespace {ModuleName}.{ Inflector.MakePlural(EntityName)}");
            codes.AppendLine("{"); // namespace

            // class
            indent++;
            codes.AppendLine(GetIndentStr(indent) + $"public interface I{EntityName}Service: ICrudService<{EntityName}Dto, int, {EntityName}Query, {EntityName}SaveInput>");
            codes.AppendLine(GetIndentStr(indent) + "{"); // class
            codes.AppendLine(GetIndentStr(indent) + "}"); // class

            codes.AppendLine("}"); // namespace

            return codes;
        }
    }
}