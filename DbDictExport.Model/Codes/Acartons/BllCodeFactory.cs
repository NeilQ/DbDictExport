using System;
using System.Linq;
using System.Text;
using DbDictExport.Core.Common;
using DbDictExport.Core.Dal;

namespace DbDictExport.Core.Codes.Acartons
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

            var existMarks = Table.Columns.Exists(t => t.Name.ToLower() == "marked_for_delete");

            // using 
            codes.AppendLine("using System.Collections.Generic;");
            codes.AppendLine("using System.Linq;");
            codes.AppendLine($"using {Constants.ACARTONS_NAMESAPCE_PREFIX}.Model;");
            codes.AppendLine($"using {Constants.ACARTONS_NAMESAPCE_PREFIX}.Bll.Interface;");
            codes.AppendLine($"using {Constants.ACARTONS_NAMESAPCE_PREFIX}.Dal.Interface;");

            // namespace
            codes.Append(Environment.NewLine);
            codes.AppendLine($"namespace {Constants.ACARTONS_NAMESAPCE_PREFIX}.Bll");
            codes.AppendLine("{"); // namespace

            // class
            indent++;
            codes.AppendLine(GetIndentStr(indent) + $"public class {EntityName}Service : I{EntityName}Service");
            codes.AppendLine(GetIndentStr(indent) + "{"); // class

            indent++;
            codes.AppendLine(GetIndentStr(indent) + "private readonly IRepoFactory _repoFactory;");
            codes.Append(Environment.NewLine);
            codes.AppendLine(GetIndentStr(indent) + $"public {EntityName}Service(IRepoFactory repoFactory)");
            codes.AppendLine(GetIndentStr(indent) + "{");
            codes.AppendLine(GetIndentStr(indent + 1) + "_repoFactory = repoFactory;");
            codes.AppendLine(GetIndentStr(indent) + "}");
            codes.Append(Environment.NewLine);

            var pkColumns = Table.Columns.Where(t => t.IsPK).ToList();


            if (pkColumns.Count >= 2)
            {
                for (int i = 0; i < pkColumns.Count; i++)
                {
                    // get by each primary key
                    codes.Append(GetIndentStr(indent) + $"public List<{EntityName}> GetBy{pkColumns[i].PropertyName}(");
                    codes.Append(pkColumns[i].DbType + " ");
                    codes.Append(ToCamelCase(pkColumns[i].PropertyName));
                    codes.AppendLine(")");
                    codes.AppendLine(GetIndentStr(indent) + "{");
                    codes.AppendLine(GetIndentStr(indent + 1) +
                                     $"return _repoFactory.{EntityName}Repo.GetBy{pkColumns[i].PropertyName}({ToCamelCase(pkColumns[i].PropertyName)});");
                    codes.AppendLine(GetIndentStr(indent) + "}");
                    codes.Append(Environment.NewLine);
                }
            }

            // methods
            if (pkColumns.Count == 1)
            {
                // get by page
                codes.AppendLine(GetIndentStr(indent) +
                                 $"public List<{EntityName}> GetByPage(out int total, int page, int size)");
                codes.AppendLine(GetIndentStr(indent) + "{");
                codes.AppendLine(GetIndentStr(indent + 1) +
                    $"return _repoFactory.{EntityName}Repo.GetByPage(out total, page, size, null, null);");
                codes.AppendLine(GetIndentStr(indent) + "}");
                codes.Append(Environment.NewLine);

                codes.AppendLine(GetIndentStr(indent) +
                                $"public List<{EntityName}> GetByPage(out int total, int page, int size, string sort)");
                codes.AppendLine(GetIndentStr(indent) + "{");
                codes.AppendLine(GetIndentStr(indent + 1) +
                    $"return _repoFactory.{EntityName}Repo.GetByPage(out total, page, size, sort, null);");
                codes.AppendLine(GetIndentStr(indent) + "}");
                codes.Append(Environment.NewLine);

                codes.AppendLine(GetIndentStr(indent) +
                                $"public List<{EntityName}> GetByPage(out int total, int page, int size, string sort, object condition)");
                codes.AppendLine(GetIndentStr(indent) + "{");
                codes.AppendLine(GetIndentStr(indent + 1) +
                    $"return _repoFactory.{EntityName}Repo.GetByPage(out total, page, size, sort, condition);");
                codes.AppendLine(GetIndentStr(indent) + "}");
                codes.Append(Environment.NewLine);
            }

            // get all
            codes.AppendLine(GetIndentStr(indent) + $"public List<{EntityName}> GetAll()");
            codes.AppendLine(GetIndentStr(indent) + "{");
            codes.AppendLine(GetIndentStr(indent + 1) + $"return _repoFactory.{EntityName}Repo.GetAll(null, null);");
            codes.AppendLine(GetIndentStr(indent) + "}");
            codes.Append(Environment.NewLine);

            codes.AppendLine(GetIndentStr(indent) + $"public List<{EntityName}> GetAll(object condition)");
            codes.AppendLine(GetIndentStr(indent) + "{");
            codes.AppendLine(GetIndentStr(indent + 1) + $"return _repoFactory.{EntityName}Repo.GetAll(null, condition);");
            codes.AppendLine(GetIndentStr(indent) + "}");
            codes.Append(Environment.NewLine);

            codes.AppendLine(GetIndentStr(indent) + $"public List<{EntityName}> GetAll(string sort, object condition)");
            codes.AppendLine(GetIndentStr(indent) + "{");
            codes.AppendLine(GetIndentStr(indent + 1) + $"return _repoFactory.{EntityName}Repo.GetAll(sort, condition);");
            codes.AppendLine(GetIndentStr(indent) + "}");
            codes.Append(Environment.NewLine);


            var tmpList = pkColumns.Select(pk => $"{pk.PropertyType} {ToCamelCase(pk.PropertyName)}").ToList();
            var paramList = pkColumns.Select(pk => $"{ToCamelCase(pk.PropertyName)}").ToList();
            if (pkColumns.Any())
            {
                // get by primary key

                codes.Append(GetIndentStr(indent) + $"public {EntityName} GetByPK(");
                codes.Append(string.Join(", ", tmpList));
                codes.Append(")");
                codes.Append(Environment.NewLine);
                codes.AppendLine(GetIndentStr(indent) + "{");
                codes.Append(GetIndentStr(indent + 1) +
                             $"return _repoFactory{EntityName}Repo.GetByPK(");
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
                             $"return _repoFactory.{EntityName}Repo.Insert(entity);");
            codes.AppendLine(GetIndentStr(indent) + "}");
            codes.Append(Environment.NewLine);

            // update
            codes.AppendLine(GetIndentStr(indent) + $"public bool Update({EntityName} entity)");
            codes.AppendLine(GetIndentStr(indent) + "{");
            codes.AppendLine(GetIndentStr(indent + 1) +
                             $"return _repoFactory.{EntityName}Repo.Update(entity);");
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

                if (existMarks)
                {
                    codes.Append(GetIndentStr(indent + 1) +
                                 $"return _repoFactory.{EntityName}Repo.SoftDelete(");
                    codes.Append(string.Join(", ", paramList));
                    codes.Append(");");
                }
                else
                {
                    codes.Append(GetIndentStr(indent + 1) +
                                 $"return _repoFactory.{EntityName}Repo.Delete(");
                    codes.Append(string.Join(", ", paramList));
                    codes.Append(");");
                }

                codes.Append(Environment.NewLine);
                codes.AppendLine(GetIndentStr(indent) + "}");
                codes.Append(Environment.NewLine);
            }


            // bulk delete
            if (pkColumns.Count == 1)
            {
                codes.AppendLine(GetIndentStr(indent) + "public bool Delete(IEnumerable<int> idList)");
                codes.AppendLine(GetIndentStr(indent) + "{");

                if (existMarks)
                {
                    codes.AppendLine(GetIndentStr(indent + 1) +
                              $"return _repoFactory.{EntityName}Repo.SoftDelete(idList.Select<int, object>(t => t).ToList());");
                }
                else
                {
                    codes.AppendLine(GetIndentStr(indent + 1) +
                                $"return _repoFactory.{EntityName}Repo.Delete(idList.Select<int, object>(t => t).ToList());");
                }


                //codes.AppendLine(GetIndentStr(indent + 1) +
                //                 $"throw new NotImplementedException();");
                codes.AppendLine(GetIndentStr(indent) + "}");
                codes.Append(Environment.NewLine);
            }

            // exists
            if (pkColumns.Count == 1)
            {
                codes.AppendLine(GetIndentStr(indent) + $"public bool Exists({pkColumns[0].PropertyType} {ToCamelCase(pkColumns[0].PropertyName)})");
                codes.AppendLine(GetIndentStr(indent) + "{");
                codes.AppendLine(GetIndentStr(indent + 1) +
                    $"return _repoFactory.{EntityName}Repo.Exists({ToCamelCase(pkColumns[0].PropertyName)});");
                codes.AppendLine(GetIndentStr(indent) + "}");
            }

            indent--;
            codes.AppendLine(GetIndentStr(indent) + "}"); // class

            codes.AppendLine("}"); // namespace

            return codes;
        }
    }
}