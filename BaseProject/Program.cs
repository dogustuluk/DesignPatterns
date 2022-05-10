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

namespace BaseProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            //scope almam�z�n sebebi >>> scope ile beraber AppIdentityDbContext'ten bir nesne �rne�i al�yoruz ve i�imiz bitti�i zaman memory'de bu nesneler tutulmas�n
            //istiyoruz.
            using var scope = host.Services.CreateScope();

            var identityDbContext = scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();
            //ServiceProvider'�n �zelli�i >> startup.cs taraf�nda ConfigureServices taraf�nda ekledi�im servisleri alabilmeme imkan sa�l�yor. Burada bir
            //constructor olmad��� i�in nesne �rne�ini almak i�in serviceProvider ile startup.cs'den al�yoruz.
            //GetRequiredService ile GetService fark� >>>>>>
            //GetRequiredService'te servisi alamazsa geriye hata f�rlat�r, kesinlikle bir servisi almas� gerekir
            //GetService ise servisi alamaz ise geriye hata f�rlatmay�p null d�ner


            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            //kullan�c� kaydetmek i�in var userManager dedik. identity k�t�phanesinden gelen UserManager kulland�k.

            identityDbContext.Database.Migrate();
            //bu kod ile (Migrate() ile) >>> update-database komutunu vermemize gerek yok. uygulama aya�a kalkt���nda migration'lar veri tabana�na uygulanmad�ysa
            //bunlar� uygular ayn� zamanda veri taban� yok ise kendisi s�f�rdan olu�turur. Veri taban� var ise uygulanmayan migration'lar� uygular.

            if (!userManager.Users.Any()) //veri taban�na kay�t i�lemini her defas�nda yapmamak i�in kullan�c�n�n olup olmad���n� kontrol ediyoruz
            {
                userManager.CreateAsync(new AppUser() { UserName = "user1", Email = "user1@gmail.com" }, "Password12*").Wait();
                userManager.CreateAsync(new AppUser() { UserName = "user2", Email = "user2@gmail.com" }, "Password12*").Wait();
                userManager.CreateAsync(new AppUser() { UserName = "user3", Email = "user3@gmail.com" }, "Password12*").Wait();
                userManager.CreateAsync(new AppUser() { UserName = "user4", Email = "user4@gmail.com" }, "Password12*").Wait();
                userManager.CreateAsync(new AppUser() { UserName = "user5", Email = "user5@gmail.com" }, "Password12*").Wait();
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
