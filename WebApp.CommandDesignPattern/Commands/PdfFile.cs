using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.CommandDesignPattern.Commands
{
    public class PdfFile<T>
    {
        public readonly List<T> _list;
        public string FileName => $"{typeof(T).Name}.pdf";
        public string FileTypr => "application/octet-stream";
    }
}
