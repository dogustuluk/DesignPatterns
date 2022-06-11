using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ChainOfResponsibilityDesignPattern.ChainOfResponsibility
{
    public class ZipFileProccessHandler<T> : ProccessHandler
    {
        
        public override object handle(object o)
        {
            var excelMemoryStream = o as MemoryStream; //bir önceki halkadan memory stream geldiğinden dolayı oluşturduk. gelen objeyi bir memoryStream'e çevirdik.

            excelMemoryStream.Position = 0; //Yazdırma işlemi yapacağımızdan dolayı pozisyonunu alıyoruz.
                                            //gelen dosyada bir byteArray olduğundan başlangıcı 0 olacaktır.

            using (var zipStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, true)) //stream'i açık bırakmamız gerekmektedir(true yapıyoruz).
                                //çünkü "return base.handle(zipStream)" kodu ile erişim sağlamamız gerekiyor.
                {
                    var zipFile = archive.CreateEntry($"{typeof(T).Name}.xlsx"); //dosyayı dinamik olarak geçmek için class'ı generic yapıyoruz.

                    using (var zipEntry = zipFile.Open()) //gelen stream dosyasını açıyoruz.
                    {
                        excelMemoryStream.CopyTo(zipEntry); //gelen memorystream'i (excelmemoryStream) zipEntry'e kopyalıyoruz.
                    }
                }
                return base.handle(zipStream);

            }

        }
    }
}
