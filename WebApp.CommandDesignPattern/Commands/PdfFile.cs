﻿using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.CommandDesignPattern.Commands
{
    public class PdfFile<T>
    {
        public readonly List<T> _list;
        public readonly HttpContext _context;
        public PdfFile(List<T> list, HttpContext context)
        {
            _list = list;
            _context = context;
        }
        public string FileName => $"{typeof(T).Name}.pdf";
        public string FileType => "application/octet-stream";

        public MemoryStream Create()
        {
            var type = typeof(T);

            var sb = new StringBuilder();

            sb.Append($@"<html>
                            <head></head>
                            <body>
                                <div class='text-center'><h1>{type.Name} tablo</h1></div>
                                <table class = 'table table-striped' align='center'>");
            sb.Append("<tr>");
            type.GetProperties().ToList().ForEach(x => 
            {
                sb.Append($"<th>{x.Name}</th>");
            });
            sb.Append("</tr>");

            //satırlar _list'ten gelir.
            _list.ForEach(x =>
            {
                var values = type.GetProperties().Select(propertyInfo => propertyInfo.GetValue(x, null)).ToList();// array'e dönüştürmedik çünkü ForEach
                                                                                                                  // ile içerisinde dönüyoruz.
                sb.Append("<tr>");
                values.ForEach(value => 
                {
                    sb.Append($"<td>{value}</td>");
                });
                sb.Append("</tr>");

            });
            sb.Append("</table></body></html>");


            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                                  ColorMode = ColorMode.Color,
                                  Orientation = Orientation.Portrait,
                                  PaperSize = PaperKind.A4,
    },
                //dinkToPdf kütüphanesi aşağıdadır.
                Objects = 
            {
                 new ObjectSettings() 
                {
                    PagesCount = true,
                    HtmlContent = sb.ToString(),
                    WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/lib/bootstrap/dist/css/bootstrap.css") },
                    HeaderSettings = { FontSize = 9, Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812 }
                }
            }
            };

            var convertor = _context.RequestServices.GetRequiredService<IConverter>();

            return new(convertor.Convert(doc)); //"new MemoryStream()" yazmaya gerek yok, C#9.0 ile dönüş tipini otomatik olarak alıyor.
        }
    }
}
