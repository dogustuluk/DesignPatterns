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


    }
}
