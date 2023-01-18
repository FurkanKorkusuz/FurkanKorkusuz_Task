using Core.DataAccess.Dapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.CodeGenerator
{
    public class Generate_Manuel
    {

        private static Manuel_Model _model;

        public Generate_Manuel(Manuel_Model model)
        {
            _model = model;
        }

        public string CoreEntityConcrete()
        {
            string properties = "";
            string filterProperties = "";
            foreach (var item in _model.Properties)
            {
                properties += $@"
         public {item.Type}{(item.IsNullAble && item.Type != "string" ? "?" : "")} {item.Name} {{ get; set; }}";
                filterProperties += $@"
         public {item.Type}{(item.Type == "string" ? "" : "?")} {item.Name} {{ get; set; }}";
            }
            return $@"using System.ComponentModel.DataAnnotations.Schema;
using Core.Entities.Abstract;

namespace Core.Entities.Concrete
{{
  
    public class {_model.EntityName} 
    {{
{properties}
    }}

    public class {_model.EntityName}Filter : BaseFilter
    {{
{filterProperties}
    }}
}}";
        }

        public string CoreEntityViewDto()
        {
            string properties = "";
            if (_model.Dtos != null)
            {
                foreach (var item in _model.Dtos)
                {
                    properties += $@"
                    public string {item.Substring(item.Length - 2)}Name {{ get; set; }}
                ";
                }
            }

            return $@"using System;
using Core.Entities.Concrete;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.DTOs
{{
    public class {_model.EntityName}ViewDto : {_model.EntityName}
    {{
         {properties}
    }}
}}


        ";
        }

        public string CoreEntityAddDto()
        {
            string properties = "";
            if (_model.Dtos != null)
            {
                foreach (var item in _model.Dtos)
                {
                    properties += $@"
                    public string List<{item.Substring(item.Length - 2)}> {item.Substring(item.Length - 2)}List {{ get; set; }}
                ";
                }
            }
            return $@"using System;
using Core.Entities.Concrete;
using System.Collections.Generic;

namespace Core.Entities.DTOs
{{
    public class {_model.EntityName}AddDto 
    {{
        public {_model.EntityName} {_model.EntityName} {{ get; set; }}
        {properties}
    }}
}}

        ";
        }

        public string DataAccessAbstract()
        {
            string over = "";
            return $@"using Core.DataAccess.Abstract;
using Core.Entities.Concrete;
using Core.Entities.DTOs;
using Core.BaseResults;
using System;
using System.Collections.Generic;
using System.Linq;
using Core.DataAccess.Dapper;

namespace DataAccess.Abstract
{{
    public interface I{_model.EntityName}Dal 
    {{
        {over}
    }}
}}


        ";
        }

        public string DataAccessConcrete()
        {
            string over = "";
            return $@"using Core.BaseResults;
using Core.DataAccess.Dapper;
using Core.DataAccess.Repositories;
using Core.Entities.Concrete;
using Core.Entities.DTOs;
using DataAccess.Abstract;
using System.Linq;
using System.Text;
using System.Data;
using static Dapper.SqlMapper;
using Core.DataAccess.Abstract;
using System.Collections.Generic;
using System;

namespace DataAccess.Concrete.Dapper
{{
    public class Dp{_model.EntityName}Dal : I{_model.EntityName}Dal
    {{     

        {over}
    }}
}}          
        ";
        }

        public string BusinessAbstract()
        {
            string over = "";
            return $@"using Core.BaseResults;
using Core.DataAccess.Dapper;
using Core.Entities.Concrete;
using Core.Entities.DTOs;
using System.Collections.Generic;
using Core.Utilities.Business;

namespace Business.Abstract
{{
    public interface I{_model.EntityName}Service
    {{
        {over}
    }}
}}

        ";
        }

        public string BusinessConcrete()
        {
            string over = "";
            return $@"using Business.Abstract;
using Core.BaseResults;
using Core.Entities.Concrete;
using Core.Entities.DTOs;
using Core.Utilities.Business;
using System.Collections.Generic;
using DataAccess.Abstract;
using Core.DataAccess.Dapper;

namespace Business.Concrete
{{
    public class {_model.EntityName}Manager : I{_model.EntityName}Service
    {{
        I{_model.EntityName}Dal _{_model.PascalCaseName}Dal;
        public {_model.EntityName}Manager(
        I{_model.EntityName}Dal {_model.PascalCaseName}Dal
        )
        {{
            _{_model.PascalCaseName}Dal = {_model.PascalCaseName}Dal;
        }}
        {over}
    }}
}}            
        ";
        }

        public string AddDependencyResolve()
        {
            return $@"
            builder.RegisterType<{_model.EntityName}Manager>().As<I{_model.EntityName}Service>();
            builder.RegisterType<Dp{_model.EntityName}Dal>().As<I{_model.EntityName}Dal>();
        ";
        }

        public string AddFluentValidator()
        {
            string properties = "";

            foreach (var item in _model.Properties)
            {
                if (!item.IsNullAble)
                {
                    properties += $@"
             RuleFor(p => p.{item.Name}).NotEmpty().WithMessage(""{item.Name} boş geçilemez."");";
                }
                if (item.Type == "string" && (item.MaxLength ?? 0) > 0)
                {
                    properties += $@"
             RuleFor(p => p.{item.Name}).Length(0, {item.MaxLength}).WithMessage(""{item.Name} 1 ile {item.MaxLength} karakter arasında olmalıdır."");";
                }
            }

            return $@"using Core.Entities.Concrete;
using FluentValidation;
using System;

namespace Business.ValidationRules.FluentValidation
{{
    public class {_model.EntityName}Validator : AbstractValidator<{_model.EntityName}>
    {{
        public {_model.EntityName}Validator()
        {{
            {properties}
        }}
    }}
}}
        ";
        }

        public string APIController()
        {
            string over = "";
            string dtoListService1 = "",
                dtoListService2 = "",
                dtoListService3 = "";


            if (_model.Dtos != null)
            {
                foreach (string item in _model.Dtos)
                {
                    dtoListService1 += @$"
I{item}Service _{item.Substring(0, 1).ToLower() + item.Substring(-1)}Service;";


                    dtoListService2 += @$",
I{item}Service {item.Substring(0, 1).ToLower() + item.Substring(-1)}Service";

                    dtoListService3 += @$",
 _{item}Service = {item.Substring(0, 1).ToLower() + item.Substring(-1)}Service;";
                }
            }


            string constructor = $@"
    [Route(""{_model.RouteName}"")]
    public class {_model.EntityName}Controller
    {{
        I{_model.EntityName}Service _{_model.PascalCaseName}Service;
        {dtoListService1}
        public {_model.EntityName}Controller(
            I{_model.EntityName}Service {_model.PascalCaseName}Service{dtoListService2}
            ) 
        {{
            _{_model.PascalCaseName}Service = {_model.PascalCaseName}Service;
            {dtoListService3}
        }}
                ";


            if (_model.HasViewDto)
            {

                over += $@" 

        [Route(""list-dto"")]
        [HttpPost]
        public override ActionResult GetList(QueryParameter<{_model.EntityName}Filter> queryParameter)
        {{
            return Json(_{_model.PascalCaseName}Service.GetDtoList(queryParameter));
        }}
                ";
            }
            return $@"using Business.Abstract;
using Core.Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using Core.DataAccess.Dapper;
using API.Controllers;

namespace API.Controllers
{{
    {constructor}
    {over}
    }}
}}

        ";
        }


    }

    public class Manuel_Property
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsNullAble { get; set; }
        public bool IsPrimaryKey { get; set; }
        public int? MaxLength { get; set; }

    }



    public class Manuel_Model
    {
        public string TableName { get; set; }
        public string EntityName
        {
            get
            {
                return GetEntityName();
            }
        }
        private string GetEntityName()
        {
            string entity = "";

            foreach (string item in TableName.Split('_'))
            {
                string name = TableName.TrimEnd('s');
                if (name.EndsWith("es"))
                    name = name.Remove(name.LastIndexOf("es"));
                if (name.EndsWith("ies"))
                    name = name.Remove(name.LastIndexOf("ies"));
                entity += name;
            }


            return entity;
        }
        public string RouteName
        {
            get
            {
                return GetRouteName();
            }
        }
        private string GetRouteName()
        {
            string entity = "";

            foreach (string item in TableName.Split('_'))
            {
                string name = TableName.TrimEnd('s');
                if (name.EndsWith("es"))
                    name = name.Remove(name.LastIndexOf("es"));
                if (name.EndsWith("ies"))
                    name = name.Remove(name.LastIndexOf("ies"));
                entity += name.ToLower() + "-";
            }


            return entity.TrimEnd('-');
        }
        public string PascalCaseName
        {
            get
            {
                return EntityName.Substring(0, 1).ToLower() + EntityName.Remove(0, 1);
            }
        }

        public bool HasAddandEditPage { get; set; } = false;

        public bool HasUIPage { get; set; } = true;

        public bool HasAddDto { get; set; } = false;
        public bool HasViewDto { get; set; } = false;

        public List<string> Dtos { get; set; }


        public List<Property> Properties { get; set; } = new List<Property>();



        public void Generate(Manuel_Model model)
        {

            CreateFile(model, "CoreEntityConcrete");
            if (model.HasViewDto)
            {
                CreateFile(model, "CoreEntityViewDto");
            }
            if (model.HasAddDto)
            {
                CreateFile(model, "CoreEntityAddDto");
            }
            CreateFile(model, "DataAccessAbstract");
            CreateFile(model, "DataAccessConcrete");

            CreateFile(model, "BusinessAbstract");
            CreateFile(model, "BusinessConcrete");
            CreateFile(model, "AddDependencyResolve");
            CreateFile(model, "AddFluentValidator");

            CreateFile(model, "APIController");
        }


        private void CreateFile(Manuel_Model model, string file)
        {
            Generate_Manuel coder = new Generate_Manuel(model);

            Type thisType = typeof(Generate_Manuel);
            MethodInfo theMethod = thisType.GetMethod(file);
            string code = theMethod.Invoke(coder, null).ToString();
            string filePath = Directory.GetCurrentDirectory().Replace("\\API", "");
            string paht = "";
            switch (file)
            {
                case "CoreEntityConcrete":
                    paht = $"\\Core\\Entities\\Concrete\\{model.EntityName}.cs";
                    break;
                case "CoreEntityViewDto":
                    paht = $"\\Core\\Entities\\DTOs\\{model.EntityName}ViewDto.cs";
                    break;
                case "CoreEntityAddDto":
                    paht = $"\\Core\\Entities\\DTOs\\{model.EntityName}AddDto.cs";
                    break;
                case "DataAccessAbstract":
                    paht = $"\\DataAccess\\Abstract\\I{model.EntityName}Dal.cs";
                    break;
                case "DataAccessConcrete":
                    paht = $"\\DataAccess\\Concrete\\Dapper\\Dp{model.EntityName}Dal.cs";
                    break;
                case "BusinessAbstract":
                    paht = $"\\Business\\Abstract\\I{model.EntityName}Service.cs";
                    break;
                case "BusinessConcrete":
                    paht = $"\\Business\\Concrete\\{model.EntityName}Manager.cs";
                    break;
                case "AddFluentValidator":
                    paht = $"\\Business\\ValidationRules\\FluentValidation\\{model.EntityName}Validator.cs";
                    break;
                case "AddDependencyResolve":
                    paht = $"\\Business\\DependencyResolvers\\Autofac\\BusinessModule.cs";

                    string currentCode = "";
                    using (StreamReader reader = System.IO.File.OpenText(filePath + paht))
                    {
                        currentCode = reader.ReadToEnd();
                    }

                    if (!currentCode.Contains(code))
                    {
                        code = currentCode.Replace("// [CHANGE]", @"// [CHANGE]" + code);
                        using (StreamWriter writer = System.IO.File.CreateText(filePath + paht))
                        {
                            writer.Write(code);
                        }
                    }

                    break;
                case "APIController":
                    paht = $"\\API\\Controllers\\{model.EntityName}Controller.cs";
                    break;

            }

            if (!File.Exists(filePath + paht))
            {
                using (StreamWriter writer = System.IO.File.CreateText(filePath + paht))
                {
                    writer.Write(code);
                }
            }
        }

        private List<Property> GetDBData()
        {
            string sql = $@"
SELECT 
c.COLUMN_NAME Name, 
CASE WHEN IS_NULLABLE ='YES' THEN 1 ELSE 0 END AS IsNullAble,
CASE WHEN c.ORDINAL_POSITION =1 THEN 1 ELSE 0 END AS IsPrimaryKey,
  CASE c.DATA_TYPE
    WHEN 'nvarchar' THEN 'string'
    WHEN 'int' THEN 'int'
    WHEN 'text' THEN 'string'
    WHEN 'bit' THEN 'bool'
    WHEN 'datetime' THEN 'string'
    WHEN 'tinyint' THEN 'byte'
    WHEN 'varchar' THEN 'string'
    WHEN 'smallint' THEN 'short'
    WHEN 'smalldatetime' THEN 'string'
	ELSE 'string'
  END AS [Type],
  c.CHARACTER_MAXIMUM_LENGTH [MaxLength]
FROM INFORMATION_SCHEMA.TABLES t
JOIN INFORMATION_SCHEMA.COLUMNS c on t.TABLE_NAME = c.TABLE_NAME
WHERE TABLE_TYPE = 'BASE TABLE' AND t.TABLE_CATALOG='Alkapidacom' AND t.TABLE_NAME = @TableName 
ORDER BY c.ORDINAL_POSITION

            ";

            return DapperUtil.Query<Property>(sql, new { TableName = TableName }).ToList();
        }
    }
}
