using BaseProject.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.DecoratorDesignPattern.Repositories;
using WebApp.DecoratorDesignPattern.Repositories.Decorator;

namespace BaseProject
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
            services.AddMemoryCache();
            services.AddScoped<IProductRepository>(sp =>
            {
                var context = sp.GetRequiredService<AppIdentityDbContext>();
                var memoryCache = sp.GetRequiredService<IMemoryCache>();
                var productRepository = new ProductRepository(context);
                var logService = sp.GetRequiredService<ILogger<ProductRepositoryLoggingDecorator>>();
                var cacheDecorator = new ProductRepositoryCacheDecorator(productRepository, memoryCache);
                var logDecorator = new ProductRepositoryLoggingDecorator(cacheDecorator, logService); //cacheDecorator almam�z�n sebebi ise IProductRepository'i implemente eden ProductRepository
                                                                                                      //istiyor. Burada isterleri kar��layan s�n�ftan birisi de ProductRepositoryCacheDecorator s�n�f�
                                                                                                      //olmaktad�r. E�er cacheDecorator'� al�rsak do�ru bir cevap vermi� oluruz.
                                                                                                      //ProductRepositoryCacheDecorator s�n�f� BaseProductRepositoryDecorator'� miras almaktad�r.
                                                                                                      //bu s�n�f ise IProductRepository'i miras al�yor.
                                                                                                      //>>e�er 3.bir decorator olursa bu sefer cacheDecorator yaz�lan yere logDecorator'� ge�memiz
                                                                                                      //gerekmektedir.
                return logDecorator;
            });
            services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SqlServer"));
            });

            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<AppIdentityDbContext>();

            services.AddControllersWithViews();
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

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
