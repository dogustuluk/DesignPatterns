using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.CompositeDesignPattern.Composite
{
    interface IComponent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        int Count(); //
        string Display(); //ul tag'leri arasında bir kategori listesi yapıyor olucaz. Eğer cshtml kodları içerisinde hazırlamak istersek işlem oldukça 
        //zor bir hale gelecektir. Dolayısıyla işi kolaylaştırmak için cs kısmında yapıyor olucaz bu listeleme kısmını. Bunu sağlaması için Display'i kullanırız.
    }
}
