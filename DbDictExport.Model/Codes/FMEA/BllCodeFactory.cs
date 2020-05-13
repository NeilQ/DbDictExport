using System;
using System.Text;
using DbDictExport.Core.Common;
using DbDictExport.Core.Dal;

namespace DbDictExport.Core.Codes.FMEA
{
    public class BllCodeFactory : AbstractCodeFactory
    {
        public BllCodeFactory(string entityName, string moduleName, Table dbTable)
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
            codes.AppendLine("using System.Linq;");
            codes.AppendLine("using FMEA.Core;");
            codes.AppendLine("using FMEA.Core.Repositories;");
            codes.AppendLine("using FMEA.Core.Services;");

            // namespace
            codes.Append(Environment.NewLine);
            codes.AppendLine($"namespace {ModuleName}.{ Inflector.MakePlural(EntityName)}");
            codes.AppendLine("{"); // namespace

            // class
            indent++;
            codes.AppendLine(GetIndentStr(indent) + $"public class {EntityName}Service: CrudService<{EntityName}, {EntityName}Dto, int, {EntityName}Query, {EntityName}SaveInput>, I{EntityName}Service");
            codes.AppendLine(GetIndentStr(indent) + "{"); // class

            indent++;
            codes.AppendLine(GetIndentStr(indent) + $"public {EntityName}Service(IRepository<{EntityName}> repository, IServiceAggregator serviceAggregator): ");
            indent++;
            codes.AppendLine(GetIndentStr(indent) + "base(repository, serviceAggregator)");
            indent--;
            codes.AppendLine(GetIndentStr(indent) + "{");
            codes.AppendLine(GetIndentStr(indent) + "}");
            codes.Append(Environment.NewLine);

            // methods
            codes.AppendLine(GetIndentStr(indent) +
                             $"protected override IQueryable<{EntityName}> CreateFilteredQuery({EntityName}Query input)");
            codes.AppendLine(GetIndentStr(indent) + "{");
            indent++;
            codes.AppendLine(GetIndentStr(indent) + "var query = base.CreateFilteredQuery(input);");
            codes.AppendLine(GetIndentStr(indent) + "// 附加查询条件");
            codes.AppendLine(Environment.NewLine);
            codes.AppendLine(GetIndentStr(indent) + "return query;");
            indent--;
            codes.AppendLine(GetIndentStr(indent) + "}");


            indent--;

            codes.AppendLine(GetIndentStr(indent) + "}"); // class

            codes.AppendLine("}"); // namespace

            return codes;
        }
    }
}