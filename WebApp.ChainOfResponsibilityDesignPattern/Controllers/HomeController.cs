using BaseProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ChainOfResponsibilityDesignPattern.ChainOfResponsibility;
using WebApp.ChainOfResponsibilityDesignPattern.Models;

namespace BaseProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppIdentityDbContext _context;
        public HomeController(ILogger<HomeController> logger, AppIdentityDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SendEmail()
        {
            //var products = await _context.Products.ToListAsync();
            ////devamında ise zincirdeki halkaları tanıtmamız gerekmektedir.
            //var excellProccessHandler = new ExcellProccessHandler<Product>();
            //var zipFileProccessHandler = new ZipFileProccessHandler<Product>();
            //var sendEmailProccessHandler = new SendEmailProccessHandler("product.zip", "dogus.tuluk@gmail.com");

            ////şimdi ise halkaları birbirlerine bağlayalım.
            //excellProccessHandler.SetNext(zipFileProccessHandler).SetNext(sendEmailProccessHandler);

            //excellProccessHandler.handle(products);

            //return View(nameof(Index));
            var products = await _context.Products.ToListAsync();

            var excelProcessHandler = new ExcellProccessHandler<Product>();

            var zipFileProcessHandler = new ZipFileProccessHandler<Product>();

            var sendEmailProcessHandler = new SendEmailProccessHandler("product.zip", "dogus.tuluk@gmail.com");

            excelProcessHandler.SetNext(zipFileProcessHandler).SetNext(sendEmailProcessHandler);

            excelProcessHandler.handle(products);

            return View(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
