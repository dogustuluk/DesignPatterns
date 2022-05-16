using BaseProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.TemplateDesignPattern.UserCards
{
    public abstract class UserCardTemplate
    {
        protected AppUser AppUser { get; set; }

        public void SetUser(AppUser appUser)
        {
            AppUser = appUser;
        }

        //protected kullanılır çünkü subclass'ların da bu method'lara erişmesini istiyoruz.
        protected abstract string SetFooter();
        protected abstract string SetPicture();
    }
}
