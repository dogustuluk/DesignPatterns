using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.CompositeDesignPattern.Composite
{
    public class BookComponent : IComponent
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public BookComponent(string name, int id)
        {
            Id = id;
            Name = name;
        }

        public int Count()
        {
            return 1; //desendeki dal-yaprak ilişkisini düşünürsek; burası dal kısmını temsil etmektedir. burası son noktadır, artık alt kategori bulunmaz
        }

        public string Display()
        {
            return $"<li class='list-group-item'>{Name}</li>";
        }
    }
}
