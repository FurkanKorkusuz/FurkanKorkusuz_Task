
using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Interceptors.Autofac
{
    /// <summary>
    /// AttributeTargets.Class Class ların başına yazılıp bu Classdaki tüm metotlarda kullanılabilir.
    /// AttributeTargets.Method Method ların başına yazılıp o metotda kullanılabilir.
    /// AllowMultiple Birden fazla kullanılabilsin.
    /// Inherited Miras alınan yerde kullanılabilsin.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method,AllowMultiple =true,Inherited =true)]
    public abstract class MethodInterceptionBaseAttribute : Attribute, IInterceptor
    {
        public int Priority { get; set; }

        public virtual void Intercept(IInvocation invocation)
        {
           
        }
    }
}
