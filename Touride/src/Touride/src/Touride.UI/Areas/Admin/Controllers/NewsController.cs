using Microsoft.AspNetCore.Mvc;

namespace Touride.UI.Areas.Admin.Controllers
{
    [Route("admin/[controller]/[action]")]
    public class NewsController : AdminAreaBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
