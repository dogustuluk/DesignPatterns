using BaseProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.CommandDesignPattern.Controllers
{
    public class ProductsController : Controller
    {
        public readonly AppIdentityDbContext _context;
        public ProductsController(AppIdentityDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }
    }
}
