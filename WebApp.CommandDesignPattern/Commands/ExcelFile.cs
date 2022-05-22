using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.CommandDesignPattern.Commands
{ //uml diyagramındaki Receiver'dır
    public class ExcelFile<T>
    {
        public readonly List<T> _list; //readonly dememizin sebebi>>>>> mutlaka ya constructor içerisinde ya da tanımlandığı yerde initialize edilsin,
                                       //daha sonra değiştirilmesin diye
        public string FileName => $"{typeof(T).Name}.xlsx";
        public string FileType => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        public ExcelFile(List<T> list)
        {
            _list = list;
        }

        public MemoryStream Create() //geriye MemoryStream dönüyoruz >> excel dosyasını memory'de bir "bind array" olarak tutucaz.
        {
            var wb = new XLWorkbook();
            //ardından dataset oluşturuyoruz. DataTable'ları tutan bir veritabanı olarak düşünülebilir.

            var ds = new DataSet();

            ds.Tables.Add(GetTable());

            wb.Worksheets.Add(ds);

            var excelMemory = new MemoryStream();

            wb.SaveAs(excelMemory);
            return excelMemory;

            //artık memory'de bir excel dosyası var, bunu kullanının download edebileceği bir file context'e dönüştürebilirim.
        }

        private DataTable GetTable() //private yapıyoruz çünkü bunu dış dünyaya açmak istemiyoruz.
        {
            var table = new DataTable();

            var type = typeof(T); // generic bir şekilde alıyoruz çünkü içerisine istediğimiz parametreyi geçmek istiyoruz.

            type.GetProperties().ToList().ForEach(x => table.Columns.Add(x.Name, x.PropertyType)); //"x.Name" diyerek tablodaki tüm alanların isimlerini otomatik
                                                                                                   //olarak almış oluyoruz

            _list.ForEach(x =>
            {
                var values = type.GetProperties().Select(propertyInfo => propertyInfo.GetValue(x, null)).ToArray();

                table.Rows.Add(values);
            });
            return table;
        }
    }
}
