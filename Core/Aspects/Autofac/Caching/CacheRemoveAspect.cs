using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Caching;
using Core.Utilities.Interceptors.Autofac;
using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Aspects.Autofac.Caching
{
    public class CacheRemoveAspect : MethodInterception
    {
        private string _pattern;
        private ICacheService _cacheService;
        private bool _isBaseEntityManager= false;
        public CacheRemoveAspect(string pattern)
        {
            _pattern = pattern;
            _cacheService = ServiceTool.ServiceProvider.GetService<ICacheService>();
        }

        public CacheRemoveAspect()
        {
            _cacheService = ServiceTool.ServiceProvider.GetService<ICacheService>();
        }
        public CacheRemoveAspect(bool isBaseEntityManager)
        {
            _cacheService = ServiceTool.ServiceProvider.GetService<ICacheService>();
            _isBaseEntityManager = isBaseEntityManager;
        }

        protected override void OnSuccess(IInvocation invocation)
        {
            if (_isBaseEntityManager)
            {
                _pattern= invocation.TargetType.Name.Replace("Manager", "");
            }

            if (String.IsNullOrEmpty(_pattern))
            {
                _cacheService.RemoveByPattern(_pattern);
            }
            else
            {
                _cacheService.Remove(_pattern);
            }
           
        }
    }
}
