using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.AdapterDesignPattern.Services
{
    public class AdvanceImageProcessAdapter: IImageProcess
    {//bu sınıf client'ın bilmiş olduğu interface'i implemente edecek.
        //yani HomeController'dakini
        private readonly IAdvanceImageProcess _advanceImageProcess;

        public AdvanceImageProcessAdapter(IAdvanceImageProcess advanceImageProcess)
        {
            _advanceImageProcess = advanceImageProcess;
        }

        public void AddWatermark(string text, string filename, Stream imageStream)
        {//burada farklı olan sınıfları birbiriyle aynı olacak şekle getiriyoruz.
            _advanceImageProcess.AddWatermarkImage(imageStream, text, $"wwwroot/watermarks/{filename}", Color.FromArgb(128, 150, 120, 110), Color.FromArgb(10, 45, 60, 90));
            //arka planda third party olarak eklenen watermark'ı çağırıyor olucaz.
        }
    }
}
