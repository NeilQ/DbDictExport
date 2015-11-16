using System.Linq;
using System.Text;
using DbDictExport.Core.Common;

namespace DbDictExport.Core.Codes
{
    public class ModelKdCodeFactory : AbstractKdCodeFactory
    {
        public ModelKdCodeFactory(string entityName, string moduleName, DbTable dbTable)
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
            if (Table.ColumnList == null || Table.ColumnList.Count == 0) return null;
            var codes = new StringBuilder();
            var indent = 0;

            // using 

            // namespace
            codes.AppendLine($"namespace {Constants.KDCODE_NAMESPACE_PREFIX}{ModuleName}.Model");
            codes.AppendLine("{"); // namespace

            indent++;
            var pkColumns = Table.ColumnList.Where(t => t.PrimaryKey).ToList();
            //attributes
            codes.AppendLine(GetIndentStr(indent) + $"[TableName(\"{EntityName}\")]");
            if (pkColumns.Count == 1)
            {
                codes.AppendLine(GetIndentStr(indent) + $"[PrimaryKey(\"{pkColumns[0].Name}\")]");
            }

            // class
            bool hasBaseFields = Table.ColumnList.Exists(t => t.Name == "AddUser")
                && Table.ColumnList.Exists(t => t.Name == "AddTime")
                && Table.ColumnList.Exists(t => t.Name == "UpdateUser")
                && Table.ColumnList.Exists(t => t.Name == "UpdateTime")
                && Table.ColumnList.Exists(t => t.Name == "Marks");
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
            foreach (var column in Table.ColumnList)
            {
                // annatation
                codes.AppendLine(GetIndentStr(indent) + "/// <summary>");
                codes.AppendLine(GetIndentStr(indent) + $"/// {column.Description}");
                codes.AppendLine(GetIndentStr(indent) + "/// </summary>");

                // field
                codes.AppendLine(GetIndentStr(indent) +
                                 $"public {GetCSharpType(column.DbType)} {column.Name} {{ get; set; }}");
            }

            indent--;
            codes.AppendLine(GetIndentStr(indent) + "}");

            codes.AppendLine("}"); // namespace

            return codes;
        }
    }
}