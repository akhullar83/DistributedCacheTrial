using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace DistributedCacheTrial
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            ConfigurationOptions config = new ConfigurationOptions
            {
                EndPoints =
                {
                    { "35.232.241.220", 6379 },
                },
                
                KeepAlive = 180,
            };

            services.AddDistributedRedisCache(o =>
            {
                o.ConfigurationOptions = config;
            });

            services.AddSession(options => {
                options.CookieName = "Test.Session";
                options.IdleTimeout = TimeSpan.FromSeconds(3600);
            });

            services.AddMvc();



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseSession();
            app.UseMvc();
            //app.Run(context => {
            //    return context.Response.Headers.Response.WriteAsync("Hello Readers!");
            //});
        }
    }
}
