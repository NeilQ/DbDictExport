using System;
using System.Collections.Generic;
using System.Linq;
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
            codes.AppendLine("using NPoco;");
            codes.AppendLine("using System.Collections.Generic;");
            codes.AppendLine($"using {Constants.ACARTONS_NAMESAPCE_PREFIX}.Core.Dal.Interface;");
            codes.AppendLine($"using {Constants.ACARTONS_NAMESAPCE_PREFIX}.Core.Models;");
            codes.AppendLine($"using {Constants.ACARTONS_NAMESAPCE_PREFIX}.Core.Entities;");

            // namespace
            codes.Append(Environment.NewLine);
            codes.AppendLine($"namespace {Constants.ACARTONS_NAMESAPCE_PREFIX}.Core.Dal");
            codes.AppendLine("{");

            // class
            indent++;
            codes.AppendLine(GetIndentStr(indent) +
                             string.Format("public class {0}Repo : RepoBase<{0}>, I{0}Repo", EntityName));
            codes.AppendLine(GetIndentStr(indent) + "{");

            indent++;
            // constructor
            codes.Append(Environment.NewLine);
            codes.AppendLine(GetIndentStr(indent) +
                             "public {0}Repo(IUserContext userContext) : base(userContext) { }");


            // methods
            // get by primary
            var pkColumns = Table.Columns.Where(t => t.IsPK).ToList();
            if (pkColumns.Count > 1)
            {
                codes.Append(Environment.NewLine);
                codes.Append(GetIndentStr(indent) + $"public {EntityName} GetByPk(");
                var tmpList = pkColumns.Select(pk => $"{pk.PropertyType} {ToCamelCase(pk.PropertyName)}");
                codes.Append(string.Join(", ", tmpList));
                codes.Append(")\r\n");
                codes.AppendLine(GetIndentStr(indent) + "{");

                // method body
                indent++;
                codes.Append(GetIndentStr(indent) + $"return Db.SingleOrDefault<{EntityName}>({(existMarks ? "$" : "")}\"WHERE ");
                var whereStr = new List<string>();
                if (existMarks)
                {
                    whereStr.Add("{DbFieldNames.AddTime}=false");
                }
                int index = 0;
                foreach (var pk in pkColumns)
                {
                    whereStr.Add($"{pk.Name} = @{index++}");
                }
                codes.Append(string.Join(" AND ", whereStr));
                codes.Append($"\", {string.Join(", ", pkColumns.Select(pk => $"{ToCamelCase(pk.PropertyName)}"))});");
                codes.Append(Environment.NewLine);

                indent--;
                codes.AppendLine(GetIndentStr(indent) + "}");
            }

            codes.Append(Environment.NewLine);

            // methods
            // get based on the composition of each primary keys
            if (pkColumns.Count >= 2)
            {
                for (var i = 0; i < pkColumns.Count; i++)
                {
                    codes.Append(GetIndentStr(indent) + $"public List<{EntityName}> GetBy{pkColumns[i].PropertyName}(");
                    codes.Append($"{pkColumns[i].PropertyType} {ToCamelCase(pkColumns[i].PropertyName)}");
                    codes.Append(")\r\n");
                    codes.AppendLine(GetIndentStr(indent) + "{");

                    // method body
                    indent++;
                    codes.Append(GetIndentStr(indent) + $"return Db.Fetch<{EntityName}>({(existMarks ? "$" : "")}\"WHERE ");

                    var whereStr = new List<string>();
                    if (existMarks)
                    {
                        whereStr.Add("{DbFieldNames.AddTime}=false");
                    }
                    whereStr.Add($"{pkColumns[i].Name} = @0 ");

                    codes.Append(string.Join(" AND ", whereStr) + "\",");
                    codes.AppendLine(ToCamelCase(pkColumns[i].PropertyName) + ");");
                    indent--;
                    codes.AppendLine(GetIndentStr(indent) + "}");
                    codes.Append(Environment.NewLine);
                }

            }


            if (pkColumns.Count < 2)
            {
                codes.AppendLine(GetIndentStr(indent) +
                                 $"public List<{EntityName}> GetByPage(out int total, int page, int size, string sort, object condition)");
                codes.AppendLine(GetIndentStr(indent) + "{");
                indent++;
                codes.AppendLine(GetIndentStr(indent) +
                                 "var sql = new Sql()");
                indent++;
                codes.AppendLine(GetIndentStr(indent) + ".Select(\"*\")");

                if (existMarks)
                {
                    codes.AppendLine(GetIndentStr(indent) + ".From(PocoData.TableInfo.TableName)");
                    codes.AppendLine(GetIndentStr(indent) + ".Where($\"{DbFieldNames.MarkedForDelete}=false\");");
                }
                else
                {
                    codes.AppendLine(GetIndentStr(indent) + ".From(PocoData.TableInfo.TableName);");
                }

                indent--;

                codes.AppendLine(GetIndentStr(indent) + "if (string.IsNullOrEmpty(sort))");
                codes.AppendLine(GetIndentStr(indent) + "{");
                codes.AppendLine(GetIndentStr(indent + 1) + "sort = $\"{DbFieldNames.AddTime} desc\";");
                codes.AppendLine(GetIndentStr(indent) + "}");

                codes.AppendLine(GetIndentStr(indent) + "sql.OrderBy(sort);");
                codes.AppendLine(GetIndentStr(indent) + $"var data = Db.Page<{EntityName}>(page, size, sql);");
                codes.AppendLine(GetIndentStr(indent) + "total = (int)data.TotalItems;");
                codes.AppendLine(GetIndentStr(indent) + "return data.Items;");

                indent--;
                codes.AppendLine(GetIndentStr(indent) + "}");
                codes.AppendLine(Environment.NewLine);
            }

            // get all
            codes.AppendLine(GetIndentStr(indent) +
                                             $"public List<{EntityName}> GetAll(string sort, object condition)");
            codes.AppendLine(GetIndentStr(indent) + "{");
            indent++;
            codes.AppendLine(GetIndentStr(indent) +
                             "var sql = new Sql()");
            indent++;
            codes.AppendLine(GetIndentStr(indent) + ".Select(\"*\")");
            if (existMarks)
            {
                codes.AppendLine(GetIndentStr(indent) + ".From(PocoData.TableInfo.TableName)");
                codes.AppendLine(GetIndentStr(indent) + ".Where($\"{DbFieldNames.MarkedForDelete}=false\");");
            }
            else
            {
                codes.Append(GetIndentStr(indent) + ".From(PocoData.TableInfo.TableName);");
                codes.Append(Environment.NewLine);

            }
            indent--;

            codes.AppendLine(GetIndentStr(indent) + "if (!string.IsNullOrEmpty(sort))");
            codes.AppendLine(GetIndentStr(indent) + "{");
            codes.AppendLine(GetIndentStr(indent + 1) + "sql.OrderBy(sort);");
            codes.AppendLine(GetIndentStr(indent) + "}");

            codes.AppendLine(GetIndentStr(indent) + $"var data = Db.Fetch<{EntityName}>(sql);");
            codes.AppendLine(GetIndentStr(indent) + "return data;");

            indent--;
            codes.AppendLine(GetIndentStr(indent) + "}");
            codes.AppendLine(Environment.NewLine);

            indent--;
            codes.AppendLine(GetIndentStr(indent) + "}"); // class 
            codes.AppendLine("}"); // namespace

            return codes;
        }

    }
}