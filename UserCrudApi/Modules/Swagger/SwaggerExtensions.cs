using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;
using System.Linq;
using UserCrudApiChallenge.WebApi.Modules.Swagger;
using Microsoft.OpenApi.Models;

namespace UserCrudApiChallenge.WebApi.Modules.Swagger
{
    public static class SwaggerExtensions
    {
        public class MyBaseClass
        {
        }
        public class MyDerivedClass : MyBaseClass
        {
        }
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("V1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API DOCUMENTATION",
                    Description = "Especificaciones ",
                    Contact = new OpenApiContact
                    {
                        Name = "Camilo Ortuzar",
                        Email = "camilo.ortuzar@outlook.com",
                        Url = new Uri("https://github.com/sektor1987/USerCrudChallenge")

                    },
                    License = new OpenApiLicense
                    {
                        Name = "Uso exclusivo",
                        Url = new Uri("https://github.com/sektor1987/USerCrudChallenge")
                    }
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Authorization por API Key",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Name = "Authorization"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement   {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id= "Bearer"
                            }
                        },
                        new  string []{}
                    }
                });

            });
            return services;
        }
    }
}
