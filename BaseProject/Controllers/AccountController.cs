using BaseProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var hasUser = await _userManager.FindByEmailAsync(email);
            
            if (hasUser == null) return View(); //eğer kullanıcı yoksa hata mesajı vermek yerine aynı sayfaya yönlendiriyoruz.

            var signInResult = await _signInManager.PasswordSignInAsync(hasUser, password, true, false);
            //isPersistent >> cookie'de saklanmasını sağlar. true olursa tarayıcı kapandığında her defasında login olma işlemi ile uğraşılmaz.
            //locoutOnFailure >>> kullanıcı hatalı bilgiler girdiğinde bloklanması ile ilgili, 5dk bekle sonra tekrar login bilgilerini gir gibi. false ise bloklama olmaz.
            //var signInResult'dan true veya false dönmez. Sebebi ise eğer iki faktörlü doğrulama açık ise onunla ilgili gerekli kontrolleri sağlaması için vs..
            //true veya false kontrolünü başarılı ise şeklinde aşağıdaki if bloğunda inceliyoruz.


            if (!signInResult.Succeeded)
            {
                return View();
            }

            return RedirectToAction(nameof(HomeController.Index), "Home"); //"nameof" kullanmamızın nedeni >>> eğer HomeController içerisindeki Index'in adı değişirse,
            //uygulamamızın patlaması için
            
        }
    }
}
