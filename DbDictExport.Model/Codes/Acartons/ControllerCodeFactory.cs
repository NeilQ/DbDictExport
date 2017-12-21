using System;
using System.Linq;
using System.Text;
using DbDictExport.Core.Common;
using DbDictExport.Core.Dal;

namespace DbDictExport.Core.Codes.Acartons
{
    public class ControllerCodeFactory : AbstractCodeFactory
    {
        public readonly string _camelEntityName;
        public readonly string _apiRouteName;
        public ControllerCodeFactory(string entityName, string moduleName, Table dbTable)
        {
            _camelEntityName = Extentions.ToRequiredFormatString(entityName, Models.NamingRule.Camel);
            _apiRouteName = Inflector.MakePlural(Inflector.ToHumanCase(entityName)).ToLower();
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
            // codes.AppendLine("using PetaPoco;");
            codes.AppendLine($"using {Constants.ACARTONS_NAMESAPCE_PREFIX}.Core.Bll.Interface;");
            codes.AppendLine($"using {Constants.ACARTONS_NAMESAPCE_PREFIX}.Core.Models;");
            codes.AppendLine($"using {Constants.ACARTONS_NAMESAPCE_PREFIX}.Core.Dtos;");
            codes.AppendLine($"using {Constants.ACARTONS_NAMESAPCE_PREFIX}.Core.Api.Filters;");
            codes.AppendLine("using Microsoft.AspNetCore.Http.Extensions;");
            codes.AppendLine("using Microsoft.AspNetCore.Mvc;");

            // namespace
            codes.Append(Environment.NewLine);
            codes.AppendLine($"namespace {Constants.ACARTONS_NAMESAPCE_PREFIX}.Core.Api.Controllers");
            codes.AppendLine("{");

            // class
            indent++;
            codes.AppendLine(GetIndentStr(indent) + "/// <summary>");
            codes.AppendLine(GetIndentStr(indent) + "/// ");
            codes.AppendLine(GetIndentStr(indent) + "/// </summary>");
            codes.AppendLine(GetIndentStr(indent) + "[Produces(\"application/json\")]");
            codes.AppendLine(GetIndentStr(indent) + $"[Route(\"api/{_apiRouteName}\")]");
            codes.AppendLine(GetIndentStr(indent) + $"public class {Inflector.MakePlural(EntityName)}Controller : ApiController");
            codes.AppendLine(GetIndentStr(indent) + "{");

            //files
            indent++;
            codes.Append(Environment.NewLine);
            codes.Append(GetIndentStr(indent) + $"private readonly I{EntityName}Service _{_camelEntityName}Service;");

            //constructor
            codes.Append(Environment.NewLine);
            codes.AppendLine(GetIndentStr(indent) + $"public {Inflector.MakePlural(EntityName)}Controller(I{EntityName}Service {_camelEntityName}Service)");
            codes.AppendLine(GetIndentStr(indent) + "{");
            codes.AppendLine(GetIndentStr(indent + 1) + $"_{_camelEntityName}Service = {_camelEntityName}Service;");
            codes.AppendLine(GetIndentStr(indent) + "}");


            if (pkColumns.Count < 2)
            {
                // methods
                // get page data
                codes.Append(Environment.NewLine);
                codes.AppendLine(GetIndentStr(indent) + "/// <summary>");
                codes.AppendLine(GetIndentStr(indent) + "/// 获取实体列表");
                codes.AppendLine(GetIndentStr(indent) + "/// </summary>");
                codes.AppendLine(GetIndentStr(indent) + "/// <param name=\"size\"></param>");
                codes.AppendLine(GetIndentStr(indent) + "/// <param name=\"page\"></param>");
                codes.AppendLine(GetIndentStr(indent) + "/// <param name=\"sort\"></param>");
                codes.AppendLine(GetIndentStr(indent) + "[HttpGet]");
                codes.AppendLine(GetIndentStr(indent) + $"[ProducesResponseType(typeof(PageModel<{EntityName}>), 200)]");
                codes.AppendLine(GetIndentStr(indent) +
                                 "public IActionResult Get(int page, int size, string sort = \"add_time desc\")");
                codes.AppendLine(GetIndentStr(indent) + "{");
                codes.AppendLine(GetIndentStr(indent + 1) +
                                 $"var data = _{_camelEntityName}Service.GetByPage(out int total, page, size, sort);");
                codes.AppendLine(GetIndentStr(indent + 1) + $"return Ok(new PageModel<{EntityName}>");
                codes.AppendLine(GetIndentStr(indent + 1) + "{");
                codes.AppendLine(GetIndentStr(indent + 2) + "Total = total,");
                codes.AppendLine(GetIndentStr(indent + 2) + "Data = data");
                codes.AppendLine(GetIndentStr(indent + 1) + "});");
                codes.AppendLine(GetIndentStr(indent) + "}");
            }

            // methods
            // get single data
            codes.Append(Environment.NewLine);
            codes.AppendLine(GetIndentStr(indent) + "/// <summary>");
            codes.AppendLine(GetIndentStr(indent) + "/// 根据id获取实体信息");
            codes.AppendLine(GetIndentStr(indent) + "/// </summary>");
            codes.AppendLine(GetIndentStr(indent) + "/// <param name=\"id\">主键</param>");
            codes.AppendLine(GetIndentStr(indent) + "/// <returns></returns>");
            codes.AppendLine(GetIndentStr(indent) + "[HttpGet(\"{id}\")]");
            codes.AppendLine(GetIndentStr(indent) + $"[ProducesResponseType(typeof({EntityName}), 200)]");
            codes.AppendLine(GetIndentStr(indent) + "public IActionResult Get(int id)");
            codes.AppendLine(GetIndentStr(indent) + "{");
            codes.AppendLine(GetIndentStr(indent + 1) + $"var data = _{_camelEntityName}Service.GetByPK(id);");
            codes.AppendLine(GetIndentStr(indent + 1) + "if (data == null) return NotFound();");
            codes.AppendLine(GetIndentStr(indent + 1) + "return Ok(data);");
            codes.AppendLine(GetIndentStr(indent) + "}");

            // methods
            // post data
            codes.Append(Environment.NewLine);
            codes.AppendLine(GetIndentStr(indent) + "/// <summary>");
            codes.AppendLine(GetIndentStr(indent) + "/// 添加实体");
            codes.AppendLine(GetIndentStr(indent) + "/// </summary>");
            codes.AppendLine(GetIndentStr(indent) + "[Validate]");
            codes.AppendLine(GetIndentStr(indent) + "[HttpPost]");
            codes.AppendLine(GetIndentStr(indent) + $"public IActionResult Post([FromBody]{EntityName} model)");
            codes.AppendLine(GetIndentStr(indent) + "{");
            codes.AppendLine(GetIndentStr(indent + 1) + $"var id = _{_camelEntityName}Service.Add(model);");
            codes.Append(Environment.NewLine);
            codes.AppendLine(GetIndentStr(indent + 1) + "return Created(Request.GetEncodedUrl() + $\"/{id}\", id);");
            codes.AppendLine(GetIndentStr(indent) + "}");

            // methods
            // put data
            codes.Append(Environment.NewLine);
            codes.AppendLine(GetIndentStr(indent) + "/// <summary>");
            codes.AppendLine(GetIndentStr(indent) + "/// 更新实体");
            codes.AppendLine(GetIndentStr(indent) + "/// </summary>");
            codes.AppendLine(GetIndentStr(indent) + "[Validate]");
            codes.AppendLine(GetIndentStr(indent) + "[HttpPut(\"{id}\")]");
            codes.AppendLine(GetIndentStr(indent) + $"public IActionResult Put(int id, [FromBody]{EntityName} model)");
            codes.AppendLine(GetIndentStr(indent) + "{");
            codes.AppendLine(GetIndentStr(indent + 1) + $"if (!_{_camelEntityName}Service.Exists(id)) return NotFound();");
            codes.Append(Environment.NewLine);
            codes.AppendLine(GetIndentStr(indent + 1) + $"_{_camelEntityName}Service.Update(model);");
            codes.Append(Environment.NewLine);
            codes.AppendLine(GetIndentStr(indent + 1) + "return Ok();");
            codes.AppendLine(GetIndentStr(indent) + "}");

            // methods
            // delete single data
            codes.Append(Environment.NewLine);
            codes.AppendLine(GetIndentStr(indent) + "/// <summary>");
            codes.AppendLine(GetIndentStr(indent) + "/// 删除实体");
            codes.AppendLine(GetIndentStr(indent) + "/// </summary>");
            codes.AppendLine(GetIndentStr(indent) + "[HttpDelete(\"{id}\")]");
            codes.AppendLine(GetIndentStr(indent) + "public IActionResult Delete(int id)");
            codes.AppendLine(GetIndentStr(indent) + "{");
            codes.AppendLine(GetIndentStr(indent + 1) + $"var result = _{_camelEntityName}Service.Delete(id);");
            codes.AppendLine(GetIndentStr(indent + 1) + "if (result)");
            codes.AppendLine(GetIndentStr(indent + 1) + "{");
            codes.AppendLine(GetIndentStr(indent + 2) + "return Ok();");
            codes.AppendLine(GetIndentStr(indent + 1) + "}");
            codes.AppendLine(GetIndentStr(indent + 1) + "return NotFound();");
            codes.AppendLine(GetIndentStr(indent) + "}");

            // methods
            // delete single data
            codes.Append(Environment.NewLine);
            codes.AppendLine(GetIndentStr(indent) + "/// <summary>");
            codes.AppendLine(GetIndentStr(indent) + "/// 批量删除实体");
            codes.AppendLine(GetIndentStr(indent) + "/// </summary>");
            codes.AppendLine(GetIndentStr(indent) + "/// <param name=\"idList\">主键ID的list集合</param>");
            codes.AppendLine(GetIndentStr(indent) + "[HttpPost(\"bulk_delete\")]");
            codes.AppendLine(GetIndentStr(indent) + "public IActionResult Delete([FromBody] List<int> idList)");
            codes.AppendLine(GetIndentStr(indent) + "{");
            codes.AppendLine(GetIndentStr(indent + 1) + "if (idList == null || idList.Count == 0)");
            codes.AppendLine(GetIndentStr(indent + 1) + "{");
            codes.AppendLine(GetIndentStr(indent + 2) + "return Ok();");
            codes.AppendLine(GetIndentStr(indent + 1) + "}");
            codes.AppendLine(GetIndentStr(indent + 1) + $"_{_camelEntityName}Service.Delete(idList);");
            codes.AppendLine(GetIndentStr(indent + 1) + "return Ok();");
            codes.AppendLine(GetIndentStr(indent) + "}");

            codes.Append(Environment.NewLine);
            indent--;
            codes.AppendLine(GetIndentStr(indent) + "}"); // class

            codes.AppendLine("}"); // namespace
            return codes;
        }
    }
}
