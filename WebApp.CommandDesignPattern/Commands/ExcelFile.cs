using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.CommandDesignPattern.Commands
{
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
