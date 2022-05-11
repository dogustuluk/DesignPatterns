using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.StrategyDesignPattern.Models;

namespace WebApp.StrategyDesignPattern.Repositories
{
    public class ProductRepositoryFromMongoDb : IProductRepository
    {
        private readonly IMongoCollection<Product> _productCollection;
        public ProductRepositoryFromMongoDb(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MongoDb");

            var client = new MongoClient(connectionString);

            var database = client.GetDatabase("ProductDb");

            _productCollection = database.GetCollection<Product>("Products");
        }

        public Task Delete(Product product)
        {
            throw new NotImplementedException();
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
