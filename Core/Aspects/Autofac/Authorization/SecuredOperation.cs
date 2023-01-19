using Castle.DynamicProxy;
using Core.Extensions;
using Core.Utilities.Interceptors.Autofac;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace Core.Aspects.Autofac.Authorization
{


    public class SecuredOperation : MethodInterception
    {
        private List<string> _roles;
        private bool _isBaseEntityManager = false;
        private IHttpContextAccessor _httpContextAccessor;
        public SecuredOperation(string roles)
        {
            _roles = roles.Split(',').ToList();
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
            base.Priority = -1; // first run attribute. 

        }

        /// <summary>
        /// IF no parameter is BaseEntityManager
        /// 
        /// [SecuredOperation()]
        /// Example ProductManager.GetByID(...)
        /// {
        ///     ... 
        /// }
        /// 
        /// _roles = {"Admin","Product.Manager","Product.Get"}
        /// 
        /// </summary>
        public SecuredOperation()
        {
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
            _isBaseEntityManager = true;
            _roles = new List<string>();
            base.Priority = -1; // first run attribute. 
        }


        protected override void OnBefore(IInvocation invocation)
        {
            var roleClaims = _httpContextAccessor.HttpContext.User.ClaimRoles();
            if (
                  roleClaims.Contains("Admin") || // Admin yetkisi
                  roleClaims.Contains(invocation.TargetType.Name.Replace("Manager", ".Manager")) // [Entity]Manager perm
                ) 
            {
                return;
            }
            return;

            if (_isBaseEntityManager)
            {
                string type = invocation.TargetType.Name.Replace("Manager", "");
                switch (invocation.Method.Name)
                {
                    case string a when a.StartsWith("Update"):
                        _roles.Add(type + ".Update");
                        break;
                    case string a when a.StartsWith("Add"):
                        _roles.Add(type + ".Add");
                        break;
                    case string a when a.StartsWith("Delete"):
                        _roles.Add(type + ".Delete");
                        break;
                    case string a when a.StartsWith("Get"):
                        _roles.Add(type + ".Get");
                        break;
                }

            }




            foreach (var role in _roles)
            {
                if (roleClaims.Contains(role))
                {
                    return;
                }
            }
            throw new AuthenticationException(Utilities.Messages.AuthenticationMessage.UnAuthorize);
        }
    }
}
