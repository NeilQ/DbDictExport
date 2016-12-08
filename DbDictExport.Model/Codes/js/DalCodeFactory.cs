using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbDictExport.Core.Common;
using DbDictExport.Core.Dal;

namespace DbDictExport.Core.Codes.js
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
            var existMarks = Table.Columns.Exists(t => t.Name.ToLower() == "marks");
            var codes = new StringBuilder();
            var indent = 0;
            // using
            codes.AppendLine("using System.Collections.Generic;");
            codes.AppendLine("using System.Linq;");
            codes.AppendLine("using PetaPoco;");
            codes.AppendLine($"using {Constants.KDCODE_NAMESPACE_PREFIX}{ModuleName}.DAL.MySql;");
            codes.AppendLine($"using {Constants.KDCODE_NAMESPACE_PREFIX}{ModuleName}.IDAL;");
            codes.AppendLine($"using {Constants.KDCODE_NAMESPACE_PREFIX}{ModuleName}.Model;");
            codes.AppendLine($"using {Constants.KDCODE_NAMESPACE_PREFIX}{ModuleName}.Utility;");

            // namespace
            codes.Append(Environment.NewLine);
            codes.AppendLine($"namespace {Constants.KDCODE_NAMESPACE_PREFIX}{ModuleName}.DAL.MySql");
            codes.AppendLine("{");

            // class
            indent++;
            codes.AppendLine(GetIndentStr(indent) +
                             string.Format("public class {0}Manager : ManagerBase<{0}>, I{0}Manager", EntityName));
            codes.AppendLine(GetIndentStr(indent) + "{");

            // methods
            // get by primary
            indent++;
            var pkColumns = Table.Columns.Where(t => t.IsPK).ToList();
            if (pkColumns.Any())
            {
                codes.Append(Environment.NewLine);
                codes.Append(GetIndentStr(indent) + $"public {EntityName} GetByPK(");
                var tmpList = pkColumns.Select(pk => $"{MapCSharpType(pk.DbType)} {ToCamelCase(pk.Name)}");
                codes.Append(string.Join(", ", tmpList));
                codes.Append(")\r\n");
                codes.AppendLine(GetIndentStr(indent) + "{");

                // method body
                indent++;
                codes.Append(GetIndentStr(indent) + $"return Db.SingleOrDefault<{EntityName}>(\"WHERE ");
                //codes.Append("Marks = 1 and ID=@ID");
                var whereStr = new List<string>();
                if (existMarks)
                {
                    whereStr.Add("Marks=1");
                }
                int index = 0;
                foreach (var pk in pkColumns)
                {
                    whereStr.Add($"{pk.Name} = @{index++}");
                }
                codes.Append(string.Join(" AND ", whereStr));
                var list = pkColumns.Select(pk => $"{ToCamelCase(pk.Name)}");
                codes.Append($"\", {string.Join(", ", list)});");
                codes.Append(Environment.NewLine);

                indent--;
                codes.AppendLine(GetIndentStr(indent) + "}");
            }

            codes.Append(Environment.NewLine);

            // methods
            // get based on the composition of each primary keys
            if (pkColumns.Count >= 2)
            {
                string tempStr = string.Empty;
                List<string> function = new List<string>();
                List<string> condition = new List<string>();
                List<string> variate = new List<string>();
                for (int i = 0; i < pkColumns.Count; i++)
                {

                    codes.Append(GetIndentStr(indent) + $"public List<{EntityName}> GetBy{pkColumns[i].Name}(");
                    codes.Append( $"{MapCSharpType(pkColumns[i].DbType)} {ToCamelCase(pkColumns[i].Name)}");
                    codes.Append(")\r\n");
                    codes.AppendLine(GetIndentStr(indent) + "{");

                    // method body
                    indent++;
                    codes.Append(GetIndentStr(indent) + $"return Db.Query<{EntityName}>(\"WHERE ");
                    //codes.Append("Marks = 1 and ID=@ID");
                    var whereStr = new List<string>();
                    if (existMarks)
                    {
                        whereStr.Add("Marks=1");
                    }
                    whereStr.Add($"{pkColumns[i].Name} = @0 ");
                    codes.Append(string.Join(" AND ", whereStr)+"\",");
                    codes.AppendLine(Extentions.ToRequiredFormatString(pkColumns[i].Name+");",Models.NamingRule.Camel));
                    indent--;
                    codes.AppendLine(GetIndentStr(indent) + "}");
                    codes.Append(Environment.NewLine);
                }
                
            }


            if (pkColumns.Count < 2)
            {
                codes.AppendLine(GetIndentStr(indent) +
                                 $"public List<{EntityName}> GetByPage(out int total, int page, int size, string sort, bool asc)");
                codes.AppendLine(GetIndentStr(indent) + "{");
                indent++;
                codes.AppendLine(GetIndentStr(indent) +
                                 "var sql = new Sql()");
                indent++;
                codes.AppendLine(GetIndentStr(indent) + ".Select(\"*\")");
                if (existMarks)
                {

                    codes.AppendLine(GetIndentStr(indent) + ".From(PocoData.TableInfo.TableName)");
                    codes.AppendLine(GetIndentStr(indent) + ".Where(\"Marks=1\");");
                }
                else
                {
                    codes.AppendLine(GetIndentStr(indent) + ".From(PocoData.TableInfo.TableName);");
                }

                indent--;

                codes.AppendLine(GetIndentStr(indent) + "if (string.IsNullOrEmpty(sort))");
                codes.AppendLine(GetIndentStr(indent) + "{");
                codes.AppendLine(GetIndentStr(indent + 1) + "sort = \"AddTime\";");
                codes.AppendLine(GetIndentStr(indent) + "}");

                codes.AppendLine(GetIndentStr(indent) + "sql.OrderBy(sort + (asc ? \" ASC\" : \" DESC\"));");
                codes.AppendLine(GetIndentStr(indent) + $"var data = Db.Page<{EntityName}>(page, size, sql);");
                codes.AppendLine(GetIndentStr(indent) + "total = (int)data.TotalItems;");
                codes.AppendLine(GetIndentStr(indent) + "return data.Items;");

                indent--;
                codes.AppendLine(GetIndentStr(indent) + "}");
                codes.AppendLine(Environment.NewLine);
            }

            indent--;
            codes.AppendLine(GetIndentStr(indent) + "}"); // class 
            codes.AppendLine("}"); // namespace

            return codes;
        }
        
    }
}