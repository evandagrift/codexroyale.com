using System.Threading;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using NLog.Web;
using RoyaleTrackerAPI.Models;
using Microsoft.Extensions.Logging;

namespace RoyaleTrackerAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                TRContext db = scope.ServiceProvider.GetRequiredService<TRContext>();
                db.Database.EnsureCreated();

                Client client = scope.ServiceProvider.GetRequiredService<Client>();
                AutoUpdater updater = new AutoUpdater(client);

                //add new thread here woooooh!
                ThreadStart childref = new ThreadStart(updater.Update);

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
