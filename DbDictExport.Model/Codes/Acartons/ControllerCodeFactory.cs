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
            _apiRouteName = Inflector.MakePlural(Inflector.ToHumanCase(entityName.ToLower()));
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
            codes.AppendLine("using System.Web.Http;");
            codes.AppendLine("using System.Web.Http.Description;");
            // codes.AppendLine("using PetaPoco;");
            codes.AppendLine($"using {Constants.ACARTONS_NAMESAPCE_PREFIX}.Bll.Interface;");
            codes.AppendLine($"using {Constants.ACARTONS_NAMESAPCE_PREFIX}.Model;");
            codes.AppendLine($"using {Constants.ACARTONS_NAMESAPCE_PREFIX}.Model.Dtos;");
            codes.AppendLine($"using {Constants.ACARTONS_NAMESAPCE_PREFIX}.Utils;");

            // namespace
            codes.Append(Environment.NewLine);
            codes.AppendLine($"namespace {Constants.ACARTONS_NAMESAPCE_PREFIX}.Api.Controllers");
            codes.AppendLine("{");

            // class
            indent++;
            codes.AppendLine(GetIndentStr(indent) + "/// <summary>");
            codes.AppendLine(GetIndentStr(indent) + "/// ");
            codes.AppendLine(GetIndentStr(indent) + "/// </summary>");
            codes.AppendLine(GetIndentStr(indent) + $"[RoutePrefix(\"api/{_apiRouteName}\")]");
            codes.AppendLine(GetIndentStr(indent) + $"public class {EntityName}Controller : ApiController");
            codes.AppendLine(GetIndentStr(indent) + "{");

            //files
            indent++;
            codes.Append(Environment.NewLine);
            codes.Append(GetIndentStr(indent) + $"private readonly I{EntityName}Service _{_camelEntityName}Service;");

            //constructor
            codes.Append(Environment.NewLine);
            codes.AppendLine(GetIndentStr(indent) + $"public {EntityName}Controller(I{EntityName}Service {_camelEntityName}Service)");
            codes.AppendLine(GetIndentStr(indent) + "{");
            codes.AppendLine(GetIndentStr(indent + 1) + $"_{_camelEntityName}Service = {_camelEntityName}Service;");
            codes.AppendLine(GetIndentStr(indent) + "}");


            if (pkColumns.Count < 2)
            {
                // methods
                // get page data
                codes.Append(Environment.NewLine);
                codes.AppendLine(GetIndentStr(indent) + "/// <summary>");
                codes.AppendLine(GetIndentStr(indent) + "/// 分页获取实体列表");
                codes.AppendLine(GetIndentStr(indent) + "/// </summary>");
                codes.AppendLine(GetIndentStr(indent) + "/// <param name=\"num\"></param>");
                codes.AppendLine(GetIndentStr(indent) + "/// <param name=\"page\"></param>");
                codes.AppendLine(GetIndentStr(indent) + "/// <param name=\"sort\"></param>");
                codes.AppendLine(GetIndentStr(indent) + "[Route(\"\")]");
                codes.AppendLine(GetIndentStr(indent) + $"[ResponseType(typeof(PageModel<{EntityName}>))]");
                codes.AppendLine(GetIndentStr(indent) +
                                 "public IHttpActionResult Get(int page, int num, string sort = \"add_time desc\")");
                codes.AppendLine(GetIndentStr(indent) + "{");
                codes.AppendLine(GetIndentStr(indent + 1) + "// validate");
                codes.AppendLine(GetIndentStr(indent + 1) + "if (page <= 0 || num <= 0)");
                codes.AppendLine(GetIndentStr(indent + 1) + "{");
                codes.AppendLine(GetIndentStr(indent + 2) + "return BadRequest(MessageFactory.CreatePageParamsInvalidMessage());");
                codes.AppendLine(GetIndentStr(indent + 1) + "}");
                codes.Append(Environment.NewLine);
                codes.AppendLine(GetIndentStr(indent + 1) + "int total;");
                codes.AppendLine(GetIndentStr(indent + 1) +
                                 $"var data = _{_camelEntityName}Service.GetByPage(out total, page, num, sort);");
                codes.AppendLine(GetIndentStr(indent + 1) + $"return Ok(new ResponseModel<{EntityName}>");
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
            codes.AppendLine(GetIndentStr(indent) + "[Route(\"{id}\")]");
            codes.AppendLine(GetIndentStr(indent) + $"[ResponseType(typeof({EntityName}))]");
            codes.AppendLine(GetIndentStr(indent) + "public IHttpActionResult Get(int id)");
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
            codes.AppendLine(GetIndentStr(indent) + "[Route(\"\")]");
            codes.AppendLine(GetIndentStr(indent) + $"public IHttpActionResult Post([FromBody]{EntityName} model)");
            codes.AppendLine(GetIndentStr(indent) + "{");
            codes.AppendLine(GetIndentStr(indent + 1) + "if (model == null)");
            codes.AppendLine(GetIndentStr(indent + 1) + "{");
            codes.AppendLine(GetIndentStr(indent + 2) + "return BadRequest(MessageFactory.CreateParamsIsNullMessage());");
            codes.AppendLine(GetIndentStr(indent + 1) + "}");
            codes.AppendLine(GetIndentStr(indent + 1) + "// Validate");
            codes.AppendLine(GetIndentStr(indent + 1) + "if (!ModelState.IsValid)");
            codes.AppendLine(GetIndentStr(indent + 1) + "{");
            codes.AppendLine(GetIndentStr(indent + 2) + "return BadRequest(ModelState);");
            codes.AppendLine(GetIndentStr(indent + 1) + "}");
            codes.Append(Environment.NewLine);
            codes.AppendLine(GetIndentStr(indent + 1) + $"var key = _{_camelEntityName}Service.Add(model);");
            codes.Append(Environment.NewLine);
            codes.AppendLine(GetIndentStr(indent + 1) + "return Created(Request.RequestUri + key.ToString(), key);");
            codes.AppendLine(GetIndentStr(indent) + "}");

            // methods
            // put data
            codes.Append(Environment.NewLine);
            codes.AppendLine(GetIndentStr(indent) + "/// <summary>");
            codes.AppendLine(GetIndentStr(indent) + "/// 更新实体");
            codes.AppendLine(GetIndentStr(indent) + "/// </summary>");
            codes.AppendLine(GetIndentStr(indent) + "[Route(\"{id}\")]");
            codes.AppendLine(GetIndentStr(indent) + $"public IHttpActionResult Put(int id, [FromBody]{EntityName} model)");
            codes.AppendLine(GetIndentStr(indent) + "{");
            codes.AppendLine(GetIndentStr(indent + 1) + "if (model == null)");
            codes.AppendLine(GetIndentStr(indent + 1) + "{");
            codes.AppendLine(GetIndentStr(indent + 2) + "return BadRequest(MessageFactory.CreateParamsIsNullMessage());");
            codes.AppendLine(GetIndentStr(indent + 1) + "}");
            codes.AppendLine(GetIndentStr(indent + 1) + "// Validate");
            codes.AppendLine(GetIndentStr(indent + 1) + "if (!ModelState.IsValid)");
            codes.AppendLine(GetIndentStr(indent + 1) + "{");
            codes.AppendLine(GetIndentStr(indent + 2) + "return BadRequest(ModelState);");
            codes.AppendLine(GetIndentStr(indent + 1) + "}");
            codes.Append(Environment.NewLine);
            codes.AppendLine(GetIndentStr(indent + 1) + $"var entity = _{_camelEntityName}Service.GetByPK(id);");
            codes.AppendLine(GetIndentStr(indent + 1) + "if (entity == null)");
            codes.AppendLine(GetIndentStr(indent + 1) + "{");
            codes.AppendLine(GetIndentStr(indent + 2) + "return NotFound();");
            codes.AppendLine(GetIndentStr(indent + 1) + "}");
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
            codes.AppendLine(GetIndentStr(indent) + "[Route(\"{id}\")]");
            codes.AppendLine(GetIndentStr(indent) + "public IHttpActionResult Delete(int id)");
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
            codes.AppendLine(GetIndentStr(indent) + "[Route(\"bulk_delete\")]");
            codes.AppendLine(GetIndentStr(indent) + "[HttpPost]");
            codes.AppendLine(GetIndentStr(indent) + "public IHttpActionResult Delete([FromBody] List<int> idList)");
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
