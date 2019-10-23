using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApplication.Services;

namespace WebApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var redisConnectionString = Configuration.GetConnectionString("redis");
            if (!string.IsNullOrEmpty(redisConnectionString)) {
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = redisConnectionString;
                    options.InstanceName = "WebCache";
                });
            }

            services.AddSingleton<IHealthService, HealthService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IHealthService healthService)
        {
            app.Use(async (ctx, next) => {
                if (ctx.Request.Path == "/health") {
                    if (healthService.IsHealthy) {
                        healthService.AddPing(true);
                        ctx.Response.StatusCode = 200;
                        await ctx.Response.WriteAsync("Healthy");
                        return;
                    }

                    healthService.AddPing(false);
                    ctx.Response.StatusCode = 500;
                    return;
                }
                await next();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
