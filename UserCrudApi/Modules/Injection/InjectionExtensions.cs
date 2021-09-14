
using Auth.Demo;
using LoggerService;
//using LoggerService;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
//using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using UserCrudApiChallenge.Application.Interface;
using UserCrudApiChallenge.Application.Main;
using UserCrudApiChallenge.Domain.Core;
using UserCrudApiChallenge.Domain.Interface;
using UserCrudApiChallenge.Infraestructure.Interface;
using UserCrudApiChallenge.Infraestructure.Repository;
using UserCrudChallenge.CrossCutting.Common;


namespace UserCrudApiChallenge.Application.WebApi.Modules.Injection
{
    public static class InjectionExtensions
    {
        public static IServiceCollection AddInjection(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddSingleton<IConfiguration>(configuration);
            services.AddScoped<IUserAplication, UserApplication>();
            services.AddScoped<IUserDomain, UserDomain>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILoggerManager, LoggerManager>();
            services.AddScoped<IManagerEncryptDecrypt, ManagerEncryptDecrypt>();
            

            return services;
        }
    }
}
