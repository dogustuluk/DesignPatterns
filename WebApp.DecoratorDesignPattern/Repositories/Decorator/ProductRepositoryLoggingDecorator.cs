using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.DecoratorDesignPattern.Models;

namespace WebApp.DecoratorDesignPattern.Repositories.Decorator
{
    public class ProductRepositoryLoggingDecorator : BaseProductRepositoryDecorator //base'in constructor'ı olduğu için burada da constructor geçiyoruz.
    {
        private readonly ILogger<ProductRepositoryLoggingDecorator> _log;
        public ProductRepositoryLoggingDecorator(IProductRepository productRepository, ILogger<ProductRepositoryLoggingDecorator> log) : base(productRepository)
        {
            _log = log;
        }

        public override Task<List<Product>> GetAll()
        {
            _log.LogInformation("GetAll metodu çalıştı");
            return base.GetAll();
        }

        public override Task<List<Product>> GetAll(string userId)
        {
            _log.LogInformation("GetAll(string userId) metodu çalıştı");
            return base.GetAll(userId);
        }

        public override Task<Product> GetById(int id)
        {
            _log.LogInformation("GetById metodu çalıştı");
            return base.GetById(id);
        }

        public override Task<Product> Save(Product product)
        {
            _log.LogInformation("Save metodu çalıştı");
            return base.Save(product);
        }

        public override Task Update(Product product)
        {
            _log.LogInformation("Update metodu çalıştı");
            return base.Update(product);
        }

        public override Task Remove(Product product)
        {
            _log.LogInformation("Remove metodu çalıştı");
            return base.Remove(product);
        }
    }
}
