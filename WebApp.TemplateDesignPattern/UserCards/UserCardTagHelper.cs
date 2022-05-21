using BaseProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.TemplateDesignPattern.UserCards
{
    public class UserCardTagHelper : TagHelper
    {//"UserCardTagHelper" >> TagHelper'dan önce verilen isim bizim 'tag helper'ımız olur.
        public AppUser AppUser { get; set; }

        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserCardTagHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            UserCardTemplate userCardTemplate;
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                userCardTemplate = new PrimeUserCardTemplate();
            }
            else
            {
                userCardTemplate = new DefaultUserCardTemplate();
            }
            userCardTemplate.SetUser(AppUser);//Build işleminin gerçekleşmesi için mutlaka verilmesi lazım.
                                              //bunu istenildiği takdirde constructor'da da alabiliriz.

            output.Content.SetHtmlContent(userCardTemplate.Build());
        }
        /*
        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            UserCardTemplate userCardTemplate;
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                userCardTemplate = new PrimeUserCardTemplate();
            }
            else
            {
                userCardTemplate = new DefaultUserCardTemplate();
            }
            userCardTemplate.SetUser(AppUser);//Build işleminin gerçekleşmesi için mutlaka verilmesi lazım.
                                              //bunu istenildiği takdirde constructor'da da alabiliriz.

            output.Content.SetHtmlContent(userCardTemplate.Build());
            return Task.CompletedTask; //asenkron olan method'ta asenkron kullanmadığımız için bunu yazabiliriz veya asenkron metodu override edebiliriz.
            //üstte override gösteriliyor.
        }
        */

    }
}
