using BaseProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.CommandDesignPattern.Models;

namespace BaseProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            //scope almamýzýn sebebi >>> scope ile beraber AppIdentityDbContext'ten bir nesne örneði alýyoruz ve iþimiz bittiði zaman memory'de bu nesneler tutulmasýn
            //istiyoruz.
            using var scope = host.Services.CreateScope();

            var identityDbContext = scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();
            //ServiceProvider'ýn özelliði >> startup.cs tarafýnda ConfigureServices tarafýnda eklediðim servisleri alabilmeme imkan saðlýyor. Burada bir
            //constructor olmadýðý için nesne örneðini almak için serviceProvider ile startup.cs'den alýyoruz.
            //GetRequiredService ile GetService farký >>>>>>
            //GetRequiredService'te servisi alamazsa geriye hata fýrlatýr, kesinlikle bir servisi almasý gerekir
            //GetService ise servisi alamaz ise geriye hata fýrlatmayýp null döner


            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            //kullanýcý kaydetmek için var userManager dedik. identity kütüphanesinden gelen UserManager kullandýk.

            identityDbContext.Database.Migrate();
            //bu kod ile (Migrate() ile) >>> update-database komutunu vermemize gerek yok. uygulama ayaða kalktýðýnda migration'lar veri tabanýna uygulanmadýysa
            //bunlarý uygular ayný zamanda veri tabaný yok ise kendisi sýfýrdan oluþturur. Veri tabaný var ise uygulanmayan migration'larý uygular.

            if (!userManager.Users.Any()) //veri tabanýna kayýt iþlemini her defasýnda yapmamak için kullanýcýnýn olup olmadýðýný kontrol ediyoruz
            {
                //blok içerisinde "Wait()" kullandýk çünkü uygulama ayaða kalktýðýnda sadece bir kere çalýþacak ve devamýnda olan her çalýþmada aktif olarak 
                //çalýþmayacaðýndan dolayý. Diðer sýnýflarýmýzda ise asenkron programlama yapmaya özen gösteriyoruz çünkü var olan thread'leri bloklamamamýz 
                //gerekiyor. Burada asenkron yapmamýza ise sunulan gerekçeden dolayý gerekli olmamaktadýr.
                //await'ler gelen thread'i bloklamazlar, Wait'ler ise gelen thread'i bloklar.
                //Wait bir method, await ise bir keyword'tür.
                //buradaki kodlarda asenkron bir method kullanmaz ciddi bir etki göstermez projemizde fakat diðer sýnýflarda, sürekli çalýþacak olan sýnýflarda
                //asenkron method'lar kullanmamýz ciddi performans artýþlarýna neden olacaktýr.
                userManager.CreateAsync(new AppUser() { UserName = "user1", Email = "user1@gmail.com" }, "Password12*").Wait();
                userManager.CreateAsync(new AppUser() { UserName = "user2", Email = "user2@gmail.com" }, "Password12*").Wait();
                userManager.CreateAsync(new AppUser() { UserName = "user3", Email = "user3@gmail.com" }, "Password12*").Wait();
                userManager.CreateAsync(new AppUser() { UserName = "user4", Email = "user4@gmail.com" }, "Password12*").Wait();
                userManager.CreateAsync(new AppUser() { UserName = "user5", Email = "user5@gmail.com" }, "Password12*").Wait();


                Enumerable.Range(1, 30).ToList().ForEach(x =>
                 {
                     identityDbContext.Products.Add(new Product { Name = $"kalem {x}", Price =x *100 ,Stock = x + 25 });
                 });
                identityDbContext.SaveChanges();
            }



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
