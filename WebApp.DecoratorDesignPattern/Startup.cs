using BaseProject.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
            services.AddHttpContextAccessor();

            //2.yol
            ////scrutor uzantýsý ile yapýlan kýsa yol çözüm.
            //services.AddScoped<IProductRepository, ProductRepository>().Decorate<IProductRepository, ProductRepositoryCacheDecorator>().Decorate<IProductRepository, ProductRepositoryLoggingDecorator>();
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>


            //altta 1.yol vardýr. Üst tarafta ise scrutor adlý uzantý ile tüm bu kod satýrlarýný oldukça kýsa bir hale getiriyor olucaz.
            //services.AddScoped<IProductRepository>(sp =>
            //{
            //    var context = sp.GetRequiredService<AppIdentityDbContext>();
            //    var memoryCache = sp.GetRequiredService<IMemoryCache>();
            //    var productRepository = new ProductRepository(context);
            //    var logService = sp.GetRequiredService<ILogger<ProductRepositoryLoggingDecorator>>();
            //    var cacheDecorator = new ProductRepositoryCacheDecorator(productRepository, memoryCache);
            //    var logDecorator = new ProductRepositoryLoggingDecorator(cacheDecorator, logService); //cacheDecorator almamýzýn sebebi ise IProductRepository'i implemente eden ProductRepository
            //                                                                                          //istiyor. Burada isterleri karþýlayan sýnýftan birisi de ProductRepositoryCacheDecorator sýnýfý
            //                                                                                          //olmaktadýr. Eðer cacheDecorator'ý alýrsak doðru bir cevap vermiþ oluruz.
            //                                                                                          //ProductRepositoryCacheDecorator sýnýfý BaseProductRepositoryDecorator'ý miras almaktadýr.
            //                                                                                          //bu sýnýf ise IProductRepository'i miras alýyor.
            //                                                                                          //>>eðer 3.bir decorator olursa bu sefer cacheDecorator yazýlan yere logDecorator'ý geçmemiz
            //                                                                                          //gerekmektedir.
            //    return logDecorator;
            //});

            //3.yol >>>>>>>>>>>>>>> runtime esnasýnda deðiþiklik yapma
            services.AddScoped<IProductRepository>(sp =>
            { //öncelikle giriþ yapmýþ olan kullanýcýyý bulmamýz gerekmektedir. Bunu ise Http Accessor üzerinden elde edebiliriz. Servis olarak üste ekleyelim. Daha sonra bunu burada elde edelim.
                var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();

                var context = sp.GetRequiredService<AppIdentityDbContext>();
                var memoryCache = sp.GetRequiredService<IMemoryCache>();
                var productRepository = new ProductRepository(context);
                var logService = sp.GetRequiredService<ILogger<ProductRepositoryLoggingDecorator>>();

                if (httpContextAccessor.HttpContext.User.Identity.Name == "user1")
                {
                    var cacheDecorator = new ProductRepositoryCacheDecorator(productRepository, memoryCache);
                    return cacheDecorator;
                }

                var logDecorator = new ProductRepositoryLoggingDecorator(productRepository, logService);
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
