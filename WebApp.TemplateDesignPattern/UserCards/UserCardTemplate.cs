using BaseProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        
        public string Build()
        {
            if (AppUser == null) throw new ArgumentNullException(nameof(AppUser));

            var stringBuilder = new StringBuilder();

            stringBuilder.Append("<div class='card'>");
            stringBuilder.Append(SetPicture());
            //"@" işareti ile istenildiği gibi girinti verebiliriz.
            stringBuilder.Append($@"<div class= 'card-body'>
                                <h5>{AppUser.UserName}</h5>
                                <p>{AppUser.Description}</p>");
            stringBuilder.Append(SetFooter());
            stringBuilder.Append("</div>");
            stringBuilder.Append("</div>");

            return stringBuilder.ToString();
        }


        //protected kullanılır çünkü subclass'ların da bu method'lara erişmesini istiyoruz.
        protected abstract string SetFooter();
        protected abstract string SetPicture();
    }
}
