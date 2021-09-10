using Auth.Demo;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserCrudApiChallenge.Application.WebApi.Modules.Injection;
using UserCrudApiChallenge.WebApi.Modules.Feature;
using UserCrudApiChallenge.WebApi.Modules.Mapper;
using UserCrudApiChallenge.WebApi.Modules.Swagger;

namespace UserCrudApi
{
    public class Startup
    {
        readonly string myPolicy = "Todos";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
  //          services.AddDataProtection()
  //.PersistKeysToAWSSystemsManager("/MyApplication/DataProtection");
            var tokenKey = Configuration.GetValue<string>("TokenKey");
            var key = Encoding.ASCII.GetBytes(tokenKey);
            services.AddSwagger();
            services.AddControllers();
            services.AddMapper();
            services.AddFeature(this.Configuration);
            services.AddInjection(this.Configuration);
            //services.AddAuthentication(this.Configuration);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
          .AddJwtBearer(x =>
          {
              x.RequireHttpsMetadata = false;
              x.SaveToken = true;
              x.TokenValidationParameters = new TokenValidationParameters
              {
                  ValidateIssuerSigningKey = true,
                  IssuerSigningKey = new SymmetricSecurityKey(key),
                  ValidateIssuer = false,
                  ValidateAudience = false,
                  ValidateLifetime = true,
                  //ClockSkew = TimeSpan.Zero, //Quitamos los 5 min x defecto
                  ClockSkew = TimeSpan.FromSeconds(100000)
              };
          });

            services.AddSingleton<ITokenRefresher>(x =>
    new TokenRefresher(key, x.GetService<IJWTAuthenticationManager>()));
            services.AddSingleton<IRefreshTokenGenerator, RefreshTokenGenerator>();
            services.AddSingleton<IJWTAuthenticationManager>(x =>
                new JWTAuthenticationManager(tokenKey, x.GetService<IRefreshTokenGenerator>()));
            //services.AddDataProtection()
            //.PersistKeysToAWSSystemsManager("/RegisterTest");
   //         services.AddDataProtection()
   //.PersistKeysToAWSSystemsManager($"/MyApp/DataProtection");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseSwagger(c =>
            {
                c.RouteTemplate = "/swagger/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/V1/swagger.json", "API UserCRUD Web V1");

            });

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(myPolicy);
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


        }
    }
}
