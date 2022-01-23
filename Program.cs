using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using RoyaleTrackerAPI.Models;

namespace RoyaleTrackerAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {


            var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

            try
            {
                logger.Debug("init main");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception exception)
            {
                //NLog: catch setup errors
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }



            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                TRContext db = scope.ServiceProvider.GetRequiredService<TRContext>();
                db.Database.EnsureCreated();

                Client client = scope.ServiceProvider.GetRequiredService<Client>();
                AutoUpdater updater = new AutoUpdater(client);

                //add new thread here woooooh!
                ThreadStart childref = new ThreadStart(updater.Update);
                Console.WriteLine("In Main: Creating the Child thread");
                Thread childThread = new Thread(childref);
                childThread.Start();
            }

            host.Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>().UseUrls(new[] { "http://localhost:52003" }); // now the Kestrel server will listen on port 5001!
				}).ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                })
        .UseNLog();
    }
}
