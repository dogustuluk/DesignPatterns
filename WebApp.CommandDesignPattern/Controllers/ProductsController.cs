using BaseProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.CommandDesignPattern.Commands;
using WebApp.CommandDesignPattern.Models;

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
        public async Task<IActionResult> CreateFile(int type)
        {
            var products = await _context.Products.ToListAsync();

            FileCreateInvoker fileCreateInvoker = new();

            EFileType fileType = (EFileType)type;

            switch (fileType)
            {
                case EFileType.excel:
                    ExcelFile<Product> excelFile = new(products);

                    fileCreateInvoker.SetCommand(new CreateExcelTableActionCommand<Product>(excelFile));
                    break;
                case EFileType.pdf:
                    PdfFile<Product> pdfFile = new(products, HttpContext);
                    fileCreateInvoker.SetCommand(new CreatePdfTableActionCommand<Product>(pdfFile));
                    break;
                default:
                    break;
            }
            return fileCreateInvoker.CreateFile(); //burdan IActionResult döner. Bu dönen IActionResult ise bir FileContentResult dönmüş olacaktır.
            //CreateFile>> FileCreateInvoker'dan gelen metot.
        }
    }
}