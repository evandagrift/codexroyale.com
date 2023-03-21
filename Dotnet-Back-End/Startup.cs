using CodexRoyaleClassesCore3;
using CodexRoyaleClassesCore3.Models;
using CodexRoyaleClassesCore3.Models.Email;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace RoyaleTrackerAPI
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
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                options.KnownProxies.Add(IPAddress.Parse("127.0.10.1"));
            });


            services.AddControllers();

            //services.AddCors(options =>
            //{
            //    options.AddPolicy("hosted",
            //                      builder =>
            //                      {
            //                          builder.WithOrigins("http://localhost:3000")
            //                          .AllowAnyHeader().AllowAnyMethod().AllowCredentials();
            //                      });

            //});
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("local",
            //                      builder =>
            //                      {
            //                          builder.WithOrigins("http://localhost:3000")
            //                          .AllowAnyHeader().AllowAnyMethod().AllowCredentials();
            //                      });

            //});

            services.AddDbContext<TRContext>(options => options.UseMySQL(Configuration["ConnectionStrings:DBConnectionString"]), ServiceLifetime.Transient);



            //services.AddDbContext<TRContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:DBConnectionString"]), ServiceLifetime.Transient);

            services.AddAuthorizationCore(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                options.AddPolicy("All", policy => policy.RequireRole("Admin", "User"));
            });

            services.AddSingleton<CustomAuthenticationManager>();


            var emailConfig = Configuration
    .GetSection("EmailConfiguration")
    .Get<AuthMessageSenderOptions>();



            services.AddSingleton<AuthMessageSenderOptions>(emailConfig);
            services.AddSingleton<EmailSender>();
            services.AddSingleton<Client>(new Client(Configuration["ConnectionStrings:BearerToken"]));
            //allow connection between origins

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders();

            app.UseRouting();
            //app.UseCors("hosted");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}