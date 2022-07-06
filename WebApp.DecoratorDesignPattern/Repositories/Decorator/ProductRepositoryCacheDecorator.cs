using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.DecoratorDesignPattern.Models;

namespace WebApp.DecoratorDesignPattern.Repositories.Decorator
{
    public class ProductRepositoryCacheDecorator : BaseProductRepositoryDecorator
    {
        private readonly IMemoryCache _memoryCache;
        private const string ProductsCacheName = "products";
        public ProductRepositoryCacheDecorator(IProductRepository productRepository, IMemoryCache memoryCache) : base(productRepository)
        {
            _memoryCache = memoryCache;
        }

        public async override Task<List<Product>> GetAll()
        {
            if (_memoryCache.TryGetValue(ProductsCacheName, out List<Product> cacheProducts)) //eğer data varsa "cacheProducts"a atacak ve true dönecek.
            {
                return cacheProducts;
            }
            //cache'te yok ise alttaki kodlara geçilir.
            await UpdateCache(); //veri tabanında çeker ve cache'i set eder.
            //ardından cache'de data var olur, datayı dönebiliriz.
            return _memoryCache.Get<List<Product>>(ProductsCacheName);
        }

        public async override Task<List<Product>> GetAll(string userId)
        {
            //return base.GetAll(userId); >>>> eğer base'den alırsak veri tabanından çekmiş oluruz dataları, cache'den değil.
            var products = await GetAll(); //yukarıda override edilen GetAll metodunu çağırırız.
            return products.Where(x => x.UserId == userId).ToList();
        }

        public async override Task<Product> Save(Product product)
        {
            await base.Save(product);
            await UpdateCache();
            return product;
        }

        public async override Task Update(Product product)
        {
            await base.Update(product);
            await UpdateCache();
        }
        public async override Task Remove(Product product)
        {
            await base.Remove(product);
            await UpdateCache();
        }
        public async Task UpdateCache()
        {
            _memoryCache.Set(ProductsCacheName, await base.GetAll());
        }
    }
}
