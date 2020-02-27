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
            codes.AppendLine("using Acartons.Core.Repositories;");
            codes.AppendLine(Environment.NewLine);
            codes.AppendLine($"namespace {ModuleName}");
            codes.AppendLine("{"); // namespace

            indent++;
            // class
            codes.AppendLine(string.Format("{1}public interface I{0}Repo : INPocoRepository<{0}>", EntityName,
                GetIndentStr(indent)));
            codes.AppendLine(GetIndentStr(indent) + "{"); //class
            codes.AppendLine(GetIndentStr(indent) + "}"); // class 

            codes.AppendLine("}"); // namespace
            return codes;
        }


    }
}