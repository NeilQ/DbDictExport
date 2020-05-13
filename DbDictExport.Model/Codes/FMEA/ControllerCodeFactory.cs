using System;
using System.Linq;
using System.Text;
using DbDictExport.Core.Common;
using DbDictExport.Core.Dal;

namespace DbDictExport.Core.Codes.FMEA
{
    public class ControllerCodeFactory : AbstractCodeFactory
    {
        private readonly string _camelEntityName;
        private readonly string _apiRouteName;
        public ControllerCodeFactory(string entityName, string moduleName, Table dbTable)
        {
            _camelEntityName = Extentions.ToRequiredFormatString(entityName, Models.NamingRule.Camel);
            _apiRouteName = Inflector.AddUnderscores(Inflector.MakePlural(entityName)).ToLower();
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
            var pkColumns = Table.Columns.Where(p => p.IsPK).ToList();

            var codes = new StringBuilder();
            var indent = 0;
            // using
            codes.AppendLine("using System.Collections.Generic;");
            codes.AppendLine("using Microsoft.AspNetCore.Mvc;");
            codes.AppendLine("using FMEA.Core.WebApi.Controllers;");
            codes.AppendLine($"using {ModuleName}.{ Inflector.MakePlural(EntityName)};");

            // namespace
            codes.Append(Environment.NewLine);
            codes.AppendLine($"namespace FMEA.Api.Controllers");
            codes.AppendLine("{");

            // class
            indent++;
            codes.AppendLine(GetIndentStr(indent) + $"[Route(\"api/{_apiRouteName}\")]");
            codes.AppendLine(GetIndentStr(indent) + "[ApiExplorerSettings(GroupName = \"\")]");
            //codes.AppendLine(GetIndentStr(indent) + "[Authorize]");
            codes.AppendLine(GetIndentStr(indent) + $"public class {Inflector.MakePlural(EntityName)}Controller");
            indent++;
            codes.AppendLine(GetIndentStr(indent) + $": CrudControllerBase<{EntityName}Dto, int, {EntityName}Query, {EntityName}SaveInput>");
            indent--;
            codes.AppendLine(GetIndentStr(indent) + "{");


            //constructor
            indent++;
            codes.Append(Environment.NewLine);
            codes.AppendLine(GetIndentStr(indent) + $"public {Inflector.MakePlural(EntityName)}Controller(I{EntityName}Service service) :");
            indent++;
            codes.AppendLine(GetIndentStr(indent) + "base(service)");
            indent--;
            codes.AppendLine(GetIndentStr(indent) + "{");
            codes.AppendLine(GetIndentStr(indent) + "}");

            codes.Append(Environment.NewLine);
            indent--;
            codes.AppendLine(GetIndentStr(indent) + "}"); // class

            codes.AppendLine("}"); // namespace
            return codes;
        }
    }
}
