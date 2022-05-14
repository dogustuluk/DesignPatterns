using BaseProject.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.StrategyDesignPattern.Models;
using WebApp.StrategyDesignPattern.Repositories;

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
            services.AddHttpContextAccessor(); //bu servis üzerinden herhangi bir class'ýn constructor'ýna veya bir service provider üzerinden http.context'e eriþebiliriz.

            services.AddScoped<IProductRepository>(serviceProvider =>
            {
                var context = serviceProvider.GetRequiredService<AppIdentityDbContext>();
                var httpContextAccesor = serviceProvider.GetRequiredService<IHttpContextAccessor>(); //artýk bu servis üzerinden httpContex'e eriþebiliriz.

                var claim = httpContextAccesor.HttpContext.User.Claims.Where(x => x.Type == Settings.claimDatabaseType).FirstOrDefault(); //claim olup olmadýðýný kontrol etmek için
                if (claim == null) return new ProductRepositoryFromSqlServer(context);

                var databaseType = (EDatabaseType)int.Parse(claim.Value); //null deðilse çalýþýr.

                return databaseType switch
                {
                    EDatabaseType.SqlServer => new ProductRepositoryFromSqlServer(context),
                    EDatabaseType.MongoDb => new ProductRepositoryFromMongoDb(Configuration),
                    _ => throw new NotImplementedException()
                };
                

                
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
