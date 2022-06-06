using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ChainOfResponsibilityDesignPattern.ChainOfResponsibility
{
    public abstract class ProccessHandler : IProccessHandler
    {
        private IProccessHandler nextProccessHandler;

        //ProccessHandler metodunu aldığımızda aşağıdaki "handle" metodunu override etmiş olacağız. alt sınıfların uygulaması için >> virtual
        public virtual object handle(object o) //virtual >>>alt sınıfların da ezilmesi için metodu "virtual" olarak işaretleriz.
        {
            if (nextProccessHandler != null)
            {
                return nextProccessHandler.handle(o);
            }
            return null;
        }

        public IProccessHandler SetNext(IProccessHandler proccessHandler)
        {
            nextProccessHandler = proccessHandler;
            return nextProccessHandler;
        }
    }
}
