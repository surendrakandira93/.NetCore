using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetCore.Services;

namespace NetCore
{
    public class Startup
    {
        private IConfiguration configuration;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder().SetBasePath(env.ContentRootPath)
                   .AddJsonFile("appsettings.json");
            configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddSingleton(provider => configuration);
            services.AddSingleton<IGreeter, Greeter>();
            services.AddSingleton<IStudentRepository, StudentRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            IGreeter greeter)
        {
            loggerFactory.AddConsole();

            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseMvc(ConfigRoutes);

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync(greeter.GetGreeter());
            //});
        }

        private void ConfigRoutes(IRouteBuilder route)
        {
            route.MapRoute(name: "Default", template: "{controller=Home}/{action=Index}/{id?}");
        }
    }
}