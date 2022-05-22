using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.CommandDesignPattern.Commands
{
    public class CreateExcelTableActionCommand<T> : ITableActionCommand
    { //receiver alması gerekli (receiver > asıl işlem yapan class)
        private readonly ExcelFile<T> _excelFile;
        public CreateExcelTableActionCommand(ExcelFile<T> excelFile)
        {
            _excelFile = excelFile;
        }
        public IActionResult Execute()
        {
            //_excelFile'dan bize MemoryStream gelmektedir, artık bunu IActionResult'a dönüştürebiliriz.
            var excelMemoryStream = _excelFile.Create();

            //geriye download edilebilir bir dosya döndürmek istediğimizde "FileContentResul" kullanılabilir.
            return new FileContentResult(excelMemoryStream.ToArray(), _excelFile.FileType) { FileDownloadName = _excelFile.FileName };
        }
    }
}
