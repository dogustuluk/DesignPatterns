using BaseProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ObserverDesignPattern.Models;
using WebApp.ObserverDesignPattern.Observer;

namespace BaseProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserObserverSubject _userObserverSubject;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, UserObserverSubject userObserverSubject)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userObserverSubject = userObserverSubject;
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

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(UserCreateViewModel userCreateViewModel)
        {
            var appUser = new AppUser() { UserName = userCreateViewModel.UserName, Email = userCreateViewModel.Email };

            var identityResult = await _userManager.CreateAsync(appUser, userCreateViewModel.Password);

            if (identityResult.Succeeded)
            {

                _userObserverSubject.NotifyObserver(appUser);
                ViewBag.message = "Üyelik işlemi başarıyla gerçekleştirildi";
            }
            else
            {
                ViewBag.message = identityResult.Errors.ToList().First().Description;
            }

            return View();
        }
    }
}
