using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.DecoratorDesignPattern.Models;

namespace WebApp.DecoratorDesignPattern.Repositories.Decorator
{                   //Bir base yani abstract class'tır. Nesne örneği alamaz.
                    //burada implemente edilen metotlar virtual olmalıdır. Çünkü alt sınıflarında override edilecektir.
                    //Bu abstract sınıfın oluşmasının mantığı ise >>>>>
                                //Tüm metotları oluşturukacak olan alt sınıflarda tekrar tekrar yazmamak için; yazmak istenilen metotları override ederiz.
                    //Oluşturulacak olan tüm decorator'lar bu abstract sınıfı miras alacak, istediği metodu override edip kendisi ek özellikler kazandıracaktır.
    public class BaseProductRepositoryDecorator : IProductRepository
    {
        public readonly IProductRepository _productRepository;
        public BaseProductRepositoryDecorator(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public virtual async Task<List<Product>> GetAll()
        {
            return await _productRepository.GetAll();
        }

        public virtual async Task<List<Product>> GetAll(string userId)
        {
            return await _productRepository.GetAll(userId);
        }

        public virtual async Task<Product> GetById(int id)
        {
            return await _productRepository.GetById(id);
        }

        public virtual async Task Remove(Product product)
        {
            await _productRepository.Remove(product);
        }

        public virtual async Task<Product> Save(Product product)
        {
            return await _productRepository.Save(product);
        }

        public virtual async Task Update(Product product)
        {
            await _productRepository.Update(product);
        }
    }
}
