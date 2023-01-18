using Core.Extensions;
using Core.Utilities.IoC;
using Core.Utilities.Messages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace Core.Aspects
{


    public class ControllerAuthorize : Attribute, IActionFilter
    {
        private List<string> _claims;
        private IHttpContextAccessor _httpContextAccessor;
        public ControllerAuthorize(string claims)
        {
            _claims = claims.Split(",").ToList();
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            List<string> permissions = _httpContextAccessor.HttpContext.User.ClaimRoles();
            // Admin ya da ...Manager yetkisi varsa yetkilendirillsin. (... Dinamik oluşuyor ProductManager, CategoryManager vs)
            if (permissions.Contains("Admin"))
            {
                return;
            }
            

            foreach (string role in _claims)
            {
                if (permissions.Contains(role))
                {
                    return;
                }
            }


            context.Result = new RedirectToRouteResult(
                 new RouteValueDictionary
                 {
                    { "controller", "auth" },
                    { "action", "login" }
                 });


        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }



        private string CreateLogDetail(IHttpContextAccessor context, string exceptionMessage)
        {
            //var logParameters = invocation.Arguments.Select(x =>
            //new LogParameter
            //{
            //    Value = x,
            //    Type = x.GetType().Name,

            //}).ToList();
            //return Newtonsoft.Json.JsonConvert.SerializeObject(new LogDetail
            //{
            //    MethodName = invocation.Method.Name,
            //    LogParameters = logParameters,
            //    ExceptionMessage = exceptionMessage,
            //    FullName = invocation.TargetType.FullName
            //});
            return "";
        }


    }
}
