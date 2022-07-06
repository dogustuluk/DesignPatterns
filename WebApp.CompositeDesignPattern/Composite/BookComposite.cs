using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.CompositeDesignPattern.Composite
{
    [Authorize]
    public class BookComposite : IComponent
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //Composite olduğundan dolayı bir list tutması gerekmektedir.
        private List<IComponent> _components; //buradaki listeye ilgili component'leri ekleyecek ve silecek metotların da tanımlanması gerekmektedir.

        public IReadOnlyCollection<IComponent> Components => _components; //yukarıdaki _components listesini dış dünyaya sadece okunabilir şekilde açıyoruz.

        public BookComposite(int id, string name)
        {
            Id = id;
            Name = name;
            _components = new List<IComponent>();
        }

        public void Add(IComponent component) //IComponent'i implemente eden bir obje almak zorundadır.
        {
            _components.Add(component);
        }
        public void Remove(IComponent component)
        {
            _components.Remove(component);
        }

        public int Count()
        {
            return _components.Sum(x => x.Count());
        }

        public string Display()
        {
            var sb = new StringBuilder();

            sb.Append($"<div class='text-primary my-1'><a href='#' class='menu'>{Name} ({Count()}) </a></div>");

            if (!_components.Any()) return sb.ToString();

            sb.Append("<ul class='list-group list-group-flush ms-3'>");

            foreach (var item in _components)
            {
                sb.Append(item.Display()); //buraya kadar geldi ise BookComponent'teki display metodunu çağırır.
            }

            sb.Append("</ul>");

            return sb.ToString();
        }
    }
}
