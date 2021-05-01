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
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Net.Http.Headers;

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

            services.AddControllers();




            services.AddDbContext<TRContext>(options => options.UseMySQL(Configuration["ConnectionStrings:DBConnectionString"]));


             services.AddAuthentication("Basic").AddScheme<AuthenticationSchemeOptions, CustomAuthenticationHandler>("Basic", null);
       
     
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                options.AddPolicy("All", policy => policy.RequireRole("Admin", "User"));
            });

            services.AddCors(options =>
            {

                options.AddPolicy("Policy",
                    builder =>
                    {
                        //"Access-Control-Allow-Origin: http://localhost:3000/"
                        builder
                        .WithHeaders("Access-Control-Allow-Headers:*", 
                        "Content-Type", "Content-Range", "Content-Disposition", "Content-Description")
                        .AllowAnyMethod()
                        .WithOrigins("http://localhost:3000/");


                    });

            });
            services.AddSingleton<CustomAuthenticationManager>();
            services.AddSingleton<string>(Configuration["ConnectionStrings:BearerToken"]);
            //allow connection between origins

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var officialToken = Configuration["ConnectionStrings:BearerToken"];
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("Policy");


            app.UseAuthorization();
            app.UseAuthentication();


            //            // global cors policy
            //            app.UseCors(builder =>
            //            {builder
            //   .AllowAnyHeader()
            //.WithOrigins("http://localhost:3000/")
            //   .AllowAnyMethod();
            //            });


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireCors("Policy");
            });
        }
    }
}
