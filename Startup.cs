using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RoyaleTrackerAPI.Models;
using Microsoft.Net.Http.Headers;
using RoyaleTrackerAPI.Models.Authentication;
using RoyaleTrackerAPI.Models.Email;
using RoyaleTrackerAPI.Controllers;

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

            services.AddCors(options =>
            {
                options.AddPolicy(name: "hosted",
                                  builder =>
                                  {
                                      builder.WithOrigins("https://codexroyale.com")
                                      .AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                                  });
            });


            services.AddControllers().AddNewtonsoftJson();

            services.AddDbContext<TRContext>(options => options.UseMySQL(Configuration["ConnectionStrings:DBConnectionString"]), ServiceLifetime.Transient);


            services.AddAuthentication("Basic").AddScheme<AuthenticationSchemeOptions, CustomAuthenticationHandler>("Basic", null);



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

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCors("hosted");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
