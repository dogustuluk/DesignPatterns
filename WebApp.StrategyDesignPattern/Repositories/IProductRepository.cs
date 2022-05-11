using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.StrategyDesignPattern.Models;

namespace WebApp.StrategyDesignPattern.Repositories
{
    interface IProductRepository
    {
        Task<Product> GetById(string id);
        Task<List<Product>> GetAllByUserId(string userId);
        Task<Product> Save(Product product);
        Task<Product> Delete(Product product);
        Task<Product> Update(Product product);
    }
}
