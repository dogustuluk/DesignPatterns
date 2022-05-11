using BaseProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.StrategyDesignPattern.Models;

namespace WebApp.StrategyDesignPattern.Repositories
{
    //1.stratejinin olduğu class.
    //startup.cs dosyasına alttaki kodu yazmıyor olacağız strategy design pattern ile
    //services.AddScoped<IProductRepository, ProductRepositoryFromSqlServer>();// böyle bir kod sadece "compile time"da programı etkiler, runtime'da etkilemez.
    public class ProductRepositoryFromSqlServer : IProductRepository
    {
        private readonly AppIdentityDbContext _context;
        public ProductRepositoryFromSqlServer(AppIdentityDbContext appIdentityDbContext)
        {
            _context = appIdentityDbContext;
        }

        public async Task Delete(Product product)
        {
            _context.Products.Remove(product); //Remove metodunun asenkronu yoktur. Nedeni ise remove metodunun işlemi gerçekleştirmeyip memory'de ilgili objenin
            //state'ini değiştirmesidir. alttaki kod olmadan yapılması istenen işlem veritabanına yansımamaktadır.
            //property set eder.

            await _context.SaveChangesAsync();
        }

        public async Task<List<Product>> GetAllByUserId(string userId)
        {
            return await _context.Products.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<Product> GetById(string id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product> Save(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task Update(Product product)
        {
            _context.Products.Update(product); //update metodunun da asenkronu yoktur. Çünkü update de Remove metodu gibi çalışır; anlık olarak memory'de ilgili
            //objenin property'sini set eder (state'ini değiştirir). 
            await _context.SaveChangesAsync();
        }
    }
}
