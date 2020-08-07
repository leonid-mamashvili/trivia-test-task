using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TRIVIA_GAME_API.Hubs;
using TRIVIA_GAME_API.ServiceExtensions;
using TRIVIA_GAME_API.Services;

namespace TRIVIA_GAME_API
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

            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.ConfigureDBContext(Configuration);

            services.ConfigureRepositoryWrapper();

            services.AddServicesForModels();

            services.AddDataCreator();

            services.AddCors();

            services.AddSignalR();

            services.ConfigureLoggerService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            IWebHostEnvironment env, 
            IDataCreatorService dataCreator,
            ILoggerManager logger)
        {

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<TriviaContext>();
                var isJustCreated = context.Database.EnsureCreated();
                if (isJustCreated)
                {
                    dataCreator.CreateTestData();
                }
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.ConfigureCustomExceptionMiddleware();

            var frontEndUrl = Configuration.GetValue<string>("UrlFrontEnd");
            app.UseCors(
                options => options.WithOrigins(frontEndUrl).AllowAnyMethod()
                );

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<TriviaHub>("/send");
                endpoints.MapHub<TriviaHub>("/join");
                endpoints.MapHub<TriviaHub>("/leave");
            });

           

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
