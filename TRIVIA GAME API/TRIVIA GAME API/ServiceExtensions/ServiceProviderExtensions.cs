using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Entities;
using Microsoft.EntityFrameworkCore;
using TRIVIA_GAME_API.Services;
using LoggerService;

namespace TRIVIA_GAME_API.ServiceExtensions
{
    public static class ServiceProviderExtensions
    {
        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        }

        public static void ConfigureDBContext(this IServiceCollection services, IConfiguration config)
        {
            string connection = config.GetConnectionString("DefaultConnection");
            services.AddDbContext<TriviaContext>(opts =>
                opts.UseSqlServer(connection));
        }
        

        public static void AddDataCreator(this IServiceCollection serivces)
        {
            serivces.AddTransient<IDataCreatorService, DataCreatorService>();
        }

        public static void AddServicesForModels(this IServiceCollection service)
        {
            service.AddScoped<IModelCreatorService, ModelCreatorService>();
            service.AddTransient<ICategoryService, CategoryService>();
            service.AddTransient<IPlayerService, PlayerService>();
            service.AddTransient<IQuestionService, QuestionService>();
        }

        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }


    }
}
