using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.AdapterDesignPattern.Services
{
    public class ImageProcess : IImageProcess
    {
        public void AddWatermark(string text, string filename, Stream imageStream)
        {
            throw new NotImplementedException();
        }
    }
}
