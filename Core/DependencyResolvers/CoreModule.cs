using Core.CrossCuttingConcerns.Caching;
using Core.CrossCuttingConcerns.Caching.MemoryChache;
using Core.DataAccess.Abstract;
using Core.DataAccess.Connections;
using Core.DataAccess.Dapper;
using Core.DataAccess.EF;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using static Slapper.AutoMapper;

namespace Core.DependencyResolvers
{
    /// <summary>
    /// Artık Startup kısmında servise ekleyeceğim modulleri buradan merkezi olarak ekleyebilirim.
    /// services.AddMemoryCache(); yazdığımda services den sonraki referanslara ulaşamadım bu yüzden CoreModule kullanımından şimdilik vazgeçtim.
    /// services.AddMemoryCache(); kodumu Startup a yazacağım.
    /// </summary>
    public class CoreModule : ICoreModule
    {
        public void Load(IServiceCollection services)
        {
            services.AddMemoryCache();

            // dependency resolver...
            services.AddSingleton<ICacheService, MemoryCacheManager>();


            //  User.ClaimRoles() ClaimRoles u extent etmiştik. User nesnesi Claimsten gelir ancak sadece MVC de geliyordu.
            // Core ve Business projelerine Aspnetcore.Http paketini yükledik ve HttpContextAccessor ü çözümledik.

            // HttpContext.User.---   erişebilmek için token ı göndermek gerek.
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<Stopwatch>();

            services.AddSingleton<ILogDal, DpLogDal>();


            // Entity Framework Context
            services.AddDbContext<Furkan_TaskDBContext>(options =>
                    options.UseSqlServer(SqlConnectionTools.ConnectionString("furkan", "f4kW5DGreVw5G2b")));
        }
    }
}
