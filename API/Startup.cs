using Core.DataAccess.EF;
using Core.DependencyResolvers;
using Core.Entities.Concrete;
using Core.Extensions;
using Core.Utilities.IoC;
using Core.Utilities.Security.Authentication.Utils;
using Core.Utilities.Security.Encryption;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();

            // Eklemem gereken servisleri bu þekilde ekleyebilirim ancak kendi servis Tool umu yazarak burayý deðiþtirmeden Core katmanýnda müdahele edebilirim. Ýleride yani proje oluþturursam servislerim core dan ekli gelir.
            // services.AddMemoryCache();

            // Yukarýdaki eklemeyi yapmak yerine Core dan eklediðim servisleri burada çalýþtýrmak için
            services.AddDependencyResolvers(new ICoreModule[]
            {
                new CoreModule(),
            });



            // Cors webApý ye eriþimlerin vc kontrol edildði bir yer.
            // Örneðin bu site alkapida.com domaini ile yayýnlanacaksa bu web apý nin orjinal kullanýcýsý (ya da admin gibi birþey) bu domain olacaktýr. O zaman builder.WithOrigins("https://alkapida.com")  yazýlmalýdýr.
            services.AddCors(options =>
            {
                options.AddPolicy(name: "MyPolicy",
                                 builder =>
                                 {
                                     //builder.WithOrigins("https://localhost:3000"
                                     //                    ,
                                     //                    "https://localhost:44354"
                                     //                    //"http://localhost:44354/Auth/Login"
                                     //                    );
                                     builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();

                                 });
                options.AddPolicy(name: "AllowOrigin",
                                builder => builder.WithOrigins("https://localhost:3000"));


            });



            // appsettings deki tokenOptions u oku
            var tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // Issuer bilgisi token dan alýnsýn mý
                    ValidateIssuer = true,

                    // Audience  bilgisi token dan alýnsýn mý
                    ValidateAudience = true,

                    // token ýn yaþam ömrünü kontrol etsin mi (yoksa token hep gecerli olur.)
                    ValidateLifetime = true,

                    ValidIssuer = tokenOptions.Issuer,
                    ValidAudience = tokenOptions.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey),
                };
            });


            // JSON stringlerimde nesnelerimin adýný camelCase yapýyordu ancak ben default olarak yazýldýðý gibi gelmesini istedim. 
            services.AddMvc().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

            // Swagger çalýþtýrmak için.
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPI", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "admin yetkili örnek token aþaðýdadýr. \r\n\n\n Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjQiLCJlbWFpbCI6ImFkbWluQHRlc3QuY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6ImFkbWluIiwibmJmIjoxNjc0MTQ5ODk0LCJleHAiOjE3MDU2ODU4OTQsImlzcyI6ImZ1cmthbmtvcmt1c3V6LmNvbSIsImF1ZCI6ImZ1cmthbmtvcmt1c3V6LmNvbSJ9.72JFpwKTUJiD4mfoXi0nFagggaWxKU0T8sWt6trKJ2A",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                    }
                });
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI v1"));

            //Exception için yazdýðýmýz middleware
            app.ConfigureCustomExceptionMiddleware();

            // Yukarýda Cors ekledik burada çaðýrmamýz lazým (burada sýra önemli.)
            // Buradaki builder http://localhost:3000 sitesinden gelen her türlü (get,post,put,delete) istege cevap ver demektir. 


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(builder => builder.WithOrigins("https://localhost:3000").AllowAnyHeader());

            // Sonradan eklendi. 
            app.UseAuthentication(); // API ye kimler eriþebilir.

            app.UseAuthorization(); // Apý deki hangi mmetodlara kimler  eriþebilir



            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers()
                .RequireCors("MyPolicy");
            });
        }
    }
}
