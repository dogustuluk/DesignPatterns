using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.CompositeDesignPattern.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public int ReferenceId { get; set; } //hangi kategorinin altında olduğunu belli etmek için kullanırız. örnek için aşağıdaki şemaya bak
        public ICollection<Book> Books { get; set; }
    }
}

// Id   Name    UserId      ReferenceId
// 1    kitap   1           0           >>>>>>>>> 0 demek ana kategori anlamına gelmektedir.
// 2    roman   1           1           >>>>>>>>> 1 numaralı kategorinin alt kategorisi olduğu anlamına gelmektedir.