using BaseProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApp.CompositeDesignPattern.Composite;
using WebApp.CompositeDesignPattern.Models;

namespace WebApp.CompositeDesignPattern.Controllers
{
    public class CategoryMenuController : Controller
    {
        private readonly AppIdentityDbContext _context;
        public CategoryMenuController(AppIdentityDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            //category'ler BookComposite sınıfından gelecek
            //book'lar ise BookComponent sınıfından gelecek
            var userId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value; //cookie'den userId alınmış oldu. sadece userId'ye ihtiyaç olduğu için usermanager üzerinden işlem
                                                                                            //yapmaya ihtiyaç duyulmadı.

            var categories = await _context.Categories.Include(x => x.Books).Where(x => x.UserId == userId).OrderBy(x => x.Id).ToListAsync();

            var menu = GetMenus(categories, new Category { Name = "TopCategory", Id = 0 }, new BookComposite(0, "TopMenu")); //id'leri 0 olanlar bizim köklerimizi oluşturmaktadır.

            ViewBag.menu = menu;

            ViewBag.selectList = menu.Components.SelectMany(x => ((BookComposite)x).GetSelectListItem(""));

            return View();
        }


        //alttaki metotta root yani kök menü gelmektedir. Gelen kök menünün içerisinde diğer elemanlar da var.
        public BookComposite GetMenus(List<Category> categories, Category topCategory, BookComposite topBookComposite, BookComposite last = null)
        {
            categories.Where(x => x.ReferenceId == topCategory.Id).ToList().ForEach(categoryItem =>
            {
                var bookComposite = new BookComposite(categoryItem.Id, categoryItem.Name);

                categoryItem.Books.ToList().ForEach(bookItem =>
                {
                    bookComposite.Add(new BookComponent(bookItem.Id, bookItem.Name)); //BookComponent sınıfımız IComponent sınıfını implemente ettiği için direkt olarak "new BookComponent" diyebiliyoruz.
                });
                if (last != null)
                {
                    last.Add(bookComposite);
                }
                else
                {
                    topBookComposite.Add(bookComposite);
                }

                GetMenus(categories, categoryItem, topBookComposite, bookComposite);
             
            });
            return topBookComposite;
        }
    }
}
