using Autofac;
using Autofac.Extras.DynamicProxy;
using Business.Abstract;
using Business.Concrete;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors.Autofac;
using Core.Utilities.IoC;
using Core.Utilities.Security.Authentication.JWT;
using Core.Utilities.Security.Authentication.Utils;
using DataAccess.Abstract;
using DataAccess.Concrete.Dapper;
using DataAccess.Concrete.EF;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DependencyResolvers.Autofac
{
    public class BusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            // Code generator
            // [CHANGE]        
            builder.RegisterType<ProductManager>().As<IProductService>();
            builder.RegisterType<DpProductDal>().As<IProductDal>();

            builder.RegisterType<EfProductManager>().As<IEfProductService>();
            builder.RegisterType<EfProductDal>().As<IEfProductDal>();




            // Authentication TOKEN
            builder.RegisterType<AuthManager>().As<IAuthService>();
            builder.RegisterType<JWTHelper>().As<ITokenHelper>();

            builder.RegisterType<UserManager>().As<IUserService>();
            builder.RegisterType<DpUserDal>().As<IUserDal>();

            // Mevcut assembly' ye ula�.
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            // builder.RegisterAssemblyTypes(assembly) >>>>> bu assembly deki t�m tipleri kaydet.
            // ProxyGenerationOptions() >>> araya girme
            // Selector >>> araya girecek olan nesne
            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                {
                    Selector = new AspectInterceptorSelector()
                }).SingleInstance();
        }
    }
}
