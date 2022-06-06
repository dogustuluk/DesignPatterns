using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ChainOfResponsibilityDesignPattern.ChainOfResponsibility
{
    public interface IProccessHandler
    {
        IProccessHandler SetNext(IProccessHandler proccessHandler); //bir sonraki halkanın varlığını kontrol etmeye yaramaktadır/ ne olacağını da belirler.

        Object handle(Object o); //üzerine almış olduğumuz data üzerinde bir işlem yapıp, bir sonraki halkaya aktarmıyorsak geriye direkt olarak 
                //>>> void dönebiliriz fakat biz burdaki örnekte geriye data döndürüp bir sonraki halkaya işlemi taşıyor olacağımız için Object alıp geriye de 
                //Object dönüyoruz.
    }
}
