using Microsoft.AspNetCore.Mvc;

namespace Touride.UI.Areas.Admin.Controllers
{

    public class UserController : AdminAreaBaseController
    {
        public IActionResult Index()
        {
            ViewBag.Title = "Index";
            return View();
        }

        public IActionResult UserSubscribe()
        {
            ViewBag.Title = "Kullanıcı Üyelikleri";
            return View();
        }

        public IActionResult UserManagement()
        {
            ViewBag.Title = "Kullanıcı Yönetimi";
            return View();
        }

        public IActionResult Coupons()
        {

            ViewBag.Title = "Kuponlar";
            return View();
        }
        
        public IActionResult ContentComplaint()
        {
            ViewBag.Title = "İçerik Şikayetleri";
            return View();
        }
    }
}
