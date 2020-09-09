using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using IoT.Agent.Services;
using IoT.EventBus;
using IoT.Shared.Entities;
using IoT.Shared.Models;
using System.IO;
using Microsoft.AspNetCore.SignalR;
using IoT.Agent.Hubs;
using IoT.ServiceHost.Gpio;

namespace IoT.Agent
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
            //var dbPath = EnsureDatabaseFolder();
            services.AddControllersWithViews();
            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddMvc();
            services.AddSingleton<IMapper>(new Mapper(GetMappingConfig()));
            services.AddEventBusService();
            services.AddLogging();
            services.AddSignalR();
            //services.AddHostedService<RabbitMqService>();
            services.AddHostedService<AdminService>();
            services.AddHostedService<RPiService>();
            services.AddSingleton<IDuplexSerialService,DuplexSerialService>();
            services.AddSingleton<IMetricsService,MetricsService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");


                endpoints.MapHub<PiStateHub>("/nodehub");
            });

        }

        private MapperConfiguration GetMappingConfig()
        {
            var config = new MapperConfiguration(cfg =>
            {
            });

            return config;
        }

        private string EnsureDatabaseFolder()
        {
            var execAssem = System.Reflection.Assembly.GetExecutingAssembly();
            var execAssemDir = Path.GetDirectoryName(execAssem.Location);
            var dataDir = Path.Combine(execAssemDir, "Data");

            if (!Directory.Exists(dataDir))
            {
                Directory.CreateDirectory(dataDir);
            }

            var dbPath = Path.Combine(dataDir, "Agent.db");

            return dbPath;
        }
    }
}
