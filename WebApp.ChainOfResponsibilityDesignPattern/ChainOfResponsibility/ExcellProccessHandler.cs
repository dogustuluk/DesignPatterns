using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ChainOfResponsibilityDesignPattern.ChainOfResponsibility
{
    public class ExcellProccessHandler<T> : ProccessHandler
    {
        private DataTable GetTable(Object o)
        {
            var table = new DataTable();

            var type = typeof(T);

            type.GetProperties().ToList().ForEach(x => table.Columns.Add(x.Name, x.PropertyType));

            var list = o as List<T>; //listenin datası parametreden gelen "Object o" içerisinde fakat listenin türü ise class'a dinamik olarak gelen generic<T>'dedir.
            //listenin tipi belirlenmiş oldu. listeyi >> o tipinde bir liste alabilecek bir hale getiriyoruz

            list.ForEach(x =>
            {
                var values = type.GetProperties().Select(propertyInfo => propertyInfo.GetValue(x, null)).ToArray();

                table.Rows.Add(values);
                //DataTable'ı doldurmuş olduk
            });

            return table;
        }

        public override object handle(object o) //Object olarak parametre almak oldukça önemlidir. bu sayede ileride oluşacak senaryolarda, zincire yeni halkalar
            //eklendiğiğnde istediğimiz bir tipte bir halkadan bir diğer halkaya data geçebiliriz.
        {//asıl işi yapacak olan metot
            //bu metot içerisinde işlenmiş olan verileri excel formatına çekmiş bulunuyoruz.
            var workBook = new XLWorkbook();

            var dataSet = new DataSet(); //DataSet'i bir veri tabanı gibi düşünebiliriz.

            dataSet.Tables.Add(GetTable(o));

            workBook.Worksheets.Add(dataSet);

            var excelMeomoryStream = new MemoryStream();

            workBook.SaveAs(excelMeomoryStream);

            return base.handle(excelMeomoryStream); //dönüş tipi olarak Object nesnesi geçtiğimiz için istediğimiz türden bir nesneyi döndürebiliriz.
            //eğer burada MemoryStream nesnesi yollasaydık ilerleyen süreçlerde programda döndürülecek olan tip farklılaşırsa işlem yapmamız gerekecekti.
            //Object ile geri dönersek programın esneklik kapasitesini artırmaktadır.
        }
    }
}
