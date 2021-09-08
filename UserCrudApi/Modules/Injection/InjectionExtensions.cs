//using Kapsch.Sor.Application.Interface;
//using Kapsch.Sor.Application.Main;
//using Kapsch.Sor.CrossCutting.AD;
//using Kapsch.Sor.Domain.Core;
//using Kapsch.Sor.Domain.Interface;
//using Kapsch.Sor.Infrastructure.Data;
//using Kapsch.Sor.Infrastructure.Interface;
//using Kapsch.Sor.Infrastructure.Repository;
//using Kapsch.Sor.CrossCutting.Common;
//using Kapsch.Sor.CrossCutting.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
//using Kapsch.Sor.Services.WebApi.Helpers;
using UserCrudApiChallenge.Application.Interface;
using UserCrudApiChallenge.Application.Main;
using UserCrudApiChallenge.Domain.Core;
using UserCrudApiChallenge.Domain.Interface;
using UserCrudApiChallenge.Infraestructure.Interface;
using UserCrudApiChallenge.Infraestructure.Repository;

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


            //services.AddSingleton<IConnectionFactory, ConnectionFactory>();
            //services.AddScoped<IManagerEncryptDecrypt, ManagerEncryptDecrypt>();

            //services.AddScoped<ICommonApplication, CommonApplication>();
            //services.AddScoped<ICommonDomain, CommonDomain>();
            //services.AddScoped<ICommonRepository, CommonRepository>();

            //services.AddScoped<IAliasApplication, AliasApplication>();
            //services.AddScoped<IAliasDomain, AliasDomain>();
            //services.AddScoped<IAliasRepository, AliasRepository>();

            //services.AddScoped<IGeneralParametersApplication, GeneralParametersApplication>();
            //services.AddScoped<IGeneralParametersDomain, GeneralParametersDomain>();
            //services.AddScoped<IGeneralParametersRepository, GeneralParametersRepository>();

            //services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

            //services.AddScoped<IFileManagerApplication, FileManagerApplication>();
            //services.AddScoped<IFileManagerDomain, FileManagerDomain>();
            //services.AddScoped<IFileManagerRepository, FileManagerRepository>();

            //services.AddScoped<IUserApplication, UsersApplication>();
            //services.AddScoped<IUsersDomain, UsersDomain>();
            //services.AddScoped<IUsersRepository, UsersRepository>();

            //services.AddScoped<IProfileApplication, ProfileApplication>();
            //services.AddScoped<IProfileDomain, ProfileDomain>();
            //services.AddScoped<IProfilesRepository, ProfilesRepository>();

            //services.AddScoped<IMenuApplication, MenuApplication>();
            //services.AddScoped<IMenuDomain, MenuDomain>();
            //services.AddScoped<IMenuRepository, MenuRepository>();

            //services.AddScoped<IConfigurationApplication, ConfigurationApplication>();
            //services.AddScoped<IConfigurationDomain, ConfigurationDomain>();
            //services.AddScoped<IConfigurationRepository, ConcessionairesRepository>();

            //services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
            //services.AddScoped<IUserProvider, AdUserProvider>(); // register an IUserProvider of type AdUserProvider with Dependency Injection with a Scoped lifecycle (per HTTP request):

            return services;
        }
    }
}
