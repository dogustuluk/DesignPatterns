using BaseProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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
                    //excelFile.Create(); burada da Create metodunu kullanabiliriz fakat amacımız bunu burda yazmak değil. Bu işlemi kapsüllemek.
                    //kapsülleme olayını ise invoker'ın içerisinde yani FileCreateInvoker class'ında kapsüllüyoruz.
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
        public async Task<IActionResult> CreateFiles()
        {
            var products = await _context.Products.ToListAsync();
            
            ExcelFile<Product> excelFile = new(products);
            
            PdfFile<Product> pdfFile = new(products, HttpContext);
            
            FileCreateInvoker fileCreateInvoker = new();

            fileCreateInvoker.AddCommand(new CreateExcelTableActionCommand<Product>(excelFile));
            
            fileCreateInvoker.AddCommand(new CreatePdfTableActionCommand<Product>(pdfFile));
            
            var filesResult = fileCreateInvoker.CreateFiles(); //FileContentResult döner

            //IActionResult'lar şuan elimde var, artık zip dosyası oluşturabiliriz.
            using (var zipMemoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(zipMemoryStream, ZipArchiveMode.Create))
                {
                    foreach (var item in filesResult)
                    {
                        var fileContent = item as FileContentResult;

                        var zipFile = archive.CreateEntry(fileContent.FileDownloadName);

                        using (var zipEntryStream = zipFile.Open())
                        {
                            await new MemoryStream(fileContent.FileContents).CopyToAsync(zipEntryStream);
                        }
                    }
                }

                return File(zipMemoryStream.ToArray(), "application/zip", "all.zip");



            }
        }
    }
}