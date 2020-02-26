using System;
using System.Text;
using DbDictExport.Core.Common;
using DbDictExport.Core.Dal;

namespace DbDictExport.Core.Codes.Acartons
{
    public class DalCodeFactory : AbstractCodeFactory
    {
        public DalCodeFactory(string entityName, string moduleName, Table dbTable)
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
            var existMarks = Table.Columns.Exists(t => t.Name.ToLower() == "marked_for_delete");
            var codes = new StringBuilder();
            var indent = 0;
            // using
            codes.AppendLine("using Acartons.Core.Repositories;");
            codes.AppendLine("using Acartons.Core.Sessions;");

            // namespace
            codes.Append(Environment.NewLine);
            codes.AppendLine($"namespace {ModuleName}");
            codes.AppendLine("{");

            // class
            indent++;
            codes.AppendLine(GetIndentStr(indent) +
                             string.Format("public class {0}Repo : NPocoRepository<{0}>, I{0}Repo", EntityName));
            codes.AppendLine(GetIndentStr(indent) + "{");

            indent++;
            // constructor
            codes.Append(Environment.NewLine);
            codes.AppendLine(GetIndentStr(indent) +
                             $"public {EntityName}Repo(IUserContext userContext) : base(userContext) {{ }}");

            indent--;
            codes.AppendLine(GetIndentStr(indent) + "}"); // class 
            codes.AppendLine("}"); // namespace

            return codes;
        }

    }
}