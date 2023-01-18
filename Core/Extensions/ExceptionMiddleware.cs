using Core.DataAccess.Abstract;
using Core.Entities.Concrete;
using Core.Utilities.IoC;
using Core.Utilities.Messages;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Security.Authentication;
using Task = System.Threading.Tasks.Task;

namespace Core.Extensions
{
    public class ExceptionMiddleware
    {
        /// <summary>
        /// Startup konfigrasyonundaki sırasıyla çalışan işlemler. ( app.UseAuthentication)
        /// </summary>
        private RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Eğer hata oluşursa yakalasın ve benim yazdıgım metoda göndersin.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(context, e);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception e)
        {

    
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

            string message = "Bir hata oluştu.";

            // Validasyondaki hataları kullanıcıya göstermek için tuttum.
            if (e.GetType() == typeof(ValidationException)) 
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                return context.Response.WriteAsync(new ErrorDetails
                {
                    Message = e.Message
                }.ToString());
            }
            if (e.GetType() == typeof(AuthenticationException))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                AddLog(e);
                return context.Response.WriteAsync(new ErrorDetails
                {
                    Message = AuthenticationMessage.UnAuthorize
                }.ToString());
            }

            AddLog(e);
            return context.Response.WriteAsync(new ErrorDetails
            {
                Message = message
            }.ToString()) ;
        }

        private static void AddLog(Exception e)
        {

            Log log = new Log
            {
                Date = DateTime.Now,
                Detail = $@"{e.Message}",
                Audit = (byte)LogQualification.Error,

            };

            ILogDal logDal = ServiceTool.ServiceProvider.GetService<ILogDal>();
            logDal.Create(log);
        }
 
        
    }
}
