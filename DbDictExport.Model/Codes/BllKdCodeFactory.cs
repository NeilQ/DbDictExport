using System;
using System.Linq;
using System.Text;
using DbDictExport.Core.Common;

namespace DbDictExport.Core.Codes
{
    public class BllKdCodeFactory : AbstractKdCodeFactory
    {
        public BllKdCodeFactory(string entityName, string moduleName, DbTable dbTable)
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
            codes.AppendLine("using System.Collections.Generic;");
            codes.AppendLine("using System.Linq;");
            codes.AppendLine($"using {Constants.KDCODE_NAMESPACE_PREFIX}{ModuleName}.Model;");
            codes.AppendLine($"using {Constants.KDCODE_NAMESPACE_PREFIX}{ModuleName}.IBLL;");
            codes.AppendLine($"using {Constants.KDCODE_NAMESPACE_PREFIX}{ModuleName}.IDAL;");

            // namespace
            codes.Append(Environment.NewLine);
            codes.AppendLine($"namespace {Constants.KDCODE_NAMESPACE_PREFIX}{ModuleName}.BLL");
            codes.AppendLine("{"); // namespace

            // class
            indent++;
            codes.AppendLine(GetIndentStr(indent) + $"public class {EntityName}Service : I{EntityName}Service");
            codes.AppendLine(GetIndentStr(indent) + "{"); // class

            indent++;
            codes.AppendLine(GetIndentStr(indent) + "private readonly IUnitOfWork _unitOfWork;");
            codes.Append(Environment.NewLine);
            codes.AppendLine(GetIndentStr(indent) + $"public {EntityName}Service(IUnitOfWork unitOfWork)");
            codes.AppendLine(GetIndentStr(indent) + "{");
            codes.AppendLine(GetIndentStr(indent + 1) + "_unitOfWork = unitOfWork;");
            codes.AppendLine(GetIndentStr(indent) + "}");
            codes.Append(Environment.NewLine);

            var pkColumns = Table.ColumnList.Where(t => t.PrimaryKey).ToList();
            // methods
            if (pkColumns.Count == 1)
            {
                // get by page
                codes.AppendLine(GetIndentStr(indent) +
                                 $"public List<{EntityName}> Get{EntityName}s(out int total, int page, int size, string sort, bool asc)");
                codes.AppendLine(GetIndentStr(indent) + "{");
                codes.AppendLine(GetIndentStr(indent + 1) +
                    $"return _unitOfWork.{EntityName}Manager.Get{EntityName}s(out total, page, size, sort, asc);");
                codes.AppendLine(GetIndentStr(indent) + "}");
                codes.Append(Environment.NewLine);
            }

            var tmpList = pkColumns.Select(pk => $"{MapCSharpType(pk.DbType)} {ToCamelCase(pk.Name)}").ToList();
            var paramList = pkColumns.Select(pk => $"{ToCamelCase(pk.Name)}").ToList();
            if (pkColumns.Any())
            {
                // get by primary key

                codes.Append(GetIndentStr(indent) + $"public {EntityName} Get{EntityName}(");
                codes.Append(string.Join(", ", tmpList));
                codes.Append(")");
                codes.Append(Environment.NewLine);
                codes.AppendLine(GetIndentStr(indent) + "{");
                codes.Append(GetIndentStr(indent + 1) +
                             $"return _unitOfWork.{EntityName}Manager.Get{EntityName}(");
                codes.Append(string.Join(", ", paramList));
                codes.Append(");");
                codes.Append(Environment.NewLine);
                codes.AppendLine(GetIndentStr(indent) + "}");
                codes.Append(Environment.NewLine);
            }


            // add
            codes.AppendLine(GetIndentStr(indent) + $"public int Add({EntityName} entity)");
            codes.AppendLine(GetIndentStr(indent) + "{");
            codes.AppendLine(GetIndentStr(indent + 1) +
                             $"return _unitOfWork.{EntityName}Manager.Insert(entity);");
            codes.AppendLine(GetIndentStr(indent) + "}");
            codes.Append(Environment.NewLine);

            // update
            codes.AppendLine(GetIndentStr(indent) + $"public bool Update({EntityName} entity)");
            codes.AppendLine(GetIndentStr(indent) + "{");
            codes.AppendLine(GetIndentStr(indent + 1) +
                             $"return _unitOfWork.{EntityName}Manager.Update(entity);");
            codes.AppendLine(GetIndentStr(indent) + "}");
            codes.Append(Environment.NewLine);

            // delete
            if (pkColumns.Any())
            {
                codes.Append(GetIndentStr(indent) + "public bool Delete(");
                codes.Append(string.Join(", ", tmpList));
                codes.Append(")");
                codes.Append(Environment.NewLine);
                codes.AppendLine(GetIndentStr(indent) + "{");
                codes.Append(GetIndentStr(indent + 1) +
                             $"return _unitOfWork.{EntityName}Manager.MarkDeletion(");
                codes.Append(string.Join(", ", paramList));
                codes.Append(");");
                codes.Append(Environment.NewLine);
                codes.AppendLine(GetIndentStr(indent) + "}");
                codes.Append(Environment.NewLine);
            }


            // bulk delete
            if (pkColumns.Count == 1)
            {
                codes.AppendLine(GetIndentStr(indent) + "public bool Delete(IEnumerable<int> idList)");
                codes.AppendLine(GetIndentStr(indent) + "{");
                codes.AppendLine(GetIndentStr(indent + 1) +
                                 $"return _unitOfWork.{EntityName}Manager.MarkDeletions(idList.Select<int, object>(t => t));");
                codes.AppendLine(GetIndentStr(indent) + "}");
                codes.Append(Environment.NewLine);
            }

            // exists
            if (pkColumns.Count == 1)
            {
                codes.AppendLine(GetIndentStr(indent) + $"public bool Exists({MapCSharpType(pkColumns[0].DbType)} {pkColumns[0].Name})");
                codes.AppendLine(GetIndentStr(indent) + "{");
                codes.AppendLine(GetIndentStr(indent + 1) +
                    $"return _unitOfWork.{EntityName}Manager.Exists({ToCamelCase(pkColumns[0].Name)});");
                codes.AppendLine(GetIndentStr(indent) + "}");
            }

            indent--;
            codes.AppendLine(GetIndentStr(indent) + "}"); // class

            codes.AppendLine("}"); // namespace

            return codes;
        }
    }
}