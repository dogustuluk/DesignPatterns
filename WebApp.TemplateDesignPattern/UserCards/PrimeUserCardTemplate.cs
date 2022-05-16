using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.TemplateDesignPattern.UserCards
{
    public class PrimeUserCardTemplate : UserCardTemplate
    {
        protected override string SetFooter()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("<a href='#' class='card-link'>Mesaj Gönder</a>");
            stringBuilder.Append("<a href='#' class='card-link'>Profil</a>");
            return stringBuilder.ToString();
            
        }

        protected override string SetPicture()
        {
            return $"  <img src='{AppUser.PictureUrl}' class='card-img-top'>";
        }
    }
}
