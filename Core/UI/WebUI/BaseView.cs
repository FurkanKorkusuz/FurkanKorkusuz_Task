using Core.Extensions;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.UI.WebUI
{
    public class BaseView
    {
        public static string NavBar()
        {
            IHttpContextAccessor httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
			StringBuilder
				taskMenuItems = new StringBuilder(),
                adimnMenuItems = new StringBuilder()
            ;

            // Tasks
            taskMenuItems.Append(@"<li><a class=""dropdown-item"" href=""/task/list""><i class=""fas fa-grip-horizontal""></i>&nbsp;Görevler</a></li>");
            taskMenuItems.Append(@"<li><a class=""dropdown-item"" href=""/progress-status/list""><i class=""fas fa-question-circle""></i>&nbsp;Durumlar</a></li>");


            string  bar = $@"
    <nav class=""navbar navbar-dark bg-dark text-white navbar-expand-lg"">
      <div class=""container"">
        <a class=""navbar-brand"" href=""#"">Alkapida</a>
        <button class=""navbar-toggler"" type=""button"" data-bs-toggle=""collapse"" data-bs-target=""#navbarSupportedContent"" aria-controls=""navbarSupportedContent"" aria-expanded=""false"" aria-label=""Toggle navigation"">
          <span class=""navbar-toggler-icon""></span>
        </button>

        <div class=""collapse navbar-collapse"" id=""navbarSupportedContent"">
          <ul class=""navbar-nav me-auto mb-2 mb-lg-0"">
            
             <li class=""nav-item dropdown{(adimnMenuItems.Length == 0 ? " d-none" : "")}"">
                <a class=""nav-link dropdown-toggle"" href=""#"" role=""button"" data-bs-toggle=""dropdown"" aria-expanded=""false"">
                Admin
              </a>
              <ul class=""dropdown-menu"">{adimnMenuItems}</ul>
            </li>

         <li class=""nav-item dropdown{(taskMenuItems.Length == 0 ? " d-none" : "")}"">
                <a class=""nav-link dropdown-toggle"" href=""#"" role=""button"" data-bs-toggle=""dropdown"" aria-expanded=""false"">
               <i class=""fas fa-tasks""></i> Görevler
              </a>
              <ul class=""dropdown-menu"">{taskMenuItems}</ul>
            </li>
           
	        <li class=""nav-item dropdown"" style=""float: right;"">
				<a class=""nav-link dropdown-toggle"" href=""#"" role=""button"" data-toggle=""dropdown"" aria-haspopup=""true"" aria-expanded=""false"">
                    <i class=""fa fa-fw fa-file""></i>{httpContextAccessor.HttpContext.User.Identity.Name}
                </a>
                <ul class=""dropdown-menu"">
                    <li><a class=""dropdown-item"" href=""/auth/logout""><i class=""fas fa-user""></i>&nbsp;Çıkış <i class=""fas fa-sign-out-alt""></i></a></li>
                </ul>
			</li>  
       

        </div>
      </div>
    </nav>

";
            return bar;
        }
    }
}
