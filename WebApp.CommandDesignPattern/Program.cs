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
            //bu kod ile (Migrate() ile) >>> update-database komutunu vermemize gerek yok. uygulama aya�a kalkt���nda migration'lar veri taban�na uygulanmad�ysa
            //bunlar� uygular ayn� zamanda veri taban� yok ise kendisi s�f�rdan olu�turur. Veri taban� var ise uygulanmayan migration'lar� uygular.

            if (!userManager.Users.Any()) //veri taban�na kay�t i�lemini her defas�nda yapmamak i�in kullan�c�n�n olup olmad���n� kontrol ediyoruz
            {
                //blok i�erisinde "Wait()" kulland�k ��nk� uygulama aya�a kalkt���nda sadece bir kere �al��acak ve devam�nda olan her �al��mada aktif olarak 
                //�al��mayaca��ndan dolay�. Di�er s�n�flar�m�zda ise asenkron programlama yapmaya �zen g�steriyoruz ��nk� var olan thread'leri bloklamamam�z 
                //gerekiyor. Burada asenkron yapmam�za ise sunulan gerek�eden dolay� gerekli olmamaktad�r.
                //await'ler gelen thread'i bloklamazlar, Wait'ler ise gelen thread'i bloklar.
                //Wait bir method, await ise bir keyword't�r.
                //buradaki kodlarda asenkron bir method kullanmaz ciddi bir etki g�stermez projemizde fakat di�er s�n�flarda, s�rekli �al��acak olan s�n�flarda
                //asenkron method'lar kullanmam�z ciddi performans art��lar�na neden olacakt�r.
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
