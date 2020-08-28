using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IoT.Agent
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            string assemPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Environment.CurrentDirectory = assemPath;

            var wwwRootPath = Path.Combine(assemPath,"wwwroot");
            return Host.CreateDefaultBuilder(args)
                //.UseContentRoot(assemPath)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .UseWebRoot(wwwRootPath)
                    .UseUrls("http://*:5001")
                   .UseStartup<Startup>();
                });
        }
    }
}
