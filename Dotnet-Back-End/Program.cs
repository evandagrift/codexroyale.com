using System.Threading;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
namespace RoyaleTrackerAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}


//.ConfigureLogging(logging =>
// {
//     logging.ClearProviders();
//     logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
// })
//        .UseNLog();