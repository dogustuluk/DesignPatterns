using BaseProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.StrategyDesignPattern.Models;

namespace WebApp.StrategyDesignPattern.Repositories
{
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

            await _context.SaveChangesAsync();
        }

        public Task<List<Product>> GetAllByUserId(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Product> Save(Product product)
        {
            throw new NotImplementedException();
        }

        public Task Update(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
