using BaseProject.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApp.StrategyDesignPattern.Models;

namespace WebApp.StrategyDesignPattern.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public SettingsController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            Settings settings = new();
            if (User.Claims.Where(x => x.Type == Settings.claimDatabaseType).FirstOrDefault() != null)
            {
                settings.databaseType = (EDatabaseType)int.Parse(User.Claims.First(x => x.Type == Settings.claimDatabaseType).Value);
            }
            else
            {
                settings.databaseType = settings.GetDefaultDatabaseType;
            }



            return View(settings);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeDatabase (int databaseType)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name); //kullanıcıyı bulan kod.

            //claim oluşturmamız gerekli
            var newClaim = new Claim(Settings.claimDatabaseType, databaseType.ToString()); //(string type, string value)
                                                                                           //type >> settings üzetinden gelen "claimDatabaseType", value ise parametre olarak gelen "databaseType".
                                                                                           //ardından veritabanındaki claim'leri alalım
            var claims = await _userManager.GetClaimsAsync(user); // veri tabanındaki claim'lerin olup olmadığının kontrolü için

            //ardından elimizdeki databaseType'ın claim'i var mı yok mu diye kontrol edelim.
            var hasDatabaseTypeClaim = claims.FirstOrDefault(x => x.Type == Settings.claimDatabaseType);

            if (hasDatabaseTypeClaim != null)
            {
                await _userManager.ReplaceClaimAsync(user, hasDatabaseTypeClaim, newClaim);
            }
            else
            {
                await _userManager.AddClaimAsync(user, newClaim);
            }

            //--------------------------------------------------------------------------önemli------------------------------------------------------------------------------
            //kullanıcının veritabanında claim'ini değiştirdik fakat cookie'de hala eskisi olabilir. O yüzden kullanıcıya çıkış yaptırıp tekrardan sisteme giriş yaptırıcaz.
            //yapılan bu işlemi kullanıcı farketmeyecek, arka planda yapıyor olucaz.
            await _signInManager.SignOutAsync();

            var authenticateResult = await HttpContext.AuthenticateAsync();
            //giriş yaparken kullanıcının cookie'sinde token vs. olabilir, o değerleri yine almak için yazarız veya kullanıcı "beni hatırla" özelliğini aktif yapmış olabilir.
            //bu yüzden cookie'de hazır bulunan değerleri almak için bu kodu yazarız. bu değerler ile yeni bir cookie oluşturucaz.

            //authenticateResult üzerinden kullanıcının property'lerini alabiliriz.
            await _signInManager.SignInAsync(user, authenticateResult.Properties);

            return RedirectToAction(nameof(Index));
        }
    }
}
